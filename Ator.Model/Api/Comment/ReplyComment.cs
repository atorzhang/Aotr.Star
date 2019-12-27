using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Model.Api.Comment
{
    public class ReplyComment
    {
        public string commentId { get; set; }
        public string article { get; set; }
        public CommentUser user { get; set; } = new CommentUser();
        public string content { get; set; }
        public string commentDate { get; set; }
        public string site { get; set; }
    }
}
