using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Model.Api.Comment
{
    public class Reply
    {
        public string replyId { get; set; }
        public string content { get; set; }
        public string replyDate { get; set; }
        public string site { get; set; }
        public CommentUser user { get; set; } = new CommentUser();
        public ReplyComment comment { get; set; }
    }
}
