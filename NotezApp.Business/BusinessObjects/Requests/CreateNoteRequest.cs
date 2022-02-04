﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotezApp.Business.BusinessObjects.Requests
{
    public class CreateNoteRequest
    {
        public string Title { get; set; } = String.Empty;
        public string Content { get; set; } = String.Empty;
    }
}
