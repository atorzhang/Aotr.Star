var data = [{
	"commentId": 11,
	"article": null,
	"user": {
		"userId": 11,
		"nickname": "清风",
		"headPortrait": "http://qzapp.qlogo.cn/qzapp/101477629/B4901BFB60F8DEE83F01692F2544E612/100",
		"sex": "男",
		"registrationDate": "2018-08-01 15:19:26",
		"latelyLoginTime": "2018-08-01 15:19:26",
		"commentNum": 1
	},
	"content": "<p>网站开源吗</p><p><br></p>",
	"commentDate": "2018-08-01 15:19:41",
	"site": "河北省保定市  铁通",
	"reply":[{
			"replyId": 2,
			"comment": {
				"commentId": 11,
				"article": null,
				"user": {
					"userId": 11,
					"nickname": "清风",
					"headPortrait": "http://qzapp.qlogo.cn/qzapp/101477629/B4901BFB60F8DEE83F01692F2544E612/100",
					"sex": "男",
					"registrationDate": "2018-08-01 15:19:26",
					"latelyLoginTime": "2018-08-01 15:19:26",
					"commentNum": 1
				},
				"content": "<p>网站开源吗</p><p><br></p>",
				"commentDate": "2018-08-01 15:19:41",
				"site": "河北省保定市  铁通"
			},
			"user": {
				"userId": 1,
				"nickname": "Single",
				"headPortrait": "http://qzapp.qlogo.cn/qzapp/101477629/2F1EDDE252859E5FF645F959893C6863/100",
				"sex": "男",
				"registrationDate": "2018-07-26 21:24:49",
				"latelyLoginTime": "2018-08-09 10:25:36",
				"commentNum": 1
			},
			"content": "最近有点忙，后期会开源到GitHub上的。",
			"replyDate": "2018-08-01 22:10:28",
			"site": "湖南省湘潭市  移动"
		}]
},  {
	"commentId": 2,
	"article": null,
	"user": {
		"userId": 2,
		"nickname": "Mr.Long",
		"headPortrait": "http://qzapp.qlogo.cn/qzapp/101477629/B5D5212D0429E4491D932EEEF814FE99/100",
		"sex": "男",
		"registrationDate": "2018-07-26 21:30:24",
		"latelyLoginTime": "2018-08-01 22:16:42",
		"commentNum": 4
	},
	"content": "试试<br>",
	"commentDate": "2018-07-26 21:30:52",
	"site": "湖南省湘潭市  移动",
	"reply":[]
}];


layui.use(['jquery', 'form', 'layedit', 'flow'], function(){
    var form = layui.form;
    var $ = layui.jquery;
    var layedit = layui.layedit;
    var flow = layui.flow;
    //评论和留言的编辑器
    var editIndex = layedit.build('remarkEditor', {
        height: 150,
        tool: ['face', '|', 'left', 'center', 'right', '|', 'link'],
    });
    //评论和留言的编辑器的验证
    layui.form.verify({
        content: function (value) {
            value = $.trim(layedit.getText(editIndex));
            if (value == "") return "至少得有一个字吧";
            layedit.sync(editIndex);
        },
        userId: function (value) {
            if (value == "" || value == null) return "至少你得先登录吧！";
        },
        replyContent: function (value) {
            if ($.trim(value) == "") {
                return "至少得有一个字吧!";
            }
        }
    });
    //评论显示
    flow.load({
        elem: '#commentList' //流加载容器
        , done: function (page, next) { //执行下一页的回调
            $.get("/Comment/Get?page=" + page + "&SysCmsInfoId=" + $("#article").val(), function (res) {
                data = res.data;
                var lis = [];
                for (var i = 0; i < data.length; i++) {
                    var str = "";
                    var datas = {
                        comment: data[i].commentId
                    };
                    for (var r = 0; r < data[i].reply.length; r++) {
                        str += "<div class=\"comment-child\">\n" +
                            "      <img src=\"" + data[i].reply[r].user.headPortrait + "\" alt=\"" + data[i].reply[r].user.nickname + "\" />\n" +
                            "      <div class=\"info\">\n" +
                            "          <span class=\"username\">	" + data[i].reply[r].user.nickname + " : </span>";
                        if (data[i].reply[r].user.userId == 'admin') {
                            str += "<span class=\"is_bloger\">博主</span>&nbsp;";
                        }
                        str += "回复 <span class=\"username\">@" + data[i].user.nickname + " </span>";
                        if (data[i].user.userId == 'admin') {
                            str += "<span class=\"is_bloger\">博主</span>&nbsp;";
                        }
                        str += "：<span>" + data[i].reply[r].content + "</span>\n" +
                            "      </div>\n" +
                            "      <p class=\"info\"><span class=\"time\">" + data[i].reply[r].replyDate + "</span>&nbsp;&nbsp;&nbsp;&nbsp;<span>" + data[i].reply[r].site + "</span></p>\n" +
                            "  </div>\n";
                    }
                    lis.push("<li>\n" +
                        "               <div class=\"comment-parent\">\n" +
                        "                   <img src=\"" + data[i].user.headPortrait + "\" alt=\"" + data[i].user.nickname + "\" />\n" +
                        "                   <div class=\"info\">\n" +
                        "                       <span class=\"username\">" + data[i].user.nickname + "</span>\n");
                    if (data[i].user.userId == 'admin') {
                        lis.push("<span class=\"is_bloger\">博主</span>&nbsp;");
                    }
                    lis.push("                   </div>\n" +
                        "                   <div class=\"content\">\n" +
                        "                       " + data[i].content + "\n" +
                        "                   </div>\n" +
                        "                   <p class=\"info info-footer\"><span class=\"time\">" + data[i].commentDate + "</span>&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"time\">" + data[i].site + "</span>&nbsp;&nbsp;<a class=\"btn-reply\" style=\"color: #009688;font-size:14px;\" href=\"javascript:;\" onclick=\"btnReplyClick(this)\">回复</a></p>\n" +
                        "               </div>\n" +
                        "               <hr />\n" + str +
                        "               <!-- 回复表单默认隐藏 -->\n" +
                        "               <div class=\"replycontainer layui-hide\">\n" +
                        "                   <form class=\"layui-form\" action=\"/reply/list/\">\n" +
                        "                   <input type=\"hidden\" id=\"comment\" name=\"comment\" value=\"" + data[i].commentId + "\" />\n" +
                        "                   <input type=\"hidden\" id=\"user\" lay-verify=\"userId\" name=\"user\" value=\"" + $('#user').val() + "\" />\n" +
                        "                       <div class=\"layui-form-item\">\n" +
                        "                           <textarea name=\"content\" lay-verify=\"replyContent\" placeholder=\"回复  @" + data[i].user.nickname + ":\" class=\"layui-textarea\" style=\"min-height:80px;\"></textarea>\n" +
                        "                       </div>\n" +
                        "                       <div class=\"layui-form-item\">\n" +
                        "                           <button class=\"layui-btn layui-btn-mini\" lay-submit=\"formReply\" lay-filter=\"formReply\">提交</button>\n" +
                        "                       </div>\n" +
                        "                   </form>\n" +
                        "               </div>\n" +
                        "           </li>");
                }

                //执行下一页渲染，第二参数为：满足“加载更多”的条件，即后面仍有分页
                //pages为Ajax返回的总页数，只有当前页小于总页数的情况下，才会继续出现加载更多
                next(lis.join(''), page < res.count);
                console.log("lis:" + lis.join(''));
            }, 'json');
        }
    });

    //监听留言提交
    form.on('submit(formLeaveMessage)', function (data) {
        var index = layer.load(1);
        //console.log(data.field)
        //alert(data.field)
        //模拟留言回复
        var url = '/comment/add';
        $.ajax({
            type: "POST",
            url: url,
            data: data.field,
            success: function (res) {
                if (res.success) {
                    layer.close(index);
                    var content = data.field.content;
                    var html = '<li><div class="comment-parent"><img src="' + res.data.user.headPortrait + '" alt="' + res.data.user.nickname + '"/><div class="info"><span class="username">' + res.data.user.nickname + '</span>';
                    if (res.data.user.userId == 'admin') {
                        html += " <span class=\"is_bloger\">博主</span>&nbsp;";
                    }
                    html += '</div><div class="content">' + data.field.content + '</div><p class="info info-footer"><span class="time">' + res.data.commentDate + '</span>&nbsp;&nbsp;&nbsp;&nbsp;<span>' + res.data.site + '</span>&nbsp;&nbsp;<a class="btn-reply"href="javascript:;" style="color: #009688;font-size:14px;" onclick="btnReplyClick(this)">回复</a></p></div><hr /><!--回复表单默认隐藏--><div class="replycontainer layui-hide"><form class="layui-form"action="">            <input type="hidden" id="comment" name="comment" value="' + res.data.commentId + '" />       <input type="hidden" id="user" lay-verify="userId" name="user" value="' + res.data.user.userId + '" />                    <div class="layui-form-item"><textarea name="content"lay-verify="replyContent"placeholder="回复@' + res.data.user.nickname + '"class="layui-textarea"style="min-height:80px;"></textarea></div><div class="layui-form-item"><button class="layui-btn layui-btn-mini"lay-submit="formReply"lay-filter="formReply">提交</button></div></form></div></li>';
                    $('.blog-comment').prepend(html);
                    $('#remarkEditor').val('');
                    editIndex = layui.layedit.build('remarkEditor', {
                        height: 150,
                        tool: ['face', '|', 'left', 'center', 'right', '|', 'link'],
                    });
                    layer.msg("评论成功", {
                        icon: 1
                    });
                } else {
                    layer.msg("评论失败！");
                }
            },
            error: function (data) {
                layer.msg("网络错误！");
                layer.close(index);
            }
        });
        return false;
    });

    //监听留言回复提交
    form.on('submit(formReply)', function (data) {
        var index = layer.load(1);
        //模拟留言回复
        var url = '/comment/add';
        $.ajax({
            type: "POST",
            url: url,
            data: data.field,
            success: function (res) {
                if (res.success) {
                    layer.close(index);
                    var html = '<div class="comment-child"><img src="' + res.data.user.headPortrait + '" alt="' + res.data.user.nickname + '"/><div class="info"><span class="username">' + res.data.user.nickname + ' : </span>';
                    if (res.data.user.userId == 'admin') {
                        html += " <span class=\"is_bloger\">博主</span>&nbsp;";
                    }
                    html += "回复 <span class=\"username\">@" + res.data.comment.user.nickname + " </span>";
                    if (res.data.comment.user.userId == 'admin') {
                        html += " <span class=\"is_bloger\">博主</span>&nbsp;";
                    }
                    html += '：<span>' + data.field.content + '</span></div><p class="info"><span class="time">' + res.data.replyDate + '</span>&nbsp;&nbsp;&nbsp;&nbsp;<span>' + res.data.site + '</span></p></div>';
                    $(data.form).find('textarea').val('');
                    $(data.form).parent('.replycontainer').before(html).siblings('.comment-parent').children('p').children('a').click();
                    layer.msg("回复成功", {
                        icon: 1
                    });
                } else {
                    layer.msg("回复失败！");
                }
            },
            error: function (data) {
                layer.msg("网络错误！");
            }
        });
        return false;
    });
});

function btnReplyClick(elem) {
    var $ = layui.jquery;
    $(elem).parent('p').parent('.comment-parent').siblings('.replycontainer').toggleClass('layui-hide');
    if ($(elem).text() == '回复') {
        $(elem).text('收起')
    } else {
        $(elem).text('回复')
    }
}

$(document).ready(function() {
    $(".fa-file-text").parent().parent().addClass("layui-this");
});
