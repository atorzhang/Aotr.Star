var _idName = 'Id';//主键名称默认Id
var _getOneUrl = ''; // /TempletPdf/GetOne
var _saveUrl = ''; // /TempletPdf/Save
var _isClose = true;//默认保存后关闭页面

var _formartGetOne = function(data){

}
var _afterGetOne = function (data) {

}
var _init = function (layer, form) {

}

layui.use(['form', 'laydate'], function () {
    var form = layui.form
        , layer = layui.layer
        , laydate = layui.laydate;
    //初始化
    _init(layer, form);

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

    var id = $("#Id").val();//虚拟Id值
    if (id != '') {
        $.get(_getOneUrl, { Id: id }, function (res) {
            if (res.code == 0) {
                _formartGetOne(res.data);
                form.val('from', res.data);
                _afterGetOne(res.data);
            }
        }, 'json');
    }
    //监听提交
    form.on('submit(submit)', function (data) {
        var index = layer.load(1);
        data.field[_idName] = id;//把Id赋值真的主键
        $.post(_saveUrl, data.field, function (res) {
            layer.closeAll('loading');
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
        });
        return false;
    });
});
