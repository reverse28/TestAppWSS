using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using TestAppWSS.Domain.Entities.Base;

namespace TestAppWSS.Domain.Entities
{
    public class Node : Entity
    {
        [Required]
        [MaxLength(24)]
        public string Name { get; set; } = null!;

        [Required]
        public int Depth { get; set; } //глубина нахождения

        [Required]
        public int DepthId { get; set; } //Id на текущей глубине 

        [Display(Name = "Parent")]
        public int? ParentId { get; set; }


        [JsonIgnore, XmlIgnoreAttribute, NotMapped]
        public virtual Node? Parent { get; set; }


        [JsonIgnore, XmlIgnoreAttribute, NotMapped]
        public virtual ICollection<Node>? Children { get; set; }
    }
}