var _idName = 'Id';
var _getOneUrl = ''; // /TempletPdf/GetOne
var _saveUrl = ''; // /TempletPdf/Save
var _isClose = true;//默认保存后关闭页面

var _formartGetOne = function (data) {

}
var _afterGetOne = function (data) {

}

//对数据提交前进行处理
var _beforeSubmit = function (data) {
    return data;
}
var _afterSubmit = function (data) {
    return data;
}

//表格转为json
var formToJson = function (data) {
    data = data.replace(/&/g, "\",\"");
    data = data.replace(/=/g, "\":\"");
    data = "{\"" + data + "\"}";
    return JSON.parse(data);
};

//单张图片上传插件初始化
var uploadImageInit = function (btn, imgid, inputid, txtid) {
    layui.use(['layer', 'jquery', 'upload'], function () {
        var layer = layui.layer,
            upload = layui.upload,
            $ = layui.jquery;
        //删除图片
        window.deleteImage = function (imgid, inputid) {
            $('#' + imgid).attr('src', "");
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
                    $('#' + imgid).attr('src', result); //图片链接（base64）
                });
            }
            , done: function (res) {
                //如果上传失败
                if (res.code > 0) {
                    return layer.msg('上传失败');
                }
                //上传成功
                var $demoText = $('#' + txtid);
                $demoText.html('<a class="layui-btn layui-btn-danger layui-btn-xs demo-reload" onclick="deleteImage(\'' + imgid + '\',\'' + inputid + '\')">删除</a>');
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

$(function () {
    layui.use(['form', 'laydate'], function () {
        var form = layui.form
            , layer = layui.layer
            , laydate = layui.laydate;

        $(":text").each(function () {
            var formart = $(this).attr('formart');
            if (formart != '' && formart != undefined) {
                switch (formart) {
                    case 'datetime':
                        laydate.render({
                            elem: '#' + $(this).attr('id')
                            , type: 'datetime'
                        });
                        break;
                    case 'date':
                        laydate.render({
                            elem: '#' + $(this).attr('id')
                            , type: 'date'
                        });
                        break;
                    default:
                        break;
                }
            }
        });

        var id = $("#" + _idName).val();//虚拟Id值
        if (id != '') {
            $.get(_getOneUrl, { Id: id }, function (res) {
                if (res.code == 0) {
                    var formartData = _formartGetOne(res.data);
                    if (formartData == undefined) {
                        formartData = res.data;
                    }
                    form.val('from', formartData);
                    _afterGetOne(formartData);
                }
            }, 'json');
        }
        //监听提交
        form.on('submit(submit)', function (data1) {
            data1.field[_idName] = id;//把Id赋值真的主键
            var data = _beforeSubmit(data1.field);//提交前处理数据
            //判断是否要提交，数据为空不提交
            if (data == "") {
                return false;
            }
            var index = layer.load(1);
            $.post(_saveUrl, data, function (res) {
                layer.closeAll('loading');
                _afterSubmit(data);
                parent.reSearch();//父级页面刷新
                if (_isClose) {
                    var index = parent.layer.getFrameIndex(window.name);
                    if (res.code == 0) {
                        parent.layer.msg(res.msg == '' ? "保存成功" : res.msg, { icon: 1 });
                        parent.layer.close(index);//关闭当前页
                    }
                    else {
                        layer.msg(res.msg == '' ? "保存失败" : res.msg, { icon: 2 });
                    }
                }
                else {
                    if (res.code == 0) {
                        layer.msg(res.msg == '' ? "保存成功" : res.msg, { icon: 1 });
                    }
                    else {
                        layer.msg(res.msg, { icon: 2 });
                    }
                }
            }, 'json').error(function (xhr, errorText, errorType) {
                layer.closeAll('loading');
                layer.msg('保存失败', { icon: 2 });
            });;
            return false;
        });
    });
})

