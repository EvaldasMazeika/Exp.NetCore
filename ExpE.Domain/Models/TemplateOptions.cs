using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExpE.Domain
{
    public class TemplateOptions
    {
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "placeholder")]
        public string Placeholder { get; set; }

        [JsonProperty(PropertyName = "required")]
        public Boolean? Required { get; set; }

        [JsonProperty(PropertyName = "options")]
        public IEnumerable<DropDownOptions> Options { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "formId")]
        public string FormId { get; set; }

        [JsonProperty(PropertyName = "isMultiFile")]
        public Boolean? IsMultiFile { get; set; }

        [JsonProperty(PropertyName = "isTime")]
        public Boolean? IsTime { get; set; }

        [JsonProperty(PropertyName = "dateFormat")]
        public string DateFormat { get; set; }
    }
}
