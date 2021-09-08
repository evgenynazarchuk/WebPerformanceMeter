namespace GrpcWebApplication.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserAccount
    {
        public int Id { get; set; }

        public string Name { get; set; }

        List<UserGroup> UserGroups { get; set; }
    }
}
