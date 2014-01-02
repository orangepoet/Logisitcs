
/// <reference path="jquery/jquery-1.6.2-vsdoc.js" />
/// <reference path="ligerUI/ligerForm.js" />
/// <reference path="ligerUI/ligerui.min.js" />
/// <reference path="ligerUI/ligerTab.js" />
/// <reference path="ligerUI/ligerui.all.js" />

var tab;
$(function () {
    $("article").ligerLayout({
        height: '100%',
        heightDiff: -3,
        leftWidth: 140,
        minLeftWidth: 120
    });
    var bodyHeight = $(".l-layout-center:first").height();
    tab = $("#tabs").ligerTab({ height: bodyHeight, contextmenu: true });
    $("#nav").ligerAccordion({ height: bodyHeight - 24, speed: null });

    $("ul.menulist li").live("click", function () {
        var $menuitem = $(this);
        var url = $menuitem.attr("url");
        var tabid = $menuitem.attr("tabid");
        if (!url)
            return;
        if (!tabid) {
            tabid = tab.getNewTabid();
            if (url.indexOf('?') != -1) {
                url += "&menuno=" + $menuitem.attr("menuno");
            } else {
                url += "?menuno=" + $menuitem.attr("menuno");
            }
            $menuitem.attr("tabid", tabid);
            $menuitem.attr("url", url);
        }
        text = $menuitem.find("span:first").html();
        tab.addTabItem({ tabid: tabid, text: text, url: url });
    }).live("mouseover", function () {
        $(this).addClass("over");
    }).live("mouseout", function () {
        $(this).removeClass("over");
    });

    $("#alogoff").click(function () {
        $.ligerDialog.confirm("确认退出吗?", function (r) {
            if (r) {
                history.forward(1);
                window.location.replace('System/LogOff');
            }
        });
    });

    $("#achangepassword").click(function () {
        child = $.ligerDialog.open({ width: 400, height: 200, url: 'System/ChangePassword', isResize: true, modal: true, title: '修改密码' });
    });
    $("#pageloading").hide();
});

function addtab(url, text) {
    if (!tab) return;
    var tabid = tab.getNewTabid();
    tab.addTabItem({ tabid: tabid, url: url, text: text });
};

function overridetabe(url, text) {
    if (!tab) return;
    tab.overrideSelectedTabItem({ url: url, text: text });
};