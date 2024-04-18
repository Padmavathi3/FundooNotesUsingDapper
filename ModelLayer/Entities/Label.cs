﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entities
{
    public class Label
    {
        [ForeignKey("UserNote")]
        public string NoteId { get; set; } = "";

        [ForeignKey("User")]
        public string Email { get; set; } = "";
        public string LabelName { get; set; } = "";
    }
}
