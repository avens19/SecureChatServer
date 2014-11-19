using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SecureChatServer.View_Models;

namespace SecureChatServer.Models
{
    /// <summary>
    /// Represents a single message sent from one user to at least one other user
    /// </summary>
    public class Message
    {
        /// <summary>
        /// The unique id for this message
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        /// <summary>
        /// The encrypted contents of this message, one for each recipient and one again for the sender
        /// </summary>
        public virtual ICollection<EncryptedMessage> Contents { get; set; }
        /// <summary>
        /// The user that sent this message
        /// </summary>
        public virtual User Sender { get; set; }
        /// <summary>
        /// The date this message was created
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// The type of message
        /// </summary>
        public MessageType Type { get; set; }

        public MessageViewModel ToViewModel()
        {
            return new MessageViewModel
            {
                id = Id,
                contents = Contents.Select(c => c.ToViewModel()).ToArray(),
                sender = Sender.ToViewModel(),
                dateCreated = DateCreated,
                messageType = Type.ToString()
            };
        }
    }
}