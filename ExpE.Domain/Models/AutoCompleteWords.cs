using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpE.Domain.Models
{
    public class AutoCompleteWords
    {
        [JsonProperty(PropertyName = "formId")]
        public string FormId { get; set; }

        [JsonProperty(PropertyName = "words")]
        public IEnumerable<WordsPair> Words { get; set; }
    }
}
