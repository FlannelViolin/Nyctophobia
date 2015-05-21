//Georgia Partyka
//Jesse Charland
//David McCann

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Nyctophobia
{
    /// <summary>
    /// User controlled class
    /// </summary>
    public class Player : GameObject
    {
        #region Fields

        // ///////////////////////////////////////////
        // Player Animation State Information
        // ///////////////////////////////////////////

        // Stores the starting frame position for each animation cycle
        private const int DOWN = 6;     // Starting position for downward animation
        private const int LEFT = 9;     // Starting position for left animation
        private const int UP = 0;       // Starting position for upward animation
        private const int RIGHT = 3;    // Starting position for right animation

        private int _playerState;       // Current player animation state

        // ///////////////////////////////////////////
        // Sprite Sheet Information
        // ///////////////////////////////////////////

        Texture2D _texture;                     // Player sprite sheet
        List<Rectangle> _frames;                // List of animation frame rectangles

        private const int NUM_FRAMES = 12;      // Number of frames in the sprite sheet
        private const int FRAMES_PER_CYCLE = 3; // Number of frames to complete one animation loop

        // ///////////////////////////////////////////
        // Animation Information
        // ///////////////////////////////////////////

        TimeSpan _animationTimer;   // Timer to keep track of when to increase frame
        int _currentFrame;          // Current player frame index
        int _speed;                 // Speed player moves between tiles
        double _timePerFrame = 100; // Time (in milliseconds) it takes to move to next frame
        bool _isMoving;             // Determines whether player is moving and animating

        // ///////////////////////////////////////////
        // Movement Information
        // ///////////////////////////////////////////

        int _newPos;                // Projected location used for movement
        Queue<Keys> _keyQueue;      // Player movement key queue

        #endregion

        #region Properties

        // ///////////////////////////////////////////
        // Property Information
        // ///////////////////////////////////////////

        /// <summary>
        /// Returns a queue of keys
        /// </summary>
        public Queue<Keys> KeyQueue { get { return _keyQueue; } }

        #endregion

        #region Constructor

        // //////////////////////////////////////
        // Constructor
        // //////////////////////////////////////

        /// <summary>
        /// Initializes a new player
        /// </summary>
        /// <param name="content">Content manager for loading data</param>
        /// <param name="drawSize">Draw size of the game object</param>
        /// <param name="x">X starting coordinate</param>
        /// <param name="y">Y starting coordinate</param>
        public Player(ContentManager content, int x = 0, int y = 0)
            : base(x, y)
        {
            // Initialize animation frames
            _texture = content.Load<Texture2D>("Images\\NycSheet");
            ChangeState(UP);
            _frames = new List<Rectangle>();

            // Add frames to frame list
            int frameWidth = _texture.Width / NUM_FRAMES;
            int frameHeight = _texture.Height;
            for (int i = 0; i < NUM_FRAMES; i++)
            {
                _frames.Add(new Rectangle(
                    frameWidth * i,
                    0,
                    frameWidth,
                    frameHeight));
            }

            // Initialize animation values
            _animationTimer = TimeSpan.Zero;
            _keyQueue = new Queue<Keys>();
            _speed = Game1._spritePx / 10;
        }

        #endregion

        #region Update

        // ///////////////////////////////////////
        // Update
        // ///////////////////////////////////////

        /// <summary>
        /// Update the player information
        /// </summary>
        /// <param name="gameTime">Game Time</param>
        /// <param name="puzzle">Current Puzzle</param>
        /// <param name="mapWidth">Number of tile columns per row</param>
        /// <param name="mapHeight">Number of tile rows</param>
        public void Update(GameTime gameTime, Puzzle<LightTile> puzzle, int mapWidth, int mapHeight)
        {
            bool movementStopped = false;   // Update more information if player has moved

            #region Update Animation

            // Add time to the animation timer
            if (_isMoving) { _animationTimer += gameTime.ElapsedGameTime; }

            // Increase frame if past the time per frame mark
            if (_animationTimer.Milliseconds > _timePerFrame)
            {
                _currentFrame = (_currentFrame + 1) % FRAMES_PER_CYCLE;
                _animationTimer = TimeSpan.Zero;
            }

            #endregion

            #region Update Player Movement

            // Update player position if moving
            if (_isMoving)
            {
                switch (_playerState)
                {
                    case UP:
                        if (yPx > _newPos) { yPx -= _speed; }
                        else
                        {
                            yPx = _newPos;
                            movementStopped = true;
                        }
                        break;

                    case DOWN:
                        if (yPx < _newPos) { yPx += _speed; }
                        else
                        {
                            yPx = _newPos;
                            movementStopped = true;
                        }
                        break;

                    case LEFT:
                        if (xPx > _newPos) { xPx -= _speed; }
                        else
                        {
                            xPx = _newPos;
                            movementStopped = true;
                        }
                        break;

                    case RIGHT:
                        if (xPx < _newPos) { xPx += _speed; }
                        else
                        {
                            xPx = _newPos;
                            movementStopped = true;
                        }
                        break;
                }
            } // End updating player movement
            
            // If player stopped, change animation values
            if (movementStopped)
            {
                if (_keyQueue.Count > 0)
                {
                    // Dequeue the key and reset animation update values
                    _keyQueue.Dequeue();
                }
                _animationTimer = TimeSpan.Zero;
                _isMoving = false;
                _currentFrame = 0;
            }

            #endregion

            #region Update Key Presses

            // Add keys to the queue if a key is pressed and there is space
            // The key queue can only hold two values
            if (_keyQueue.Count <= 1)
            {
                if (InputManager.onKeyPress(Keys.Up)) { _keyQueue.Enqueue(Keys.Up); }
                if (InputManager.onKeyPress(Keys.Down)) { _keyQueue.Enqueue(Keys.Down); }
                if (InputManager.onKeyPress(Keys.Right)) { _keyQueue.Enqueue(Keys.Right); }
                if (InputManager.onKeyPress(Keys.Left)) { _keyQueue.Enqueue(Keys.Left); }
            }

            #endregion

            #region Update Player State

            // Update player movement state based on keys in queue
            if (_keyQueue.Count > 0 && _isMoving == false)
            {
                switch (_keyQueue.Peek())
                {
                    case Keys.Up:
                        ChangeState(UP);
                        if (puzzle.handleStep(this, xPos, yPos - 1))
                        {
                            _newPos = yPx - Game1._spritePx;
                            _isMoving = true;
                        }
                        else { _keyQueue.Dequeue(); }
                        break;

                    case Keys.Down:
                        ChangeState(DOWN);
                        if (puzzle.handleStep(this, xPos, yPos + 1))
                        {
                            _newPos = yPx + Game1._spritePx;
                            _isMoving = true;
                        }
                        else { _keyQueue.Dequeue(); }
                        break;

                    case Keys.Left:
                        ChangeState(LEFT);
                        if (puzzle.handleStep(this, xPos -1, yPos))
                        {
                            _newPos = xPx - Game1._spritePx;
                            _isMoving = true;
                        }
                        else { _keyQueue.Dequeue(); }
                        break;

                    case Keys.Right:
                        ChangeState(RIGHT);
                        if (puzzle.handleStep(this, xPos + 1, yPos))
                        {
                            _newPos = xPx + Game1._spritePx;
                            _isMoving = true;
                        }
                        else { _keyQueue.Dequeue(); }
                        break;
                }
            }

            #endregion

            #region Update Player In Bounds

            // Check to see if player location still in bounds
            if (xPx > mapWidth * Game1._spritePx)
            {
                _newPos = xPos = mapWidth;
            }
            else if (xPx < 0)
            {
                _newPos = xPos = 0;
            }

            if (yPx > mapHeight * Game1._spritePx)
            {
                _newPos = yPos = mapHeight;
            }
            else if (yPx < 0)
            {
                _newPos = yPos = 0;
            }

            #endregion
        }

        #endregion

        #region Draw

        // ///////////////////////////////////////
        // Draw
        // ///////////////////////////////////////

        /// <summary>
        /// Draws the player on screen
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to draw object</param>
        /// <param name="width">Destination rectangle width</param>
        /// <param name="height">Destination rectangle height</param>
        public override void Draw(SpriteBatch spriteBatch, Rectangle drawRect)
        {
            spriteBatch.Draw(
                _texture,
                new Rectangle(
                    drawRect.X + xPx,
                    drawRect.Y + yPx,
                    Game1._spritePx,
                    Game1._spritePx),
                _frames[_playerState + _currentFrame],
                Color.White);
        }

        #endregion

        #region Helper Methods

        public void ResetPlayer()
        {
            _keyQueue.Clear();
            ChangeState(UP);
            xPos = 0;
            yPos = 0;
        }

        /// <summary>
        /// Change the player state and set the current frame to the
        /// beginning of the animation loop
        /// </summary>
        /// <param name="state"></param>
        public void ChangeState(int state)
        {
            _playerState = state;
            _isMoving = false;
            _currentFrame = 0;
        }
        #endregion
    }
}
