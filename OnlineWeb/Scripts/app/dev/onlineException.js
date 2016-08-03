/**
 *  网上阅卷
 */
define(['util', 'submodular/paperViewer'], function (util, paperViewer) {
    'use strict';

    var scoller, cacheQuery, testletsStructId, testletsId;
    var params = util.getQueryParams();
    var initializeScroller = _.once(_initializeScroller);

    $(function () {
        // 参数初始化
        testletsStructId = $('#testlets-struct-id').val();
        // 初始化
        initializeLoading();
        //注册事件
        addListeners();
    });

    function initializeLoading() {
        // 获取缓存
        var clearLoading = util.showLoadingInfo('问题卷初始化中');

        $.getJSON('/Business/GetProblematics', {
            testletsStructId: testletsStructId
        }).done(function (data) {
            if (data.Success === 0) {
                // 开始缓存题组
                cacheQuery = _.map(data.Data, function (item) {
                    var image = document.createElement('img');

                    image.src = item.ImageUrl;

                    return {
                        image: image,
                        id: item.TestletsId
                    };
                });

                // 获取下一项
                getNextTestlets();
            } else {
                util.showAlert(data.Message || '获取问题卷列表失败');
            }
        })
            .fail(util.ajaxFail)
            .always(function () {
                clearLoading();
            });
    }

    function addListeners() {
        // 提交评阅
        $('#form-scoring').on('submit', function (event) {
            event.preventDefault();

            var $this = $(this);

            if ($this.check('check')) {
                var clearLoading = util.showLoadingInfo('提交评阅中');
                var post = {
                    QuestionGroupId: testletsStructId,
                    AnswerCheckId: testletsId,
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
                    url: '/Business/ReviewProblematics',
                    type: 'POST',
                    contentType: "application/json;charset=UTF-8",
                    data: JSON.stringify(post)
                }).done(function (data) {
                    if (data.Success === 0) {
                        util.showAlert(data.Message || '评阅成功');
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
    }

    /**
     * 获取下题组
     */
    function getNextTestlets() {
        if (cacheQuery.length > 0) {
            var item = cacheQuery.shift();

            // 设置题组ID
            testletsId = item.id;
            initializeDrawing(item);
            $("#bh").html(testletsId);
        } else {
            layer.alert('问题卷已评阅完毕，请关闭页面', {
                closeBtn: 0,
                shift: 4
            });
        }
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
});
