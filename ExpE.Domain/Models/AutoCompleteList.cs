using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExpE.Domain.Models
{
    public class AutoCompleteList
    {
        [JsonProperty(PropertyName = "formId")]
        public string FormId { get; set; }

        [JsonProperty(PropertyName = "properties")]
        public IEnumerable<string> Properties { get; set; }
    }
}
