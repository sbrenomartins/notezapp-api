using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotezApp.Business.BusinessObjects.Responses
{
    public class CheckUserResponse
    {
        public bool Exists { get; set; }
        public string? Email { get; set; } = String.Empty;
        public string Error { get; set; } = String.Empty;
    }
}
