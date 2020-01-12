using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System.Windows.Threading;
//using System.Timers;
using TestSimEngine;

namespace MonoApp
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        BasicEffect _basicEffect;
        private SpriteFont _mainFont;
        private Texture2D _ballTexture;
        private Texture2D _ballTexture50;
        private Texture2D _ballTexture72;
        private readonly GameEngine _game = new GameEngine();
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(1000);
        private Dispatcher _gameDispatcher;

        public Game1()
        {
            _gameDispatcher = Dispatcher.CurrentDispatcher;
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = 1220;
            //_graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            IsFixedTimeStep = false;
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
            _game.Create();

            //setup fps timer
            _timer.Elapsed += (o, a) =>
            {
                _gameDispatcher.Invoke(() =>
                {
                    Window.Title = $"Frame rate {FramesPerSecond} (calculation took -- ms)";
                });
                
                FramesPerSecond = 0;
            };
            _timer.AutoReset = true;
            _timer.Enabled = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _basicEffect = new BasicEffect(GraphicsDevice);
            Viewport viewport = this.GraphicsDevice.Viewport;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
            _basicEffect.Projection = projection;
            _basicEffect.TextureEnabled = true;
            _basicEffect.View = Matrix.CreateTranslation(300.5f, 0.5f, 0.0f);
            //_basicEffect.World = Matrix.CreateScale(0.1f);

            // TODO: use this.Content to load your game content here
            _ballTexture = Content.Load<Texture2D>("ball-tennis");
            _ballTexture50 = Content.Load<Texture2D>("ball-tennis-50");
            _ballTexture72 = Content.Load<Texture2D>("ball-tennis-72");
            _mainFont = Content.Load<SpriteFont>("Main");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!IsActive)
                return;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _game.Tick(gameTime.ElapsedGameTime.TotalSeconds);
            //System.Diagnostics.Debug.WriteLine($"{gameTime.ElapsedGameTime.TotalSeconds} ms; slowly: {gameTime.IsRunningSlowly}");
           
            //Thread.Sleep(20);
            base.Update(gameTime);
        }

        private int FramesPerSecond = 0;
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw( GameTime gameTime)
        {
            FramesPerSecond++;



            //_spriteEffect.Parameters[0].GetValueMatrix()
            //_spriteEffect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(effect: _basicEffect);

            //var ballScale = new Vector2(.1f, .1f);
            //var testballScale = new Vector2(1.7f, 1.7f);
            float scale = 800;// 0;
            Vector2 position;

            var objs = _game.Objects;
            for (int i = 0; i < objs.Length; i++)
            { 
                position.X = (float)objs[i].X * scale;
                position.Y = (float)objs[i].Y * scale;
                //_spriteBatch.Draw(_ballTexture, position: position);
                _spriteBatch.Draw(_ballTexture72, position: position);
                //spriteBatch.DrawString(_mainFont, objs[i].Name, position, Color.Black);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
