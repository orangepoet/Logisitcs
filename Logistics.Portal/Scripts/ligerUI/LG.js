/// <reference path="jquery/jquery-1.6.2-vsdoc.js" />
/// <reference path="ligerui.all.js" />
/// <reference path="jquery/jquery.metadata.js" />


(function ($) {
    window["LG"] = {};
    LG.ajax = function (options) {
        var p = options || {};
        //  var ashxUrl = options.ashxUrl || "/Admin/User/";
        //   var url = p.url || ashxUrl + LG.param({ method: p.method });

        $.ajax({
            cache: false,
            async: true,
            url: p.url,
            data: p.data,
            dataType: 'json',
            type: 'post',
            beforeSend: function () {
                LG.loading = true;
                if (p.beforeSend)
                    p.beforeSend();
                else
                    LG.showLoading(p.loading);
            },
            complete: function () {
                LG.loading = false;
                if (p.complete)
                    p.complete();
                else
                    LG.hideLoading();
            },
            success: function (result) {
                if (!result) return;
                if (!result.IsError) {
                    if (p.success)
                        p.success(result.Data, result.Message);
                }
                else {
                    if (p.error)
                        p.error(result.Message);
                }
            },
            error: function (result, b) {
                LG.tip('发现系统错误 <BR>错误码：' + result.status);
            }
        });
    };

    LG.closeAndReloadParent = function (tabid, parentMenuNo) {
        LG.closeCurrentTab(tabid);
    };

    LG.closeCurrentTab = function (tabid) {
        if (!top.tab) return;
        if (!tabid) {
            tabid = top.tab.getSelectedTabItemID();
        }
        top.tab.removeTabItem(tabid);
    };

    LG.showLoading = function (message) {
        message = message || "正在加载中...";
        $('body').append("<div class='jloading'>" + message + "</div>");
        $.ligerui.win.mask();
    };

    LG.hideLoading = function (message) {
        $('body > div.jloading').remove();
        $.ligerui.win.unmask({ id: new Date().getTime() });
    }


    LG.showError = function (message, callback) {
        if (typeof (message) == "function" || arguments.length == 0) {
            callback = message;
            message = "操作失败!";
        }
        $.ligerDialog.error(message, '提示信息', callback);
    };

    LG.showSuccess = function (message, callback) {
        if (typeof (message) == "function" || arguments.length == 0) {
            callback = message;
            message = "操作成功!";
        }
        $.ligerDialog.success(message, '提示信息', callback);
    };



    LG.tip = function (message) {
        if (LG.wintip) {
            LG.wintip.set('content', message);
            LG.wintip.show();
        }
        else {
            LG.wintip = $.ligerDialog.tip({ content: message });
        }
        setTimeout(function () {
            LG.wintip.hide()
        }, 4000);
    };

    LG.submitForm = function (form, success) {
        if (!form)
            form = $("form:first");
        if (form.valid()) {
            form.ajaxSubmit({
                dataType: 'json',
                success: success,
                beforeSubmit: function (formData, jqForm, options) {
                    //针对复选框和单选框 处理
                    $(":checkbox,:radio", jqForm).each(function () {
                        if (!existInFormData(formData, this.name)) {
                            formData.push({ name: this.name, type: this.type, value: this.checked });
                        }
                    });
                    for (var i = 0, l = formData.length; i < l; i++) {
                        var o = formData[i];
                        if (o.type == "checkbox" || o.type == "radio") {
                            o.value = $("[name=" + o.name + "]", jqForm)[0].checked ? "true" : "false";
                        }
                    }
                },
                beforeSend: function (a, b, c) {
                    LG.showLoading('正在保存数据中...');

                },
                complete: function () {
                    LG.hideLoading();
                },
                error: function (result) {
                    LG.tip('发现系统错误 <BR>错误码：' + result.status);
                }
            });
        } else {
            LG.showInvalid();
        }
        function existInFormData(formData, name) {
            for (var i = 0, l = formData.length; i < l; i++) {
                var o = formData[i];
                if (o.name == name) return true;
            }
            return false;
        }
    };
    LG.setFormStatus = function (form, editable, parent) {
        if (editable) {
            $.metadata.setType("attr", "validate");
            LG.formvalidate(form);
            var $btnsubmit = $("#btnsubmit");
            if (!$btnsubmit) return;
            $btnsubmit.click(function () {
                LG.submitForm(form, function (data) {
                    if (!data) {
                        LG.showError("操作错误!");
                    } else {
                        LG.showSuccess("保存成功", function () {
                            //LG.closeAndReloadParent(null, menuno);
                            if (parent.child) parent.child.close();
                            if (parent.grid) parent.grid.loadData();
                        });
                    }
                });
            });
        } else {
            $("input,select,textarea", form).attr("readonly", "readonly");
            var btnsubmit = $("#btnsubmit");
            if (!btnsubmit) return;
            $("#btnsubmit").hide();
        }
    }

    LG.gridpost = function (action, data, grid, needfresh) {
        $.post(action, data, function (r) {
            if (r) {
                if (grid != undefined) grid.loadData();
                $.ligerDialog.success('操作成功', '提示'
                        , function (e) {
                            if (parent.child) parent.child.close();
                            if (needfresh) LG.refreshtab();
                        });
            } else {
                $.ligerDialog.error('操作失败');
            }
        }, 'json');
    }


    LG.showInvalid = function (validator) {
        validator = validator || LG.validator;
        if (!validator) return;
        //var message = '<div class="invalid">有错误，请检查!</div>';
        var message = '<div class="invalid">存在' + validator.errorList.length + '个字段验证不通过，请检查!</div>';
        $.ligerDialog.error(message);
    };
    LG.refreshtab = function () {
        top.tab.reload(top.tab.getSelectedTabItemID());
    }

    LG.loadForm = function (form, options, callback) {
        options = options || {};
        if (!form)
            form = $("form:first");
        var p = $.extend({
            beforeSend: function () {
                LG.showLoading('正在加载表单数据中...');
            },
            complete: function () {
                LG.hideLoading();
            },
            success: function (data) {
                var preID = options.preID || "";
                //根据返回的属性名，找到相应ID的表单元素，并赋值
                for (var p in data) {
                    var ele = $("[name=" + (preID + p) + "]", form);
                    //针对复选框和单选框 处理
                    if (ele.is(":checkbox,:radio")) {
                        ele[0].checked = data[p] ? true : false;
                    }
                    else {
                        ele.val(data[p]);
                    }
                }
                //下面是更新表单的样式
                var managers = $.ligerui.find($.ligerui.controls.Input);
                for (var i = 0, l = managers.length; i < l; i++) {
                    //改变了表单的值，需要调用这个方法来更新ligerui样式
                    var o = managers[i];
                    o.updateStyle();
                    if (managers[i] instanceof $.ligerui.controls.TextBox)
                        o.checkValue();
                }
                if (callback)
                    callback(data);
            },
            error: function (message) {
                LG.showError('数据加载失败!<BR>错误信息：' + message);
            }
        }, options);
        $.ajax(p);
    };

    LG.formvalidate = function (form, options) {
        if (typeof (form) == "string")
            form = $(form);
        else if (typeof (form) == "object" && form.NodeType == 1)
            form = $(form);

        options = $.extend({
            errorPlacement: function (lable, element) {
                if (!element.attr("id"))
                    element.attr("id", new Date().getTime());
                if (element.hasClass("l-textarea")) {
                    element.addClass("l-textarea-invalid");
                }
                else if (element.hasClass("l-text-field")) {
                    element.parent().addClass("l-text-invalid");
                }
                $(element).removeAttr("title").ligerHideTip();
                $(element).attr("title", lable.html()).ligerTip({
                    distanceX: 5,
                    distanceY: -3,
                    auto: true
                });
            },
            success: function (lable) {
                if (!lable.attr("for")) return;
                var element = $("#" + lable.attr("for"));

                if (element.hasClass("l-textarea")) {
                    element.removeClass("l-textarea-invalid");
                }
                else if (element.hasClass("l-text-field")) {
                    element.parent().removeClass("l-text-invalid");
                }
                $(element).removeAttr("title").ligerHideTip();
            }
        }, options || {});
        LG.validator = form.validate(options);
        return LG.validator;
    };

    LG.validate = function (form, options) {
        if (typeof (form) == "string")
            form = $(form);
        else if (typeof (form) == "object" && form.NodeType == 1)
            form = $(form);

        options = $.extend({
            errorPlacement: function (lable, element) {
                if (!element.attr("id"))
                    element.attr("id", new Date().getTime());
                if (element.hasClass("l-textarea")) {
                    element.addClass("l-textarea-invalid");
                }
                else if (element.hasClass("l-text-field")) {
                    element.parent().addClass("l-text-invalid");
                }
                $(element).removeAttr("title").ligerHideTip();
                $(element).attr("title", lable.html()).ligerTip({
                    distanceX: 5,
                    distanceY: -3,
                    auto: true
                });
            },
            success: function (lable) {
                if (!lable.attr("for")) return;
                var element = $("#" + lable.attr("for"));

                if (element.hasClass("l-textarea")) {
                    element.removeClass("l-textarea-invalid");
                }
                else if (element.hasClass("l-text-field")) {
                    element.parent().removeClass("l-text-invalid");
                }
                $(element).removeAttr("title").ligerHideTip();
            }
        }, options || {});
        LG.validator = form.validate(options);
        return LG.validator;
    };
})(jQuery);