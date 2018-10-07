using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models
{
    public class Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
    }
}
