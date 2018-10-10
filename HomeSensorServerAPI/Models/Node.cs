using HomeSensorServerAPI.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models
{
    [Table("nodes")]
    public class Node : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string Identifier { get; set; }

        [BindNever]
        public User Creator { get; set; }

        public string LoginName { get; set; }
        public string LoginPassword { get; set; }
        public ENodeType? NodeType { get; set; }
        public string RegistredProperties { get; set; }
        public string IpAddress { get; set; }
        public string GatewayAddress { get; set; }
        public bool IsOn { get; set; }
    }
}
