using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Models
{
    [Table("dictionary_genders")]
    public class UserGender : Entity
    {
        public int Value { get; set; }
        public string Dictionary { get; set; }
    }
}
