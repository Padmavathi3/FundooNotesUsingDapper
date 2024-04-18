using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ModelLayer.Entities
{
    public class Label
    {
        [ForeignKey("UserNote")]
        public int NoteId { get; set; } 

        [ForeignKey("User")]
        [JsonIgnore]
        public string? Email { get; set; } 
        public string LabelName { get; set; } 
    }
}
