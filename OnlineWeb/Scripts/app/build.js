({
    appDir: './dev',
    baseUrl: './',
    dir: './dest',
    optimize: 'uglify2',
    preserveLicenseComments: false,
    removeCombined: true,
    generateSourceMaps: true,
    optimizeCss: 'standard',
    paths: {
        mock: '../../mockjs/dist/mock-min',
        jquery: '../../jquery',
        underscore: '../../underscore-min',
        backbone: '../../backbone/backbone',
        nicescroll: '../../nicescroll/jquery.nicescroll.min',
        elist: '../../elist/jquery-elist',
        egrid: '../../egrid/jquery-egrid',
        echeck: '../../echeck/jquery-echeck',
        epaging: '../../epaging/jquery-epaging',
        jBox: '../../jBox/jBox',
        colorbox: '../../colorbox/jquery.colorbox',
        history: '../../history/jquery.history',
        viewer: '../../viewer/viewer.min',
        echarts: '../../echarts/echarts-all',
        ztree: '../../ztree/js/jquery.ztree.all-3.5.min',
        nstSlider: '../../nstSlider/jquery.nstSlider.min',
        layer: '../../layer2.1/layer',
        layerExtend: '../../layer2.1/extend/layer.ext',
        laydate: '../../laydate/laydate',
        printarea: '../../printarea/jquery.printarea',
        unslider: '../../unslider/unslider.min',
        chosen: '../../chosen/chosen.jquery.min',
        icheck: '../../icheck/icheck.min',
        html2canvas: '../../html2canvas',
        customScroll: '../../customScroll/jquery.mCustomScrollbar.concat.min',
        colorpicker: '../../tinycolorpicker/lib/jquery.tinycolorpicker',
        tpl: '../../underscore-tpl',
        scroller: '../../scroller/src/Scroller',
        easyScroller: '../../scroller/src/easyScroller',
        scrollerAnimate: '../../scroller/src/Animate'
    },
    shim: {
        backbone: {
            deps: ['underscore', 'jquery'],
            exports: 'Backbone'
        },
        nicescroll: ['jquery'],
        elist: ['jquery', 'css!../../elist/css/default.css'],
        egrid: ['jquery'],
        echeck: ['jquery'],
        epaging: ['jquery'],
        echarts: {
            exports: 'echarts'
        },
        history: ['jquery'],
        jBox: ['jquery', 'css!../../jBox/jBox.css'],
        colorbox: ['jquery', 'css!../../../colorbox.css'],
        viewer: ['jquery', 'css!../../viewer/css/viewer.css'],
        ztree: ['jquery', 'css!../../ztree/css/zTreeStyle/metroStyle.css'],
        nstSlider: ['jquery'],
        layer: ['jquery'],
        layerExtend: ['layer'],
        laydate: {
            exports: 'laydate'
        },
        printarea: ['jquery'],
        unslider: ['jquery'],
        chosen: ['jquery', 'css!../../chosen/chosen.css'],
        icheck: ['jquery', 'css!../../icheck/skins/square/blue.css'],
        customScroll: ['jquery', 'css!../../customScroll/jquery.mCustomScrollbar.min'],
        colorpicker: ['jquery'],
        scroller: {
            deps: ['scrollerAnimate', 'easyScroller'],
            exports: 'Scroller'
        },
        easyScroller: {
            exports: 'EasyScroller'
        },
        scrollerAnimate: {
            exports: 'core'
        }
    },
    modules: [
        {
            name: 'util'
        }
    ]
})
