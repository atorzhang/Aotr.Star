layui.use(['jquery','carousel','flow','layer'], function () {
    var $ = layui.jquery;
    var flow = layui.flow;
    var layer = layui.layer;

    var width = width || window.innerWeight || document.documentElement.clientWidth || document.body.clientWidth;
    width = width>1200 ? 1170 : (width > 992 ? 962 : width);
    var carousel = layui.carousel;
    //建造实例
    carousel.render({
      elem: '#carousel'
      ,width: width+'px' //设置容器宽度
      ,height:'360px'
      ,indicator: 'inside'
      ,arrow: 'always' //始终显示箭头
      ,anim: 'default' //切换动画方式
      
    });

    flow.load({
        elem: '#layArticleList' //流加载容器
        , done: function (page, next) { //执行下一页的回调
            var lis = [];
            $.get("/Home/GetInfoData?page=" + page, function (res) {
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
                    lis.push('<span><i class="fa fa-tag"></i>&nbsp;&nbsp;<a href="/Home/Aritcle?id=' + item.SysCmsColumnId + '"> ' + item.InfoType + '</a></span>');
                    lis.push('<span class="article-viewinfo"><i class="fa fa-eye"></i>&nbsp;' + item.InfoClicks + '</span>');
                    lis.push('<span class="article-viewinfo"><i class="fa fa-commenting"></i>&nbsp;' + item.InfoComments + '</span>');
                    lis.push('</div></div> ');
                });
                //执行下一页渲染，第二参数为：满足“加载更多”的条件，即后面仍有分页
                //pages为Ajax返回的总页数，只有当前页小于总页数的情况下，才会继续出现加载更多
                next(lis.join(''), page < res.count);
            }, "json")
        }
    });

    $(".home-tips-container span").click(function(){
        layer.msg($(this).text(), {
            time: 20000, //20s后自动关闭
            btn: ['明白了']
        });
    });


    $(".recent-list .hotusers-list-item").mouseover(function () {
        var name = $(this).children(".remark-user-avator").attr("data-name");
        var str = "【"+name+"】的评论";
        layer.tips(str, this, {
            tips: [2,'#666666']
        });
    });


    $("#QQjl").mouseover(function(){
        layer.tips('QQ交流', this,{
            tips: 1
        });
    });
    $("#gwxx").mouseover(function(){
        layer.tips('给我写信', this,{
            tips: 1
        });
    });
    $("#xlwb").mouseover(function(){
        layer.tips('新浪微博', this,{
            tips: 1
        });
    });
    $("#htgl").mouseover(function(){
        layer.tips('后台管理', this,{
            tips: 1
        });
    });




    $(function () {

        $(".fa-home").parent().parent().addClass("layui-this");
        //播放公告
        playAnnouncement(5000);
    });
    
    function playAnnouncement(interval) {
        var index = 0;
        var $announcement = $('.home-tips-container>span');
        //自动轮换
        setInterval(function () {
            index++;    //下标更新
            if (index >= $announcement.length) {
                index = 0;
            }
            $announcement.eq(index).stop(true, true).fadeIn().siblings('span').fadeOut();  //下标对应的图片显示，同辈元素隐藏
        }, interval);
    }


});

function classifyList(id){
    	layer.msg('功能要自己写');
}





