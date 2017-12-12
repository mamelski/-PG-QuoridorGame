using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using QuoridorClient.Screens;

namespace QuoridorClient
{
    public class ScreenManager
    {
        private static ScreenManager _instance;
        public static ScreenManager Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new ScreenManager();
                return _instance;
            }
        }

        public GameScreen CurrentScreen { get; set; }

        private ScreenManager()
        {
            //CurrentScreen = new GameBoard();
            //App.Current.
        }

        public void LoadContent(ContentManager content)
        {
            if(CurrentScreen != null)
                CurrentScreen.LoadContent(content);
        }

        public void UnloadContent()
        {
            if (CurrentScreen != null)
                CurrentScreen.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            CurrentScreen.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentScreen.Draw(spriteBatch);
        }

        public void SwitchScreen(GameScreen screen)
        {
            CurrentScreen = screen;
        }
    }
}
