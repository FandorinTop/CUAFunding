using CUAFunding.DomainEntities.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CUAFunding.DomainEntities.Entities
{
    public class Message : BaseEntity
    {
        [ForeignKey(nameof(ChatUser))]
        public string ChatUserId { get; set; }

        public virtual ChatUser ChatUser { get; set; }

        [MaxLength(2000)]
        public string Value { get; set; }

        public DateTime SendingTyme { get; set; }
    }
}
