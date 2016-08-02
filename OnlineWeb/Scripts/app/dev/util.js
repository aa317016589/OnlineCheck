define(['jquery', 'underscore', 'layer'], function() {
    'use strict';

    /*插件配置*/
    var currentRequests = {};

    $.ajaxSetup({
        timeout: 300000
    });

    layer.config({
        path: '/Scripts/layer2.1/'
    });

    _.mixin({
        'mutation': function(object, interceptor) {
            return interceptor(object);
        }
    });

    /*通用工具*/
    var util = {};
    var pluginsOptions = {
        'elist': {
            //是否多选
            multiple: false,
            //是否自动选择第一项
            selectedFirstOption: true,
            //是否在点击后关闭下拉菜单
            closeDropdownAfterClick: true
        },
        'nicescroll': {
            cursorcolor: '#7db7fb',
            cursorwidth: '6px',
            cursorborderradius: 2,
            autohidemode: true,
            background: '#d0d0d0',
            cursoropacitymin: 1,
            cursorborder: 'none',
            horizrailenabled: true,
            railvalign: 'top'
        },
        'chosen': {
            no_results_text: '未找到',
            placeholder_text_multiple: '选择若干项',
            placeholder_text_single: '选择一项',
            disable_search_threshold: 10
        },
        'icheck': {
            checkboxClass: 'icheckbox_square-blue',
            radioClass: 'iradio_square-blue',
            increaseArea: '20%'
        },
        'customScroll': {
            axis: 'x',
            theme: 'dark',
            scrollbarPosition: 'inside',
            setLeft: 0
        }
    };

    /*测试配置*/
    util.testConfig = true;

    /**
     * ajax获取失败的提示
     */
    util.ajaxFail = function() {
        util.showAlert('数据请求失败 请联系管理员');
    };

    /**
     * 显示警告
     * @param  {String} text 提示文本
     * @doc http://layer.layui.com/api.html#layer.msg
     */
    util.showAlert = function(text) {
        if(typeof layer !== 'undefined'){
            layer.msg(text);
        }
    };

    /**
     * 显示提示
     * @param  {String} text 提示文本
     * @doc http://layer.layui.com/api.html#layer.msg
     */
    util.showInfo = function(text) {
        if(typeof layer !== 'undefined'){
            layer.msg(text);
        }
    };

    /**
     * 显示载入提示
     * @param  {String} text 提示文本
     * @return {Number} index 索引值 用来关闭
     * @doc http://layer.layui.com/api.html#layer.msg
     */
    util.showLoadingInfo = function(text) {
        if (typeof layer !== 'undefined') {
            var index;
            var timeIndex = setTimeout(function () {
                index = layer.msg(text, { icon: 16, time: 0, shade: [0.6, '#000'] });
            }, 500);

            return function () {
                window.clearTimeout(timeIndex);
                if (index) {
                    layer.close(index);
                }
            };
        }
    };

    /**
     * 页面弹窗
     * @param  {Object} config 配置项
     * @doc http://layer.layui.com/api.html#layer.open
     * @return {Number} index 索引值 用来关闭
     */
    util.showDialog = function(config) {
        if(typeof layer !=='undefined'){
            var opts = $.extend(true, {
                type: 1,
                scrollbar: false
            }, config);

            return layer.open(opts);
        }
    };

    /*Cookies*/
    util.docCookies = {
        getItem: function(sKey) {
            return decodeURIComponent(document.cookie.replace(new RegExp("(?:(?:^|.*;)\\s*" + encodeURIComponent(sKey).replace(/[\-\.\+\*]/g, "\\$&") + "\\s*\\=\\s*([^;]*).*$)|^.*$"), "$1")) || null;
        },
        setItem: function(sKey, sValue, vEnd, sPath, sDomain, bSecure) {
            if (!sKey || /^(?:expires|max\-age|path|domain|secure)$/i.test(sKey)) {
                return false;
            }
            var sExpires = "";
            if (vEnd) {
                switch (vEnd.constructor) {
                    case Number:
                        sExpires = vEnd === Infinity ? "; expires=Fri, 31 Dec 9999 23:59:59 GMT" : "; max-age=" + vEnd;
                        break;
                    case String:
                        sExpires = "; expires=" + vEnd;
                        break;
                    case Date:
                        sExpires = "; expires=" + vEnd.toUTCString();
                        break;
                }
            }
            document.cookie = encodeURIComponent(sKey) + "=" + encodeURIComponent(sValue) + sExpires + (sDomain ? "; domain=" + sDomain : "") + (sPath ? "; path=" + sPath : "") + (bSecure ? "; secure" : "");
            return true;
        },
        removeItem: function(sKey, sPath, sDomain) {
            if (!sKey || !this.hasItem(sKey)) {
                return false;
            }
            document.cookie = encodeURIComponent(sKey) + "=; expires=Thu, 01 Jan 1970 00:00:00 GMT" + (sDomain ? "; domain=" + sDomain : "") + (sPath ? "; path=" + sPath : "");
            return true;
        },
        hasItem: function(sKey) {
            return (new RegExp("(?:^|;\\s*)" + encodeURIComponent(sKey).replace(/[\-\.\+\*]/g, "\\$&") + "\\s*\\=")).test(document.cookie);
        },
        keys: /* optional method: you can safely remove it! */ function() {
            var aKeys = document.cookie.replace(/((?:^|\s*;)[^\=]+)(?=;|$)|^\s*|\s*(?:\=[^;]*)?(?:\1|$)/g, "").split(/\s*(?:\=[^;]*)?;\s*/);
            for (var nIdx = 0; nIdx < aKeys.length; nIdx++) {
                aKeys[nIdx] = decodeURIComponent(aKeys[nIdx]);
            }
            return aKeys;
        }
    };

    /*退出提示*/
    util.unload = {
        bind: function(callback) {
            if(typeof callback === 'function') {
                $(window).bind('beforeunload', callback);
            } else {
                $.error('callback must be a function');
            }
        },
        unbind: function() {
            $(window).unbind('beforeunload');
        }
    };

    /**
     * 获取地址栏参数
     * @return {String}      值
     */
    util.getQueryParams = function() {
        var queryString = {};
        var query = window.location.search.substring(1);
        var vars = query.split("&");

        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            if (typeof queryString[pair[0]] === "undefined") {
                queryString[pair[0]] = decodeURIComponent(pair[1]);
            } else if (typeof queryString[pair[0]] === "string") {
                var arr = [queryString[pair[0]], decodeURIComponent(pair[1])];
                queryString[pair[0]] = arr;
            } else {
                queryString[pair[0]].push(decodeURIComponent(pair[1]));
            }
        }
        return queryString;
    };

    /**
     * 获取URL的参数对象
     * @param  {String} url url
     * @return {Object} 参数对象
     */
    util.getQueryParamsByUrl = function(url) {
        var queryString = {};
        var query = url.split('?');
        var vars = '';

        if(query.length === 2){
            query = query[1];
            vars = query.split('&');

            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split('=');
                if (typeof queryString[pair[0]] === 'undefined') {
                    queryString[pair[0]] = decodeURIComponent(pair[1]);
                } else if (typeof queryString[pair[0]] === 'string') {
                    var arr = [queryString[pair[0]], decodeURIComponent(pair[1])];
                    queryString[pair[0]] = arr;
                } else {
                    queryString[pair[0]].push(decodeURIComponent(pair[1]));
                }
            }

            return queryString;
        }else{
            return {};
        }
    };

    /**
     * 序列化表单数据
     * @param  {Jquery DOm} $form 表单jquery对象
     * @return {Object}       表单数据对象
     */
    util.getFormParams = function($form) {
        if (typeof $form.serializeArray !== 'undefined') {
            var list = $form.serializeArray();
            var array = {};

            _.each(list, function(value, key, list) {
                array[value.name] = value.value;
            });

            return array;
        }
    };

    /**
     * 获取图片真实大小
     * @param  {Dom}   image    图片dom对象
     * @param  {Function} callback 回调
     */
    util.getImageSize = function(image, callback) {
        var newImage;

        // Modern browsers
        if (image.naturalWidth) {
            return callback(image.naturalWidth, image.naturalHeight);
        }

        // IE8: Don't use `new Image()` here
        newImage = document.createElement('img');

        newImage.onload = function() {
            callback(this.width, this.height);
        };

        newImage.src = image.src;
    };

    /**
     * 获取插件配置项
     */
    util.getPluginsConfig = function(name) {
        try {
            return pluginsOptions[name];
        } catch (e) {
            throw {
                name: 'option is undefined',
                message: name + ' option is undefined'
            };
        }
    };

    /**
     * 保存历史状态
     * @param  {Object} history 插件History对象
     * @param  {Object} state   参数对象
     * @return {Boolean}        成功标志
     */
    util.saveHistoryState = function(history, state) {
        if(history) {
            history.replaceState(null, document.title || null, '?' + $.param(state));
            return true;
        } else {
            return false;
        }
    };

    /**
     * 读取历史状态
     * @param  {Object} history 插件History对象
     * @return {Object}         参数对象
     */
    util.loadHistoryState = function(history) {
        if(history) {
            var state = history.getState();
            var urlParams = util.getQueryParamsByUrl(state.cleanUrl);

            return urlParams;
        } else {
            return undefined;
        }
    };

    /**
     * 通用Post方法
     * @param  {Object} config 配置
     * @return {promise}
     */
    util.post = function(config) {
        var option = _.extend({
            url: undefined,
            data: undefined
        }, config);

        var dtd = $.Deferred();

        if(currentRequests[option.url]) {
            return false;
        }

        currentRequests[option.url] = true;

        $.post(option.url, option.data)
            .done(function(data) {
                dtd.resolve(data);
            })
            .fail(function() {
                dtd.reject(arguments);
            })
            .always(function() {
                currentRequests[option.url] = false;
            });

        return dtd.promise();
    };

    /**
     * 列表初始化
     * @param  {Jquery dom} $dom 需要包装的元素
     * @param  {Object} options 配置项
     * @return {Object}      nicescroll对象
     */
    util.initList = function($dom, options) {
        var config = $.extend(true, {}, pluginsOptions.elist, options || {});

        require(['elist'], function() {
            return $dom.elist(config);
        });
    };

    /**
     * 滚动条初始化
     * @param  {Jquery dom} $dom 需要包装的元素
     * @param  {Object} options 配置项
     * @return {Object}      nicescroll对象
     */
    util.initScroll = function($dom, options) {
        var config = $.extend(true, {}, pluginsOptions.nicescroll, options || {});

        require(['nicescroll'], function() {
            return $dom.niceScroll(config);
        });
    };

    return util;
});
