using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpE.Domain.Models
{
    public class Record
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "formId")]
        public string FormId { get; set; }

        [JsonProperty(PropertyName = "body")]
        public ExpandoObject Body { get; set; }
    }
}
