/**
 * 阅卷组件模块
 */
define(['util', 'scroller', 'echeck', 'colorpicker'], function(util, Scroller) {
    'use strict';

    var $paperWrapper = $('#img-wrapper');
    var $paperImage = $paperWrapper.find('img');
    var scoreCheck = /^-?(\d+\.\d{1,1}|\d+)$/;
    var rules = {
        scoreLimit: {
            func: function($this) {
                var val = parseFloat($this.val());
                var maxScore = parseFloat($this.data('max'));

                if (scoreCheck.test(val)) {
                    if (val > maxScore) {
                        this.errorMsg = '本题满分为 <b>' + maxScore + '</b>';
                        return false;
                    }

                    return true;
                }

                this.errorMsg = '请输入正整数或一位小数';
                return false;
            }
        },
        trimval: {
            errorMsg: '',
            func: function($this) {
                $this.val($.trim($this.val()));
                return true;
            }
        }
    };

    $(function() {
        //校验
        $('#form-scoring').check({
            rules: rules
        });
        //滚动条初始化
        util.initScroll($('.right-fixed'));

        //初始化
        addListeners();
    });


    /**
     * 初始化阅卷组件
     */
    function initializeScrollViewer() {
        var $aim = $('#scoring-item');
        var docCookies = util.docCookies;
        var scroller, scroll, defaultZoom = 1;

        /* 初始化图片尺寸 */
        if (docCookies.hasItem('scale')) {
            //cookies中有保存的比例时
            var scale = parseFloat(docCookies.getItem('scale'));

            defaultZoom = scale;
        } else {
            //没有保存的比例时
            var viewportWidth = $paperWrapper.width();

            _.each($paperImage, function(item, key, list) {
                var currentWidth = $aim.width();

                if (currentWidth > viewportWidth) {
                    defaultZoom = getFullscreenScale(currentWidth);
                }
            });
        }

        /* 初始化查看器 */
        scroller = new EasyScroller($aim.get(0), {
            zooming: true,
            animation: false,
            defaultZoom: defaultZoom
        });
        scroll = scroller.scroller;

        /* 侦听键盘事件 */
        var keydownHandler = _.throttle(function(event) {
            var tagName = event.target.tagName.toLowerCase();

            if (tagName !== 'input') {
                switch (event.code) {
                    case 'ArrowUp': // 上键
                        scroll.scrollBy(0, -150, true);
                        break;
                    case 'ArrowDown': // 下键
                        scroll.scrollBy(0, 150, true);
                        break;
                    case 'ArrowLeft': // 左键
                        scroll.scrollBy(-150, 0, true);
                        break;
                    case 'ArrowRight': // 右键
                        scroll.scrollBy(150, 0, true);
                        break;
                    case 'NumpadAdd': // +键
                        scroll.zoomBy(1.2, true);
                        break;
                    case 'NumpadSubtract': // -键
                        scroll.zoomBy(0.8, true);
                        break;
                    default:
                }
            }
        }, 400);

        document.addEventListener('keydown', keydownHandler);

        /* 初始化工具栏状态 */
        if (docCookies.hasItem('tool')) {
            var status = docCookies.getItem('tool');

            if (status === 'open') {
                $('#cd-stretchy-nav').addClass('nav-is-visible');
            }
        }

        /* 初始化工具栏 */
        var $cdStretchyNav = $('#cd-stretchy-nav');

        $cdStretchyNav.on('click', '.cd-nav-trigger', function(event) { // 展开收缩
            event.preventDefault();

            if ($cdStretchyNav.hasClass('nav-is-visible') === true) {
                docCookies.setItem('tool', 'close');
            } else {
                docCookies.setItem('tool', 'open');
            }

            $cdStretchyNav.toggleClass('nav-is-visible');
        }).on('click', '#btn-magnify', function() { // 放大
            scroll.zoomBy(1.2, true);
        }).on('click', '#btn-shrink', function() { // 缩小
            scroll.zoomBy(0.8, true);
        }).on('click', '#btn-normal', function() { // 正常比例
            scroll.zoomTo(1, true);
        }).on('click', '#btn-fullscreen', function() { // 铺满
            scroll.zoomTo(getFullscreenScale($aim.width()), true);
        }).on('click', '#btn-restore', function() { // 还原
            //还原
            if (docCookies.hasItem('scale')) {
                var scale = parseFloat(docCookies.getItem('scale'));

                scroll.zoomTo(scale, true);
            } else {
                //铺满
                scroll.zoomTo(1, true);
            }
        }).on('click', '#btn-save', function() { // 保存
            //保存
            var values = scroll.getValues();

            docCookies.setItem('scale', values.zoom.toFixed(2));
            util.showInfo('保存图片比例成功');
        });

        /* 初始化题目绘图 */
        $('#form-scoring').find('.form-control').on('focus', function() {
            var $formGroup = $(this).parents('.form-group');
            var axis = (function(axisString) {
                var axisArray = axisString.split(',');

                return _.map(axisArray, function(item) {
                    return parseFloat($.trim(item));
                });
            })($formGroup.data('axis'));

            if (axis && axis.length === 4) {
                var canvas = document.getElementById('scoring-item');
                var context;

                // 获取上下文
                if (canvas.getContext) {
                    context = canvas.getContext('2d');

                    // 清除之前的矩形
                    if ($aim.data('hasRect')) {
                        context.clearRect(0, 0, canvas.width, canvas.height);
                    }

                    // 记录已绘制
                    $aim.data('hasRect', true);

                    context.strokeStyle = '#FF0008';
                    context.lineWidth = 2;
                    context.strokeRect.apply(context, axis);

                    // 移动矩形到视角中间
                    var zoom = scroll.getValues().zoom;
                    var axisX = axis[0] * zoom;
                    var axisY = axis[1] * zoom;
                    var viewportCenterLeft = (function() {
                        var itemWidth = axis[2] * zoom / 2;
                        var viewportHalfWidth = $paperWrapper.width() / 2;

                        if (itemWidth <= viewportHalfWidth) {
                            return viewportHalfWidth - itemWidth;
                        } else {
                            return itemWidth;
                        }
                    })();
                    var viewportCenterTop = (function() {
                        var itemHeight = axis[3] * zoom / 2;
                        var viewportHalfHeight = $paperWrapper.height() / 2;

                        if (itemHeight <= viewportHalfHeight) {
                            return viewportHalfHeight - itemHeight;
                        } else {
                            return itemHeight;
                        }
                    })();
                    var offset = {};

                    if (axisX > viewportCenterLeft) {
                        offset.left = axisX - viewportCenterLeft;
                    } else {
                        offset.left = 0;
                    }

                    if (axisY > viewportCenterTop) {
                        offset.top = axisY - viewportCenterTop;
                    } else {
                        offset.top = 0;
                    }

                    scroll.scrollTo(axisX, offset.top, true);
                }
            }
        }).filter(':first').focus();

        return scroller;
    }

    /**
     * 侦听器
     */
    function addListeners() {
        /*总分显示*/
        $('#form-scoring [name=score]').keyup(calculateChange).change(calculateChange);

        /*初始化颜色选择器*/
        var $colorPicker = $('#color-picker');
        var colorPicker;

        $colorPicker.tinycolorpicker();
        colorPicker = $colorPicker.data('plugin_tinycolorpicker');

        $colorPicker.on('change', function() {
            var color = colorPicker.colorHex;

            if (color) {
                var $cdStretchyNav = $('#cd-stretchy-nav');

                $('#img-wrapper').css('background-color', color);
                util.docCookies.setItem('background', color);

                // 反色工具栏
                if (color === '#FFFFFF') {
                    $cdStretchyNav.removeClass('inverse');
                } else {
                    $cdStretchyNav.addClass('inverse');
                }
            }
        });

        // 获取历史颜色
        if (util.docCookies.hasItem('background')) {
            colorPicker.setColor(util.docCookies.getItem('background') || '#fff');
            $colorPicker.trigger('change');
        }
    }

    /**
     * 总分显示
     */
    function calculateChange() {
        var $calculate = $('#calculate');
        var $scoreCalculate = $('#score-calculate', $calculate);
        var $list = $('#form-scoring [name="score"]');
        var scoreList = [];
        var result = 0;

        $list.each(function(index, el) {
            var itemVal = $(this).val();

            if (itemVal.length && scoreCheck.test(itemVal)) {
                var value = parseFloat(itemVal);

                //存在小数点则进行约为一位小数
                if (value.toString().indexOf('.') !== -1) {
                    value = parseFloat(value.toFixed(1));
                }

                scoreList.push(value);
                result += value;
            }
        });

        result = result.toFixed(1);

        if (scoreList.length > 1) {
            $scoreCalculate.text(scoreList.join('+') + '=' + result);
        } else if (scoreList.length === 1) {
            $scoreCalculate.text(scoreList[0]);
        } else {
            $scoreCalculate.text('');
        }

        $calculate.show();
    }

    /**
     * 获取全屏比例
     */
    function getFullscreenScale(currentWidth) {
        var fullWidth = $paperWrapper.width();

        return fullWidth / currentWidth;
    }

    return initializeScrollViewer;
});
