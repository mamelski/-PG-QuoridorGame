using System;
using System.Collections.Generic;
using System.Text;
using QuoridorClient.Enums;
using QuoridorClient.Model;

namespace QuoridorClient
{
    public class Session
    {
        private static Session _instance;

        public static Session Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new Session();
                return _instance;
            }
        }

        private Session()
        {
            this.GameState = GameState.NotLogged;
        }

        public GameState GameState { get; set; }

        public Player User { get; set; }

        public Player Opponent { get; set; }
    }
}
