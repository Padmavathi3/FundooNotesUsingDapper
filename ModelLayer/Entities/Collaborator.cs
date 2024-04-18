using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entities
{
    public class Collaborator
    {
        [Key]
        public int CollaboratorId { get; set; } 

        [ForeignKey("UserNote")]
        public int NoteId { get; set; } 

        [ForeignKey("User")]
        public string CollaboratorEmail { get; set; } 
    }
}
