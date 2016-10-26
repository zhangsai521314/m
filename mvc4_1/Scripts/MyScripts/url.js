$(function () {
    //加载事件开始
    var longurltext;
    var shorturltext;
    $('#SuoDuanWangZhispan').css("background-color", "#E2E2E2");
    $('#SuoDuanWangZhispan').css("border-bottom", " 1px solid white");
    $('#HuanYuanWangZhispan').css("border-bottom", " 1px solid blue");
    $('#SuoDuanDiv').css("display", "block");
    $(document).keydown(function (e) {
        if (e.keyCode == 13) {
            if ($('#SuoDuanDiv').css("display") == "block") {
                var a = $("#longurltext")[0].focus();
                $("#longurltext").focus(function () {
                    if ($('#longurltext').val().trim() == "") {
                        $('#longurltext').val("");
                    }
                })
            } else if ($('#HuanYuanDiv').css("display") == "block") {
                var b = $("#shorturltext")[0].focus();
                $("#shorturltext").focus(function () {
                    if ($('#shorturltext').val().trim() == "") {
                        $('#shorturltext').val("");
                    }
                })
            }
        }
    })
    //加载事件结束
    //单击缩短网址选项
    $('#SuoDuanWangZhispan').click(function () {
        $('#HuanYuanWangZhispan').css("background-color", "white");
        $('#SuoDuanWangZhispan').css("background-color", "#E2E2E2");
        $('#HuanYuanDiv').css("display", "none");
        $('#showShortLinkp').css("display", "none");
        $('#showLongLinkp').css("display", "none");
        $('#SuoDuanDiv').css({
            "display": "block",
            "margin": "auto"
        });
        $('#SuoDuanWangZhispan').css("border-bottom", " 1px solid white");
        $('#HuanYuanWangZhispan').css("border-bottom", " 1px solid blue");
        $('#showLongLinkp').html("");
        $('#showShortLinkp').html("");
    });
    //单击还原网址选项
    $('#HuanYuanWangZhispan').click(function () {
        $('#SuoDuanDiv').css("display", "none");
        $('#HuanYuanDiv').css({
            "display": "block",
            "margin": "auto"
        });
        $('#SuoDuanWangZhispan').css("background-color", "white");
        $('#HuanYuanWangZhispan').css("background-color", "#E2E2E2");
        $('#showShortLinkp').css("display", "none");
        $('#showLongLinkp').css("display", "none");
        $('#HuanYuanWangZhispan').css("border-bottom", " 1px solid white");
        $('#SuoDuanWangZhispan').css("border-bottom", " 1px solid blue");
        $('#showLongLinkp').html("");
        $('#showShortLinkp').html("");
    });
    //点击缩短网址按钮
    $('#SuoDuanButton').click(function () {
        longurltext = $('#longurltext').val();
        clickSuoDuanButton(longurltext);
    });
    //点击还原网址按钮
    $('#HuYuangButton').click(function () {
        shorturltext = $('#shorturltext').val();
        clickHuYuangButton(shorturltext);
    });
    //在缩短中按回车
    $('#longurltext').keydown(function (event) {
        if (event.keyCode == 13) {
            longurltext = $('#longurltext').val();
            clickSuoDuanButton(longurltext);
        }
    });
    //在还原中按回车
    $('#shorturltext').keydown(function (event) {
        if (event.keyCode == 13) {
            shorturltext = $('#shorturltext').val();
            clickHuYuangButton(shorturltext);
        }
    });
    //单击缩短网址
    function clickSuoDuanButton(longurltext) {
        if (longurltext.trim() == "") {
            alert("请输入文本");
            return;
        }
        $.ajax({
            url: "/Url/CreateShortLinkByLongLink",
            type: "post",
            dataType: "html",
            data: {
                "LongUrl": longurltext
            },
            success: function (shortUrl) {
                $('#showShortLinkp').html("短网址：" + shortUrl);
                $('#showLongLinkp').html("原网址：" + longurltext);
                $('#showShortLinkp').css("display", "block");
                $('#showLongLinkp').css("display", "block");
                $('#returnShowDiv').css("display", "block");
            }
        });
    };
    //单击还原网址
    function clickHuYuangButton(shorturltext) {
        if (shorturltext.trim() == "") {
            alert("请输入文本");
            return;
        }
        $.ajax({
            url: "/Url/GetLongLinkIDByShortLink",
            type: "post",
            dataType: "html",
            data: {
                "ShortUrl": shorturltext
            },
            success: function (LongUrl) {
                $('#showLongLinkp').html("原网址：" + LongUrl);
                $('#showLongLinkp').css("display", "block");
                $('#returnShowDiv').css("display", "block");
            }
        });
    };
});