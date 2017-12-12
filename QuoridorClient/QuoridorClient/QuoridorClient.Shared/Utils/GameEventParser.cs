using System;
using Newtonsoft.Json;
using QuoridorClient.Enums;
using QuoridorClient.Model;

namespace QuoridorClient.Utils
{
    public class GameEventParser
    {
        public static string Serialize(GameEvent gameEvent)
        {
            string serialized = JsonConvert.SerializeObject(gameEvent);
            serialized = serialized.Replace(":", "=");
            return serialized;
        }

        public static GameEvent Deserialize(string json)
        {
            json = json.Replace("=", ":");
            Type eventType = ResolveGameEventType(json);

            var gameEvent = (GameEvent)JsonConvert.DeserializeObject(json, eventType);
            return gameEvent;
        }

        private static Type ResolveGameEventType(string json)
        {
            const string resolvingStringBase = "\"EventType\":";
            if (json.Contains(resolvingStringBase + (byte)GameEventType.GameInvitation))
            {
                return typeof(GameInvitation);
            }
            if (json.Contains(resolvingStringBase + (byte)GameEventType.GameInvitationResponse))
            {
                return typeof(GameInvitationResponse);
            }
            if (json.Contains(resolvingStringBase + (byte)GameEventType.BoardChanged))
            {
                return typeof(BoardEvent);
            }

            return typeof(GameEvent);
        }
    }
}
