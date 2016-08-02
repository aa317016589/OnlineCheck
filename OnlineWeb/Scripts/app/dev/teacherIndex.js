/**
 * 教师首页模块
 */
define(['util'], function (util) {
    'use strict';

    $(function() {
        addListeners();
    });

    function addListeners() {
        var $menuList = $('#menu-list');

        /* 菜单 */
        $menuList.on('click', '.menu-link', function(event) {
            var $this = $(this);
            var $parentVirtualLink = $this.closest('.hasSubMenu').find('.menu-link-virtual');

            if ($this.hasClass('menu-link-virtual')) {
                return false;
            }

            $this.addClass('selected');

            $menuList.find('.menu-link').not($this).not($parentVirtualLink).removeClass('selected');

            // 面包屑
            $('#current-page').text($this.find('.menu-text').text()).attr('href', $this.attr('href'));
        }).on('mouseover', '.hasSubMenu', function() {
            var $this = $(this);

            $this.children('.menu-link').addClass('selected').end()
                .find('.sub-menu').show().end()
                .siblings('li').find('.sub-menu').hide();
        }).on('mouseleave', '.hasSubMenu', function() {
            var $this = $(this);
            var $subMenu = $this.find('.sub-menu');

            $subMenu.hide();

            // 如果已选中二级菜单不移除选中效果
            if ($subMenu.find('.menu-link.selected').length === 0) {
                $this.children('.menu-link').removeClass('selected')
            }
        });
    }
});
