using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NotezApp.Domain.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = "";

        [Column("email")]
        public string Email { get; set; } = "";

        [JsonIgnore]
        [Column("password")]
        public string Password { get; set; } = "";

        [Column("role")]
        public string Role { get; set; } = "";
    }
}
