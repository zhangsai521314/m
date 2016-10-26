
(function ($, window) {
    var applicationPath = window.applicationPath === "" ? "" : window.applicationPath || "../..";
    function S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }
    var dataname = "powerwebuploaderinnerdata";
    guid = WebUploader.Base.guid()
    function initWebUpload(item, options) {

        //不支持时的处理
        if (!WebUploader.Uploader.support()) {
            var error = "上传控件不支持您的浏览器！请尝试<a target='_blank' href='http://get.adobe.com/cn/flashplayer/'>升级flash版本</a>或者<a target='_blank' href='http://se.360.cn'>使用Chrome引擎的浏览器</a>。";
            var $a = $("<a target='_blank' href='http://se.360.cn'>使用Chrome引擎的浏览器</a>");
            var $a2 = $("<a target='_blank' href='http://get.adobe.com/cn/flashplayer/'>升级flash版本</a>")
            if (window.console) {
                window.console.log(error);
            }

            $(item).html(error);
            return;
        }

        var defaults = {
            hiddenInputId: "uploadifyHiddenInputId", // input hidden id
            onAllComplete: function (event) { }, // 当所有file都上传后执行的回调函数
            onComplete: function (event) { }, // 每上传一个file的回调函数
            innerOptions: {},
            fileNumLimit: undefined,
            fileSizeLimit: undefined,
            fileSingleSizeLimit: undefined,
            PostbackHold: false,
            ShowDownload: false
        };

        var myinnerOptions = {};
        if (WebUploader.Base.android) {
            console.log(WebUploader.Base.android);
        }

        var opts = $.extend({}, defaults, options);
        var hdFileData = $("#" + opts.hiddenInputId);
        this.hdFileData = hdFileData;
        var target = $(item);
        var pickerid = "";
        if (typeof guidGenerator != 'undefined')
            pickerid = guidGenerator();
        else
            pickerid = S4();

        var uploaderStrdiv = '<div class="webuploader">' +
            '<div id="thelist" class="uploader-list"></div>' +
            '<div class="btns">' +
            '<div id="' + pickerid + '">选择文件</div>' +
        //            '<br/><a id="ctlBtn" class="btn btn-default">开始上传</a>' +
            '</div>' +
        '</div>';
        target.append(uploaderStrdiv);

        var $list = target.find('#thelist'),
             $btn = target.find('#ctlBtn'),
             state = 'pending',
             uploader;
        this.$list = $list;

        var jsonData = {
            fileList: []
        };

        var webuploaderoptions = $.extend({

            // swf文件路径
            swf: '../js/WebUploader/Uploader.swf',

            // 文件接收服务端。
            server: '../ashx/WebUploaderImage.ashx?Type=addFile&guid=' + guid + "&fileType=" + options.myFileType,

            // 选择文件的按钮。可选。
            // 内部根据当前运行是创建，可能是input元素，也可能是flash.
            pick: '#' + pickerid,


            chunked: true, //开启分片

            threads: 1, //上传并发数


            // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
            resize: false,
            fileNumLimit: opts.fileNumLimit, //文件总的个数

            fileSingleSizeLimit: 1024 * 1024 * opts.fileSingleSizeLimit//单个文件的大小
        },
        opts.innerOptions);
        var uploader = WebUploader.create(webuploaderoptions);

        //还原hiddenfiled的保持数据
        var fileDataStr = hdFileData.val();
        if (fileDataStr && opts.PostbackHold) {
            jsonData = JSON.parse(fileDataStr);
            $.each(jsonData.fileList, function (index, fileData) {
                var newid = S4();
                fileData.queueId = newid;
                $list.append('<div id="' + newid + '" class="item">' +
                '<div class="info">' + fileData.name + '</div>' +
                '<div class="state">已上传</div>' +
              (opts.ShowDownload ? '<div class="download"></div>' : '') +
                '<div class="del"></div>' +
            '</div>');
            });
            hdFileData.val(JSON.stringify(jsonData));
        }


        // 文件添加进来只前的时候
        uploader.on('beforeFileQueued', function (file) {
            var isreturn = 0;
            if ($("#" + opts.bangDingId + " #thelist>div").length >= opts.fileNumLimit) {
                isreturn = 0;
            } else {
                isreturn = 1;
            }
            if (isreturn == 0) {
                return false;
            } else {
                return true;
            }
            return true;
        });

        //用户的操作超出限制
        uploader.on('error', function (handler) {
            switch (handler) {
                case "Q_EXCEED_NUM_LIMIT":
                    alert("附件数量超出，附件最多" + opts.fileNumLimit + "个");
                    break;
                case "F_EXCEED_SIZE":
                    alert("附件过大，附件最大" + opts.fileSingleSizeLimit + "M");
                    break;
            }
        });
        uploader.on('fileQueued', function (file) {
            $list.append('<div id="' + file.id + '" class="item">' +
                '<div class="info">' + file.name + '</div>' +
                '<div class="state">等待上传...</div>' +
                '<div class="download" style="display:none;"></div>' +
                '<div class="del"></div>' +
            '</div>');
        });
        uploader.on('uploadProgress', function (file, percentage) {
            var $li = target.find('#' + file.id),
                $percent = $li.find('.progress .bar');

            // 避免重复创建
            if (!$percent.length) {
                $percent = $('<span class="progress">' +
                    '<span  class="percentage"><span class="text"></span>' +
                  '<span class="bar" role="progressbar" style="width: 0%">' +
                  '</span></span>' +
                '</span>').appendTo($li).find('.bar');
            }
            $('#' + file.id + " .del").addClass("mydel");
            $('#' + file.id + " .del").removeClass("del");
            $li.find('div.state').text('上传中');
            $li.find("span.text").text(Math.round(percentage * 100) + '%');
            $percent.css('width', percentage * 100 + '%');
        });
        uploader.on('uploadSuccess', function (file, response) {
            target.find('#' + file.id).find('div.state').text('已上传');
            $('#' + file.id + " .mydel").addClass("del");
            $('#' + file.id + " .mydel").removeClass("mydel");
            if (opts.ShowDownload) {
                target.find('#' + file.id).find('div.download').show();
            }

            var fileEvent = {
                queueId: file.id,
                name: file.name,
                size: file.size,
                type: '.' + file.ext, //历史遗留的误导性命名
                extension: '.' + file.ext,
                mimetype: file.type,
                isCopy: 1, //是否需要后台执行拷贝1是
                filePath: response.filePath
            };
            jsonData.fileList.push(fileEvent)
            opts.onComplete(fileEvent);

        });

        uploader.on('uploadError', function (file) {
            target.find('#' + file.id).find('div.state').text('上传出错');
            $('#' + file.id + " .mydel").addClass("del");
            $('#' + file.id + " .mydel").removeClass("mydel");
        });

        uploader.on('uploadComplete', function (file) {
            target.find('#' + file.id).find('.progress').fadeOut();
            hdFileData.val(JSON.stringify(jsonData));
            opts.onAllComplete(jsonData.fileList);
        });

        uploader.on('fileQueued', function (file) {
            uploader.upload();
        });

        uploader.on('filesQueued', function (file) {
            uploader.upload();
        });

        uploader.on('all', function (type) {
            if (type === 'startUpload') {
                state = 'uploading';
            } else if (type === 'stopUpload') {
                state = 'paused';
            } else if (type === 'uploadFinished') {
                state = 'done';
            }
            if (state === 'uploading') {
                $btn.text('暂停上传');

            } else {
                $btn.text('开始上传');

            }
        });

        $btn.on('click', function () {
            if (state === 'uploading') {
                uploader.stop(true);
            } else {
                uploader.upload();
            }
        });
        //删除
        $list.on("click", ".del", function () {
            var $ele = $(this);
            var id = $ele.parent().attr("id");
            var num = id.split('_')[2];
            if (num >= 88888) {
                var parentid = $ele.parent().parent().parent().parent().attr("id");
                if (parentid == "WarrantyManual") {
                    $("#" + options.myhidden + num).attr("name", "myWarrantyManualUploadHiddenDelete");
                } else if (parentid == "Enclosure") {
                    $("#" + options.myhidden + num).attr("name", "myenclosureUploadHiddenDelete");
                }
                $ele.parent().remove();
            } else {
                var deletefile = {};
                $.each(jsonData.fileList, function (index, item) {
                    if (item && item.queueId === id) {
                        uploader.removeFile(uploader.getFile(id));
                        deletefile = jsonData.fileList.splice(index, 1)[0];
                        $("#" + opts.hiddenInputId).val(JSON.stringify(jsonData));
                        $.post('../ashx/WebUploaderImage.ashx?Type=delete', { 'filePath': deletefile.filePath }, function (returndata) {
                            $ele.parent().remove();
                        });
                        return false;
                    }
                });
                uploader.removeFile(uploader.getFile(id));
                return;
            }
        });

        //下载
        $list.on("click", ".download", function () {
            var $ele = $(this);
            var id = $ele.parent().attr("id");
            var downloadfile = {};
            $.each(jsonData.fileList, function (index, item) {
                if (item && item.queueId === id) {
                    downloadfile = item;
                    $("#" + opts.hiddenInputId).val(JSON.stringify(jsonData));
                    $.post(applicationPath + '/ajax/WebUploader/GetPathId', { 'filePathName': downloadfile.filePath, 'fileName': downloadfile.name }, function (pathid) {
                        window.location.href = applicationPath + '/ajax/WebUploader/Download?pathid=' + pathid;
                    });
                    return;
                }
            });
        });

    }

    initWebUpload.prototype.deleteAll = function () {
        var self = this;
        var fileDataStr = this.hdFileData.val();
        jsonData = JSON.parse(fileDataStr);
        $.post(applicationPath + '../ashx/WebUploaderImage.ashx?Type=delete&&file=all', { 'filelist': jsonData }, function (returndata) {
            self.$list.empty();
        });
    }


    $.fn.powerWebUpload = function (options) {
        var ele = this;
        $(ele).each(function (idnex, item) {

            var target = $(item);
            if (target.data("webupload")) {
                //执行方法
                if (typeof options == "string") {
                    var innerdata = target.data(dataname);
                    innerdata[options]();
                }
                return;
            }
            if (typeof options != "object") {
                return;
            }

            target.data("webupload", true);
            if (typeof PowerJs != 'undefined') {
                PowerJs.Core.lazyLoad(applicationPath + "../assets/WebUploader/webuploader.css", function () { }, 'css');
                PowerJs.Core.lazyLoad(applicationPath + "../js/WebUploader/webuploader.min.js", function () {
                    var updata = new initWebUpload(target, options);
                    target.data(dataname, updata);
                });
            }
            else {
                var updata = new initWebUpload(target, options);
                target.data(dataname, updata);
            }
        });
    }
})(jQuery, window);