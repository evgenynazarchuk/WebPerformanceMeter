namespace GrpcWebApplication.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public class Message
    {
        public Guid Id {  get; set; }

        public string Text {  get; set; }
    }
}
