using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models
{
    [Table("nodes")]
    public class Node : Entity
    {
        [DisplayName("Nazwa")]
        public string Name { get; set; }
        [DisplayName("Identyfikator")]
        public string Identifier { get; set; }
        [DisplayName("Login")]
        public string LoginName { get; set; }
        [DisplayName("Haslo")]
        public string LoginPassword { get; set; }
        [DisplayName("Typ")]
        public string Type { get; set; }
        [DisplayName("Szczegóły")]
        public string ExactType { get; set; }
        [DisplayName("Adres IP")]
        public string IpAddress { get; set; }
        [DisplayName("Adres IP bramki")]
        public string GatewayAddress { get; set; }
        [DisplayName("Włączony")]
        public bool IsOn { get; set; }
    }
}
