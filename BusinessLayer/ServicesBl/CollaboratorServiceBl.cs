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
    public class CollaboratorServiceBl : ICollaboratorBl
    {
        private readonly ICollaborator collab;
        public CollaboratorServiceBl(ICollaborator collab) 
        {
            this.collab = collab;
        }
        public Task<int> AddCollaborator(Collaborator re_var)
        {
            return collab.AddCollaborator(re_var);
        }

        public Task<int> DeleteCollaborator(int cid, int nid)
        {
            return collab.DeleteCollaborator(cid, nid);
        }

        public Task<IEnumerable<Collaborator>> GetAllCollaborators(int nid)
        {
            return collab.GetAllCollaborators(nid);
        }
    }
}
