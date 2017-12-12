using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Devices.Sensors;
using QuoridorClient.Enums;
using QuoridorClient.Model;

namespace QuoridorClient.Mocks
{
    public static class ServerMock
    {
        private static Dictionary<string, Queue<string>> messages = new Dictionary<string, Queue<string>>();
        private static Dictionary<string, string> authenticatedSessions = new Dictionary<string, string>();
        private static List<String> userList = new List<string>(); 

        static ServerMock()
        {
            userList.Add("User1");
            authenticatedSessions.Add("user1SessionId", "User1");
        }

        public static string Authenticate(string userId, string password)
        {
            string sessionId = null;
            if (userId == "User" && password == "p4ss") {
                sessionId = "mockedSessionId";
                authenticatedSessions.Add(sessionId, userId);
                messages.Add(userId, new Queue<string>());
            }
            return sessionId;
        }

        public static List<Player> GetLoggedUsers(string sessionId)
        {
            List<Player> users = new List<Player>();
            int i = 0;
            if (authenticatedSessions.ContainsKey(sessionId)) {
                foreach (KeyValuePair<string, string> authenticatedSession in authenticatedSessions) {
                    users.Add(new Player(){Id=i, Name = authenticatedSession.Value});
                    i++;
                }
            }
            return users;
        }

        public static byte SendMessage(string sessionId, string userId, string message)
        {
            var response = ResponseCode.NoSuchUser;
            if (authenticatedSessions.ContainsKey(sessionId) && userList.Contains(userId)) {
                if (!messages.ContainsKey(userId)) {
                    messages.Add(userId, new Queue<string>());
                }

                messages[userId].Enqueue(message);
                response = ResponseCode.Ok;

                messages[authenticatedSessions[sessionId]].Enqueue("Response for: " + message);
            }

            return (byte)response;
        }

        public static List<PlayerMessage> GetMessages(string sessionId)
        {
            List<PlayerMessage> userMessages = new List<PlayerMessage>();
            if (authenticatedSessions.ContainsKey(sessionId)) {
                string userId = authenticatedSessions[sessionId];
                //userMessages = messages[userId].ToList();
                foreach (var message in messages[userId]) {
                    userMessages.Add(new PlayerMessage(){Message = message, ReceiverId = 0, SenderId = 0});
                }
                messages[userId].Clear();
            }

            return userMessages;
        }

        public static int CheckMessages(string sessionId )
        {
            if (authenticatedSessions.ContainsKey(sessionId)) {
                return messages[authenticatedSessions[sessionId]].Count();
            }

            return -1;
        }
    }
}
