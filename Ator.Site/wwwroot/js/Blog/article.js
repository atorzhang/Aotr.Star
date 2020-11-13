
layui.use('form', function () {
    var form = layui.form;
    form.on('submit(searchForm)', function(data){
        var keywords=$("#keywords").val();
        if(keywords==null || keywords==""){
            layer.msg('请输入要搜索的关键字');
            return false;
        }
        search();
        return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
    });
});

$(function(){
    $(".fa-file-text").parent().parent().addClass("layui-this");
    var keywords=$("#keywords").val();
    $("#keywords").keydown(function (event) {
        if (event.keyCode == 13) {
            var keyword=$("#keywords").val();
            if(keyword==null || keyword==""){
                layer.msg('请输入要搜索的关键字');
                return false;
            }
            search();
        }
    });
    search();
});

function search() {
    layui.use('flow', function () {
        var flow = layui.flow;
        var $ = layui.jquery;
        $("#layArticleList").remove();//清除数据
        $("#parentArticleList").append('<div class="flow-default" id="layArticleList"></div>');//填充模板
        flow.load({
            elem: '#layArticleList' //流加载容器
            , done: function (page, next) { //执行下一页的回调
                var lis = [];
                $.get("/Home/GetInfoData?page=" + page + "&SysCmsColumnId=" + $("#SysCmsColumnId").val() + "&keywords=" + $("#keywords").val(), function (res) {
                    //假设你的列表返回在data集合中
                    layui.each(res.data, function (index, item) {
                        lis.push('<div class="article shadow animated zoomIn">');
                        if (item.InfoImage != '' && item.InfoImage != null) {
                            lis.push('<div class="article-left"><img src="' + item.InfoImage + '" alt="' + item.InfoTitle + '"></div >');
                        }
                        else {
                            lis.push('<div class="article-left"><img src="/images/Blog/jzytp.JPG" alt="' + item.InfoTitle + '"></div >');
                        }
                        lis.push('<div class="article-right"><div class="article-title">');
                        if (item.InfoTop > 0) {
                            lis.push('<span class="article_is_top">置顶</span>&nbsp;');
                        }
                        if (item.InfoLable == "原创") {
                            lis.push('<span class="article_is_yc">' + item.InfoLable + '</span>&nbsp;');
                        }
                        else {
                            lis.push('<span class="article_is_zz">' + item.InfoLable + '</span>&nbsp;');
                        }
                        lis.push('<a href="/Home/Detail?id=' + item.SysCmsInfoId + '">' + item.InfoTitle + '</a></div>');
                        lis.push('<div class="article-abstract">' + item.InfoAbstract + '</div></div>');
                        lis.push('<div class="clear"></div><div class="article-footer">');
                        lis.push('<span><i class="fa fa-clock-o"></i>&nbsp;&nbsp;' + item.InfoPublishTime + '</span>');
                        lis.push('<span class="article-author"><i class="fa fa-user"></i>&nbsp;&nbsp;' + item.InfoAuthor + '</span >');
                        lis.push('<span><i class="fa fa-tag"></i>&nbsp;&nbsp;<a href="javascript:searchActicle(\''+item.SysCmsColumnId + '\')"> ' + item.InfoType + '</a></span>');
                        lis.push('<span class="article-viewinfo"><i class="fa fa-eye"></i>&nbsp;' + item.InfoClicks + '</span>');
                        lis.push('<span class="article-viewinfo"><i class="fa fa-commenting"></i>&nbsp;' + item.InfoComments + '</span>');
                        lis.push('</div></div> ');
                    });
                    //执行下一页渲染，第二参数为：满足“加载更多”的条件，即后面仍有分页
                    //pages为Ajax返回的总页数，只有当前页小于总页数的情况下，才会继续出现加载更多
                    next(lis.join(''), page < res.count);
                }, "json");
            }
        });
    });
}

//筛选栏目
function searchActicle(SysCmsColumnId) {
    document.getElementById('SysCmsColumnId').value = SysCmsColumnId;
    search();
}