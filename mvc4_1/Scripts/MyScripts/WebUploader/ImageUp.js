var applicationPath = window.applicationPath === "" ? "" : window.applicationPath || "../../";
var minImageCount = 0, maxImageCount = 100, thumbnailWidth = 90, thumbnailHeight = 90, fileSingleSizeLimit = 1; FileType = "businesslicenseimage";

//上传点击按钮ID，存放图片容器ID，隐藏域Name，图片类别，最小数量，最大数量， 单个文件大小，缩略图宽度，缩略图高度
function UpImage(UpbuttonID, FileLisID, hiddenIdAndName, FileType, minImageCount, maxImageCount, fileSingleSizeLimit, thumbnailWidth, thumbnailHeight) {
    minImageCount = minImageCount; //最少选择两张图片
    maxImageCount = maxImageCount; //最多选择的图片数量
    hiddenIdAndName = hiddenIdAndName;
    var $ = jQuery,
        $list = $('#' + FileLisID),
    // 优化retina, 在retina下这个值是2
        ratio = window.devicePixelRatio || 1,
    // 缩略图大小
        thumbnailWidth = thumbnailWidth * ratio,
        thumbnailHeight = thumbnailHeight * ratio,

    // Web Uploader实例
        uploader;
    uploader = WebUploader.create({
        // 选完文件后，是否自动上传。
        auto: true,
        //限制图片的数量
        fileNumLimit: maxImageCount,

        //设定单个文件大小
        fileSingleSizeLimit: 1024 * 1024 * fileSingleSizeLimit,

        disableGlobalDnd: true,
        // swf文件路径                
        swf: '../js/WebUploader/Uploader.swf',

        // 文件接收服务端。                       
        server: '../ashx/WebUploaderImage.ashx?Type=addimage&fileType=' + FileType,

        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: '#' + UpbuttonID,

        //只允许选择图片
        accept: {
            title: 'Images',
            extensions: 'gif,jpg,jpeg,bmp,png',
            mimeTypes: 'image/*'
        }
    });

    //用户的操作超出限制
    uploader.on('error', function (handler) {
        switch (handler) {
            case "Q_EXCEED_NUM_LIMIT":
                alert("图片数量超出，图片最多" + maxImageCount + "张");
                break;
            case "Q_TYPE_DENIED":
                alert("请选择图片");
                break;
            case "F_EXCEED_SIZE":
                alert("图片大小超出，图片最大" + fileSingleSizeLimit + "M");
                break;
        }
    });

    // 文件添加进来只前的时候
    uploader.on('beforeFileQueued', function (file) {

        //处理修改信息时预览图片问题start
        var isreturn = 0;
        var img = $list.find("img");
        if (img.length >= maxImageCount) {
            isreturn = 0;
        } else {
            isreturn = 1;
        }
        if (isreturn == 0) {
            return false;
        } else {
            return true;
        }
        //处理修改信息时预览图片问题end
    });



    // 当有文件添加进来的时候
    uploader.on('fileQueued', function (file) {
        var $li = $(
                    '<div id="' + file.id + '" class="cp_img">' +
                        '<img>' +
                    '<div class="cp_img_jian"></div></div>'
                    ),
                $img = $li.find('img');


        // $list为容器jQuery实例
        $list.append($li);

        // 创建缩略图
        // 如果为非图片文件，可以不用调用此方法。
        // thumbnailWidth x thumbnailHeight 为 100 x 100
        uploader.makeThumb(file, function (error, src) {
            if (error) {
                $img.replaceWith('<span>不能预览</span>');
                return;
            }

            $img.attr('src', src);
        }, thumbnailWidth, thumbnailHeight);
    });

    // 文件上传过程中创建进度条实时显示。
    uploader.on('uploadProgress', function (file, percentage) {
        var $li = $('#' + file.id),
                $percent = $li.find('.progress span');

        // 避免重复创建
        if (!$percent.length) {
            $percent = $('<p class="progress"><span></span></p>')
                        .appendTo($li)
                        .find('span');
        }

        $percent.css('width', percentage * 100 + '%');
    });

    // 文件上传成功，给item添加成功class, 用样式标记上传成功。
    uploader.on('uploadSuccess', function (file, response) {
        $('#' + file.id).addClass('upload-state-done');
        var serverReturn = eval(response);
        var id = serverReturn.id.split('_')[2];
        $('<input />', {
            id: hiddenIdAndName + id,
            name: hiddenIdAndName,
            type: 'hidden',
            value: response.filePath,
            runat: 'server"',
            class: 'zsImage'
        }).appendTo($('#form1'));
        $('#' + hiddenIdAndName + id).val(serverReturn.filePath);
    });

    // 文件上传失败，显示上传出错。
    uploader.on('uploadError', function (file) {
        var $li = $('#' + file.id),
                $error = $li.find('div.error');

        // 避免重复创建
        if (!$error.length) {
            $error = $('<div class="error"></div>').appendTo($li);
        }

        $error.text('上传失败');
    });

    // 完成上传完了，成功或者失败，先删除进度条。
    uploader.on('uploadComplete', function (file) {
        $('#' + file.id).find('.progress').remove();
    });

    //所有文件上传完毕
    uploader.on("uploadFinished", function () {
        //提交表单

    });

    //开始上传
    $("#ctlBtn").click(function () {
        var imageCount = $("#" + FileLisID + " .cp_img").length;
        if (imageCount >= minImageCount) {
            uploader.upload();
        } else {
            alert("选择的图片少于" + minImageCount + "张");
        }
    });

    //显示删除按钮
    $(".cp_img").live("mouseover", function () {
        $(this).children(".cp_img_jian").css('display', 'block');

    });
    //隐藏删除按钮
    $(".cp_img").live("mouseout", function () {
        $(this).children(".cp_img_jian").css('display', 'none');

    });
    //执行删除方法
    $list.on("click", ".cp_img_jian", function () {
        var Id = $(this).parent().attr("id");
        var imageId = Id.split('_')[2];
        var parent = $('#' + Id).parent();
        var imagePath = $("#" + hiddenIdAndName + imageId).val();
        $.post("../ashx/WebUploaderImage.ashx?Type=delete", { 'imagePath': imagePath }, function (data) {
        });
        try {
            uploader.removeFile(uploader.getFile(Id, true));
            $(this).parent().remove();
            $("#" + hiddenIdAndName + imageId).remove();
        } catch (e) {
            //处理修改信息时预览图片问题start
            $('#' + Id).remove();
            if (imageId >= 88888) {
                if (parent.attr("id") == "BusinesslicenseFileList") {
                    $("#" + hiddenIdAndName + imageId).attr("name", "myBusinessLicenseImageDelete");
                } else if (parent.attr("id") == "CompanyList") {
                    $("#" + hiddenIdAndName + imageId).attr("name", "myCompanyImageDelete");
                } else if (parent.attr("id") == "LogList") {
                    $("#" + hiddenIdAndName + imageId).attr("name", "myLogImageDelete");
                }
            }
            //处理修改信息时预览图片问题end
        }
    });
}