define(["util","mock","submodular/paperViewer","submodular/cacheQuery"],function(e,t,s,a){"use strict";function n(){$.getJSON("/Business/QueryLoaded").done(function(t){if(0===t.Success){var s=[];t.Data&&t.Data.length&&t.Data.splice(0,1),m=new a(s,{testletsId:g}),m.start(),m.getItem(function(e){o(e)})}else e.showAlert(t.Message||"获取缓存列表失败")}).fail(e.ajaxFail)}function i(){$("#btn-comments").click(function(t){l=e.showDaialog({area:"320px",title:"回评",content:$("#popup-appraise")})}),$("#popup-appraise .btn-close").add("#popup-appraise .btn-submit").click(function(e){layer.close(l)}),$("#form-scoring").on("submit",function(t){t.preventDefault(),$.post("/Business/Review").done(function(t){t=JSON.parse(t),0===t.Success?c(t.Data):e.showAlert(t.mes||"操作失败")}).fail(e.ajaxFail)}),$("#btn-set-exception").click(function(t){layer.confirm("确认设置为问题卷？",{btn:["确定","取消"]},function(t){var s={testletsId:1,testletsStructId:1};$.post("/Business/SetProblematics",s).done(function(s){s=JSON.parse(s),0===s.Success?(c(s.Data),layer.close(t)):e.showAlert(s.mes||"操作失败")}).fail(e.ajaxFail)})})}function c(e){m.getItem(function(e){o(e)},e)}function o(t){e.getImageSize(t.image,function(e,s){$("#scoring-item").attr({width:e,height:s}).css("background-image","url("+t.image.src+")"),p()})}function u(){r=s()}t.mock("/Business/QueryLoaded",{Success:0,Message:"成功",Data:[]}),t.mock("/Business/PullTestlets",{Success:0,Message:"成功",Data:{TestletsId:1,ImageUrl:"/Content/images/demo/text3.jpg"}}),t.mock("/Business/SetProblematics",{Success:0,Message:"成功",Data:{TestletsId:1,ImageUrl:"/Content/images/demo/text2.jpg"}}),t.mock("/Business/Review",{Success:0,Message:"成功",Data:{TestletsId:1,ImageUrl:"/Content/images/demo/text1.jpg"}});var l,r,m,g,p=(e.getQueryParams(),_.once(u));$(function(){g=$("#testlets-id").val(),n(),i()})});
//# sourceMappingURL=examOnline.js.map