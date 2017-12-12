using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QuoridorClient.Model;
using QuoridorClient.Utils;

namespace QuoridorClient.ServiceConnector
{
    public class QuoridorService : IServiceConnector
    {
        private RestServiceConnector _restServiceConnector = new RestServiceConnector() {
            ServiceHostUrl = @"http://quoridor.cloudapp.net/Quoridor.svc"
        };

        private bool _isLogged;
        public int MyId { get; set; }

        public QuoridorService() { }

        /// <summary>
        /// Logs user in
        /// TODO: Get user id from server
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool LogIn(string userId, string password)
        {
            string responseString = _restServiceConnector.CallMethod("LogIn", userId, password);
            //TODO: Uncomment when method returns JSON
            //_isLogged = JsonConvert.DeserializeObject<bool>(responseString);\
            //return _isLogged;
            _isLogged = !responseString.Contains("-1");
            return _isLogged;
        }

        public bool IsLogged()
        {
            return _isLogged;
        }

        /// <summary>
        /// Retrieves list of logged in users
        /// </summary>
        /// <returns></returns>
        public List<Player> GetConnectedUsers()
        {
            string responseString = _restServiceConnector.CallMethod("GetUsersOnline", MyId.ToString());
            List<Player> connectedPlayers = JsonConvert.DeserializeObject<List<Player>>(responseString);
            return connectedPlayers;
        }

        /// <summary>
        /// Sends message to specified user
        /// </summary>
        /// <param name="userId">Id of user that'll receive message</param>
        /// <param name="message"></param>
        public void SendMessage(string userId, string message)
        {
            message = WebUtility.UrlEncode(message);
            _restServiceConnector.CallMethod("SendMessages", MyId.ToString(), userId, message);
        }

        public void SendGameEvent(string userId, GameEvent gameEvent)
        {
            string serialized = GameEventParser.Serialize(gameEvent);
            SendMessage(userId, serialized);
        }

        public bool CheckMessages()
        {
            throw new NotImplementedException();
        }

        public List<PlayerMessage> GetMessages()
        {
            string responseString = _restServiceConnector.CallMethod("GetMessages", MyId.ToString());
            List<PlayerMessage> messages = JsonConvert.DeserializeObject<List<PlayerMessage>>(responseString);
            return messages;
        }
    }
}
