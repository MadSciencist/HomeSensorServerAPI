using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models.Dictionaries
{
    [Table("dictionary_sensor_types")]
    public class SensorType : Entity, IDictionaryModel
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}
