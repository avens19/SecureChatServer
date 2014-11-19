using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecureChatServer.View_Models
{
    public class MessageViewModel
    {
        public long id { get; set; }
        public EncryptedMessageViewModel[] contents { get; set; }
        public UserViewModel sender { get; set; }
        public string messageType { get; set; }
        public DateTime dateCreated { get; set; }
    }
}