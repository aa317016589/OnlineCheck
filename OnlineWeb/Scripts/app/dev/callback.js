/**
 *  回评
 */
define(['util', 'submodular/paperViewer'], function (util, paperViewer) {
    'use strict';

    var scoller, testletsStructId, testletsId, reviewId;
    var params = util.getQueryParams();
    var initializeScroller = _.once(_initializeScroller);

    $(function(){
        // 参数初始化
        testletsStructId = $('#testlets-struct-id').val();
        // 初始化
        initializeLoading();
        //注册事件
        addListeners();
    });

    function initializeLoading() {
        // 获取缓存
        var clearLoading = util.showLoadingInfo('回评卷初始化中');

        $.getJSON('/Business/QueryOnlyCallBack', {
                testletsStructId: testletsStructId,
                reviewId: params.reviewId
            }).done(function (data) {
                if (data.Success === 0) {
                    var result = data.Data;

                    if (result) {
                        var $input = $('#form-scoring').find('.form-control');
                        var image = document.createElement('img');

                        image.src = result.ImageUrl;
                        testletsId = result.TestletsId;
                        reviewId = result.ReviewId;

                        var item = {
                            image: image,
                            id: testletsId
                        };

                        $("#bh").html(testletsId);
                        // 设置时间
                        $('#callback-date').text(result.ReviewDate);

                        _.each(result.ReviewItems, function (item) {
                            $input.filter('[data-id="' + item.ScorePointStructId + '"]').val(item.Score)
                                .data('special-id', item.Id);
                        });

                        initializeDrawing(item);
                    }
                } else {
                    util.showAlert(data.Message || '获取回评列表失败');
                }
            })
            .fail(util.ajaxFail)
            .always(function () {
                clearLoading();
            });
    }

    function addListeners () {
        // 提交评阅
        $('#form-scoring').on('submit', function(event) {
            event.preventDefault();

            var $this = $(this);

            if ($this.check('check')) {
                var clearLoading = util.showLoadingInfo('提交评阅中');
                var post = {
                    QuestionGroupId: testletsStructId,
                    AnswerCheckId: testletsId,
                    Id: reviewId,
                    Score: []
                };

                $this.find('.form-control').each(function() {
                    var $input = $(this);

                    post.Score.push({
                        Key: $input.data('id'),
                        Value: $input.val()
                    });
                });

                $.ajax({
                    url: '/Business/CallBackReview',
                    type: 'POST',
                    contentType: 'application/json;charset=UTF-8',
                    data: JSON.stringify(post)
                })
                .done(function(data){
                    if (data.Success === 0) {
                        util.showAlert(data.Message || '评阅成功');
                    } else if (data.Success === 1) {
                        util.showAlert(data.Message || '评阅失败');
                    } else {
                        util.showAlert(data.Message || '评阅失败');
                    }
                })
                .fail(util.ajaxFail)
                .always(function() {
                  $this.find('.form-control:first').trigger('focus');
                    clearLoading();
                });
            }

        });
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
