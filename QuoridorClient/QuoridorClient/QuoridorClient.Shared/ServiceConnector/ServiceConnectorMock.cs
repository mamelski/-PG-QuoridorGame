using System;
using System.Collections.Generic;
using System.Text;
using QuoridorClient.Mocks;
using QuoridorClient.Model;

namespace QuoridorClient.ServiceConnector
{
    public class ServiceConnectorMock : IServiceConnector
    {
        private string _sessionId;

        public bool LogIn(string userId, string password)
        {
            _sessionId = ServerMock.Authenticate(userId, password);
            return (_sessionId != null);
        }

        public bool IsLogged()
        {
            return (_sessionId != null);
        }

        public List<Player> GetConnectedUsers()
        {
            return ServerMock.GetLoggedUsers(_sessionId);
        }

        public void SendMessage(string userId, string message)
        {
            ServerMock.SendMessage(_sessionId, userId, message);
        }

        public bool CheckMessages()
        {
            return (ServerMock.CheckMessages(_sessionId) > 0);
        }

        public List<PlayerMessage> GetMessages()
        {
            return ServerMock.GetMessages(_sessionId);
        }
    }
}
