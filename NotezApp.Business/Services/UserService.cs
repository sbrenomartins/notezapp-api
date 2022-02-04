using NotezApp.Business.BusinessObjects.Requests;
using NotezApp.Business.BusinessObjects.Responses;
using NotezApp.Data.Contexts;
using NotezApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotezApp.Business.Services
{
    public class UserService : Service
    {
        public UserService(NotezContext context) : base(context)
        {
        }

        public List<User> ListUsersAsync()
        {
            var users = _context.Users.ToList();
            return users;
        }

        public async Task<CreateUserResponse> CreateUser(string name, string email, string password)
        {
            CreateUserResponse response = new CreateUserResponse();

            using (await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    User? user = _context.Users.FirstOrDefault(u => u.Email == email);

                    if (user != null)
                    {
                        response.User = null;
                        response.Exists = true;
                        response.Message = "Email já cadastrado";

                        return response;
                    }

                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                    user = new User
                    {
                        Email = email,
                        Name = name,
                        Password = hashedPassword,
                        Role = "user"
                    };

                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();

                    await _context.Database.CommitTransactionAsync();
                    await _context.SaveChangesAsync();

                    response.Exists = false;
                    response.User = user;
                    response.Message = "Usuário criado com sucesso";

                    return response;
                }
                catch (Exception ex)
                {
                    using (await _context.Database.BeginTransactionAsync())
                    {
                        await _context.Database.RollbackTransactionAsync();
                    }

                    response.Exists = false;
                    response.User = null;
                    response.Message = ex.Message;

                    return response;
                }
            }
        }

        public async Task<UpdateUserResponse> UpdateUser(long id, UpdateUserRequest updateUserRequest)
        {
            using (await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    User user = _context.Users.First(u => u.Id == id);

                    user.Name = updateUserRequest.Name;

                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(updateUserRequest.Password);
                    user.Password = hashedPassword;

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    await _context.Database.CommitTransactionAsync();
                    await _context.SaveChangesAsync();

                    UpdateUserResponse response = new UpdateUserResponse
                    {
                        Message = "Usuário atualizado com sucesso",
                        User = user
                    };

                    return response;
                }
                catch (Exception ex)
                {
                    using (await _context.Database.BeginTransactionAsync())
                    {
                        await _context.Database.RollbackTransactionAsync();
                    }

                    UpdateUserResponse response = new UpdateUserResponse
                    {
                        Message = "Ocorreu um erro ao tentar atualizar o usuário",
                        User = null
                    };

                    return response;
                }
            }
        }

        public async Task<bool> DeleteAccount(long userId)
        {
            using (await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    User user = _context.Users.First(u => u.Id == userId);
                    List<Note> userNotes = _context.Notes.Where(n => n.UserId == userId).ToList();

                    _context.Notes.RemoveRange(userNotes);
                    await _context.SaveChangesAsync();

                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();

                    await _context.Database.CommitTransactionAsync();
                    await _context.SaveChangesAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    using (await _context.Database.BeginTransactionAsync())
                    {
                        await _context.Database.RollbackTransactionAsync();
                    }

                    return false;
                }
            }
        }
    }
}
