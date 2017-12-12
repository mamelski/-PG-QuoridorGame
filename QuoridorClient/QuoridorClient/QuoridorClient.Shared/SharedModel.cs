using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using QuoridorClient.Model;
using QuoridorClient.ServiceConnector;

namespace QuoridorClient
{
    /// <summary>
    /// Class that shares objects between XAML and MonoGame
    /// </summary>
    public class SharedModel
    {
        private static IServiceConnector _serviceConnector;
        public static ConcurrentBag<Player> ConnectedPlayers = new ConcurrentBag<Player>();

        public static IServiceConnector GetConnector()
        {
            if (_serviceConnector == null) {
                _serviceConnector = new QuoridorService();
            }

            return _serviceConnector;
        }
    }
}
