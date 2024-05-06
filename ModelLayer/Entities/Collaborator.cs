using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ModelLayer.Entities
{
    public class Collaborator
    {
        

        [ForeignKey("UserNote")]
        public int NoteId { get; set; } 

        [ForeignKey("User")]
        public string CollaboratorEmail { get; set; }
        [JsonIgnore]
        public string? OwnerEmail { get; set; }
    }
}
