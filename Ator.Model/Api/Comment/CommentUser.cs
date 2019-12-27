using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Model.Api.Comment
{
    public class CommentUser
    {
        public string userId { get; set; }
        public string userType { get; set; }
        public string nickname { get; set; }
        public string headPortrait { get; set; }
        public string sex { get; set; }
        public string registrationDate { get; set; }
        public string latelyLoginTime { get; set; }
        public int? commentNum { get; set; }

    }
}
