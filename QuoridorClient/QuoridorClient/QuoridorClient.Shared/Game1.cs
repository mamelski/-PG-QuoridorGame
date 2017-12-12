using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using QuoridorClient.Enums;
using QuoridorClient.Model;
using QuoridorClient.Screens;
using QuoridorClient.ServiceConnector;
using QuoridorClient.Utils;

namespace QuoridorClient
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch _spriteBatch;

        TimeSpan _lastTime = new TimeSpan(0);
        private const int _timeInterval = 2;
        private int _users = 0;

        private IServiceConnector _quoridorServiceConnector;
        private Stack<PlayerMessage> _mesgStack;
        private List<PlayerMessage> _msgs;
        private Queue<GameEvent> _gameEvents;

        private KeyboardState oldState;

        private int usunToJakOgarnieszServer = 1;

        public GamePage GamePage { get; set; }

        private int _myId;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _quoridorServiceConnector = SharedModel.GetConnector();

        }

        private List<PlayerMessage> GetMessages()
        {
            List<PlayerMessage> mesgs = _mesgStack.ToList();
            _mesgStack.Clear();
            return mesgs;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            _mesgStack = new Stack<PlayerMessage>();
            _msgs = GetMessages();
            _gameEvents = new Queue<GameEvent>();

            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            base.Initialize();

            ScreenManager.Instance.CurrentScreen = new EmptyScreen(graphics.GraphicsDevice);
        }

        private void BoardMoveEvent(BoardEvent evt)
        {
            ((QuoridorService)_quoridorServiceConnector).SendGameEvent(Session.Instance.Opponent.Id.ToString(), evt);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ScreenManager.Instance.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            ScreenManager.Instance.UnloadContent();
        }

        private bool _sent;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Debug.WriteLine(gameTime.TotalGameTime);
            //if (gameTime.TotalGameTime.Seconds - _lastTime.Seconds >= _timeInterval)
            if (gameTime.TotalGameTime.Seconds % 3 == 0)
            {
                _lastTime = gameTime.TotalGameTime;
                //Debug.WriteLine(gameTime.TotalGameTime);

                // TODO: Add your update logic here
                
             
                if (_quoridorServiceConnector.IsLogged())
                {
                    if (this.usunToJakOgarnieszServer == 1)
                    {
                        var users = _quoridorServiceConnector.GetConnectedUsers();
                        if (_users != users.Count)
                        {
                            GamePage.UpdateConnectedPlayers(users);
                            _users = users.Count;
                        }

                        // tymczasowe rozwiazanie
                        if (Session.Instance.GameState != GameState.Play && _users > 0)
                        {
                            Session.Instance.GameState = GameState.Play;
                            Session.Instance.Opponent = users.First(u => u.Id != Session.Instance.User.Id);

                            var gameScreen = new GameBoard(graphics.GraphicsDevice);
                            gameScreen.MoveEvent += BoardMoveEvent;
                            ScreenManager.Instance.SwitchScreen(gameScreen);
                        }
                        this.usunToJakOgarnieszServer = 2;
                    }
                     GetAndDispatchMessages();
                        
                    if (_myId == 0)
                    {
                        QuoridorService s1 = (QuoridorService) _quoridorServiceConnector;
                        _myId = s1.MyId;
                    }
                }
            }

            //ScreenManager.Instance.Update(gameTime);

            //HandleKeybord();
            HandleGameEvents();

            base.Update(gameTime);
        }

        /// <summary>
        /// Tylko do sprawdzenia obsługi eventów.
        /// </summary>
        private void HandleKeybord()
        {
            KeyboardState state = Keyboard.GetState();
            if (oldState.IsKeyUp(Keys.A) && state.IsKeyDown(Keys.A))
            {
                _gameEvents.Enqueue(new BoardEvent { Move = MoveDirection.Left, BoardEventType = BoardEventType.PlayerMoved });
            }
            if (oldState.IsKeyUp(Keys.D) && state.IsKeyDown(Keys.D))
            {
                _gameEvents.Enqueue(new BoardEvent { Move = MoveDirection.Right, BoardEventType = BoardEventType.PlayerMoved });
            }
            if (oldState.IsKeyUp(Keys.W) && state.IsKeyDown(Keys.W))
            {
                _gameEvents.Enqueue(new BoardEvent { Move = MoveDirection.Forward, BoardEventType = BoardEventType.PlayerMoved });
            }
            if (oldState.IsKeyUp(Keys.S) && state.IsKeyDown(Keys.S))
            {
                _gameEvents.Enqueue(new BoardEvent { Move = MoveDirection.Backward, BoardEventType = BoardEventType.PlayerMoved });
            }
            oldState = state;
        }

        private void HandleGameEvents()
        {
            if (ScreenManager.Instance.CurrentScreen is GameBoard)
            {
                while (_gameEvents.Any())
                {
                    var evt = _gameEvents.Dequeue();
                    ScreenManager.Instance.CurrentScreen.HandleGameEvent(evt);
                }
            }
        }


        private void GetAndDispatchMessages()
        {
            List<PlayerMessage> msg = _quoridorServiceConnector.GetMessages();
            msg = FilterGameEvents(msg);
            _msgs.AddRange(msg);
            if (msg.Count > 0)
            {
                Debug.WriteLine(msg[0].Message);
                GamePage.UpdateMessages(msg);
            }
        }

        private List<PlayerMessage> FilterGameEvents(List<PlayerMessage> messages)
        {
            List<PlayerMessage> textMessages = new List<PlayerMessage>();
            if (messages.Count > 0)
            {
                foreach (var playerMessage in messages)
                {
                    if (IsJSON(playerMessage.Message)) {
                        var gameEvent = GameEventParser.Deserialize(playerMessage.Message);
                        _gameEvents.Enqueue(gameEvent);
                    } else {
                        textMessages.Add(playerMessage);
                    }
                }
            }

            return textMessages;
        }

        private bool IsJSON(string message)
        {
            return (message.StartsWith("{") && message.EndsWith("}"));
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // TODO: Add your drawing code here

            
            _spriteBatch.Begin();
            ScreenManager.Instance.Draw(_spriteBatch);
            //int y = 0;
            //foreach (var msg in _msgs) {
            //    //_spriteBatch.DrawString(Content.Load<SpriteFont>("font"), msg.Message, new Vector2(0, y), Color.White);
            //    y += 15;
            //}
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
