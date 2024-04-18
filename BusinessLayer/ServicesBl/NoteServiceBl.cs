using BusinessLayer.InterfaceBl;
using ModelLayer.Entities;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ServicesBl
{
    public class NoteServiceBl : INoteBl
    {
        private readonly INote note;

        public NoteServiceBl(INote note)
        {
            this.note = note;
        }

        public Task<int> CreateNote(Note re_var)
        {
            //return note.CreateNote(re_var.NoteId, re_var.Title, re_var.Description, re_var.Reminder, re_var.IsArchive, re_var.IsPinned, re_var.IsTrash, re_var.EmailId, re_var.Colour);
            return note.CreateNote(re_var);
        }
     
        public Task<int> DeleteNote(int id, string email)
        {
            return note.DeleteNote(id, email);  
        }

        public Task<IEnumerable<Note>> GetNotesByEmail(string email)
        {
            return note.GetNotesByEmail(email);
        }

        public Task<int> UpdateNote(int id, Note re_var)
        {
            return note.UpdateNote(id, re_var);
        }
        public Task<int> ArchiveNote(int id, string email)
        {
            return note.ArchiveNote(id, email);
        }
        public Task<int> PinnNote(int id, string email)
        {
            return note.PinnNote(id, email);
        }
        public Task<int> TrashNote(int id, string email)
        {
            return note.TrashNote(id, email);
        }


    }
}
