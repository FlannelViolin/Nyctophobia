using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TileTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // ///////////////////////////////////////////
        // XNA Graphics
        // ///////////////////////////////////////////

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // ///////////////////////////////////////////
        // GameState
        // ///////////////////////////////////////////

        public enum GameState
        {
            Title,
            Instructions,
            GamePlay,
            GameOver
        }

        GameState _gameState;

        // an attribute for the timer
        double _timer;

        // ///////////////////////////////////////////
        // Game window data
        // ///////////////////////////////////////////

        int _windowWidth;       // Width of game window
        int _windowHeight;      // Height of game window
        Rectangle _gameWindow;  // Rectangle to draw game screen window

        // ///////////////////////////////////////////
        // Player Information
        // ///////////////////////////////////////////

        Player _player;             // User controlled player
        Texture2D _playerTexture;   // Player texture

        // ///////////////////////////////////////////
        // Menu screen information
        // ///////////////////////////////////////////

        // TODO: Add menus for the game
        // MenuScreen _titleScreen;         // Game title screen
        // MenuScreen _instructionScreen;   // Game instruction screen menu

        // ///////////////////////////////////////////
        // Game screen information
        // ///////////////////////////////////////////

        // TODO: Add panels to game screen
        // Rectangle _mapScreen;            // Rectangle where game screen is drawn
        // Rectangle _gameInfoScreen;       // Rectangle where game info screen is drawn
        // Rectangle _solvedScreen;         // Rectangle where solved puzzle screen is drawn
        // Rectangle _instructionButton;    // Instruction button
        // Rectangle _resetPuzzleButton;    // Reset puzzle button
        // Rectangle _quitButton;           // Quit button
        // int currentLevel;                // Current game level

        // ///////////////////////////////////////////
        // Map data
        // ///////////////////////////////////////////

        List<TileMap> _maps;                    // List of tile maps
        List<TileMap> _solved;                  // List of solved maps
        int _maxMapWidth;                       // Number of tiles for map width
        int _maxMapHeight;                      // Number of tiles for map height

        // TODO: Add collectibles
        // List<Collectible[]> collectibles;    // List of collectible object array

        // ////////////////////////////////////////////
        // Tile data
        // ////////////////////////////////////////////

        Texture2D _tileSheet;   // Tile sprite sheet

        int _tileWidth;         // Width of tile on sprite sheet
        int _tileHeight;        // Height of tile on sprite sheet
        int _tileDrawWidth;     // Width of tile when drawn on screen
        int _tileDrawHeight;    // Height of tile when drawn on screen

        // ////////////////////////////////////////////
        // Input data
        // ////////////////////////////////////////////

        KeyboardState _keyState;            // Current keyboard state
        KeyboardState _prevKeyState;        // Previous keyboard state

        // TODO: Mouse state added for menu screens
        // MouseState mouseState;            // Current mouse state
        // MouseState mousePrevState;        // Previous mouse state

        // /////////////////////////////////////////////
        // SpriteFont data
        // /////////////////////////////////////////////

        // temporary fonts to write the title and instructions until the special game screens get set up
        SpriteFont _titleFont;
        SpriteFont _instructionFont;

        // TODO: Add spritefont data
        // SpriteFont _infoFont;            // Font used to draw info on screen
        // SpriteFont _debugFont;           // Font used to draw debug info on screen

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Initialize game window
            _windowWidth = GraphicsDevice.Viewport.Width;
            _windowHeight = GraphicsDevice.Viewport.Height;
            _gameWindow = new Rectangle(0, 0, _windowWidth, _windowHeight);

            // Set tile width/height to tile size in tile image(demo_tile_map.png)
            _tileWidth = 50;
            _tileHeight = 50;

            // Set max map width/height
            _maxMapWidth = 5;
            _maxMapHeight = 5;
            
            // Set tile draw width/height
            _tileDrawWidth = _windowWidth / _maxMapWidth;
            _tileDrawHeight = _windowHeight / _maxMapHeight;

            // Initialize map list
            // Maps will be loaded in the LoadContent method
            _maps = new List<TileMap>();

            // Initialize the Gamestate as the title screen
            _gameState = GameState.Title;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load player info
            _playerTexture = Content.Load<Texture2D>("Images\\NycSheet");
            _player = new Player(_playerTexture, Vector2.Zero);

            // Load in the temporary title / instruction fonts
            _titleFont = Content.Load<SpriteFont>("Title");
            _instructionFont = Content.Load<SpriteFont>("Instructions");

            // Load map info
            _tileSheet = Content.Load<Texture2D>("Images\\TileSheet");

            // Temporary map created for testing
            int[,] temp = new int[,] {
                {0, 1, 2, 3, 0},
                {4, 2, 3, 2, 1},
                {0, 0, 0, 0, 0},
                {1, 1, 2, 2, 3},
                {0, 0, 2, 2, 0}};

            // Add default map
            _maps.Add(new TileMap(_tileSheet, _tileWidth, _tileHeight, _maxMapWidth, _maxMapHeight));
            // Or Add Temp map
            //_maps.Add(new TileMap(_tileSheet, temp, _tileWidth, _tileHeight, _maxMapWidth, _maxMapHeight));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // set the keyboard states
            _prevKeyState = _keyState;
            _keyState = Keyboard.GetState();

            // finite state machine for the game state
            switch (_gameState)
            {
                case GameState.Title:
                    {
                        // if the player presses enter, start the game
                        if (SingleKeyPress(Keys.Enter))
                            _gameState = GameState.GamePlay;
                        // if the player presses i give them instructions
                        else if (SingleKeyPress(Keys.I))
                            _gameState = GameState.Instructions;

                        // set the timer for 30 sec.  (this should be done elsewhere but for now...)
                        _timer = 5;

                        break;
                    }
                case GameState.Instructions:
                    {
                        // if the player presses enter, start the game
                        if (SingleKeyPress(Keys.Enter))
                            _gameState = GameState.GamePlay;
                        // if the player presses b, send them back to the title
                        if (SingleKeyPress(Keys.B))
                            _gameState = GameState.Title;

                        break;
                    }
                case GameState.GamePlay:
                    {
                        // adjust the timer
                        _timer -= gameTime.ElapsedGameTime.TotalSeconds;

                        // Update player information
                        // TODO: Pass in current map tile to check to see if tile available
                        _player.Update(_keyState, _prevKeyState, /*_maps[currentLevel],*/ _maxMapWidth - 1, _maxMapHeight - 1);

                        // TODO: Check to see if current game level is solved
                        // _maps[currentLevel].CheckIfSolved(_solved[currentLevel];

                        // check to see if the time ran out
                        if (_timer < 0)
                            _gameState = GameState.GameOver;

                        break;
                    }
                case GameState.GameOver:
                    {
                        // if the player presses enter send them back to the title screen
                        if (SingleKeyPress(Keys.Enter))
                            _gameState = GameState.Title;

                        break;
                    }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Color to clear graphics device
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add gameStates and draw game accordingly

            // Begin spriteBatch
            spriteBatch.Begin();

            // draws to the screen if at the title screen
            if (_gameState == GameState.Title)
            {
                spriteBatch.DrawString(
                    _titleFont,
                    "NYCTOPHOBIA",
                    new Vector2(200, 125),
                    Color.Red);

                spriteBatch.DrawString(
                    _instructionFont,
                    "Press  ' Enter '  to Play",
                    new Vector2(200, 200),
                    Color.LightGray);

                spriteBatch.DrawString(
                    _instructionFont,
                    "Press  ' I '  for instructions",
                    new Vector2(400, 200),
                    Color.LightGray);
            }

            // draws to the screen if at the instruction screen
            if (_gameState == GameState.Instructions)
            {
                // these instructions are nonsense but they demonstrate the setup of the game state

                spriteBatch.DrawString(
                    _instructionFont,
                    "The object of this game is to light up the correct tiles and",
                    new Vector2(175, 100),
                    Color.LightGray);

                spriteBatch.DrawString(
                    _instructionFont,
                    "gain access to the next room of the mansion in order to find",
                    new Vector2(175, 125),
                    Color.LightGray);

                spriteBatch.DrawString(
                    _instructionFont,
                    "your lost super puppy.  But for now just have fun running around",
                    new Vector2(175, 150),
                    Color.LightGray);

                spriteBatch.DrawString(
                    _instructionFont,
                    "the screen like a kid in a candy store on crack.",
                    new Vector2(175, 175),
                    Color.LightGray);

                spriteBatch.DrawString(
                    _instructionFont,
                    "Press  ' Enter '  to Play",
                    new Vector2(175, 250),
                    Color.LightGray);

                spriteBatch.DrawString(
                    _instructionFont,
                    "Press  ' B '  to go back to the title",
                    new Vector2(375, 250),
                    Color.LightGray);
            }

            // draws to the screen if during gameplay
            if (_gameState == GameState.GamePlay)
            {
                // Draw current map
                _maps[0].Draw(spriteBatch, _tileDrawWidth, _tileDrawHeight);

                // for now just write the time left on the screen
                spriteBatch.DrawString(
                _instructionFont,
                String.Format("{0:0.00}", _timer),
                new Vector2(375, 0),
                Color.Red);

                // TODO: Draw gameScreen information

                // Draw player location
                _player.Draw(spriteBatch, _tileDrawWidth, _tileDrawHeight);
            }

            // draws to the screen if the game is over
            if (_gameState == GameState.GameOver)
            {
                // again some nonsense text but it shows the state exists and works

                spriteBatch.DrawString(
                    _titleFont,
                    "FAIL",
                    new Vector2(325, 100),
                    Color.Red);

                spriteBatch.DrawString(
                    _instructionFont,
                    "Your super puppy has now been torn apart into pieces",
                    new Vector2(225, 175),
                    Color.LightGray);

                spriteBatch.DrawString(
                    _instructionFont,
                    "because you weren't good enough to save him.  Way to be.",
                    new Vector2(225, 200),
                    Color.LightGray);

                spriteBatch.DrawString(
                    _instructionFont,
                    "Press  ' Enter '  to go back to the title Screen.",
                    new Vector2(250, 250),
                    Color.LightGray);
            }

            // End spriteBatch
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// method to check whether or not the keys was just pressed and not held down
        /// </summary>
        /// <param name="keyToCheck">the key you want to check</param>
        /// <returns>true if the key was only pressed</returns>
        bool SingleKeyPress(Keys keyToCheck)
        {
            if (_prevKeyState.IsKeyUp(keyToCheck) == true &&
                _keyState.IsKeyDown(keyToCheck) == true)
                return true;
            else
                return false;
        }
    }
}
