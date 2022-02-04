using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotezApp.Domain.Models
{
    [Table("notes")]
    public class Note
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("title")]
        public string Title { get; set; } = "";

        [Column("content")]
        public string Content { get; set; } = "";

        [Column("user_id")]
        public long UserId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("user_id")]
        public virtual User? User { get; set; }
    }
}
