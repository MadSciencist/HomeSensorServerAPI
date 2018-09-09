using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models.Dictionaries
{
    [Table("dictionary_genders")]
    public class UserGender : Entity, IDictionaryModel
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}
