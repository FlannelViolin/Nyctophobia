// Written by Tyler Wozniak

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nyctophobia
{
    /// <summary>
    /// This is the basis for any grid-based drawn objects in the game.
    /// This allows you to only access the Pixel location via getting,
    /// and setting the Pixels based on a grid location
    /// </summary>
    public abstract class GameObject
    {
        /// <summary>
        /// This is the x Pixel position
        /// Never touch this!
        /// </summary>
        private int _xPx;
        /// <summary>
        /// This is the y Pixel position
        /// Also never touch this
        /// </summary>
        private int _yPx;

        /// <summary>
        /// Call this when drawing for the X Pixel position
        /// </summary>
        public int xPx
        {
            get { return _xPx; }
            set { _xPx = value; }
        }
        /// <summary>
        /// Call this when drawing for the Y Pixel position
        /// </summary>
        public int yPx
        {
            get { return _yPx; }
            set { _yPx = value; }
        }

        /// <summary>
        /// Sets the x Pixel position, based on a grid location
        /// </summary>
        public int xPos
        {
            set { _xPx = value * Game1._spritePx; }
            get { return _xPx / Game1._spritePx; }
        }
        /// <summary>
        /// Sets the y pixel position based on a grid location
        /// </summary>
        public int yPos
        {
            set { _yPx = value * Game1._spritePx; }
            get { return _yPx / Game1._spritePx; }
        }

        /// <summary>
        /// Base GameObject constructor
        /// </summary>
        /// <param name="x">The x grid location</param>
        /// <param name="y">The y grid location</param>
        public GameObject(int x, int y)
        {
            xPos = x;
            yPos = y;
        }

        /// <summary>
        /// Default constructor
        /// DO NOT USE THIS except with generics
        /// or your object will not draw
        /// </summary>
        public GameObject()
        {

        }

        public abstract void Draw(SpriteBatch spriteBatch, Rectangle drawRect);
    }
}
