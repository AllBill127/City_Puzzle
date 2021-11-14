using System;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebServiceExample
{
    public class Repository
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("desription")]
        public string Desrciption { get; set; }
        [JsonProperty("html_url")]
        public Uri GitHubHomeUrl { get; set; }
        [JsonProperty("homepage")]
        public Uri HomePage { get; set; }
        [JsonProperty("watchers")]
        public int Watchers { get; set; }
    }
}
