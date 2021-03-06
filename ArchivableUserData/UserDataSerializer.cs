﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;

using Newtonsoft.Json;

namespace ArchivableUserData
{
    class UserDataSerializer : JsonConverter
    {
        


        public override bool CanConvert(Type objectType)
        {
            return typeof(Rhino.DocObjects.Custom.UserData).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("TypeInfo");
            writer.WriteValue(value.GetType().ToString());
            writer.WritePropertyName("Data");
            writer.WriteRawValue(JsonConvert.SerializeObject(value, Formatting.Indented));
            writer.WriteEndObject();
        }
    }
}
