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

    public class EmptyScreen : GameScreen
    {
        public EmptyScreen(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
        }

        public override void LoadContent(ContentManager content)
        {
            
        }

        public override void UnloadContent()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public override void HandleGameEvent(GameEvent evt)
        {
            
        }

        public override void HandleKeyboard(MoveDirection direction)
        {

        }
    }
}
