using System.ComponentModel.DataAnnotations;
using TestAppWSS.Domain.Entities.Base;

namespace TestAppWSS.Domain.Entities
{
    public class Node: Entity
    {
        [Required]
        [MaxLength(24)]
        public string Name { get; set; } = null!;

        [Required]
        public int Depth { get; set; } //глубина нахождения

        [Display(Name = "Parent")]
        public int? ParentId { get; set; }


        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Node? Parent { get; set; }


        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<Node>? Children { get; set; }
    }
}