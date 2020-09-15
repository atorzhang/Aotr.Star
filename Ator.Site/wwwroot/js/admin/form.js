//定义访问路径
var _ContralPath = "";
var _PageDataPath = "GetPageData";
var _DataPath = "GetData";
var _ChecksPath = "Checks";
var _DeletesPath = "Deletes";
var _SavePath = "Save";
var _AddPagePath = "Form";
var _pageName = "";
//表格转为json
var formToJson = function (data) {
    data = data.replace(/&/g, "\",\"");
    data = data.replace(/=/g, "\":\"");
    data = "{\"" + data + "\"}";
    return JSON.parse(data);
};
var parameter = "?id=";
//扩展admin
layui.extend({
    admin: '{/}/js/admin'
});

//刷新数据
window.reloadData = function () {
    document.getElementById("btnSreach").click();
};
//关闭窗口
window.colseForm = function (formName) {
    var index = layer.getFrameIndex(formName);
    layer.close(index);
};

//单张图片上传插件初始化
var uploadImageInit = function (btn, imgid, inputid, txtid) {
    layui.use(['layer', 'jquery', 'upload'], function () {
        var layer = layui.layer,
            upload = layui.upload,
            $ = layui.jquery;
        //删除图片
        window.deleteImage = function (imgid, inputid) {
            $('#' + imgid).attr('src',"");
            $('#' + inputid).val("");
            $('#' + txtid).html("");
        }
        if ($('#' + imgid)[0].src.length > 0) {
            $('#' + txtid).html('<a class="layui-btn layui-btn-danger layui-btn-xs demo-reload" onclick="deleteImage(\'' + imgid + '\',\'' + inputid + '\')">删除</a>');
        }
        var uploadInst = upload.render({
            elem: '#' + btn
            , url: '/Common/File/ImgUpload?saveDir=column_img'
            , before: function (obj) {
                //预读本地文件示例，不支持ie8
                obj.preview(function (index, file, result) {
                    $('#'+imgid).attr('src', result); //图片链接（base64）
                });
            }
            , done: function (res) {
                //如果上传失败
                if (res.code > 0) {
                    return layer.msg('上传失败');
                }
                //上传成功
                var $demoText = $('#'+txtid);
                $demoText.html('<a class="layui-btn layui-btn-danger layui-btn-xs demo-reload" onclick="deleteImage(\'' + imgid + '\',\'' + inputid +'\')">删除</a>');
                $('#' + inputid).val(res.msg);
                layer.msg('上传成功');
            }
            , error: function () {
                //演示失败状态，并实现重传
                var $demoText = $('#' + txtid);
                $demoText.html('<span style="color: #FF5722;">上传失败</span> <a class="layui-btn layui-btn-xs demo-reload">重试</a>');
                $demoText.find('.demo-reload').on('click', function () {
                    uploadInst.upload();
                });
            }
        });
    })
}
//多图上传初始化
var uploadImagesInit = function (btn, imgsid, inputid, txtid) {
    layui.use(['layer', 'jquery', 'upload'], function () {
        var layer = layui.layer,
            upload = layui.upload,
            $ = layui.jquery;
        //多图上传删除单张图片
        window.deleteImages = function (obj) {
            var src = $(obj).prev().attr('src');
            var src1 = src + ',';
            var src2 = ',' + src;
            var beforeValue = $("#" + inputid).val();
            beforeValue = beforeValue.replace(src1, '');
            beforeValue = beforeValue.replace(src2, '');
            beforeValue = beforeValue.replace(src, '');
            $("#" + inputid).val(beforeValue);
            $(obj).parent().remove();
        };
        //初始化图片显示
        var inputValues = $('#' + inputid).val().split(',');

        for (var i = 0; i < inputValues.length; i++) {
            if (inputValues[i] != '') {
                var lis = [];
                lis.push('<div class="layui-input-inline">');
                lis.push('<img src="' + inputValues[i] + '" class="layui-upload-img" style="height:120px;width:100%"> ');
                lis.push('<a class="layui-btn layui-btn-danger layui-btn-xs demo-reload" onclick="deleteImages(this)">删除</a></div>');
                $('#' + imgsid).append(lis.join(''));
            }
        }
        //初始化上传插件
        upload.render({
            elem: '#' + btn
            , url: '/Common/File/ImgUpload?saveDir=info_imgs'
            , multiple: true
            , before: function (obj) {
                //预读本地文件示例，不支持ie8
                obj.preview(function (index, file, result) {
                    //不做本地预览操作
                    //var lis = [];
                    //lis.push('<div class="layui-input-inline">');
                    //lis.push('<img src="' + result + '" alt="' + file.name + '" class="layui-upload-img" style="height:120px;width:auto">');
                    //lis.push('<a class="layui-btn layui-btn-danger layui-btn-xs demo-reload" onclick="deleteImages(this)">删除</a></div>');
                    //$('#' + imgsid).append(lis.join(''));
                });
            }
            , done: function (res) {
                //上传完毕
                if (res.success) {
                    //把预览图显示
                    var lstImg = res.msg.split(',');
                    for (var i = 0; i < lstImg.length; i++) {
                        var lis = [];
                        lis.push('<div class="layui-input-inline" style="margin-left:10px">');
                        lis.push('<img src="' + lstImg[i] + '" class="layui-upload-img" style="height:120px;width:100%">');
                        lis.push('<a class="layui-btn layui-btn-danger layui-btn-xs demo-reload" onclick="deleteImages(this)">删除</a></div>');
                        $('#' + imgsid).append(lis.join(''));
                    }
                    //添加到input中
                    var beforeValue = $("#" + inputid).val();
                    if (beforeValue.length > 0) {
                        $("#" + inputid).val(beforeValue + ',' + res.msg);
                    }
                    else {
                        $("#" + inputid).val(res.msg);
                    }
                }
            }
        });
    });
}