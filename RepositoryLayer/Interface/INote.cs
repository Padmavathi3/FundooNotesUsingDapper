using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface INote
    {
        public Task<int> CreateNote(Note re_var);

        public Task<IEnumerable<Note>> GetNotesById(int id);
        public Task<IEnumerable<Note>> GetAllNotes();
        public Task<IEnumerable<Note>> GetNotesByEmail(string email);

        public Task<IEnumerable<Note>> GetNotesByEmail(string email);
        public Task<int> UpdateNote(int id, Note re_var);
        public Task<int> DeleteNote(int id, string email);
        public Task<int> ArchiveNote(int id,string email);
        public Task<int> PinnNote(int id, string email);
        public Task<int> TrashNote(int id, string email);
        public Task<int> UpdateColour(int id, string colour);

    }
}
