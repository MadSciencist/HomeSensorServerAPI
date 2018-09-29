using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models.Dictionaries
{
    [Table("dictionary_actuator_types")]
    public class ActuatorType : Entity, IDictionaryModel
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}
