/**
 * 缓存队列
 */
define(['util'], function() {
    function CacheQuery(initialQuery, options) {
        this.options = _.extend({
            throttleNums: 10
        }, options);
        this.initialQuery = initialQuery;
        this.imageQuery = [];
    }

    /**
     * 开始缓存
     */
    CacheQuery.prototype.start = function () {
        if (this.initialQuery && this.initialQuery.length) {
            initializeQuery.apply(this);
        }

        (function next(that) {
            if (that.imageQuery.length < that.options.throttleNums) {
                var dtd = $.Deferred()

                $.getJSON('/Business/PullTestlets', {
                        id: that.options.testletsId
                    })
                    .done(function(data) {
                        if (data.Success === 0) {
                            var item = {
                                image: getImage(data.Data.ImageUrl),
                                id: data.Data.TestletsId
                            };

                            that.imageQuery.push(item);
                            dtd.resolve(item);
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
            }
        })(this);
    };

    /**
     * 获取图片
    */
    CacheQuery.prototype.getItem = function(callback, newItem) {
        var that = this;

        if (this.imageQuery.length) { // 如果还有图片直接返回
            var item = this.imageQuery.shift();

            // 补充队列
            if (newItem) {
                this.imageQuery.push({
                    image: getImage(newItem.ImageUrl),
                    id: newItem.TestletsId
                });
            } else {
                this.start();
            }

            callback(item);
        } else { // 如果没有图片则拉取一张
            // 如果正在请求中则等待完成
            if (this.wait) {
                this.wait.done(function(item) {
                    callback(item);
                });
            }
        }
    };

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
