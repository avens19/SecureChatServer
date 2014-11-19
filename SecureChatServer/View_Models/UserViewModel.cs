using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecureChatServer.View_Models
{
    public class UserViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string publicKey { get; set; }
        public DateTime dateCreated { get; set; }
    }
}