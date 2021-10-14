﻿(function ($) {
    $.extend({
        bb_markdown: function (el, obj, value, method) {
            var $el = $(el);
            $.extend(value, {
                events: {
                    blur: function () {
                        var val = $el.toastuiEditor('getMarkdown');
                        var html = $el.toastuiEditor('getHtml');
                        obj.invokeMethodAsync(method, [val, html]);
                    }
                }
            })

            // 修复弹窗内初始化值不正确问题
            var handler = window.setInterval(function () {
                if ($el.is(':visible')) {
                    window.clearInterval(handler);
                    $el.toastuiEditor(value);
                }
            }, 100);
        }
    });
})(jQuery);
