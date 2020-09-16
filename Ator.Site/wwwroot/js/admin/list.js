//参数
var parameter = "?id=";
//扩展admin
layui.extend({
    admin: '{/}/js/admin'
});
//定义访问路径
var _ContralPath = "";//默认控制器地址
var _PageDataPath = "GetPageData";//默认获取数据列表路径
var _DataPath = "GetData";//默认获取单条数据路径
var _ChecksPath = "Checks";//默认审核路径
var _DeletesPath = "Deletes";//默认删除路径
var _SavePath = "Save";//默认保存路径
var _AddPagePath = "Form";//默认新增打开路径
var _FormWidth = 900;//弹窗宽度
var _FormHeight = 600;//弹窗宽度
var _isFormFull = false;//弹窗全屏

//表格数据
var _tableId = "listTable";//默认表格Id
var _isSaveConfirm = true;//默认提示是否保存对话框显示

var _pageName = "";//关键字名称【每个页面都需要重写】
var _idFieldName = "id";//该值为主键名称【每个页面都需要重写】
var _tableTitle = "";//表格标题【每个页面都需要重写】
var _cols = [[]];//表格字段列表【每个页面都需要重写】
var intData = function (layui) { };//初始化数据方法【每个页面都需要重写】

//保存时候数据验证
var changeSaveJsonData = function (jsonObj) {
    //该方法在启用表格编辑修改数据时候，只传了id和参数命和参数值有时候不满足后台数据验证规则，比如修改用户表时修改邮箱时候只传了SysUserId和Email字段以及值，无法满足UserName数据不为空的数据验证，需要页面重写该方法，修改值jsonObj.UserName = "UserName";
}
//表格头工具栏事件监听，默认实现了添加和批量删除，批量审核成功/失败
var toolBarEvent = function (obj) {
    //obj.event为事件名称
    //obj.data为该行数据
}
//表格行工具栏事件监听，默认实现编辑和删除
var toolEvent = function (obj) {
    //obj.event为事件名称
    //obj.data为该行数据
}

//刷新数据
window.reloadData = function () {
    document.getElementById("btnSreach").click();
};
//关闭窗口
window.colseForm = function (formName) {
    var index = layer.getFrameIndex(formName);
    layer.close(index);
};
//审核数据，noReload = false时候不重新加载数据
window.checksData = function (ids, status,isReload) {
    layui.use("jquery", function () {
        var $ = layui.jquery;
        $.post(_ContralPath + _ChecksPath, { ids: ids, status: status }, function (res) {
            if (res.success) {
                if (isReload !== false) {
                    layer.msg('审核成功');
                    reloadData();
                }
            } else {
                if (isReload !== false)
                    layer.msg('审核失败！' + res.msg);
            }
        });
    });
};
//数据加载，事件绑定
window.onload = function () {
    
    layui.use(['table', 'jquery', 'layer', 'admin'], function () {
        var table = layui.table,
            form = layui.form,
            layer = layui.layer,
            $ = layui.jquery;
        intData(layui);//初始化数据
        //初始化表格
        table.render({
             elem: '#listTable'
            , url: _ContralPath + _PageDataPath
            , toolbar: '#listtoolbar'
            , title: _tableTitle
            , skin: 'line' //行边框风格
            //, even: true //开启隔行背景
            , cols: _cols
            , page: true
            , limits: [10, 15, 20, 50, 100, 500, 1000, 10000]
            , limit: 15 //每页默认显示的数量
            , curr: 1 //从第一页开始取数据
            , where: formToJson($('#aeSearch').serialize())
        });
        //绑定搜索事件
        $('#btnSreach').on('click', function () {
            table.reload('listTable', {
                curr: 1  //从第一页开始
                , where: formToJson(decodeURIComponent($('#aeSearch').serialize(), true))//搜搜条件更新
            });
        });
        //头工具栏事件
        table.on('toolbar(test)', function (obj) {
            var checkStatus = table.checkStatus(obj.config.id);
            var data = checkStatus.data;
            switch (obj.event) {
                case 'addItem':
                    if (_isFormFull) {
                        var index_ = WeAdminShow('添加' + _pageName, _ContralPath + _AddPagePath + parameter, '100%', '100%');
                        layer.full(index_);
                    }
                    else {
                        WeAdminShow('添加' + _pageName, _ContralPath + _AddPagePath + parameter, _FormWidth, _FormHeight);
                    }
                    break;
                case 'deleteItem':
                    if (data.length > 0) {
                        layer.confirm('确定要删除选中的' + data.length + '条数据吗?', function () {
                            var ids = '';
                            for (var i = 0; i < data.length; i++) {
                                ids += data[i][_idFieldName] + ',';
                            }
                            $.post(_ContralPath + _DeletesPath, { ids: ids }, function (res) {
                                if (res.success) {
                                    layer.msg('删除成功');
                                    reloadData();
                                } else {
                                    layer.msg('删除失败！' + res.msg);
                                }
                            });
                        });
                    } else {
                        layer.msg('请选择要操作的数据');
                    }
                    break;
                case 'checkPass':
                    if (data.length > 0) {
                        var ids = '';
                        for (var i = 0; i < data.length; i++) {
                            ids += data[i][_idFieldName] + ',';
                        }
                        checksData(ids, 1);
                    } else {
                        layer.msg('请选择要操作的数据');
                    }
                    break;
                case 'checkNoPass':
                    if (data.length > 0) {
                        var ids = '';
                        for (var i = 0; i < data.length; i++) {
                            ids += data[i][_idFieldName] + ',';
                        }
                        checksData(ids, 2);
                    } else {
                        layer.msg('请选择要操作的数据');
                    }
                    break;
            }
            toolBarEvent(obj);//自定义头工具栏事件监听
        });
        //监听行工具事件
        table.on('tool(test)', function (obj) {
            var data = obj.data;//该行所有数据
            //console.log(obj)
            if (obj.event === 'del') {
                layer.confirm('真的要删除吗', function (index) {
                    //服务器中删除
                    $.post(_ContralPath + _DeletesPath, { ids: data[_idFieldName] }, function (res) {
                        if (res.success) {
                            layer.msg('删除成功');
                            obj.del();
                        } else {
                            layer.msg('删除失败！' + res.msg);
                        }
                    });
                    layer.close(index);
                });
            } else if (obj.event === 'edit') {
                var index_ = WeAdminShow('修改' + _pageName, _ContralPath + _AddPagePath + parameter + data[_idFieldName], _FormWidth, _FormHeight);
                if (_isFormFull) {
                    layer.full(index_);
                }
            } else {
                //自定义行工具事件监听
                toolEvent(obj);
            }
        });
        //监听单元格编辑
        table.on('edit(test)', function (obj) {
            var value = obj.value //得到修改后的值
                , data = obj.data //得到所在行所有键值
                , field = obj.field; //得到字段
            //保存数据的方法抽出来
            var saveData = function () {
                var jsonStr = '{"' + _idFieldName + '":"' + data[_idFieldName] + '","' + field + '":"' + value + '","columns":"' + field + '"}';
                var jsonData = JSON.parse(jsonStr);
                changeSaveJsonData(jsonData);
                console.log(jsonData);
                //更新数据
                $.post(_ContralPath + _SavePath, jsonData, function (res) {
                    if (res.success) {
                        layer.msg(field + ' 字段更改为：' + value);
                    } else {
                        layer.msg("更新失败," + res.msg);
                    }
                }, "json");
            }
            if (_isSaveConfirm) {
                layer.confirm('是否保存修改的数据？', {
                    btn: ['是[不再提示]', '是', '否'] //按钮
                }, function () {
                    _isSaveConfirm = false;
                    saveData();
                }, function () {
                    saveData();
                });
            }
            else {
                saveData();
            }
        });
        //监听表格数据状态操作
        form.on('switch(statusFilter)', function (obj) {
            if (obj.elem.checked) {
                checksData(obj.elem.id, 1, false);
            } else {
                checksData(obj.elem.id, 2, false);
            }
            layer.tips(this.name + '：' + obj.elem.checked, obj.othis);
        });
    });
}
//表格转为json
var formToJson = function (data) {
    data = data.replace(/&/g, "\",\"");
    data = data.replace(/=/g, "\":\"");
    data = "{\"" + data + "\"}";
    return JSON.parse(data);
};




