using Microsoft.EntityFrameworkCore;
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
    public class NoteService : Service
    {
        public NoteService(NotezContext context) : base(context)
        {
        }

        public List<Note>? ListAll(long userId)
        {
            try
            {
                var notes = _context.Notes
                    .AsNoTracking()
                    .Include(n => n.User)
                    .Where(n => n.UserId == userId)
                    .ToList();

                return notes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ListNoteResponse ListNoteById(long userId, long noteId)
        {
            try
            {
                ListNoteResponse response = new ListNoteResponse();

                Note? note = _context.Notes.AsNoTracking().Include(n => n.User).FirstOrDefault(n => n.Id == noteId && n.UserId == userId);

                if (note == null)
                {
                    response.Note = note;
                    response.Message = "Não foi encontrada nenhuma nota para o Id informado";
                    response.NotFound = true;

                    return response;
                }

                response.Note = note;
                response.NotFound = false;
                response.Message = "Nota encontrada com sucesso";

                return response;
            }
            catch (Exception ex)
            {
                ListNoteResponse response = new ListNoteResponse
                {
                    Note = null,
                    Message = ex.Message,
                    NotFound = false
                };

                return response;
            }
        }

        public async Task<Note?> CreateNote(long userId, CreateNoteRequest createNoteRequest)
        {
            using (await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Note note = new Note
                    {
                        Title = createNoteRequest.Title,
                        Content = createNoteRequest.Content,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        UserId = userId
                    };

                    await _context.Notes.AddAsync(note);
                    await _context.SaveChangesAsync();

                    await _context.Database.CommitTransactionAsync();
                    await _context.SaveChangesAsync();

                    return note;
                }
                catch (Exception ex)
                {
                    await _context.Database.RollbackTransactionAsync();

                    return null;
                }
            }
        }

        public async Task<CreateNoteResponse> UpdateNote(long userId, long noteId, CreateNoteRequest createNoteRequest)
        {
            CreateNoteResponse response = new CreateNoteResponse();

            using (await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Note? note = _context.Notes.FirstOrDefault(n => n.Id == noteId);

                    if (note == null)
                    {
                        response.Note = note;
                        response.Message = "Não foi possível recuperar a nota";
                        response.Forbidden = false;

                        return response;
                    }

                    if (note.UserId != userId)
                    {
                        response.Note = null;
                        response.Message = "Nota não pertence ao usuário";
                        response.Forbidden = true;

                        return response;
                    }

                    note.Title = createNoteRequest.Title;
                    note.Content = createNoteRequest.Content;
                    note.UpdatedAt = DateTime.UtcNow;

                    _context.Notes.Update(note);
                    await _context.SaveChangesAsync();

                    await _context.Database.CommitTransactionAsync();
                    await _context.SaveChangesAsync();

                    response.Note = note;
                    response.Message = "Nota atualizada com sucesso";

                    return response;
                }
                catch (Exception ex)
                {
                    using (await _context.Database.BeginTransactionAsync())
                    {
                        await _context.Database.RollbackTransactionAsync();
                    }

                    response.Note = null;
                    response.Message = ex.Message;
                    response.Forbidden = false;

                    return response;
                }
            }
        }

        public async Task<DeleteNoteResponse> DeleteNote(long userId, long noteId)
        {
            DeleteNoteResponse response = new DeleteNoteResponse();

            using (await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Note? note = _context.Notes.FirstOrDefault(n => n.Id == noteId);

                    if (note == null)
                    {
                        response.Deleted = false;
                        response.Message = "Não foi possível encontrar a nota com o Id especificado";
                        response.Forbidden = false;

                        return response;
                    }

                    if (note.UserId != userId)
                    {
                        response.Deleted = false;
                        response.Forbidden = true;
                        response.Message = "Nota não pertence ao usuário";

                        return response;
                    }

                    _context.Notes.Remove(note);
                    await _context.SaveChangesAsync();

                    response.Deleted = true;
                    response.Message = "Nota deletada com sucesso";
                    response.Forbidden = false;

                    await _context.Database.CommitTransactionAsync();
                    await _context.SaveChangesAsync();

                    return response;
                }
                catch (Exception ex)
                {
                    using (await _context.Database.BeginTransactionAsync())
                    {
                        await _context.Database.RollbackTransactionAsync();
                    }

                    response.Deleted = false;
                    response.Message = ex.Message;
                    response.Forbidden = false;

                    return response;
                }
            }
        }

        public async Task<bool> DeleteAllNotes(long userId)
        {
            using (await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    List<Note> notes = _context.Notes.Where(n => n.UserId == userId).ToList();

                    _context.Notes.RemoveRange(notes);
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
