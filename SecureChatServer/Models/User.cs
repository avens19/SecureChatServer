using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SecureChatServer.View_Models;

namespace SecureChatServer.Models
{
    public class User: IdentityUser
    {
        /// <summary>
        /// The given name of this user
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The public key for this user. All messages sent to this user will be encrypted with this key
        /// </summary>
        public string PublicKey { get; set; }
        /// <summary>
        /// The date this user was created
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// This user's chats
        /// </summary>
        public virtual ICollection<Chat> Chats { get; set; }

        /// <summary>
        /// Creates a user identity token
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="authenticationType"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            
            return userIdentity;
        }

        public UserViewModel ToViewModel()
        {
            return new UserViewModel
            {
                id = Id,
                dateCreated = DateCreated,
                name = Name,
                publicKey = PublicKey,
                username = UserName
            };
        }
    }
}