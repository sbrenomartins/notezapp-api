using NotezApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotezApp.Business.BusinessObjects.Responses
{
    public class CreateUserResponse
    {
        public User? User { get; set; }
        public bool Exists { get; set; }
        public string Message { get; set; } = String.Empty;
    }
}
