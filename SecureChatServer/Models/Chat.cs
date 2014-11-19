using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SecureChatServer.View_Models;

namespace SecureChatServer.Models
{
    public class Chat
    {
        /// <summary>
        /// The unique id of this chat
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        /// <summary>
        /// The name of this chat (used for group chats only)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The date this chat was created
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// The users involved in this chat
        /// </summary>
        public virtual ICollection<User> Chatters { get; set; }
        /// <summary>
        /// The messages of this chat
        /// </summary>
        public virtual ICollection<Message> Messages { get; set; }

        public ChatViewModel ToViewModel()
        {
            return new ChatViewModel
            {
                id = Id,
                name = Name,
                chatters = Chatters.Select(c => c.ToViewModel()).ToArray(),
                dateCreated = DateCreated
            };
        }
    }
}