using NotezApp.Business.Interfaces;
using NotezApp.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotezApp.Business.Services
{
    public abstract class Service : IServiceScoped
    {
        protected readonly NotezContext _context;

        protected Service(NotezContext context)
        {
            this._context = context;
        }
    }
}
