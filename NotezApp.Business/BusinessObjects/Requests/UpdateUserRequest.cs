using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotezApp.Business.BusinessObjects.Requests
{
    public class UpdateUserRequest
    {
        public string Name { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }
}
