using System;
using Newtonsoft.Json;

namespace QuoridorClient.Utils
{
    public class JsonParser
    {
        public static string Serialize<T>(T gameEvent)
        {
            string serialized = JsonConvert.SerializeObject(gameEvent);
            serialized = serialized.Replace(":", "=");
            return serialized;
        }

        public static T Deserialize<T>(string json)
        {
            json = json.Replace("=", ":");

            var gameEvent = (T)JsonConvert.DeserializeObject(json, typeof(T));
            return gameEvent;
        }
    }
}
