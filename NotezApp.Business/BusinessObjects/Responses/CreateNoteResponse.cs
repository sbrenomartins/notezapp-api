using NotezApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotezApp.Business.BusinessObjects.Responses
{
    public class CreateNoteResponse
    {
        public Note? Note { get; set; }
        public string Message { get; set; } = String.Empty;
        public bool Forbidden { get; set; }
    }
}
