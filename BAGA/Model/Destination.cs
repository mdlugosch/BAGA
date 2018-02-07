using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Validation;

namespace Model
{
    [Table("Locations", Schema = "baga")]
    public class Destination : IObjectWithState
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
        [CustomValidation(typeof(BusinessValidations), "DescriptionRules")]
        public virtual string Description { get; set; }
        [Column(TypeName = "image")]
        public virtual byte[] Photo { get; set; }
        public virtual string TravelWarnings { get; set; }
        public virtual string ClimateInfo { get; set; }

        // virtual 
        //public List<Lodging> Lodgings { get; set; }
        public virtual ICollection<Lodging> Lodgings { get; set; }

        public State State { get; set; }

        public List<string> ModifiedProperties { get; set; }
    }
    }
