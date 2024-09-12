using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CUAFunding.DomainEntities.Entities.Base;

namespace CUAFunding.DomainEntities.Entities
{
    public class ChatUser : BaseEntity
    {
        [ForeignKey(nameof(Chat))]
        public string ChatId { get; set; }

        public virtual Chat Chat { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public bool IsBanned { get; set; }

        public ChatState State { get; set; }

        public virtual List<Message> Messages { get; set; } = new List<Message>();
    }
}
