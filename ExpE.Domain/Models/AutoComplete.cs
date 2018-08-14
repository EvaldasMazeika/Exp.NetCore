using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpE.Domain.Models
{
    public class AutoComplete
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "formId")]
        public string FormId { get; set; }

        [JsonProperty(PropertyName = "propertyKey")]
        public string PropertyKey { get; set; }

        [JsonProperty(PropertyName = "items")]
        public IEnumerable<string> Items { get; set; }

    }
}
