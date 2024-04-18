using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entities
{
    public class Note
    {
        [Key]
        public int NoteId { get; set; } 

        public string Title { get; set; }

        public string Description { get; set; } 

        public DateTime Reminder { get; set; }

        public bool IsArchive { get; set; }=false;

        public bool IsPinned { get; set; } = false;

        public bool IsTrash { get; set; } = false;
        public string IsColour {  get; set; }

        [ForeignKey("User")]
        public string EmailId { get; set; }


    }
}
