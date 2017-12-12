using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using QuoridorClient.Model;

namespace QuoridorClient.ServiceConnector
{
    public interface IServiceConnector
    {
        bool LogIn(string userId, string password);
        bool IsLogged();
        List<Player> GetConnectedUsers();
        void SendMessage(string userId, string message);
        bool CheckMessages();
        List<PlayerMessage> GetMessages();
    }
}
