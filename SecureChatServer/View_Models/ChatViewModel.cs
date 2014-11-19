using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SecureChatServer.Models;

namespace SecureChatServer.View_Models
{
    public class ChatViewModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public UserViewModel[] chatters { get; set; }
        public DateTime dateCreated { get; set; }
    }
}