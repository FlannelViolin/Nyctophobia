//Tyler Wozniak
//Jesse Charland
//Georgia Partyka
//Haydon Clore
//David McCann

// HI HAYDON I LOVE YOU
// OH TYLER YOU FOOL

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

namespace Nyctophobia
{ 
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // ///////////////////////////////////////////
        // XNA Graphics
        // ///////////////////////////////////////////

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // ///////////////////////////////////////////
        // GameState
        // ///////////////////////////////////////////

        public enum GameState
        {
            Title,
            Instructions,
            GamePlay,
            Paused,
            Win,
            GameOver,
            Credit,
        }

        GameState _gameState;
        

        // ///////////////////////////////////////////
        // Game window data
        // ///////////////////////////////////////////

        private int _windowBufferX;          // Buffer used to separate windows
        private int _windowBufferY;
        private int _windowBuffer;
        private Rectangle _gameWindow;      // Rectangle to draw game screen window
        private Rectangle _mapWindow;       // Rectangle to draw map screen
        private Rectangle _solvedWindow;    // Rectangle to draw solved map key
        private Rectangle _infoWindow;      // Rectangle to draw game info
        private Rectangle _someWindow;      // Rectangle to draw some other window

        // ///////////////////////////////////////////
        // Player Information
        // ///////////////////////////////////////////

        private static Player _player;             // User controlled player
        public static Player player
        { get { return _player; } set { _player = value; } }
        private static bool _win;
        public static bool win
        {
            get { return _win; }
            set { _win = value; }
        }

        // ///////////////////////////////////////////
        // Map data
        // ///////////////////////////////////////////

        private static Room<LightTile>[] _floor1;
        public static Room<LightTile>[] floor1
        {
            get { return _floor1; }
        }
        private static int _curFloor;
        public static int curFloor
        {
            get { return _curFloor; }
        }
        private static int _curRoom;                           // Index of the current room
        public static int curRoom
        {
            get { return _curRoom; }
            set { _curRoom = value; }
        }
        public const int MAX_MAP_HEIGHT = 10;                    // Number of tiles for map width
        public const int MAX_MAP_WIDTH = 10;                     // Number of tiles for map height
        private MapReader _mapRead = new MapReader();                   // Map reader object

        // ////////////////////////////////////////////
        // Tile data
        // ////////////////////////////////////////////

        private static Texture2D _tileSheet;   // Tile sprite sheet
        public static Texture2D tileSheet
        {
            get { return _tileSheet; }
        }
        public static int _spritePx; //Size of a tile

        public int SpritePx
        {
            get { return _spritePx; }
        }

        // temporary fonts to write the title and instructions until the special game screens get set up
        SpriteFont _titleFont;
        SpriteFont _instructionFont;
        SpriteFont _storyFont;
        Texture2D _winScreen;

        // sounds
        SoundEffect _doorUnlock;
        Song _background;
        public static bool playedSound = false; // this is temporary
        bool _backgroundStart = false;

        // stuff for the darkness, not sure if should be somwhere else???
        Texture2D _darkness;
        // an attribute for the timer
        public static double _timer;
        public static double timer
        { get { return _timer; } set { _timer = value; } }
        private int _darknessAlpha;
        public int darknessAlpha
        { get { return _darknessAlpha; } set { _darknessAlpha = value; } }
        private int _increaseDarkness;
        public int increaseDarkness
        { get { return _increaseDarkness; } set { _increaseDarkness = value; } }
        private int _darknessControl;
        public int darknessControl
        { get { return _darknessControl; } set { _darknessControl = value; } }
        int _darknessWidth;
        int _darknessHeight;

        //For the pause menu, helps control the darkness
        int tempDark = 0;

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
            _windowBufferX = ((GraphicsDevice.Viewport.Height - (9 * 50))/2);     // Buffer used to distance game windows
            _windowBufferY = ((GraphicsDevice.Viewport.Height - (9 * 50))); 
            // Initialize game window
            // The game window represents the entire game application screen
            // Used to draw full screen background images or background colors
            // Also used to place the game windows in their correct locations
            _gameWindow = new Rectangle(
                0,                                  // Start X
                0,                                  // Start Y
                GraphicsDevice.Viewport.Width,      // Window width
                GraphicsDevice.Viewport.Height);    // Window height

            // The map window represents the actual gameplay window
            // Used to draw the player and their position on the current map
            // Takes up a majority of the screen due to its importance
            _mapWindow = new Rectangle(
                _windowBufferX,                                  // Start X
                _windowBufferY,                                  // Start Y
                (4 * _gameWindow.Width / 5) - _windowBufferX,    // Window width
                _gameWindow.Height - (2 * _windowBufferX));      // Window height

            // The information window is used to display the current game location
            // Used to display the current level (any more relevant information?)
            // Draw in the upper right hand area
            _infoWindow = new Rectangle(
                _mapWindow.X + _mapWindow.Width + _windowBuffer,                                // Start X
                _windowBuffer,                                                                  // Start Y
                _gameWindow.Width - (_mapWindow.X + _mapWindow.Width) - (2 * _windowBuffer),    // Window width
                _gameWindow.Height / 5);                                                        // Window height

            // The solved window represents the solved puzzle state
            // Used to display the solved puzzle state
            // If a puzzle is solved, draw the puzzle solved texture in this window
            // Draw in the middle right hand area
            _solvedWindow = new Rectangle(
                _mapWindow.X + _mapWindow.Width + _windowBuffer,
                _infoWindow.Y + _infoWindow.Height + _windowBuffer,
                _gameWindow.Width - (_mapWindow.X + _mapWindow.Width) - (2 * _windowBuffer),
                _gameWindow.Height / 3);

            // Some window that currently has no use.  Any suggestions?
            // Place in the bottom right hand area
            _someWindow = new Rectangle(
                _mapWindow.X + _mapWindow.Width + _windowBuffer,                                        // Start X
                _solvedWindow.Y + _solvedWindow.Height + _windowBuffer,                                 // Start Y
                _gameWindow.Width - (_mapWindow.X + _mapWindow.Width) - (2 * _windowBuffer),            // Window width
                _gameWindow.Height - (_solvedWindow.Y + _solvedWindow.Height) - (2 * _windowBuffer));   // Window height


            _floor1 = new Room<LightTile>[11];
            for (int i = 0; i < _floor1.Length; i++)
            {
                _floor1[i] = new Room<LightTile>(new LightPuzzle<LightTile>(),i, "LightPuzzles.txt");
            }

            _curFloor = 0;
            _curRoom = 0;
            buildFloors();
            _spritePx = _mapWindow.Height /MAX_MAP_HEIGHT;

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

            _player = new Player(Content);

            // Load in the temporary title / instruction fonts / storyfonts
            _titleFont = Content.Load<SpriteFont>("Fonts\\Title");
            _instructionFont = Content.Load<SpriteFont>("Fonts\\Instructions");
            _storyFont = Content.Load<SpriteFont>("Fonts\\StoryFont");

            // Load map info
            _tileSheet = Content.Load<Texture2D>("Images\\TileSheet");

            // make a darkness texture
            _darkness = Content.Load<Texture2D>("Images\\Darkness");
            
            // load the winning screen
            _winScreen = Content.Load<Texture2D>("Images\\winner");

            // load sounds
            _doorUnlock = Content.Load<SoundEffect>("Sounds\\door-lock-1");
            _background = Content.Load<Song>("Sounds\\background");
            MediaPlayer.IsRepeating = true;

            // read map given
            //int[,] temp =  _mapRead.ReadMap(1/*this number is map number, currently in the test file there are numbers 1,2,3,4,5,6 and 12*/,"FileTileMap.txt");
            // set number of tiles across to length of x - array
            //_maxMapWidth = temp.GetLength(0);
            // set number of tiles down to length of the y - array
            //_maxMapHeight = temp.GetLength(1);
            // set size of the tiles
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
            InputManager.Update();

            if (_backgroundStart == false)
            {
                MediaPlayer.Play(_background);
                _backgroundStart = true;
            }

            // finite state machine for the game state
            switch (_gameState)
            {
                case GameState.Title:
                    {
                        _win = false;
                        // if the player presses enter, start the game
                        if (InputManager.onKeyPress(Keys.Enter))
                            _gameState = GameState.GamePlay;
                        // if the player presses i give them instructions
                        else if (InputManager.onKeyPress(Keys.I))
                            _gameState = GameState.Instructions;
                        else if(InputManager.onKeyPress(Keys.C))
                            _gameState = GameState.Credit;

                        // set the timer  (this should be done elsewhere but for now...)
                        _timer = 15;

                        // set how much the darkness should increase (again this should propbably be done somewhere else)
                        _darknessAlpha = 0;
                        _increaseDarkness = (int)(255 / _timer);
                        _darknessControl = 0;

                        _player.ResetPlayer();
                        for (int i = 0; i < _floor1.Length; i++)
                        {
                            _floor1[i].ResetRoom();
                        }
                        _curRoom = 0;
                        break;
                    }
                case GameState.Credit:
                    {
                        // if the player presses enter, start the game
                        if (InputManager.onKeyPress(Keys.Enter))
                            _gameState = GameState.GamePlay;
                        // if the player presses b, send them back to the title
                        if (InputManager.onKeyPress(Keys.B))
                            _gameState = GameState.Title;

                        break;
                        
                    }
                case GameState.Instructions:
                    {
                        // if the player presses enter, start the game
                        if (InputManager.onKeyPress(Keys.Enter))
                            _gameState = GameState.GamePlay;
                        // if the player presses b, send them back to the title
                        if (InputManager.onKeyPress(Keys.B))
                            _gameState = GameState.Title;

                        break;
                    }
                case GameState.Paused:
                    {
                        _darknessAlpha = 245;
                        if (InputManager.onKeyPress(Keys.P))
                        { _darknessAlpha = tempDark; _gameState = GameState.GamePlay; }
                        if (InputManager.onKeyPress(Keys.Q))
                        { _darknessAlpha = tempDark; _gameState = GameState.Title; }
                        break;
                    }
                case GameState.GamePlay:
                    {
                        // Update player information
                        _player.Update(gameTime, _floor1[_curRoom].puzzle, MAX_MAP_WIDTH - 1, MAX_MAP_HEIGHT - 1);


                        // make only the puzzle board dark
                        _darknessWidth = 500;
                        _darknessHeight = 500;

                        //Pause??
                        if (InputManager.onKeyPress(Keys.P))
                        {
                            //make the whole screen dark
                            _darknessWidth = GraphicsDevice.Viewport.Width;
                            _darknessHeight = GraphicsDevice.Viewport.Height;

                            tempDark = _darknessAlpha;
                            _gameState = GameState.Paused;
                        }


                        // Check to see if current game level is UNSOLVED
                        if (!_floor1[_curRoom].puzzle.checkSol())
                        {
                            // adjust the timer
                            _timer -= gameTime.ElapsedGameTime.TotalSeconds;
                            DarknessUpdate(timer, false);
                            playedSound = false;
                        }

                        // If SOLVED, add to the timer & lighten *up to a certain point*
                        else
                        {
                            if (timer <= 15)
                                _timer += gameTime.ElapsedGameTime.TotalSeconds;
                            if(darknessAlpha != 0)
                                DarknessUpdate(timer, true);

                            //_timer = 13;
                            if (!playedSound)
                            {
                                _doorUnlock.Play();
                                playedSound = true;
                            }
                        }

                        // check to see if the player won
                        if (_win)        // TODO: WE MUST CHANGE THIS IF STATEMENT
                        {
                            _gameState = GameState.Win;
                        }

                        // check to see if the time ran out
                        if (_timer < 0)
                        {
                            //_floor1[_curRoom].ResetRoom();
                            _gameState = GameState.GameOver;
                            
                        }
                        break;
                    }
                case GameState.Win:
                    {
                        
                        // if the player presses enter send them back to the title screen
                        if (InputManager.onKeyPress(Keys.Enter))
                            _gameState = GameState.Title;

                        break;
                    }
                case GameState.GameOver:
                    {
                       
                        // if the player presses enter send them back to the title screen
                        if (InputManager.onKeyPress(Keys.Enter))
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
                    "Press  \" Enter \"  to Play",
                    new Vector2(95, 200),
                    Color.LightGray);

                spriteBatch.DrawString(
                    _instructionFont,
                    "Press  \" I \"  for Instructions" ,                
                    new Vector2(297, 200),
                    Color.LightGray);

                spriteBatch.DrawString(
                   _instructionFont,
                   "Press  \" C \"  for Credits",
                   new Vector2(525, 200),
                   Color.LightGray);
            }

            if (_gameState == GameState.Credit)
            {
                spriteBatch.DrawString(
                    _titleFont,
                    "Credits",
                    new Vector2(265, 50),
                    Color.Red);
                spriteBatch.DrawString(
                    _instructionFont,
                    "   The Five: \n" +
                    "Haydon Clore, \n" +
                    "David McCann, \n" +
                    "Georgia Partyka, \n" +
                    "Tyler Wozniak, \n" +
                    "Jesse Charland \n\n" +
                    "        Art: \n" +
                    "Catherine Lung"
                    ,
                    new Vector2(320, 150),
                    Color.LightGray);
                spriteBatch.DrawString(
                   _instructionFont,
                   "Press  \" B \"  to go Back",
                   new Vector2(290, 400),
                   Color.LightGray);
            }

            // draws to the screen if at the instruction screen
            if (_gameState == GameState.Instructions)
            {
                // these instructions are nonsense but they demonstrate the setup of the game state

                spriteBatch.DrawString(
                    _instructionFont,
                    "The object of this game is to light up the correct tiles and",
                    new Vector2(175, 50),
                    Color.LightGray);

                spriteBatch.DrawString(
                    _instructionFont,
                    "gain access to the next room of the mansion, before the darkness gets you.",
                    new Vector2(175, 75),
                    Color.LightGray);

                spriteBatch.Draw(_tileSheet, new Rectangle(175, 150, 250,50), Color.White);
                
                spriteBatch.DrawString(
                    _instructionFont,
                    "Wall          Inactive Tile      Lit Tile ",
                    new Vector2(175, 200),
                    Color.LightGray);

                spriteBatch.DrawString(
                    _instructionFont,
                    " Door          UnlitTile",
                    new Vector2(225, 130),
                    Color.LightGray);

                spriteBatch.DrawString(
                    _instructionFont,
                    "You may move back to solved rooms to regain light",
                    new Vector2(175, 250),
                    Color.LightGray);

                spriteBatch.DrawString(
                    _instructionFont,
                    "North and South doors are safe, beware of East and West",
                    new Vector2(175, 275),
                    Color.LightGray);

                spriteBatch.DrawString(
                    _instructionFont,
                    "You move around by using the arrow keys.",
                    new Vector2(175, 325),
                    Color.LightGray);

                spriteBatch.DrawString(
                    _instructionFont,
                    "Press  \" Enter \"  to Play \n" +
                    "Press  \" B \"  to go back to the title",
                    new Vector2(175, 375),
                    Color.LightGray);
            }

            // draws to the screen if during gameplay
            if (_gameState == GameState.GamePlay || _gameState == GameState.Paused)
            {
                // Draw current map
                _floor1[_curRoom].Draw(spriteBatch, _mapWindow);
                _floor1[_curRoom].DrawSoln(spriteBatch, _tileSheet);
                _floor1[_curRoom].DrawStory(spriteBatch, _storyFont);

                // Draw player location
                _player.Draw(spriteBatch, _mapWindow);

                // draw the darkness
                spriteBatch.Draw(_darkness, new Rectangle(0, 0, _darknessWidth, _darknessHeight), new Color(255, 255, 255, _darknessAlpha));

                // IF PAUSED draw some paused info stuff
                if (_gameState == GameState.Paused)
                {
                    spriteBatch.DrawString(
                    _titleFont,
                    "Paused",
                    new Vector2(325, 100),
                    Color.Red);

                    spriteBatch.DrawString(
                        _instructionFont,
                        "Press \"P\" to return to the game",
                        new Vector2(225, 175),
                        Color.LightGray);

                    spriteBatch.DrawString(
                        _instructionFont,
                        "Press \"Q\" to quit to the Main Menu",
                        new Vector2(225, 200),
                        Color.LightGray);
                }
                //Debugging the timer stuff
                /*spriteBatch.DrawString(
                        _instructionFont,
                        timer.ToString() + " III " + darknessAlpha + " III " + darknessControl,
                        new Vector2(300, 0),
                        Color.Red);*/
            }

            // draws to the sceen if the player won
            if (_gameState == GameState.Win)
            {
                /*spriteBatch.Draw(
                    _winScreen,
                    new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
                    Color.White);*/


                spriteBatch.DrawString(
                    _titleFont,
                    "The End",
                    new Vector2(275, 100),
                    Color.Red);
                spriteBatch.DrawString(
                    _storyFont,
                "I enter the final room, and I see her, \n "+
                "          My beautiful Phoboe \n " +
                "          but her eyes are grey...\n " +
                "                  ...grey...... \n" +
                "                  ......grey...",
                    new Vector2(240, 200),
                    Color.DarkGray);
            }

            // draws to the screen if the game is over
            if (_gameState == GameState.GameOver)
            {
                // again some nonsense text but it shows the state exists and works

                spriteBatch.DrawString(
                    _titleFont,
                    "You Lose",
                    new Vector2(300, 100),
                    Color.Red);

                spriteBatch.DrawString(
                    _instructionFont,
                    "You have gone insane and your fiance has died.",
                    new Vector2(250, 175),
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
        /// This method will, sadly, require some hardcoding with each new floor,
        /// due to my Generics troubles.  Oh well!
        /// </summary>
        public void buildFloors()
        {
            for (int i = 0; i < (_floor1.Length - 1); i++)
            {
                _floor1[i].north = _floor1[i + 1];
                _floor1[i + 1].south = _floor1[i];
            }
            for (int i = 0; i < _floor1.Length; i++)
            {
                Random rand = new Random();
                int j = rand.Next(_floor1.Length);
                int k = rand.Next(_floor1.Length);
                if (j != k)
                {
                    _floor1[j].east = _floor1[k];
                    _floor1[k].west = _floor1[j];
                }
            }
        }

        public void ResetGame(Player p)
        {
            //Reset Player
            p.ResetPlayer();
            p.xPos = 0;
            p.yPos = 0;
            

            //Reset Puzzle
        }

        /// <summary>
        /// Modifies the Darkness attributes to lighten/darken the screen
        /// </summary>
        /// <param name="timer">In-Game timer</param>
        /// <param name="solved">Whether or not the current puzzle is solved</param>
        public void DarknessUpdate(double timer, bool solved)
        {
            if (solved)
            {
                _darknessControl++;
                // decrease the darkness
                if (_darknessControl % 60 == 0)
                    _darknessAlpha -= _increaseDarkness;
            }
            else
            {
                _darknessControl++;
                // increase the darkness
                if (_darknessControl % 60 == 0)
                    _darknessAlpha += _increaseDarkness;
            }
        }
    }
}
