using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Directum_Plugin.DTO
{
    public class User
    {
        [JsonProperty("FirstName")]
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
