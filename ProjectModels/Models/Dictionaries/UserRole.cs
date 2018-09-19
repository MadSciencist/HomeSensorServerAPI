using HomeSensorServerAPI.Models.Dictionaries;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models.Dictionaries
{
    [Table("dictionary_roles")]
    public class UserRole : Entity, IDictionaryModel
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}
