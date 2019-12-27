using Ator.Model.Api.Comment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.IService
{
    public interface ISysCmsInfoCommentService
    {
        List<Comment> GetComments(string SysCmsInfoId, string SysCmsColumnId, int Page, int Limit,out int  Total);

        Comment GetComment(string SysCmsInfoCommentId);

        Reply GetReply(string SysCmsInfoCommentId);
    }
}
