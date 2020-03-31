//全局变量，引用页面重写即可
//需要重写变量
var _dataUrl = '';//表格数据来源地址 '/TempletPdf/GetPage'
var _cols = [[]];//表格显示的属性
var _formUrl = '';//添加或编辑按钮跳转页面 /TempletPdf/Form
var _deleteUrl = ''//删除地址 /TempletPdf/Delete
var _idName = 'Id';//主键名称

//具有默认值属性
var _area = ['50%', '75%'];//弹出宽度
var _addTitle = "添加";
var _editTitle = "编辑";
var _method = 'post';
var _elem = 'currentTableId';
var _toolbar = 'currentToolbar';
var _limits = [10, 15, 20, 50, 100, 500, 1000];
var _limit = 15;
var _searchFiled = {};//搜索保存的参数
var _autoSort = false ;

//监听事件
var _searchBefore = function (data) { }//搜索前事件
var _searchAfter = function (data) { }//搜索后事件
var _titleBarBefore = function (obj) { }//工具栏按点击前监听
var _titleBarClick = function (obj, table, layer) { }//工具栏按钮点击后事件
var _tableToolBefore = function (obj) { }//表格工具栏点击前监听
var _tableToolClick = function (obj, table, layer) { }//表格工具栏按钮点击事件
var _tableCheckbox = function (obj) { }//监听表格复选框事件
var _init = function (form, table,layer) { };//初始化前事件
//初始化
$(function () {
    layui.use(['form', 'table', 'layer'], function () {
        var $ = layui.jquery,
            form = layui.form,
            table = layui.table,
            layer = layui.layer;
        _init(form, table, layer);
        //表格初始化
        table.render({
            method: _method,
            elem: '#' + _elem,
            url: _dataUrl,
            toolbar: '#' + _toolbar,
            defaultToolbar: ['filter', 'exports', 'print', {
                title: '提示',
                layEvent: 'LAYTABLE_TIPS',
                icon: 'layui-icon-tips'
            }],
            cols: _cols,
            limits: _limits,
            limit: _limit,
            page: true,
            autoSort: _autoSort,
            where: _searchFiled
        });

        // 监听搜索操作
        form.on('submit(data-search-btn)', function (data) {
            //执行搜索重载
            _searchBefore(data);
            _searchFiled = data.field;
            table.reload(_elem, {
                page: {
                    curr: 1
                }
                , where: data.field
            }, 'data');
            _searchAfter(data);
            return false;
        });

        /**
         * toolbar监听事件
         */
        table.on('toolbar(currentTableFilter)', function (obj) {
            _titleBarBefore(obj);
            if (obj.event === 'add') {  // 监听添加操作
                var index = layer.open({
                    title: _addTitle,
                    type: 2,
                    shade: 0.2,
                    maxmin: true,
                    shadeClose: true,
                    area: _area,
                    content: _formUrl,
                });
                $(window).on("resize", function () {
                    layer.full(index);
                });
            } else if (obj.event === 'delete') {  // 监听删除操作
                var checkStatus = table.checkStatus('currentTableId')
                    , data = checkStatus.data;
                //layer.alert(JSON.stringify(data));
                var ids = [];
                for (var i = 0; i < data.length; i++) {
                    ids.push(data[i][_idName]);
                }
                layer.confirm('真的删除选中行吗？', function (index) {
                    $.post(_deleteUrl, { id: ids.join() }, function (res) {
                        if (res.code == 0) {
                            layer.msg('删除成功', { icon: 1 });
                            obj.del();
                        }
                        else {
                            layer.msg('删除失败,' + res.msg, { icon: 2 });
                        }
                        layer.close(index);
                    }, 'json').error(function (xhr, errorText, errorType) {
                        layer.msg('删除失败', { icon: 2 });
                        layer.close(index);
                    });
                });
            } else {
                //自定义toolbar监听事件
                _titleBarClick(obj, table, layer);
            }
        });

        //监听表格复选框选择
        table.on('checkbox(currentTableFilter)', function (obj) {
            _tableCheckbox(obj);
        });

        //排序监听
        table.on('sort(currentTableFilter)', function (obj) { //注：sort 是工具条事件名，test 是 table 原始容器的属性 lay-filter="对应的值"
            _searchFiled.field = obj.field;
            _searchFiled.order = obj.type;
            table.reload(_elem, {
                initSort: obj //记录初始排序，如果不设的话，将无法标记表头的排序状态。
                , where: _searchFiled
            });
        });

        //表格内工具栏监听事件
        table.on('tool(currentTableFilter)', function (obj) {
            _tableToolBefore(obj);
            var data = obj.data;
            if (obj.event === 'edit') {
                var index = layer.open({
                    title: _editTitle,
                    type: 2,
                    shade: 0.2,
                    maxmin: true,
                    shadeClose: true,
                    area: _area,
                    content: _formUrl + '?Id=' + data[_idName],
                });
                $(window).on("resize", function () {
                    //layer.full(index);
                });
                return false;
            } else if (obj.event === 'delete') {
                layer.confirm('真的删除行么', function (index) {
                    $.post(_deleteUrl, { id: data[_idName] }, function (res) {
                        if (res.code == 0) {
                            layer.msg('删除成功', { icon: 1 });
                            obj.del();
                        }
                        else {
                            layer.msg('删除失败', { icon: 2 });
                        }
                        layer.close(index);
                    }, 'json').error(function (xhr, errorText, errorType) {
                        layer.msg('删除失败', { icon: 2 });
                        layer.close(index);
                    });
                });
            } else {
                _tableToolClick(obj, table, layer);
            }
        });
    });
});
