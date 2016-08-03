/**
 *  网上阅卷
 */
define(['util', 'submodular/paperViewer', 'submodular/cacheQuery'], function (util, paperViewer, CacheQuery) {
    'use strict';

    var modalComments, scoller, cacheQuery, testletsStructId, testletsId, appraiseTemplate, $currentNums, currentNum;
    var params = util.getQueryParams();
    var initializeScroller = _.once(_initializeScroller);
    var initializeAppraiseTemplate = _.once(_initializeAppraiseTemplate);

    $(function () {
        // 参数初始化
        testletsStructId = $('#testlets-struct-id').val();
        // 初始化
        initializeLoading();
        $currentNums = $('#current-nums');
        //注册事件
        addListeners();
    });

    function initializeLoading() {
        // 获取缓存
        //var clearLoading = util.showLoadingInfo('阅卷初始化中');
        var initializeFail = function () {
            layer.msg('阅卷初始化失败', { icon: -1, time: 0, shade: [0.6, '#000'] });
        };

        $.getJSON('/Business/QueryReviewCount', {
            TestletsStructId: testletsStructId
        })
        .done(function (data) {
            currentNum = data;
            $currentNums.text(currentNum);
        })
        .fail(initializeFail);

        window.setTimeout(function () {
            getNextTestlets();
        });
        //window.setTimeout(function () {
        //    $.getJSON('/Business/PullTestlets', {
        //        id: testletsStructId
        //    }).done(function (data) {
        //        if (data.Success === 0) {
        //            // 开始缓存题组
        //            //cacheQuery = new CacheQuery(data.Data, {
        //            //    url: '/Business/PullTestlets',
        //            //    testletsStructId: testletsStructId
        //            //});

        //            //cacheQuery.start();

        //            // 获取下一项
        //            getNextTestlets(data.Data);
        //        } else {
        //            util.showAlert(data.Message || '获取缓存列表失败');
        //            initializeFail();
        //        }
        //    })
        //    .fail(initializeFail)
        //    .always(function () {
        //        clearLoading();
        //    });
        //}, 1000);
    }

    function addListeners() {
        // 回评按钮
        $('#btn-comments').click(function (event) {
            var clearLoading = util.showLoadingInfo('获取回评题组中');

            initializeAppraiseTemplate();

            $.getJSON('/Business/QueryCallBack', {
                id: testletsStructId
            })
                .done(function (data) {
                    if (data.Success === 0) {
                        $('#table-callback').find('tbody').html(appraiseTemplate({
                            list: data.Data,
                            testletsStructId: testletsStructId
                        }));

                        modalComments = layer.open({
                            type: 1,
                            closeBtn: 1,
                            title: '回评',
                            shift: 0,
                            content: $('#popup-appraise'),
                            area: ['600px', '450px']
                        });
                    } else {
                        util.showAlert(data.Message || '获取回评题组失败');
                    }
                })
                .fail(util.ajaxFail)
                .always(function () {
                    clearLoading();
                });
        });

        $("#btn-confirm-comments").on('click', function(event) {
            var clearLoading = util.showLoadingInfo('回评确认中');

            initializeAppraiseTemplate();

            $.getJSON('/Business/ConfirmCallBack', {
                    id: testletsStructId
                }).done(function(data) {
                    if (data.Success === 0) {
                        util.showAlert(data.Message || '全部回评确认成功');
                    } else {
                        util.showAlert(data.Message || '回评确认失败');
                    }
                }).fail(util.ajaxFail)
                .always(function() {
                    clearLoading();
                });
        });


        // 关闭弹窗
        $('#popup-appraise .btn-close').add('#popup-appraise .btn-submit').click(function (event) {
            layer.close(modalComments);
        });

        // 提交评阅
        $('#form-scoring').on('submit', function (event) {
            event.preventDefault();

            var $this = $(this);

            if ($this.check('check')) {
                var clearLoading = util.showLoadingInfo('提交评阅中');
                var post = {
                    AnswerCheckId: testletsId,
                    QuestionGroupId: testletsStructId,
                    Score: []
                };

                $this.find('.form-control').each(function () {
                    var $input = $(this);

                    post.Score.push({
                        Key: $input.data('id'),
                        Value: $input.val()
                    });
                });

                $.ajax({
                    url: '/Business/Review',
                    type: 'POST',
                    contentType: "application/json;charset=UTF-8",
                    data: JSON.stringify(post)
                })
                .done(function (data) {
                    if (data.Success === 0) {
                        util.showAlert(data.Message || '评阅成功');
                        currentNum++;
                        $currentNums.text(currentNum);
                        getNextTestlets();
                    } else if (data.Success === 1) {
                        util.showAlert(data.Message || '评阅失败');
                        getNextTestlets();
                    } else {
                        util.showAlert(data.Message || '评阅失败');
                    }
                })
                .fail(util.ajaxFail)
                .always(function () {
                    $this.get(0).reset();
                    $this.find('.form-control:first').trigger('focus');
                    clearLoading();
                });
            }

        });

        // 设置问题卷
        $('#btn-set-exception').click(function (event) {
            layer.confirm('确认设置为问题卷？', {
                btn: ['确定', '取消']
            }, function (index) {
                var clearLoading = util.showLoadingInfo('提交中');
                var post = {
                    testletsId: testletsId,
                    testletsStructId: testletsStructId
                };

                $.get('/Business/SetProblematics', post)
                    .done(function (data) {
                        if (data.Success === 0) {
                            getNextTestlets();
                            util.showAlert(data.Message || '操作成功');
                            layer.close(index);
                        } else {
                            util.showAlert(data.Message || '操作失败');
                        }
                    })
                    .fail(util.ajaxFail)
                    .always(function () {
                        clearLoading();
                    });
            });
        });
    }

    /**
     * 获取下题组
     */
    //function getNextTestlets(item) {
    //    //cacheQuery.getItem(function(item) {
    //    //    // 设置题组ID
    //    //    testletsId = item.id;
    //    //    initializeDrawing(item);
    //    //});
    //    testletsId = item.TestletsId;

    //    initializeDrawing({
    //        id: item.TestletsId,
    //        image: getImage(item.ImageUrl)
    //    });
    //}


    function getNextTestlets() {
        $.getJSON('/Business/PullTestlets', {
            id: testletsStructId
        }).done(function (data) {
            if (data.Success === 0) {

                testletsId = data.Data.TestletsId;

                initializeDrawing({
                    id: data.Data.TestletsId,
                    image: getImage(data.Data.ImageUrl)
                });
                $("#bh").html(testletsId);
            } else {
                util.showAlert(data.Message || '获取缓存列表失败');
                //initializeFail();
            }
        })
    }



    function getImage(url) {
        var image = document.createElement('img');

        image.src = url;

        return image;
    }


    /**
     * 绘制题组
     */
    function initializeDrawing(data) {
        // 获取图片大小
        util.getImageSize(data.image, function (naturalWidth, naturalHeight) {
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

    /**
     * 初始化模板
     */
    function _initializeAppraiseTemplate() {
        appraiseTemplate = _.template($('#template-callback').html());
    }
});
