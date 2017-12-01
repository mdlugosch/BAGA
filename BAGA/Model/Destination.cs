using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Destination
    {
        public int DestinationId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Country { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }

        [Column(TypeName="image")]
        public byte[] Photo { get; set; }
        public List<Lodging> Lodgings { get; set; }
    }
}
