using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
