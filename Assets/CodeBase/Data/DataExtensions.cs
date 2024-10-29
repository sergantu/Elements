using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace CodeBase.Data
{
    public static class DataExtensions
    {
        public static Vector3Data AsVectorData(this Vector3 vector) => new Vector3Data(vector.x, vector.y, vector.z);

        public static Vector3 AsUnityVector(this Vector3Data vector3Data) => new Vector3(vector3Data.X, vector3Data.Y, vector3Data.Z);

        public static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value, CreateSettings());
        }

        public static T FromJson<T>(this string json)
        {
            try {
                return JsonConvert.DeserializeObject<T>(json, CreateSettings());
            } catch (Exception ex) {
                return default;
            }
        }

        private static JsonSerializerSettings CreateSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;
            return settings;
        }
    }
}