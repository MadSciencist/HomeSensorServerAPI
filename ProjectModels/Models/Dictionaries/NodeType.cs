using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models.Dictionaries
{
    [Table("dictionary_node_types")]
    public class NodeType : Entity, IDictionaryModel
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}
