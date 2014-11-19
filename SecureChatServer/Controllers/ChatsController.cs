using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.AspNet.Identity;
using SecureChatServer.Models;
using SecureChatServer.View_Models;

namespace SecureChatServer.Controllers
{
    [Authorize]
    [RoutePrefix("api/Chats")]
    public class ChatsController : ApiController
    {
        private ApplicationDbContext _db = ApplicationDbContext.Create();

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateChat(ChatViewModel chat)
        {
            string myUsername = User.Identity.GetUserName();

            if (chat == null || chat.chatters == null || !chat.chatters.Any())
            {
                return BadRequest("You must specify at least one user to chat with");
            }

            if (chat.chatters.Count() > 1 && string.IsNullOrWhiteSpace(chat.name))
            {
                return BadRequest("Group chats must specify a name");
            }

            if (chat.chatters.Any(c => c.username == myUsername))
            {
                return BadRequest("Can't start a message with yourself");
            }

            if (chat.chatters.Any(c => !_db.Users.Any(u => u.UserName == c.username)))
            {
                return BadRequest("One of the users does not exist");
            }

            string firstChatter = chat.chatters[0].username;

            if (
                _db.Chats.Any(
                    c =>
                        c.Chatters.Count() == 2 && c.Chatters.Any(u => u.UserName == myUsername) &&
                        c.Chatters.Any(u => u.UserName == firstChatter)))
            {
                return BadRequest("You already have a chat with this user");
            }

            var chatters = new List<User> {GetUser(myUsername)};
            chatters.AddRange(chat.chatters.Select(c => GetUser(c.username)));

            Chat newChat = _db.Chats.Add(new Chat
            {
                Chatters = chatters.ToArray(),
                Name = chat.name,
                DateCreated = DateTime.UtcNow
            });

            _db.SaveChanges();

            return Created(string.Format("api/Chat/{0}", newChat.Id), newChat.ToViewModel());
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetChats()
        {
            string myUsername = User.Identity.GetUserName();

            var chats = _db.Chats.Where(c => c.Chatters.Any(u => u.UserName == myUsername)).ToArray();

            return Ok(chats.Select(c => c.ToViewModel()).ToArray());
        }

        [HttpPost]
        [Route("{id}/Messages")]
        public IHttpActionResult AddMessage(long id, MessageViewModel message)
        {
            string myUsername = User.Identity.GetUserName();

            Chat chat = _db.Chats.Single(c => c.Id == id);

            if (chat == null)
            {
                return NotFound();
            }

            if (chat.Chatters.All(u => u.UserName != myUsername))
            {
                return Unauthorized();
            }

            if (!message.contents.Any())
            {
                return BadRequest("Message must have contents");
            }

            MessageType type;

            if (!MessageType.TryParse(message.messageType, out type))
            {
                return BadRequest("Message type doesn't exist");
            }

            if (message.contents.Any(c => string.IsNullOrWhiteSpace(c.content)))
            {
                return BadRequest("Message must have contents");
            }

            if (message.contents.Any(c => GetUser(c.recipientUsername) == null))
            {
                return BadRequest("Recipient doesn't exist");
            }

            if (message.contents.Any(c => chat.Chatters.All(u => u.UserName != c.recipientUsername)) || 
                chat.Chatters.Any(u => message.contents.All(c => c.recipientUsername != u.UserName)))
            {
                return BadRequest("Message must go to all users in the chat, and only those users");
            }

            var m = new Message
            {
                Contents = message.contents.Select(em => new EncryptedMessage
                {
                    Content = em.content,
                    Recipient = GetUser(em.recipientUsername),
                    PublicKey = GetUser(em.recipientUsername).PublicKey
                }).ToArray(),
                DateCreated = DateTime.UtcNow,
                Sender = GetUser(myUsername)
            };
            
            chat.Messages.Add(m);

            _db.SaveChanges();

            return Created(string.Format("api/Chats/{0}/Messages/{1}",id,m.Id),m.ToViewModel());
        }

        [HttpGet]
        [Route("{id}/Messages")]
        public IHttpActionResult GetMessages(long id, DateTime? watermark)
        {
            string myUsername = User.Identity.GetUserName();

            Chat chat = _db.Chats.SingleOrDefault(c => c.Id == id);

            if (chat == null)
            {
                return NotFound();
            }

            if (chat.Chatters.All(u => u.UserName != myUsername))
            {
                return Unauthorized();
            }

            return Ok(chat.Messages.Where(m => watermark == null || m.DateCreated >= watermark).Select(m => m.ToViewModel()));
        }

        private User GetUser(string username)
        {
            return _db.Users.SingleOrDefault(u => u.UserName == username);
        }
    }
}
