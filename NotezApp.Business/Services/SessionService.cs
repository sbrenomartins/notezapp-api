using NotezApp.Data.Contexts;
using NotezApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using NotezApp.Business.BusinessObjects.Responses;
using NotezApp.Business.Helpers;

namespace NotezApp.Business.Services
{
    public class SessionService : Service
    {
        public SessionService(NotezContext context) : base(context)
        {
        }

        public CheckUserResponse CheckUserByEmail(string email)
        {
            CheckUserResponse response = new CheckUserResponse();

            try
            {
                User? user = _context.Users.FirstOrDefault(u => u.Email == email);

                if (user == null)
                {
                    response.Email = email;
                    response.Exists = false;

                    return response;
                }

                response.Email = email;
                response.Exists = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Email = null;
                response.Exists = false;
                response.Error = ex.Message;

                return response;
            }

            
        }

        public AuthenticateUserResponse Authenticate(string email, string password)
        {
            AuthenticateUserResponse response = new AuthenticateUserResponse();

            try
            {
                User? user = _context.Users.FirstOrDefault(u => u.Email == email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    response.User = null;
                    response.Token = null;
                    response.Error = "Usuário e/ou senha incorretos";

                    return response;
                }

                var token = Token.GenerateToken(user);

                response.User = user;
                response.Token = token;

                return response;
            }
            catch (Exception ex)
            {
                response.User = null;
                response.Token = null;
                response.Error = ex.Message;

                return response;
            }
        }
    }
}
