using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface ILabel
    {
        //Add Label
        public Task<int> AddLabel(Label re_var);

        //Get
        public Task<IEnumerable<Label>> GetUserNoteLabels(string email);

        //update label name
        public Task<int> UpdateName(string newLabelName, string oldLabelName, string email);

        //delete
        public Task<int> DeleteLabel(string name, string email);
    }
}
