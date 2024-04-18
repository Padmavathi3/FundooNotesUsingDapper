using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface ICollaborator
    {
        public Task<int> AddCollaborator(Collaborator re_var);
        public Task<IEnumerable<Collaborator>> GetAllCollaborators(string email);
        public Task<int> DeleteCollaborator(int cid, int nid);
    }
}
