using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileTest
{
    /// <summary>
    /// User controlled class
    /// </summary>
    class Player
    {
        // //////////////////////////////////////
        // Fields
        // //////////////////////////////////////

        Texture2D _texture;         // Player sprite sheet
        List<Rectangle> frames;     // Animation frames
        Vector2 _coord;             // Player coordinate location
        Vector2 _speed;             // Speed player moves between tiles
        int _timeToMove;            // Time it takes to move between tile
        float _rotation;            // Player rotation
        bool _isMoving;             // Determines whether player is moving and animating

        // //////////////////////////////////////
        // Constructor
        // //////////////////////////////////////

        /// <summary>
        /// Initialize a new Player
        /// </summary>
        /// <param name="texture">Player sprite sheet</param>
        /// <param name="startPos">Starting coordinate</param>
        public Player(Texture2D texture, Vector2 startPos)
        {
            // Store values
            _texture = texture;
            //Disregard start pos parameter in favor of not starting in a wall...yeah
            _coord = new Vector2(1f);

            // TODO: Initialize animation values
        }

        // ///////////////////////////////////////
        // Methods
        // ///////////////////////////////////////

        /// <summary>
        /// Updates the player location based on user input
        /// </summary>
        /// <param name="keyState">Current keyboard state</param>
        /// <param name="prevState">Previous keyboard state</param>
        /// <param name="mapWidth">Max tile columns</param>
        /// <param name="mapHeight">Max tile rows</param>
        public void Update(KeyboardState keyState, KeyboardState prevState, int mapWidth, int mapHeight)
        {
            // TODO: Add new variable for attempted move coordinate and check to see if
            //       the tile is available before updating the player information

            // Update positon based on input
            if(SingleKeyPress(Keys.Up, keyState, prevState))
            {
                _coord += new Vector2(0, -1);
            }
            else if (SingleKeyPress(Keys.Down, keyState, prevState))
            {
                _coord += new Vector2(0, 1);
            }
            else if (SingleKeyPress(Keys.Left, keyState, prevState))
            {
                _coord += new Vector2(-1, 0);
            }
            else if (SingleKeyPress(Keys.Right, keyState, prevState))
            {
                _coord += new Vector2(1, 0);
            }

            // Check to see if player location still in bounds
            if (_coord.X > mapWidth) { _coord.X = mapWidth; }
            else if (_coord.X < 0) { _coord.X = 0; }

            if (_coord.Y > mapHeight) { _coord.Y = mapHeight; }
            else if (_coord.Y < 0) { _coord.Y = 0; }
        }

        /// <summary>
        /// Draws the player on screen
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to draw object</param>
        /// <param name="width">Destination rectangle width</param>
        /// <param name="height">Destination rectangle height</param>
        public void Draw(SpriteBatch spriteBatch, int width, int height)
        {
            // Draw player
            spriteBatch.Draw(
                _texture,                       // Player sprite
                new Rectangle(                  // Destination rectangle
                    (int)_coord.X * width,
                    (int)_coord.Y * height,
                    width,
                    height),
                    new Rectangle(0,0,50,50),
                Color.White);                   // Tint color
        }

        /// <summary>
        /// Returns whether a key has been pressed
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <param name="cur">Current keyboard state</param>
        /// <param name="prev">Previous keyboard state</param>
        /// <returns>True if key is pressed, false if key is not pressed or if key is held down</returns>
        public bool SingleKeyPress(Keys key, KeyboardState cur, KeyboardState prev)
        {
            // Return if the key is currently down and has previously been up
            return cur.IsKeyDown(key) && prev.IsKeyUp(key);
        }
    }
}
