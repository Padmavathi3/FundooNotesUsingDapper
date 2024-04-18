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
    public class LabelServiceBl : ILabelBl
    {
        private readonly ILabel label;

        public LabelServiceBl(ILabel label)
        {
            this.label = label;
        }
        public Task<int> AddLabel(Label re_var)
        {
            return label.AddLabel(re_var);
        }

        public Task<int> DeleteLabel(string name, string email)
        {
            return label.DeleteLabel(name, email);
        }

        public Task<IEnumerable<Label>> GetUserNoteLabels(string email)
        {
            return label.GetUserNoteLabels(email);
        }

        public Task<int> UpdateName(string newLabelName, string oldLabelName, string email)
        {
            return label.UpdateName(newLabelName, oldLabelName, email);
        }
    }
}
