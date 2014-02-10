using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Box.V2.Models
{
    /// <summary>
    /// Metadata request object used to send add, edit, test, and remove operations
    /// </summary>
    public class BoxMetadataRequest
    {
        /// <summary>
        /// The metadata operation to perform
        /// Can be: Add, Remove, Test
        /// Add is used to create new metadata as well as edit
        /// </summary>
        [JsonProperty(PropertyName = "op")]
        public BoxMetadataOperations Op { get; set; }

        /// <summary>
        /// The key of the metadata
        /// Do not prefix this path with '/' as the documenation states as the SDK automatically prepends this
        /// </summary>
        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }

        /// <summary>
        /// The value of the metadata
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }


    /// <summary>
    /// Available metadata operations
    /// </summary>
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
