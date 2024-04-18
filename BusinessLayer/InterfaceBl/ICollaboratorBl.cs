using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.InterfaceBl
{
    public interface ICollaboratorBl
    {
        public Task<int> AddCollaborator(Collaborator re_var);
        public Task<IEnumerable<Collaborator>> GetAllCollaborators(string email);
        public Task<int> DeleteCollaborator(int cid, int nid);
    }
}
