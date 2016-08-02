/**
 *  网上阅卷
 */
define(['util', 'mock', 'submodular/paperViewer', 'submodular/cacheQuery'], function (util, Mock, paperViewer, CacheQuery) {
    'use strict';

    /*Mock*/
    // 获取已缓存的题组
    Mock.mock('/Business/QueryLoaded', {
        Success: 0,
        Message: '成功',
        Data: []
        // 'Data|1-10': [{
        //     'TestletsId|+1': 1,
        //     'ImageUrl': "/Content/images/demo/text.jpg"
        // }]
    });

    // 获取题组
    Mock.mock('/Business/PullTestlets', {
        Success: 0,
        Message: '成功',
        'Data': {
            'TestletsId': 1,
            'ImageUrl': "/Content/images/demo/text3.jpg"
        }
    });

    // 设置为问题卷
    Mock.mock('/Business/SetProblematics', {
        Success: 0,
        Message: '成功',
        Data: {
            'TestletsId': 1,
            'ImageUrl': '/Content/images/demo/text2.jpg'
        }
    });

    // 提交预评览
    Mock.mock('/Business/Review', {
        Success: 0,
        Message: '成功',
        Data: {
            'TestletsId': 1,
            'ImageUrl': '/Content/images/demo/text1.jpg'
        }
    });
    /*Mock*/

    var modalComments, scoller, cacheQuery, testletsId;
    var params = util.getQueryParams();
    var initializeScroller = _.once(_initializeScroller);

    $(function(){
        // 参数初始化
        testletsId = $('#testlets-id').val();
        // 初始化
        initializeLoading();
        //注册事件
        addListeners();
    });

    function initializeLoading() {
        // 获取缓存
        $.getJSON('/Business/QueryLoaded').done(function(data) {
            if (data.Success === 0) {
                var currentQuery = [];

                // 是否存在缓存
                if (data.Data && data.Data.length) { // 存在缓存
                    data.Data.splice(0, 1);
                }

                // 开始缓存题组
                cacheQuery = new CacheQuery(currentQuery, {
                    testletsId: testletsId
                });

                cacheQuery.start();

                // 获取下一项
                cacheQuery.getItem(function(item) {
                    initializeDrawing(item);
                });
            } else {
                util.showAlert(data.Message || '获取缓存列表失败');
            }
        })
        .fail(util.ajaxFail);
    }

    function addListeners () {
        // 回评按钮
        $('#btn-comments').click(function(event) {
            modalComments = util.showDaialog({
                area: '320px',
                title: '回评',
                content: $('#popup-appraise')
            });
        });
        // 关闭弹窗
        $('#popup-appraise .btn-close').add('#popup-appraise .btn-submit').click(function(event) {
            layer.close(modalComments);
        });
        // 提交评览
        $('#form-scoring').on('submit', function(event) {
            event.preventDefault();

            $.post('/Business/Review')
                .done(function(data){
                    data = JSON.parse(data);

                    if (data.Success === 0) {
                        getNextTestlets(data.Data);
                    } else {
                        util.showAlert(data.mes || '操作失败');
                    }
                })
                .fail(util.ajaxFail);
        });

        // 设置问题卷
        $('#btn-set-exception').click(function(event) {
            layer.confirm('确认设置为问题卷？', {
                btn: ['确定','取消']
            }, function(index) {
                var postArray = {
                    testletsId: 1,
                    testletsStructId: 1
                };

                $.post('/Business/SetProblematics', postArray)
                    .done(function(data){
                        data = JSON.parse(data);

                        if (data.Success === 0) {
                            getNextTestlets(data.Data);
                            layer.close(index);
                        } else {
                            util.showAlert(data.mes || '操作失败');
                        }
                    })
                    .fail(util.ajaxFail);
            });
        });
    }

    /**
     * 获取下题组
     */
    function getNextTestlets(newTestlets) {
        cacheQuery.getItem(function(item) {
            initializeDrawing(item);
        }, newTestlets);
    }

    /**
     * 绘制题组
     */
    function initializeDrawing(data) {
        // 获取图片大小
        util.getImageSize(data.image, function(naturalWidth, naturalHeight) {
            $('#scoring-item').attr({
                width: naturalWidth,
                height: naturalHeight
            }).css('background-image', 'url(' + data.image.src + ')');

            initializeScroller();
        });
    }

    /**
     * 初始化画布
     */
    function _initializeScroller() {
        // 试卷控制
        scoller = paperViewer();
    }
});
