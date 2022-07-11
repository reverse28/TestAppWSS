using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestAppWSS.Domain.Entities.Base.Interfaces;

namespace TestAppWSS.Domain.Entities.Base
{
    public abstract class Entity: IEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
