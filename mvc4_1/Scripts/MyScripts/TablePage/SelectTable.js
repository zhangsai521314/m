//2016.5.24
//张赛
//不是自定义编辑和增加时候的数据验证方法名必须为Verification
//点击详情后执行的方法名必须为detailsCallback
//编辑自定义的方法名必须为zidingyieditCallback
//增加自定义的方法名必须为zidingyiaddCallback
//数据加载完成后的回调方法名必须为LoadingCompleted
//options为传过来的对象值，不这么写则值不会更改
var tableID, exiturl, geturl, detailsUrl, zidingyiExitUrl, zidingyiAddUrl, viewType, isThOrderBy, page_selectID, btnselectID,
//textselectcontext查询内容,errorColumnName验证不通过的列的名字,sortOrder(1降序，0升序)排序方式  mjson请求数据的返回对象
selecttype = 0, textselectcontext = "", sumcount = 0, pageIndex = 1, errorColumnName = "", sortOrder = 1, PowerName = "", mjson;
$.fn.selectTable = function (options) {
    //1.Settings 初始化设置 
    var zs = $.extend({
        "tableID": "editTable", //table容器的id
        "exiturl": "../ashx/EditTable.ashx", //修改操作提交的地址
        "geturl": "../ashx/GetTablePage.ashx", //获取操作的提交地址
        "detailsUrl": "/_jichu/view.aspx", //详情地址(路径写到最顶文件夹) (id参数会自动带上)
        "zidingyiExitUrl": "", //自定义修改地址 (id参数会自动带上)
        "zidingyiAddUrl": "", //自定义增加地址
        "viewType": "", //提交来源类型
        "isThOrderBy": false, //是否开启点击表头排序
        "page_selectID": "page_select", //每页数量下拉框id
        "btnselectID": "btnselect" //查询按钮ID
    }, (options));
    //------------------------------------------------------------初始加载start------------------------------------------------------------
    tableID = zs.tableID;
    exiturl = zs.exiturl;
    geturl = zs.geturl;
    detailsUrl = zs.detailsUrl;
    zidingyiExitUrl = zs.zidingyiExitUrl;
    zidingyiAddUrl = zs.zidingyiAddUrl;
    viewType = zs.viewType;
    isThOrderBy = zs.isThOrderBy;
    page_selectID = zs.page_selectID;
    btnselectID = zs.btnselectID;
    $("#" + page_selectID).change(function () {
        //每页数量下拉框改变
        LoadSource(4, 1);
    });
    $("#" + btnselectID).bind("click", function () {
        //点击查询
        LoadSource(1, 1);
    });
    ulDelThisInLiNotFirst();
    mprint();
    KeyDown();
    mexport();
    LoadSource(0, 1);
    //------------------------------------------------------------初始加载end------------------------------------------------------------
}

//打印
function mprint() {
    $("#print").click(function () {
        $("#" + tableID).jqprint();
    });
};
//下载Excel文件
function mexport() {
    $("#ExportLink").click(function () {
        $("#ExportLink").attr("href", "");
        $("#ExportLink").attr("href", geturl);

        var PageSize = $("#page_select option:selected").text();
        //获取查询选项
        var selectConditionOption = $("#selectConditionOption option:selected").text();
        //获取查询选项的内容
        textselectcontext = $("#textselectcontext").val();
        //查看分类查询是否有值，有就传给后台，让后台判断怎么查询
        var mbranchClassSelectContext = "";
        $("#span_a_branchClassSelect a").each(function () {
            if (rgb2hex($(this).css("color")) == "#ff0000") {
                mbranchClassSelectContext = $(this).html().split("[")[0];
                return false;
            }
        });
        //查看点击查询是否有值，有就传给后台，让后台判断怎么查询
        var mClieckSelectContext = "";
        $("#ul_ClieckSelect .a_select").each(function () {
            mClieckSelectContext = $(this).html() + "," + mClieckSelectContext;
        });
        if (!isNull(mClieckSelectContext)) {
            mClieckSelectContext = mClieckSelectContext.substr(mClieckSelectContext, mClieckSelectContext.length - 1);
        }
        //查看当前排序列及排序方式，有就传给后台，让后台判断怎么查询
        var $order = $(".XuanZhong").eq(0);
        var sortName = "";
        if ($order.length > 0) {
            sortName = $.trim($order.html());
            sortOrder = $order.attr("orderby");
        }
        var href = $("#ExportLink").attr("href");
        if (href.toLocaleLowerCase().indexOf("?") > 0) {
            href = href + "&export=" + 1 + "&viewType=" + viewType + "&pageindex=" + pageIndex + "&pageSize=" + PageSize + "&selectConditionOption=" + selectConditionOption + "&textselectcontext=" + textselectcontext + "&branchClassSelectContext=" + mbranchClassSelectContext + "&openClieckSelectContext=" + mClieckSelectContext + "&selectType=" + selecttype + "&sortOrder=" + sortOrder + "&sortName=" + sortName;
        } else {
            href = href + "?export=" + 1 + "&viewType=" + viewType + "&pageindex=" + pageIndex + "&pageSize=" + PageSize + "&selectConditionOption=" + selectConditionOption + "&textselectcontext=" + textselectcontext + "&branchClassSelectContext=" + mbranchClassSelectContext + "&openClieckSelectContext=" + mClieckSelectContext + "&selectType=" + selecttype + "&sortOrder=" + sortOrder + "&sortName=" + sortName;
        }
        $("#ExportLink").attr("href", href);
    });
};

//加载数据start
//selectType使用的是哪种查询方式0页面加载，1按钮搜索，2分类查询，3快速查询 ,4分页 ,5表头排序
//branchClassSelectContext分类查询的内容
//openClieckSelectContext快速查询的内容,分割
function LoadSource(selectType, Pageindex, branchClassSelectContext, openClieckSelectContext, sortName, sortOrder) {
    try {
        $("#divload").css("display", "none");
        pageIndex = Pageindex;
        selecttype = selectType;
        var PageSize = $("#page_select option:selected").text();
        //获取查询选项
        var selectConditionOption = $("#selectConditionOption option:selected").text();
        //获取查询选项的内容
        textselectcontext = $("#textselectcontext").val();
        //查看分类查询是否有值，有就传给后台，让后台判断怎么查询
        if (isNull(branchClassSelectContext)) {
            $("#span_a_branchClassSelect a").each(function () {
                if (rgb2hex($(this).css("color")) == "#ff0000") {
                    branchClassSelectContext = $(this).attr("v");
                    return false;
                }
            });
        };
        //查看点击查询是否有值，有就传给后台，让后台判断怎么查询
        if (isNull(openClieckSelectContext)) {
            openClieckSelectContext = ""
            $("#ul_ClieckSelect .a_select").each(function () {
                openClieckSelectContext = $(this).html() + "，" + openClieckSelectContext;
            });
            if (!isNull(openClieckSelectContext)) {
                openClieckSelectContext = openClieckSelectContext.substr(openClieckSelectContext, openClieckSelectContext.length - 1);
            }
        };
        //查看当前排序列及排序方式，有就传给后台，让后台判断怎么查询
        if (isNull(sortName) || isNull(sortOrder)) {
            var $order = $(".XuanZhong").eq(0);
            sortName = $.trim($order.html());
            sortOrder = $order.attr("orderby");
        }
        $.ajax({
            async: false,
            type: "POST",
            url: geturl,
            data: { export: 0, viewType: viewType, pageindex: Pageindex, pageSize: PageSize, selectConditionOption: selectConditionOption, textselectcontext: textselectcontext, branchClassSelectContext: branchClassSelectContext, openClieckSelectContext: openClieckSelectContext, selectType: selectType, sortOrder: sortOrder, sortName: sortName },
            dataType: "json", //可以解析正常的引号
            success: function (source) {
                if (!isNull(source)) {
                    //mjson = JSON.parse(mjson);不能解析正常的引号
                    mjson = source;
                    if (mjson.Status == 1) {
                        PowerName = mjson.PowerName;
                        sumcount = mjson.sumcount;
                        $("#span_a_branchClassSelect").html(mjson.BranchClassSelect); //快速定位
                        branchClassSelect(branchClassSelectContext);
                        var $editTable = $("#" + tableID);
                        $editTable.html(mjson.Context); //查询结果数据
                        if (isThOrderBy) {
                            ThOrderBy();
                        }
                        $("#span_sumPage").html(mjson.TotalPages);
                        //填充表格的第一行第一列，定义自己想要的内容
                        if (mjson.IsAddTableFirstOperation) {
                            $("#" + tableID + " tr:first th:first").append($(mjson.Table_TdFirstContext.replace(/@@/g, "'")));
                        }
                        //头行的操作事件绑定start
                        HeadOperation();
                        //头行的操作事件绑定end
                        //编辑表格代码start
                        $("#divSource").css("display", "block");
                        //编辑表格代码end
                        trClickChecked();
                        if (mjson.IsAddTableLastOperation && $("#" + tableID + " tr").eq(1).find("td span").html() != "查询结果为空!!") {
                            $("#" + tableID).handleTable({
                                "tableID": tableID,
                                "handleFirst": false,
                                "TableIdColumnindex": mjson.TableIdColumnindex,
                                "cancel": " <span class='glyphicon glyphicon-remove'>取消</span> ",
                                "edit": " <span class='glyphicon glyphicon-edit'>编辑</span> ",
                                "add": " <span class='glyphicon glyphicon-plus'>增加</span> ",
                                "save": " <span class='glyphicon glyphicon-saved'>保存</span> ",
                                "confirm": " <span class='glyphicon glyphicon-ok'>确认</span> ",
                                "del": " <span class='glyphicon glyphicon-remove'>删除</span> ",
                                "details": "<span class='glyphicon glyphicon-info-sign'>详情</span>",
                                "zidingyiedit": "<span class='glyphicon glyphicon-info-sign'>编辑</span>",
                                "zidingyiadd": "<span class='glyphicon glyphicon-info-sign'>增加</span>",
                                "operatePos": -1,
                                "editableCols": "all",
                                "order": mjson.OperationContext,
                                "saveCallback": function (data, isSuccess) {
                                    var jsonData = JSON.parse(data);
                                    var verification = true;
                                    if (typeof (Verification) == "function") {
                                        verification = Verification(jsonData.dataList, "editOrAdd");
                                    }
                                    if (verification) {
                                        var json = Callback(data, "save");
                                        if (!isNull(json) && json.Status == 1) {
                                            isSuccess();
                                            trClickChecked(jsonData.dataList[0].index);
                                        }
                                        else {
                                            alert("保存失败或验证不同过,请重试");
                                        }
                                    } else {
                                        alert("验证失败");
                                    }
                                },
                                "addCallback": function (data, isSuccess) {
                                    var jsonData = JSON.parse(data);
                                    var verification = true;
                                    if (typeof (Verification) == "function") {
                                        verification = Verification(jsonData.dataList, "editOrAdd");
                                    }
                                    if (verification) {
                                        var json = Callback(data, "add");
                                        if (!isNull(json) && json.Status == 1) {
                                            isSuccess();
                                            $("#" + tableID + " tbody tr").eq(json.index).find('input[type="checkbox"]').eq(0).attr("value", json.id); //根据返回的行号设置复选框的值
                                            $("#" + tableID + " tbody tr").eq(json.index).find('td').eq(1).html(json.id); //根据返回的行号设置id列的值
                                            //trClickChecked(json.index);
                                            LoadSource(0, 1);
                                        } else {
                                            var jsonData = JSON.parse(data);
                                            var mindex;
                                            $.each(jsonData.dataList, function (index, item) {
                                                mindex = item.index;
                                                return false;
                                            });
                                            var tds = $("#" + tableID + " tbody tr").eq(mindex).find('td');
                                            for (var i = 2; i < tds.length - 1; i++) {//去除0索引的复选框和1索引的ID列
                                                tds.eq(i).html("<input type=\"text\" value='" + tds.eq(i).html() + "' class=\"form-control\"/>");
                                            }
                                            alert("保存失败,请重试");
                                        }
                                    } else {
                                        //此处理在验证方法里执行
                                        //var jsonData = JSON.parse(data);
                                        //var mindex;
                                        //$.each(jsonData.dataList, function (index, item) {
                                        //    mindex = item.index;
                                        //    return false;
                                        //});
                                        //var tds = $("#" + tableID + " tbody tr").eq(mindex).find('td');
                                        //for (var i = 2; i < tds.length - 1; i++) {//去除0索引的复选框和1索引的ID列
                                        //    tds.eq(i).html("<input type=\"text\" value='" + tds.eq(i).html() + "' class=\"form-control\"/>");
                                        //}
                                        alert("验证失败");
                                    }
                                },
                                "delCallback": function (data, isSuccess) {
                                    var json = Callback(data, "del");
                                    if (!isNull(json) && json.Status == 1) {
                                        delLoadSource(1);
                                    }
                                    else {
                                        alert("删除失败,请重试");
                                    }
                                },
                                "detailsCallback": function (data) {
                                    if (typeof (detailsCallback) == "function") {
                                        //详情自定义的回调
                                        detailsCallback(JSON.parse(data));
                                    } else {
                                        var realPath = getRealPath();
                                        var jsonData = JSON.parse(data);
                                        var id;
                                        $.each(jsonData.dataList, function (index, item) {
                                            id = item.id;
                                            return false;
                                        });
                                        if (detailsUrl.toLocaleLowerCase().indexOf("?") > 0) {
                                            window.open(realPath + detailsUrl + "&ID=" + id);
                                        } else {
                                            window.open(realPath + detailsUrl + "?ID=" + id);
                                        }
                                    }
                                },
                                "zidingyieditCallback": function (data) {
                                    if (typeof (zidingyieditCallback) == "function") {
                                        //编辑自定义的回调
                                        zidingyieditCallback(JSON.parse(data));
                                    } else {
                                        var realPath = getRealPath();
                                        var jsonData = JSON.parse(data);
                                        var id;
                                        $.each(jsonData.dataList, function (index, item) {
                                            id = item.id;
                                            return false;
                                        });
                                        if (zidingyiExitUrl.toLocaleLowerCase().indexOf("?") > 0) {
                                            var url = realPath + zidingyiExitUrl + "&ID=" + id
                                            window.location.href = url;
                                        } else {
                                            var url = realPath + zidingyiExitUrl + "?ID=" + id;
                                            window.location.href = url;
                                        }
                                    }
                                },
                                "zidingyiaddCallback": function (data) {
                                    if (typeof (zidingyiaddCallback) == "function") {
                                        //增加自定义的回调
                                        zidingyiaddCallback(JSON.parse(data));
                                    } else {
                                        var url = getRealPath() + zidingyiAddUrl;
                                        window.location.href = url;
                                    }
                                }, "cancelCallback": function (trindex) {
                                    trClickChecked(trindex);
                                }
                            });
                        } else {
                            //不显示操作列
                        };
                        //分页代码开始
                        if ($("#" + tableID + " tr").eq(1).find("td span").html() != "查询结果为空!!") {
                            laypage({
                                cont: 'page', //容器。
                                pages: mjson.TotalPages, //通过后台拿到的总页数  
                                curr: Pageindex, //初始化当前页  
                                skin: '#FF4B4B', //皮肤颜色
                                groups: 4, //连续显示分页数  
                                skip: true, //是否开启跳页  
                                first: '首页', //若不显示，设置false即可  
                                last: '尾页', //若不显示，设置false即可  
                                jump: function (e, firlst) { //触发分页后的回
                                    if (!firlst) {
                                        if (textselectcontext != $("#textselectcontext").val()) {
                                            //判断查询条件是否改变，改变则查询第一页
                                            LoadSource(4, 1);
                                        } else {
                                            LoadSource(4, e.curr);
                                        }
                                    }
                                }
                            });
                        };
                        if (mjson.TotalPages > 0) {
                            $("#ul_Pagecount").css("display", "block");
                        } else {
                            var $trempty = $("<tr><td id='td_selectNull' style='text-align: center;' colspan='" + $("#" + tableID + " tr:first th").length + "'> <span style='color:red; font-size:20px'>查询结果为空!!</td></tr>").appendTo($("#" + tableID));
                            $("#ul_Pagecount").css("display", "none");
                        };
                        //分页代码end
                    } else if (mjson.Status == 2) {

                        $("#divloadContext").html("抱歉，恁暂无权限！");
                        $("#divSource").css("display", "none");
                        $("#divload").css("display", "block");
                    }
                    else {
                        $("#divloadContext").html("数据为空，请刷新重试");
                        $("#divSource").css("display", "none");
                        $("#divload").css("display", "block");
                    }
                } else {
                    $("#divloadContext").html("加载失败，请刷新重试");
                    $("#divSource").css("display", "none");
                    $("#divload").css("display", "block");
                };
                if (!isNull(mjson.Js)) {
                    eval(mjson.Js); //执行后台返回的js
                }
                //数据加载完成后的回调
                if (typeof (LoadingCompleted) == "function") {
                    LoadingCompleted(PowerName);
                }
            },
            error: function () {
                $("#divloadContext").html("网络请求错误，请刷新重试");
                $("#divSource").css("display", "none");
                $("#divload").css("display", "block");
            }
        });
    } catch (ex) {
        $("#divloadContext").html("解析出错，请刷新重试");
        $("#divSource").css("display", "none");
        $("#divload").css("display", "block");
        alert(ex);
    }
};
//加载数据end

//editTable处理完的回调start
function Callback(data, type) {
    var backdata;
    try {
        $.ajax({
            async: false,
            type: "POST",
            url: exiturl,
            data: { viewType: viewType, data: data, type: type },
            dataType: "json", //可以解析正常的引号
            success: function (source) {
                backdata = source;
            },
            error: function () {
                backdata = "";
            }
        });
    } catch (ex) {
        backdata = "";
    }
    return backdata;
};
//editTable处理完的回调end

//点击表头排序
function ThOrderBy() {
    //选取表第一行中除了第一列和最后一列中所有class=canorder的th
    // tr:first th:not(:first):not(:last)[class='canorder']
    $("#" + tableID + " tr:first th[class='canorder']").each(function () {
        var $e = $(this);
        var sortName;
        $e.attr("orderby", sortOrder);
        $e.click(function () {
            $("#" + tableID + " tr：first").each(function () {
                $(this).removeClass("XuanZhong");
            });
            $e.addClass("XuanZhong");
            if ($e.attr("orderby") == 1) {
                //升序
                $e.attr("orderby", "0");
                sortOrder = 0;
            } else {
                //降序
                $e.attr("orderby", "1");
                sortOrder = 1;
            }
            sortName = $.trim($e.html());
            LoadSource(5, 1, "", "", sortName, sortOrder)
        });
    });
};


//注册开启筛选
function ulDelThisInLiNotFirst() {
    $("#ck_openClieckSelect").change(function () {
        if ($("#ck_openClieckSelect").attr("checked")) {
            trClickChecked();
        } else {
            $("#ul_ClieckSelect li:not(:first)").remove();
            LoadSource(0, 1);
        }
    });
};

//点击筛选项，将此项删除(拼接时事件加不上去才这样循环加事件)
function a_openClieckSelectClickDelThis() {
    $(".a_select").each(function (index, item) {
        $(item).click(function () {
            $(this).parent("li").remove();
            tdClickSelectTable(); //执行查询
        });
    });
}

//点击a标签分类的快速查询
function branchClassSelect(a_select) {
    var $a_select = $("#span_a_branchClassSelect a");
    $a_select.each(function (index, item) {
        $(this).click(function () {
            if ($(this).attr("v") == a_select) {
                //二次点击取消该分类查询
                $(this).css("color", "");
                LoadSource(0, 1);
            } else {
                LoadSource(2, 1, $(this).attr("v"));
            }
        });
        //设置重新加载后的样式
        $a_select.each(function () {
            if ($(this).attr("v") == a_select) {
                $(this).css("color", "#ff0000");
            }
        });
    });
};

//转换rgb值为#xxx
function rgb2hex(rgb) {
    rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
    function hex(x) {
        return ("0" + parseInt(x).toString(16)).slice(-2);
    }
    return "#" + hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]);
}

//点击列选中或取消行头中复选框状态或点击列快速查询start
function trClickChecked(trindex, isunbindclick) {
    if (!isunbindclick) {
        var clicktype = 0;
        if ($("#ck_openClieckSelect").attr("checked")) {
            //开启点击筛选
            clicktype = 1;
        }
        if (isNull(trindex)) {
            $("#" + tableID + " tr:not(:first)").each(function () {
                s(this, clicktype);
            });
        } else {
            //为新加的行注册事件
            $("#" + tableID + " tbody tr").eq(trindex).each(function () {
                s(this, clicktype);
            });
        }
    } else {
        //移除本行td的click
        $("#" + tableID + " tr").eq(trindex).find("td").each(function () {
            $(this).unbind("click");
        });
    }
};
function s(e, clicktype) {
    var select = ".canclickSelectedCK"; //td:not(:last)
    if (clicktype == 1) {
        //开启筛选
        //本行除了第一个td和最后一个td中class=canclickselect的（因为不能查找的列的class都不带canclickselect，可以缩写只查本行中class=canclickselect）
        select = ".canclickselect"; //td:not(:first):not(:last)
    };
    $(e).find(select).each(function () {
        if ($(this).find("input[type=text]").length <= 0) {
            //有文本框的td不注册单击方法
            $(this).click(function () {
                if (clicktype == 0) {
                    var ck = $("#" + tableID + " tr:not(:first)").eq($(this).parent('tr').index()).find('input[type="checkbox"]').eq(0); //选取点击列所在行的第一列中的复选框
                    if (ck.attr("checked")) {
                        ck.attr("checked", false);
                    } else {
                        ck.attr("checked", true);
                    }
                } else {
                    var columindex = this.cellIndex;
                    $("<li><a href='javascript:void(0)' class='a_select'  columindex='" + columindex + "'>" + $.trim($("#" + tableID + " tr:first th").eq(columindex).html()) + "=" + $(this).html() + "</a></li>").appendTo("#ul_ClieckSelect");
                    a_openClieckSelectClickDelThis();
                    tdClickSelectTable(); //执行查询
                }
            });
        }
    });

    //把已有的条件的列的单击事件取消
    $("#ul_ClieckSelect .a_select").each(function () {
        var columindex = $(this).attr("columindex");
        $("#" + tableID + " tr:not(:first)").each(function () {
            $(this).find("td").eq(columindex).unbind("click");
        });
    });
};
//        //为该列在次注册单击事件
//        function tdClick(columindex) {
//            $("#" + tableID + " tr:not(:first)").each(function () {
//                $(this).find("td").eq(columindex).each(function () {
//                    $(this).click(function () {
//                        $("<li><a href='javascript:void(0)' class='a_select' columindex='" + columindex + "'>" + $.trim($("#" + tableID + " tr:first th").eq(columindex).html()) + "=" + $(this).html() + "</a></li>").appendTo("#ul_ClieckSelect");
//                        a_openClieckSelectClickDelThis();
//                        $("#" + tableID + " tr:not(:first)").each(function () {
//                            $(this).find("td").eq(columindex).unbind("click");
//                        });
//                    });
//                });
//            });
//        };
//开启筛选后点击列查询或者是当删除一个查询条件的时候执行查询
function tdClickSelectTable() {
    var ClieckSelectContext = "";
    $("#ul_ClieckSelect .a_select").each(function () {
        ClieckSelectContext = $(this).html() + "，" + ClieckSelectContext;
    });
    if (!isNull(ClieckSelectContext)) {
        ClieckSelectContext = ClieckSelectContext.substr(ClieckSelectContext, ClieckSelectContext.length - 1);
    }
    LoadSource(3, 1, "", ClieckSelectContext);
};
//点击行选中或取消行头复选框end

//全选
function btnSelect(e) {
    if ($(e).attr("value") == "全选") {
        $(e).attr("value", "取消全选");
        $(".cktablesource").each(function () {
            $(this).attr("checked", true);
        });
    } else {
        $(e).attr("value", "全选");
        $(".cktablesource").each(function () {
            $(this).attr("checked", false);
        });
    }
};

//反选,绑定的id
function btnReverseSelection() {
    $(".cktablesource").each(function () {
        if ($(this).attr("checked")) {
            $(this).attr("checked", false);
        } else {
            $(this).attr("checked", true);
        }
    });
};

////批量删除,绑定的id
//function btnBatchDelete() {
//    var id = "";
//    $(".cktablesource").each(function () {
//        if ($(this).attr("checked")) {
//            id = $(this).attr("value") + "," + id;
//        }
//    });
//    if (!isNull(id)) {
//        if (!confirm("是否批量移除已选择的数据？")) return;
//        id = id.substr(id, id.length - 1);
//        var count = [];
//        count = id.split(",");
//        $.ajax({
//            async: false,
//            type: "POST",
//            url: exiturl,
//            data: { viewType: viewType, data: id, type: "BatchDelete" },
//            dataType: "json", //可以解析正常的引号
//            success: function (source) {
//                delLoadSource(count.length);
//            },
//            error: function () {
//                return false;
//            }
//        });
//    } else {
//        alert("请选择删除项");
//        return;
//    }
//    return false;
//};

////批量审核,绑定的id
//function btnBatchExamine(columnName) {
//    var id = "";
//    $(".cktablesource").each(function () {
//        if ($(this).attr("checked")) {
//            id = $(this).attr("value") + "," + id;
//        }
//    });
//    if (!isNull(id)) {
//        if (!confirm("是否批量审核已选择的数据？")) return;
//        id = id.substr(id, id.length - 1);
//        $.ajax({
//            async: false,
//            type: "POST",
//            url: exiturl,
//            data: { viewType: viewType, columnName: columnName, data: id, type: "BatchExamine" },
//            dataType: "json", //可以解析正常的引号
//            success: function (source) {
//                LoadSource(0, pageIndex);
//            },
//            error: function () {
//                return false;
//            }
//        });
//    } else {
//        alert("请选择审核项");
//        return;
//    }
//    return false;
//};

////批量锁定,绑定的id
//function btnBatchLocking(columnName) {
//    var id = "";
//    $(".cktablesource").each(function () {
//        if ($(this).attr("checked")) {
//            id = $(this).attr("value") + "," + id;
//        }
//    });
//    if (!isNull(id)) {
//        if (!confirm("是否批量锁定已选择的数据？")) return;
//        id = id.substr(id, id.length - 1);
//        $.ajax({
//            async: false,
//            type: "POST",
//            url: exiturl,
//            data: { viewType: viewType, columnName: columnName, data: id, type: "BatchLocking" },
//            dataType: "json", //可以解析正常的引号
//            success: function (source) {
//                LoadSource(0, pageIndex);
//            },
//            error: function () {
//                return false;
//            }
//        });
//    } else {
//        alert("请选择锁定项");
//        return;
//    }
//    return false;
//};

//根据id的批量操作
function btnBatch(columnName, confirmMsg, alertMsg, type) {
    var id = "";
    $(".cktablesource").each(function () {
        if ($(this).attr("checked")) {
            id = $(this).attr("value") + "," + id;
        }
    });
    if (!isNull(id)) {
        if (!confirm(confirmMsg)) return;
        id = id.substr(id, id.length - 1);
        $.ajax({
            async: false,
            type: "POST",
            url: exiturl,
            data: { viewType: viewType, columnName: columnName, data: id, type: type },
            dataType: "json", //可以解析正常的引号
            success: function (source) {
                LoadSource(0, pageIndex);
            },
            error: function () {
                return false;
            }
        });
    } else {
        alert(alertMsg);
        return;
    }
    return false;
};

//批量指定,（ _jichu/jxc_Select_cplb.aspx 中使用）
function jxc_Select_cplb_btnBatchAppoint() {
    var dearid = window.document.location.href.split("=")[1];
    var productTypeID = "", productTypeName = "", mtdindex;
    var mytr = $("#" + tableID + " tr");
    $("#" + tableID + " tr:not(first) th").each(function (index, item) {
        if ($.trim($(item).html()) == "产品类别") {
            mtdindex = index;
            return false;
        }
    });
    $(".cktablesource").each(function () {
        if ($(this).attr("checked")) {
            productTypeID = $(this).attr("value") + "," + productTypeID;
            productTypeName = $.trim(mytr.eq($(this).parents("tr").index() + 1).find("td").eq(mtdindex).html()) + "," + productTypeName;
        }
    });
    if (!isNull(productTypeID)) {
        if (!confirm("是否批量指定已选择的数据？")) return;
        productTypeID = productTypeID.substr(productTypeID, productTypeID.length - 1);
        if (!isNull(productTypeName)) {
            productTypeName = productTypeName.substr(productTypeName, productTypeName.length - 1);
        }
        $.ajax({
            async: false,
            type: "POST",
            url: exiturl,
            data: { viewType: viewType, data: productTypeID, dearid: dearid, productTypeName: productTypeName, type: "jxc_Select_cplb_BatchAppoint" },
            dataType: "json",
            success: function (source) {
                delLoadSource(source.InfluenceCount);
                if (typeof (btnBatchAppointCallBack) == "function") {
                    btnBatchAppointCallBack(source);
                }
            },
            error: function () {
                if (typeof (btnBatchAppointCallBack) == "function") {
                    btnBatchAppointCallBack(0);
                }
            }
        });
    } else {
        alert("请选择指定项");
        return;
    }
    return false;
};

//批量指定,（_jichu/jxshtgz_Edit_Select_cplb.aspx中使用）
function jxshtgz_Edit_Select_cplb_btnBatchAppoint() {
    var ContractRulesID = window.document.location.href.split("=")[1];
    var productTypeID = "", productTypeName = "", mtdindex;
    var mytr = $("#editTable tr");
    $("#editTable tr:not(first) th").each(function (index, item) {
        if ($.trim($(item).html()) == "产品类别") {
            mtdindex = index;
            return false;
        }
    });
    $(".cktablesource").each(function () {
        if ($(this).attr("checked")) {
            productTypeID = $(this).attr("value") + "," + productTypeID;
            productTypeName = $.trim(mytr.eq($(this).parents("tr").index() + 1).find("td").eq(mtdindex).html()) + "," + productTypeName;
        }
    });
    if (!isNull(productTypeID)) {
        if (!confirm("是否批量指定已选择的数据？")) return;
        productTypeID = productTypeID.substr(productTypeID, productTypeID.length - 1);
        if (!isNull(productTypeName)) {
            productTypeName = productTypeName.substr(productTypeName, productTypeName.length - 1);
        }
        $.ajax({
            async: false,
            type: "POST",
            url: exiturl,
            data: { viewType: viewType, data: productTypeID, ContractRulesID: ContractRulesID, productTypeName: productTypeName, type: "jxshtgz_Edit_Select_cplb_btnBatchAppoint" },
            dataType: "json",
            success: function (source) {
                delLoadSource(source.InfluenceCount);
                //window.location = "jxshtgz_Edit_Select_cplb.aspx?id=" + dearid;
                if (typeof (btnBatchAppointCallBack) == "function") {
                    btnBatchAppointCallBack(source);
                }
            },
            error: function () {
                if (typeof (btnBatchAppointCallBack) == "function") {
                    btnBatchAppointCallBack(0);
                }
            }
        });
    } else {
        alert("请选择指定项");
        return;
    }
    return false;
};

//批量删除已指定的规则（jxshtgz_edit_select_cplb_2）
function jxshtgz_Edit_Select_cplb_2_batchdelete() {
    var dearid = window.document.location.href.split("=")[1];
    var productTypeID = "", productTypeName = "", mtdindex;
    var mytr = $("#editTable tr");
    $("#editTable tr:not(first) th").each(function (index, item) {
        if ($.trim($(item).html()) == "产品类别") {
            mtdindex = index;
            return false;
        }
    });
    $(".cktablesource").each(function () {
        if ($(this).attr("checked")) {
            productTypeID = $(this).attr("value") + "," + productTypeID;
            productTypeName = $.trim(mytr.eq($(this).parents("tr").index() + 1).find("td").eq(mtdindex).html()) + "," + productTypeName;
        }
    });
    if (!isNull(productTypeID)) {
        if (!confirm("是否批量删除已选择的数据？")) return;
        productTypeID = productTypeID.substr(productTypeID, productTypeID.length - 1);
        if (!isNull(productTypeName)) {
            productTypeName = productTypeName.substr(productTypeName, productTypeName.length - 1);
        }
        $.ajax({
            async: false,
            type: "POST",
            url: exiturl,
            data: { viewType: viewType, data: productTypeID, dearid: dearid, productTypeName: productTypeName, type: "jxshtgz_Edit_Select_cplb_2_batchdelete" },
            dataType: "json",
            success: function (source) {
                delLoadSource(source.InfluenceCount);
                if (typeof (btnBatchAppointCallBack) == "function") {
                    btnBatchAppointCallBack(source);
                }
            },
            error: function () {
                if (typeof (btnBatchAppointCallBack) == "function") {
                    btnBatchAppointCallBack(0);
                }
            }
        });
    } else {
        alert("请选择删除项");
        return;
    }
    return false;
};

//头部操作方法
function HeadOperation() {
    $("#button_quanxuan").click(function () {
        btnSelect(this);
    });
    $("#btnReverseSelection").click(function () {
        btnReverseSelection();
    });
    $("#btnBatchDelete").click(function () {
        //        btnBatchDelete();
        btnBatch($("#btnBatchDelete").attr("columnName"), "是否删除选中项", "请选择删除项", "BatchDelete");
    });
    $("#btnBatchExamine").click(function () {
        //        btnBatchExamine($("#btnBatchExamine").attr("columnName"));
        btnBatch($("#btnBatchExamine").attr("columnName"), "是否审核选中项", "请选择审核项", "BatchExamine");
    });
    $("#btnBatchLocking").click(function () {
        //        btnBatchLocking($("#btnBatchLocking").attr("columnName"));
        btnBatch($("#btnBatchLocking").attr("columnName"), "是否锁定选中项", "请选择锁定项", "BatchLocking");
    });
    $("#jxc_Select_cplb_btnBatchAppoint").click(function () {
        jxc_Select_cplb_btnBatchAppoint();
    });
    $("#jxshtgz_Edit_Select_cplb_btnBatchAppoint").click(function () {
        jxshtgz_Edit_Select_cplb_btnBatchAppoint();
    });
    $("#jxshtgz_Edit_Select_cplb_2_batchdelete").click(function () {
        jxshtgz_Edit_Select_cplb_2_batchdelete();
    });
    $("#btnBatchRelieveLocking").click(function () {
        btnBatch($("#btnBatchRelieveLocking").attr("columnName"), "是否解除选中项", "请选择解除项", "BatchRelieveLocking");
    });
};

//删除后判断应该加载当前页还是上一页
function delLoadSource(delcount) {
    PageSize = $("#page_select option:selected").text();
    sumcount = sumcount - delcount;
    var ye;
    if (sumcount % PageSize > 0) {
        //向上取整
        ye = Math.ceil(sumcount / PageSize);
    }
    else {
        //向下取整
        ye = Math.floor(sumcount / PageSize);
    }
    if (pageIndex > ye) {
        pageIndex = pageIndex - 1;
    }
    LoadSource(4, pageIndex);
};

//查询数据为空时，点击新增。生成文本框
function CreateText(trindex) {
    var $inserttr = $("#editTable tbody tr").eq(trindex);
    //循环表头列
    $("#td_selectNull").remove();
    $("#editTable tr:first th").each(function (index, item) {
        if (isNull($(item).html())) {
            if ($inserttr.find("td").eq(index).length <= 0) {
                $("<td></td>").appendTo($inserttr);
            }
        } else
            if ($(item).attr("class") == "canorder") {
                if ($inserttr.find("td").eq(index).length > 0) {
                    //行下面已有此索引的td。,取出值给文本框
                    $inserttr.find("td").eq(index).html("<input type='text' key=" + $(item).html() + " class='text_insert' value='" + $inserttr.find("td").eq(index).html() + "' />");
                } else {
                    $("<td class='canedit'><input type='text' key=" + $(item).html() + " class='text_insert' /></td>").appendTo($inserttr);
                }
            } else if ($(item).html() == "操作") {
                if ($inserttr.find("td").eq(index).length <= 0) {
                    $("<td><a href='javascript:void(0)' onclick='a_insert()'>增加</a></td>").appendTo($inserttr);
                }
            } else if ($(item).html() == "ID") {
                //隐藏的头部列
                if ($inserttr.find("td").eq(index).length <= 0) {
                    $("<td style='display:none'></td>").appendTo($inserttr);
                }
            }
    });
};

//使光标总是定位在文本的最后start
$.fn.setCursorPosition = function (position) {
    if (this.lengh == 0) return this;
    return $(this).setSelection(position, position);
}
$.fn.setSelection = function (selectionStart, selectionEnd) {
    if (this.lengh == 0) return this;
    input = this[0];

    if (input.createTextRange) {
        var range = input.createTextRange();
        range.collapse(true);
        range.moveEnd('character', selectionEnd);
        range.moveStart('character', selectionStart);
        range.select();
    } else if (input.setSelectionRange) {
        input.focus();
        input.setSelectionRange(selectionStart, selectionEnd);
    }

    return this;
}
$.fn.focusEnd = function () {
    this.setCursorPosition(this.val().length);
}
//使光标总是定位在文本的最后end

//判断值是否为空
function isNull(str) {
    if (str == null || typeof (str) == "undefined" || $.trim(str) == "") {
        return true;
    } else {
        return false;
    }
};

//键盘按下事件
function KeyDown() {
    $(document).keydown(function (event) {
        if (event.keyCode == 13) {
            var $focused = $(document.activeElement); //获取当前获取焦点的对象
            if ($focused.length > 0) {
                if ($focused.attr("id") == "textselectcontext") {
                    LoadSource(1, 1);
                }
            }
        }
    });
};

//获取根路径
function getRealPath() {
    //获取当前网址，如： http://localhost:8083/myproj/view/my.jsp
    var curWwwPath = window.document.location.href;
    //获取主机地址之后的目录，如： myproj/view/my.jsp
    var pathName = window.document.location.pathname;
    var pos = curWwwPath.indexOf(pathName);
    //获取主机地址，如： http://localhost:8083
    var localhostPaht = curWwwPath.substring(0, pos);
    //获取带"/"的项目名，如：/myproj
    var projectName = pathName.substring(0, pathName.substr(1).indexOf('/') + 1);

    //得到了 http://localhost:8083/myproj
    var realPath = localhostPaht + projectName;

    //本地测试地址，发布需要注销
    realPath = realPath + "/_jichu";
    //本地测试地址，发布需要注销

    return realPath;
};



//提交后台之前的验证start(例子)
// var isbool = 1;
// var trindex;
// $.each(data, function (index, item) {
//     trindex = item.index;
//     if (item.name == "部门") {
//         if (isNull(item.value)) {
//             isbool = 0;
//             errorColumnName = item.name;
//             return false;
//         }
//     };
// });
// if (isbool == 0) {
//     //验证不通过的处理
//     var tds = $('#editTable tbody tr').eq(trindex).find('td');
//     for (var i = 2; i < tds.length - 1; i++) {
//         if (tds.eq(i).find("input").length > 0) {
//             tds.eq(i).html(tds.eq(i).html());
//         } else {
//             tds.eq(i).html("<input type=\"text\" value='" + tds.eq(i).html() + "' class=\"form-control\"/>");
//         }
//     }
//     var d = 2;
//     $('#editTable tr').eq(0).find("th").each(function (i) {
//         if ($(this).html() == errorColumnName) {
//             d = $(this).index();
//             return false;
//         }
//     })
//     $($('#editTable tbody tr').eq(trindex).find('input[type="text"]').eq(d - 2)).focusEnd();
//     return false;
// } else {
//     return true;
// }
//提交后台之前的验证end
