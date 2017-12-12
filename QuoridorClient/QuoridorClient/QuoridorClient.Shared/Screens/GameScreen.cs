using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using QuoridorClient.Model;

namespace QuoridorClient.Screens
{
    using QuoridorClient.Enums;

    public abstract class GameScreen
    {
        protected GraphicsDevice graphicsDevice;
        public GameScreen(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public abstract void LoadContent(ContentManager content);

        public abstract void UnloadContent();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void HandleGameEvent(GameEvent evt);

        public abstract void HandleKeyboard(MoveDirection direction);
    }
}
