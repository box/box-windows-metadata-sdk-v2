using Box.V2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box.V2.Converter
{
    internal class BoxMetadataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(BoxMetadata).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Null check so that the JObject.Load doesn't throw an exception
            if (reader.TokenType == JsonToken.Null)
                return null;

            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create BoxMetadata target object
            var target = new BoxMetadata();

            // *** Custom Parsing for Box Metadata ***
            // Because the API returns system properties in $key format, we must use completely customized parsing.
            // Reflection is used to set the properties since the objects are immutable. This was chosen over an overloaded constructor
            // since additional system properties can be added at any time
            JToken token;
            if (jObject.TryGetValue(BoxMetadata.FieldId, out token))
            { 
                token.Parent.Remove();
                typeof(BoxMetadata).GetProperty("Id").SetValue(target, token.ToObject<string>(), null);
            }
            if (jObject.TryGetValue(BoxMetadata.FieldType, out token))
            {
                token.Parent.Remove();
                typeof(BoxMetadata).GetProperty("Type").SetValue(target, token.ToObject<string>(), null);
            }
            if (jObject.TryGetValue(BoxMetadata.FieldParent, out token))
            {
                token.Parent.Remove();
                typeof(BoxMetadata).GetProperty("Parent").SetValue(target, token.ToObject<string>(), null);
            }

            // Populate the remaining properties as key value pairs in the dictionary
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
