using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Box.V2.Models
{
    public class BoxMetadataRequest
    {
        [JsonProperty(PropertyName = "op")]
        public BoxMetadataOperations Op { get; set; }

        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum BoxMetadataOperations
    {
        [EnumMember(Value="add")]
        Add,
        [EnumMember(Value="remove")]
        Remove,
        [EnumMember(Value="test")]
        Test
    }
}
