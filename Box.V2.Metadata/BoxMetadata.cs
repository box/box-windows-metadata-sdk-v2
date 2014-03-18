using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box.V2.Models
{
    /// <summary>
    /// Box Metadata object that contains the key value mapping of the metadata
    /// </summary>
    public class BoxMetadata : Dictionary<string, string>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public BoxMetadata() { }

        internal const string FieldId = "$id";
        internal const string FieldType = "$type";
        internal const string FieldParent = "$parent";

        /// <summary>
        /// The ID of the Metadata Type Instance. This is a system property
        /// </summary>
        public string Id { get; private set; }


        /// <summary>
        /// The Type of the Metadata Type Instance. This is a system property
        /// </summary>
        [JsonProperty(PropertyName = FieldType)]
        public string Type { get; private set; }

        /// <summary>
        /// The parent of the Metadata Type Instance in the format: "{item_type}_{id}" (eg. "file_1234"). This is a system property
        /// </summary>
        [JsonProperty(PropertyName = FieldParent)]
        public string Parent { get; private set; }
    }
}
