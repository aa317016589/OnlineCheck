/**
 * 缓存队列
 */
define(['util'], function(util) {
    var DELAY = 3000;

    function CacheQuery(initialQuery, options) {
        this.options = _.extend({
            throttleNums: 10
        }, options);
        this.initialQuery = initialQuery;
        this.imageQuery = [];
        this.isStarted = false;
        // 0 成功 1 等待 2 已完成
        this.state = 0;

        if (this.initialQuery && this.initialQuery.length) {
            initializeQuery.apply(this);
        }
    }

    /**
     * 开始缓存
     */
    CacheQuery.prototype.start = function () {
        if (this.isStarted === false) {
          var next = _.throttle(function(that) {
              // 缓存数量未到阀值 之前状态不是 1 等待中 或 2 已完成
              if (that.imageQuery.length < that.options.throttleNums && that.state !== 1 && that.state !== 2) {
                  var dtd = $.Deferred();

                  $.getJSON(that.options.url, {
                      id: that.options.testletsStructId
                  })
                  .done(function(data) {
                      // 设置状态
                      that.state = data.Success;

                      if (data.Success === 0) {
                          var item = {
                              image: getImage(data.Data.ImageUrl),
                              id: data.Data.TestletsId
                          };

                          that.imageQuery.push(item);
                          dtd.resolve(item);
                      } else {
                          dtd.reject(data);
                      }
                  })
                  .fail(function() {
                      dtd.reject();
                  })
                  .always(function() {
                      delete that.wait;
                      next(that);
                  });

                  that.wait = dtd.promise();
              } else {
                // 未启动
                that.isStarted = false;
              }
          }, DELAY);

          next(this);
        }
    };

    /**
     * 获取图片
    */
    CacheQuery.prototype.getItem = function(callback) {
        var that = this;

        if (this.imageQuery.length) { // 如果还有图片直接返回
            var item = this.imageQuery.shift();

            // 补充队列
            this.start();

            callback(item);
        } else { // 如果没有图片则拉取一张
            // 如果之前的状态还是有题
            if (this.state === 0) {
                // 如果正在请求中则等待完成
                if (this.wait) {
                    this.wait
                      .done(function(item) {
                          for (var i = 0, len = that.imageQuery.length; i < len; i++) {
                              if (item.id === that.imageQuery[i].id) {
                                  that.imageQuery.splice(i, 1);
                                  break;
                              }
                          }
                          callback(item);
                      })
                      .fail(function(data) {
                          that.getItem(callback);
                      });
                } else {
                    this.getItem(callback);
                }
            } else if (this.state === 1) {
                var loadingHide = util.showLoadingInfo('正在等待题组中');
                var next = _.throttle(function(that) {
                    $.getJSON(that.options.url, {
                        id: that.options.testletsStructId
                    })
                    .done(function(data) {
                        if (data.Success === 0) {
                            var item = {
                                image: getImage(data.Data.ImageUrl),
                                id: data.Data.TestletsId,
                                otherData:data.Data.OtherData
                            };

                            loadingHide();
                            callback(item);
                        } else if(data.Success === 1) {
                            next(that);
                        } else if(data.Success === 2) {
                            this.showInfo();
                        }
                    })
                    .fail(function() {
                        next(that);
                    });
                }, DELAY);

                next(this);
            } else if (this.state === 2) {
                this.showInfo();
            }
        }
    };

    /**
     * 显示提示信息
     */
    CacheQuery.prototype.showInfo = function(state) {
        // 已完成
        if (state === 2) {
            layer.msg('评阅已完成 请关闭页面', { icon: 1, time: 0, shade: [0.6, '#000'] });
        }
    }

    /**
     * 初始化队列
     */
    function initializeQuery() {
        this.imageQuery = _.map(this.initialQuery, function(item) {
            return {
                image: getImage(item.ImageUrl),
                id: item.TestletsId
            };
        })

        delete this.initialQuery;
    }

    /**
     * 缓存图片
     */
    function getImage(url) {
        var image = document.createElement('img');

        image.src = url;

        return image;
    }

    return CacheQuery;
});
