namespace GrpcWebApplication.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Message
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public MessageType MessageType { get; set; }

        public int FromUserAccountId { get; set; }

        public UserAccount FromUserAccount { get; set; }

        public int ToUserAccountId { get; set; }

        public UserAccount ToUserAccount { get; set; }

        public int UserGroupId { get; set; }

        public UserGroup UserGroup { get; set; }
    }
}
