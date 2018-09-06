using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HomeSensorServerAPI.Models
{
    public class Entity
    {
        [BindNever]
        public virtual int Id { get; set; }
    }
}
