namespace GrpcWebApplication.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserGroup
    {
        public int Id { get; set; }

        public string Name { get; set; }

        List<UserAccount> UserAccounts { get; set; }
    }
}
