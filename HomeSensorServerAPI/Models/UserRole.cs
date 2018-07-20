using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models
{
    [Table("dictionary_roles")]
    public class UserRole : Entity
    {
        public int Value { get; set; }
        public string Dictionary { get; set; }
    }
}
