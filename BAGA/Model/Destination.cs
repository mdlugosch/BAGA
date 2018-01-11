using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Table("Locations", Schema = "baga")]
    public class Destination
    {
        public Destination()
        {
            //this.Lodgings = new List<Lodging>();
        }

        [Column("LocationID")]
        public virtual int DestinationId { get; set; }
        [Required, Column("LocationName")]
        [MaxLength(200)]
        public virtual string Name { get; set; }
        public virtual string Country { get; set; }
        [MaxLength(500)]
        public virtual string Description { get; set; }
        [Column(TypeName = "image")]
        public virtual byte[] Photo { get; set; }
        public virtual string TravelWarnings { get; set; }
        public virtual string ClimateInfo { get; set; }

        // virtual 
        //public List<Lodging> Lodgings { get; set; }
        public virtual ICollection<Lodging> Lodgings { get; set; }
    }
}
