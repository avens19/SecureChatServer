using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecureChatServer.View_Models
{
    public class EncryptedMessageViewModel
    {
        public long id { get; set; }
        public string content { get; set; }
        public string recipientUsername { get; set; }
        public string publicKey { get; set; }
        public DateTime? dateRead { get; set; }
    }
}