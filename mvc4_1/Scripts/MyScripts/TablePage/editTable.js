//2016.5.24
//张赛
(function ($) {
    //options为传过来的对象值，不这么写则值不会更改
    $.fn.handleTable = function (options) {
        //1.Settings 初始化设置 
        var c = $.extend({
            "tableID": "editTable", //table容器的id
            "TableIdColumnindex": 0,  //id所在列的索引
            "operatePos": -1, //操作列的位置，-1表示默认操作列为最后一列 
            "handleFirst": false, //第一行是否作为操作的对象 
            //示例：<span class='glyphicon glyphicon-remove'>取消</span>去消链接只需把值为""
            "edit": "编辑",
            "save": "保存",
            "cancel": "取消",
            "add": "添加",
            "confirm": "确认",
            "del": "删除",
            "details": "详情",
            "zidingyiedit": "自定义修改",
            "zidingyiadd": "自定义增加",
            "editableCols": "all", //一行中能执行编辑操作的列，从0开始 。也可以数组的方式指定[2,3,4]。
            "order": ["edit", "add", "del"], //指定要显示的功能及顺序 
            "saveCallback": function (data, isSuccess) { //此isSuccess为editTable.js的内部方法,如果新增和编辑成功请调用此方法,请勿在页面中重复定义此方法
                //data: 返回的json数据id，name,value 
                //isSuccess: 方法，用于保存数据成功后，将可编辑状态变为不可编辑状态 
                //ajax请求成功（保存数据成功），才回调isSuccess函数（修改保存状态为编辑状态）
                //示例：
                //var flag = true; //flag为true的时候，才回调isSuccess函数使行的状态为正常状态否则为编辑状态（其余操作和此一样）
                //if (flag) {
                //    isSuccess();
                //    alert(data + " 保存成功");
                //} else {
                //    alert(data + " 保存失败");
                //}
                //return true; 
            },
            "addCallback": function (data, isSuccess) {
                //data: 返回的json数据id，name,value 
                //isSuccess: 方法，用于添加数据成功后，将可编辑状态变为不可编辑状态 
            },
            "delCallback": function (data, isSuccess) {
                //data中包含被删除的id
                //获取id
                //var jsonData = JSON.parse(data);
                //var id;
                //$.each(jsonData.dataList, function (index, item) {
                //    alert(item.id);
                //    id = item.id;
                //    return false;
                //});
                //isSuccess: 方法，用于删除数据成功后，将对应行删除 
            },
            "detailsCallback": function (data) {
                //data中包含选择的id
            }, "zidingyieditCallback": function (data) {
                //data中包含选择的id
            }, "zidingyiaddCallback": function (data) {
                //data中包含选择的id
            }, "cancelCallback": function (trindex) {
                //点击取消的方法，必须
            }
        }, options);


        //表格的列数 
        var colsNum = $(this).find('tr').last().children().size();
        $table = $(this);
        //2.初始化操作列，默认为最后一列，从1算起 
        if (c.operatePos == -1) {
            c.operatePos = colsNum - 1;
        }
        //3.获取所有需要被操作的行 
        var rows = $(this).find('tr');

        //这是正规设置表头没有操作列，但是选择器失效,暂时使用 $table.find("tr").eq(0).find("th").last().html("操作");
        //if (!c.handleFirst) {
        //    rows = rows.not(":eq(0)");
        //}

        //4.获取放置“操作”的列，通过operatePos获取 
        var rowsTd = [];
        var allTd = rows.children();
        for (var i = c.operatePos; i <= allTd.size(); i += colsNum) {
            if (c.handleFirst) { //如果操作第一行，就把放置操作的列内容置为空 
                allTd.eq(i).html("");
            }
            rowsTd.push(allTd.eq(i)[0]);
        }
        //6.修改设置 order 为空时的默认值 
        if (c.order.length == 0) {
            c.order = ["edit"];
        }
        //7.保存可编辑的列 
        var cols = getEditableCols();
        //8.初始化链接的构建 
        var saveLink = "", cancelLink = "", editLink = "", addLink = "", confirmLink = "", delLink = "", detailsLink = "", zidingyieditLink = "", zidingyiaddLink = "";
        initLink();
        //9.初始化操作 
        initFunc(c.order, rowsTd);
        /** 
        * 创建操作链接 
        */
        function createLink(str) {
            return "<a href=\"javascript:void(0)\" style=\"margin:0 3px\">" + str + "</a>";
        }
        /** 
        * 初始各种操作的链接 
        */
        function initLink() {
            for (var i = 0; i < c.order.length; i++) {
                switch (c.order[i]) {
                    case "edit":
                        //“编辑”链接 
                        editLink = createLink(c.edit);
                        saveLink = createLink(c.save);
                        cancelLink = createLink(c.cancel);
                        break;
                    case "add":
                        //“添加”链接 
                        addLink = createLink(c.add);
                        //“确认”链接 
                        confirmLink = createLink(c.confirm);
                        //“取消”链接 
                        cancelLink = createLink(c.cancel);
                        break;
                    case "del":
                        //“删除”链接 
                        delLink = createLink(c.del);
                        break;
                    case "details":
                        detailsLink = createLink(c.details);
                        break;
                    case "zidingyiedit":
                        zidingyieditLink = createLink(c.zidingyiedit);
                        break;
                    case "zidingyiadd":
                        zidingyiaddLink = createLink(c.zidingyiadd);
                        break;
                }
            }
        }
        /** 
        * 获取可进行编辑操作的列 
        */
        function getEditableCols() {
            var cols = c.editableCols;
            if ($.type(c.editableCols) != "array" && cols == "all") { //如果是所有列都可以编辑的话 
                cols = [];
                for (var i = 0; i < colsNum; i++) {
                    if (i != c.operatePos) { //排除放置操作的列 
                        cols.push(i);
                    }
                }
            } else if ($.type(c.editableCols) == "array") { //有指定选择编辑的列的话需要排序放置“编辑”功能的列 
                var copyCols = [];
                for (var i = 0; i < cols.length; i++) {
                    if (cols[i] != c.operatePos) {
                        copyCols.push(cols[i]);
                    }
                }
                cols = copyCols;
            }
            return cols;
        }
        /** 
        * 根据c.order参数设置提供的操作 
        * @param func 需要设置的操作 
        * @param cols 放置操作的列 
        */
        function initFunc(func, cols) {
            for (var i = 0; i < func.length; i++) {
                var o = func[i];
                switch (o) {
                    case "edit":
                        createEdit(cols);
                        break;
                    case "add":
                        createAdd(cols);
                        break;
                    case "del":
                        createDel(cols);
                        break;
                    case "details":
                        createDetails(cols);
                        break;
                    case "zidingyiedit":
                        createzidingyiedit(cols);
                        break;
                    case "zidingyiadd":
                        createzidingyiadd(cols);
                        break;
                }
            }
            if (!c.handleFirst) {
                $table.find("tr").eq(0).find("th").last().html("操作");
            }
        }
        /** 
        * 创建“编辑一行”的功能 
        * @param operateCol 放置编辑操作的列 
        */
        function createEdit(operateCol) {
            $(editLink).appendTo(operateCol).on("click", function () {
                if (replaceQuote($(this).html()) == replaceQuote(c.edit)) { //如果此时是编辑状态 
                    toSave(this); //将编辑状态变为保存状态 
                } else if (replaceQuote($(this).html()) == replaceQuote(c.save)) { //如果此时是保存状态 
                    var p = $(this).parents('tr'); //获取被点击的当前行 
                    var data = {
                        dataList: []
                    };
                    //保存修改后的数据到数据内，去除0索引的复选框和1索引的ID列 
                    for (var i = 0; i < cols.length; i++) {
                        var td = p.children().eq(cols[i]);
                        if (td.hasClass("canedit")) {
                            //选择能编辑的列
                            var thname = $("#" + c.tableID + " th").eq(cols[i]).html(); //获取更改列的名字
                            var id = p.children().eq(c.TableIdColumnindex).html(); //获取id
                            var inputValue = td.children('input').val();
                            var editData = {
                                id: id,
                                name: thname,
                                value: inputValue,
                                index: p.index()
                            };
                            data.dataList.push(editData);
                        }
                    }
                    $this = this; //此时的this表示的是 被点击的 编辑链接 
                    c.saveCallback(JSON.stringify(data), function () {
                        toEdit($this, true);
                    });
                }
            });
            var afterSave = []; //保存修改前的信息，用于用户点击取消后的数值返回操作 
            //修改为“保存”状态 
            function toSave(ele) {
                $(ele).html(c.save); //修改为“保存” 
                $(ele).after(cancelLink); //添加相应的取消保存的“取消链接” 
                $(ele).next().on('click', function () {
                    //点击取消执行
                    var trindex = toEdit(ele, false);
                    c.cancelCallback(trindex, function () {

                    });
                });
                //获取被点击编辑的当前行 tr jQuery对象 
                var p = $(ele).parents('tr');
                afterSave = []; //清空原来保存的数据 
                for (var i = 0; i < cols.length; i++) {
                    var editTr;
                    var td = p.children().eq(cols[i]);
                    if (td.hasClass("canedit")) {
                        //选择能编辑的列让其编程文本框
                        editTr = "<input type=\"text\" class=\"form-control\" value=\"" + td.html() + "\"/>";
                    } else {
                        editTr = td.html();
                    }
                    afterSave.push(td.html()); //保存未修改前的数据 
                    td.html(editTr);
                    td.unbind("click");
                }
            }
            //修改为“编辑”状态（此时，需要通过isSave标志判断是 
            // 因为点击了“保存”（保存成功）变为“编辑”状态的，还是因为点击了“取消”变为“编辑”状态的） 
            function toEdit(ele, isSave) {
                $(ele).html(c.edit);
                if (replaceQuote($(ele).next().html()) == replaceQuote(c.cancel)) {
                    $(ele).next().remove();
                }
                var p = $(ele).parents('tr');
                for (var i = 0; i < cols.length; i++) {
                    var td = p.children().eq(cols[i]);
                    if (td.hasClass("canedit")) {
                        //选取能编辑的列
                        var value;
                        if (isSave) {
                            value = td.children('input').val();
                        } else {
                            value = afterSave[i];
                        }
                        td.html(value);
                    }
                }
                return p.index();
            }
        }
        /** 
        * 创建“添加一行”的功能 
        * @param operateCol 
        */
        function createAdd(operateCol) {
            $(addLink).appendTo(operateCol).on("click", function () {
                //获取被点击“添加”的当前行 tr jQuery对象 
                var p = $(this).parents('tr'); //这是点击创建时创建按钮所在的行
                var copyRow = p.clone(); //构建新的一行 
                var input = "<input type=\"text\"/>";
                var childLen = p.children().length;
                for (var i = 0; i < childLen; i++) {
                    if (copyRow.children().eq(i).hasClass("canedit")) {
                        //能编辑的列才放置文本框
                        copyRow.children().eq(i).html("<input type=\"text\" class=\"form-control\"/>");
                    }
                }
                //最后一行是操作行 
                var last = copyRow.children().eq(c.operatePos);
                last.html("");
                p.after(copyRow);
                var confirm = $(confirmLink).appendTo(last).on("click", function () {
                    var myp = $(this).parents('tr'); //这是点击确认时要创建的内容所在的行
                    var data = {
                        dataList: []
                    };
                    for (var i = 0; i < childLen; i++) {
                        if (i != c.operatePos) {
                            if (copyRow.children().eq(i).hasClass("canedit")) {
                                //选取能编辑的列的值
                                var v = copyRow.children().eq(i).children("input").val();
                                var thname = $("#" + c.tableID + " th").eq(i).html();
                                var index = myp.index(); //行索引
                                var editData = {
                                    name: thname,
                                    value: v,
                                    index: index
                                };
                                data.dataList.push(editData);
                                copyRow.children().eq(i).html(v);
                            }
                        }
                    }
                    c.addCallback(JSON.stringify(data), function () {
                        last.html("");
                        //------------这里可以进行修改 
                        initFunc(c.order, last);
                    });
                });
                $(confirm).after(cancelLink); //添加相应的取消保存的“取消链接” 
                $(confirm).next().on('click', function () {
                    copyRow.remove();
                });
            });
        }
        /** 
        * 创建“删除一行”的功能 
        * @param operateCol 
        */
        function createDel(operateCol) {
            $(delLink).appendTo(operateCol).on("click", function () {
                if (confirm("确认删除?")) {
                    var p = $(this).parents('tr'); //这是点击确认时要创建的内容所在的行
                    var _this = this;
                    var id = p.children().eq(c.TableIdColumnindex).html();   //获取id
                    var data = {
                        dataList: []
                    };
                    var editData = {
                        id: id
                    };
                    data.dataList.push(editData);
                    c.delCallback(JSON.stringify(data), function () {
                        $(_this).parents('tr').remove();
                    });
                }
            });
        }

        /** 
        * 创建“详情”的功能 
        * @param operateCol             
        */
        function createDetails(operateCol) {
            $(detailsLink).appendTo(operateCol).on("click", function () {
                var p = $(this).parents('tr'); //这是点击确认时要创建的内容所在的行
                var _this = this;
                var id = p.children().eq(c.TableIdColumnindex).html(); //获取id
                var data = {
                    dataList: []
                };
                var editData = {
                    id: id
                };
                data.dataList.push(editData);
                c.detailsCallback(JSON.stringify(data), function () {

                });
            });
        }

        /** 
        * 创建“自定义修改”的功能 
        * @param operateCol 
        */
        function createzidingyiedit(operateCol) {
            $(zidingyieditLink).appendTo(operateCol).on("click", function () {
                var p = $(this).parents('tr'); //这是点击确认时要创建的内容所在的行
                var _this = this;
                var id = p.children().eq(c.TableIdColumnindex).html(); //获取id
                var data = {
                    dataList: []
                };
                var editData = {
                    id: id
                };
                data.dataList.push(editData);
                c.zidingyieditCallback(JSON.stringify(data), function () {

                });
            });
        }

        /** 
        * 创建“自定义增加”的功能 
        * @param operateCol 
        */
        function createzidingyiadd(operateCol) {
            $(zidingyiaddLink).appendTo(operateCol).on("click", function () {
                var p = $(this).parents('tr'); //这是点击确认时要创建的内容所在的行
                var _this = this;
                var id = p.children().eq(c.TableIdColumnindex).html(); //获取id
                var data = {
                    dataList: []
                };
                var editData = {
                    id: id
                };
                data.dataList.push(editData);
                c.zidingyiaddCallback(JSON.stringify(data), function () {

                });
            });
        }
        /** 
        * 将str中的单引号转为双引号 
        * @param str 
        */
        function replaceQuote(str) {
            return str.replace(/\'/g, "\"");
        }
    };
})(jQuery)





//调用示例
//编辑表格代码start
//var editTable = $(".editTable");
//if (editTable.length > 0) {
//    $('.editTable').handleTable({
//        "handleFirst": false,
//        "TableIdColumnindex": 1,
//        "dayunNumColumnedit": 1,
//        "cancel": " <span class='glyphicon glyphicon-remove'>取消</span> ",
//        "edit": " <span class='glyphicon glyphicon-edit'>编辑</span> ",
//        "add": " <span class='glyphicon glyphicon-plus'>增加</span> ",
//        "save": " <span class='glyphicon glyphicon-saved'>保存</span> ",
//        "confirm": " <span class='glyphicon glyphicon-ok'>确认</span> ",
//        "del": " <span class='glyphicon glyphicon-remove'>删除</span> ",
//        "details": "<span class='glyphicon glyphicon-info-sign'>详情</span>",
//        "zidingyiedit": "<span class='glyphicon glyphicon-info-sign'>我的修改</span>",
//        "zidingyiadd": "<span class='glyphicon glyphicon-info-sign'>我的增加</span>",
//        "operatePos": -1,
//        "editableCols": "all",
//        "order": ["edit", "add", "details", "del", "zidingyiedit", "zidingyiadd"],
//        "saveCallback": function (data, isSuccess) {
//            var jsonData = JSON.parse(data);
//            var verification = Verification(jsonData.dataList);
//            var flag = false;
//            if (verification) {
//                flag = Callback(data, "save");
//            }
//            if (flag) {
//                isSuccess();
//            }
//            return true;
//        },
//        "addCallback": function (data, isSuccess) {
//            var jsonData = JSON.parse(data);
//            var verification = Verification(jsonData.dataList);
//            var flag = false;
//            if (verification) {
//                flag = Callback(data, "add");
//            }
//            if (flag && verification) {
//                isSuccess();
//                var json = eval("(" + returndata + ")");
//                $('.editTable tbody tr').eq(json.index).find('input[type="checkbox"]').eq(0).attr("value", json.id); //根据返回的行号设置复选框的值
//                $('.editTable tbody tr').eq(json.index).find('td').eq(1).html(json.id); //根据返回的行号设置id列的值
//            } else {
//                var jsonData = JSON.parse(data);
//                var mindex;
//                $.each(jsonData.dataList, function (index, item) {
//                    mindex = item.index;
//                    return false;
//                });
//                var tr = $('.editTable tbody tr').eq(mindex);
//                var tds = tr.find('td');
//                for (var i = 1; i < tds.length - 1; i++) {
//                    tds.eq(i).html("<input type=\"text\" value='" + tds.eq(i).html() + "' class=\"form-control\"/>");
//                }
//            }
//        },
//        "delCallback": function (data, isSuccess) {
//            var flag = Callback(data, "del");
//            if (flag) {
//                LoadSource(1);
//            }
//        },
//        "detailsCallback": function (data) {
//            var jsonData = JSON.parse(data);
//            var id;
//            $.each(jsonData.dataList, function (index, item) {
//                alert(item.id);
//                id = item.id;
//                return false;
//            });
//            window.open("https://www.baidu.com/");
//        },
//        "zidingyieditCallback": function (data) {
//            var jsonData = JSON.parse(data);
//            var id;
//            $.each(jsonData.dataList, function (index, item) {
//                alert(item.id);
//                id = item.id;
//                return false;
//            });
//            window.open("https://www.baidu.com/");
//        },
//        "zidingyiaddCallback": function (data) {
//            var jsonData = JSON.parse(data);
//            var id;
//            $.each(jsonData.dataList, function (index, item) {
//                alert(item.id);
//                id = item.id;
//                return false;
//            });
//            window.open("https://www.baidu.com/");
//        }
//    });

//};
//编辑表格代码end


////点击列选中或取消行头中复选框状态start
//function trClickChecked(trindex) {
//    $(".editTable td").each(function () {
//        //移除所有td的click
//        $(this).unbind("click");
//    });
//    var clicktype = 0;
//    if ($("#ck_openClieckSelect").attr("checked")) {
//        //开启点击筛选
//        clicktype = 1;
//    }
//    if (isNull(trindex)) {
//        $(".editTable tr:not(:first)").each(function () {
//            s(this, clicktype);
//        });
//    } else {
//        //为新加的行注册事件
//        $('.editTable tbody tr').eq(trindex).each(function () {
//            s(this, clicktype);
//        });
//    }
//};
//function s(e, clicktype) {
//    var select = "td:not(:last)";
//    if (clicktype == 1) {
//        //开启筛选
//        select = "td:not(:first):not(:last)";
//    }
//    $(e).find(select).each(function () {
//        $(this).click(function () {
//            if (clicktype == 0) {
//                var ck = $(".editTable tr:not(:first)").eq($(this).parent('tr').index()).find('input[type="checkbox"]').eq(0); //选取点击列所在行的第一列中的复选框
//                if (ck.attr("checked")) {
//                    ck.attr("checked", false);
//                } else {
//                    ck.attr("checked", true);
//                }
//            } else {
//                var columindex = this.cellIndex;
//                $("<li><a href='javascript:void(0)' class='a_select' columindex='" + columindex + "' onclick='a_openClieckSelectClickDelThis(this," + columindex + ")'>" + $.trim($(".editTable tr:first th").eq(columindex).html()) + "=" + $.trim($(this).html()) + "</a></li>").appendTo("#ul_ClieckSelect");
//                //                        $(".editTable tr:not(:first)").each(function () {
//                //                            $(this).find("td").eq(columindex).unbind("click");
//                //                        });
//                tdClickSelectTable();
//            }
//        });
//    });

//    //把已有的条件的列的单击事件取消
//    $("#ul_ClieckSelect .a_select").each(function () {
//        var columindex = $(this).attr("columindex");
//        $(".editTable tr:not(:first)").each(function () {
//            $(this).find("td").eq(columindex).unbind("click");
//        });
//    });
//};
////为该列在次注册单击事件
//function tdClick(columindex) {
//    $(".editTable tr:not(:first)").each(function () {
//        $(this).find("td").eq(columindex).each(function () {
//            $(this).click(function () {
//                $("<li><a href='javascript:void(0)' columindex='" + columindex + "' onclick='a_openClieckSelectClickDelThis(this," + columindex + ")'>" + $.trim($(".editTable tr:first th").eq(columindex).html()) + "=" + $.trim($(this).html()) + "</a></li>").appendTo("#ul_ClieckSelect");
//                $(".editTable tr:not(:first)").each(function () {
//                    $(this).find("td").eq(columindex).unbind("click");
//                });
//            });
//        });
//    });
//};
////开启筛选后点击列查询
//function tdClickSelectTable() {
//    var ClieckSelectContext = "";
//    $("#ul_ClieckSelect .a_select").each(function () {
//        ClieckSelectContext = $(this).html() + "," + ClieckSelectContext;
//    });
//    if (!isNull(ClieckSelectContext)) {
//        ClieckSelectContext = ClieckSelectContext.substr(ClieckSelectContext, ClieckSelectContext.length - 1);
//    }
//    LoadSource(3, 1, "", ClieckSelectContext);
//};
////点击行选中或取消行头复选框end