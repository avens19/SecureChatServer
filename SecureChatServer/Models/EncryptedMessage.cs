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
    /// Encapsulates a message that has been encrypted by a given user's (the recipient) public key
    /// </summary>
    public class EncryptedMessage
    {
        /// <summary>
        /// The unique id for this encrypted message
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        /// <summary>
        /// The base 64 encoded message that was encrypted
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// The user that will be able to read this message with their private key
        /// </summary>
        public virtual User Recipient { get; set; }
        /// <summary>
        /// The date this message was read by the recipient
        /// </summary>
        public DateTime? DateRead { get; set; }
        /// <summary>
        /// The public key this message was encrypted with
        /// </summary>
        public string PublicKey { get; set; }

        public EncryptedMessageViewModel ToViewModel()
        {
            return new EncryptedMessageViewModel
            {
                id = Id,
                content = Content,
                recipientUsername = Recipient.UserName,
                dateRead = DateRead,
                publicKey = PublicKey
            };
        }
    }
}