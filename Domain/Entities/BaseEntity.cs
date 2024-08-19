using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [DefaultDateNow]
        public DateTime CreatedAt { get; set; }

        [DefaultDateNow]
        public DateTime? UpdatedAt { get; set; }

        public bool Ativo { get; set; }

        protected BaseEntity()
        {
            this.CreatedAt = DateTime.Now;
        }
    }
}
