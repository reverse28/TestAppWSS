using System.ComponentModel.DataAnnotations;
using TestAppWSS.Domain.Entities.Base;

namespace TestAppWSS.Domain
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


        public virtual Node? Parent { get; set; }
        public virtual ICollection<Node>? Children { get; set; }
    }
}