using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Windows.Foundation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuoridorClient.Enums;
using QuoridorClient.Model;

namespace QuoridorClient.Screens
{
    public class GameBoard : GameScreen
    {
        private KeyboardState oldState;

        private Board _board;

        private Texture2D _fieldRectangleTexture;

        private Texture2D _pjonek;

        private Texture2D _pjonekOpponent;

        public event Action<BoardEvent> MoveEvent;

        public GameBoard(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            _board = new Board(500);//265
            var fieldSize = _board.FieldSizePx;
            _fieldRectangleTexture = GetRectangle(fieldSize, fieldSize, Color.White);
            _pjonek = GetRectangle(20, 20, Color.Blue);
            _pjonekOpponent = GetRectangle(20, 20, Color.Tomato);
        }

        public override void LoadContent(ContentManager content)
        {
        }

        public override void UnloadContent()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
           // HandleKeyboardInput();
        }

        private void HandleKeyboardInput()
        {
            KeyboardState state = Keyboard.GetState();
            if (oldState.IsKeyUp(Keys.Left) && state.IsKeyDown(Keys.Left))
            {
                _board.MoveUser(MoveDirection.Left);
                InvokeMoveEvent(MoveDirection.Left);
            }
            else if (oldState.IsKeyUp(Keys.Right) && state.IsKeyDown(Keys.Right))
            {
                _board.MoveUser(MoveDirection.Right);
                InvokeMoveEvent(MoveDirection.Right);
            }
            else if (oldState.IsKeyUp(Keys.Up) && state.IsKeyDown(Keys.Up))
            {
                _board.MoveUser(MoveDirection.Forward);
                InvokeMoveEvent(MoveDirection.Forward);
            }
            else if (oldState.IsKeyUp(Keys.Down) && state.IsKeyDown(Keys.Down))
            {
                _board.MoveUser(MoveDirection.Backward);
                InvokeMoveEvent(MoveDirection.Backward);
            }
            oldState = state;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Board.BOARD_SIZE; i++)
            {
                for (int k = 0; k < Board.BOARD_SIZE; k++)
                {
                    var coor = _board.GetFieldPosition(i, k);
                    spriteBatch.Draw(_fieldRectangleTexture, coor, Color.White);
                }
            }
            spriteBatch.Draw(_pjonek, _board.GetUserPositionPx(), Color.White);
            spriteBatch.Draw(_pjonekOpponent, _board.GetOpponentPositionPx(), Color.White);
        }

        public override void HandleGameEvent(GameEvent evt)
        {
            var moveEvent = evt as BoardEvent;
            if (moveEvent == null) return;

            if (moveEvent.BoardEventType == BoardEventType.PlayerMoved)
            {
                _board.MoveOpponent(moveEvent.Move);
            }
        }
        
        public override void HandleKeyboard(MoveDirection direction)
        {
            //ScreenManager.Instance.Draw(Game1._spriteBatch);
            _board.MoveUser(direction);
           InvokeMoveEvent(direction);
        }

        private Texture2D GetRectangle(int width, int height, Color color)
        {
            var rect = new Texture2D(graphicsDevice, width, height);

            var data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i) data[i] = color;
            rect.SetData(data);

            return rect;
        }

        private void InvokeMoveEvent(MoveDirection move)
        {
            if (MoveEvent != null)
            {
                MoveEvent.Invoke(new BoardEvent
                {
                    BoardEventType = BoardEventType.PlayerMoved,
                    Move = move
                });
            }
        }
    }
}
