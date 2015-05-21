//Tyler Wozniak

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nyctophobia
{
    public class Tile : GameObject
    {
        //Attributes
        protected bool walkable;
        private Texture2D _texture;
        private int _tileState;
        public int tileState
        {
            get { return _tileState; }
        }
        protected TileType _type;
        public TileType type
        { get { return _type; } set { _type = value; } }

        public enum TileType
        {
            Wall,
            Door,
            Floor
        }

        //Properties
        public bool Walkable
        { get { return walkable; } set { walkable = value; } }
        
        /// <summary>
        /// A default constructor for Tile generic purposes
        /// </summary>
        public Tile()
        {
            this._texture = Game1.tileSheet;
        }

        /// <summary>
        /// Tile Constructor
        /// </summary>
        /// <param name="x">Tile's grid-based X location</param>
        /// <param name="y">Tile's grid-based Y location</param>
        public Tile(int x, int y)
            : base(x, y)
        {
            this._texture = Game1.tileSheet;
        }

        /// <summary>
        /// Another method for allowing generics to work properly
        /// </summary>
        /// <param name="x">integer X index to draw to</param>
        /// <param name="y">integer Y index to draw to</param>
        public void initTile(int x, int y)
        {
            this.xPos = x;
            this.yPos = y;
            this._texture = Game1.tileSheet;
        }

        public virtual void setState(int state)
        {
            _tileState = state;
            this.walkable = true;
            if (state <= 0)
            {
                type = TileType.Wall;
                this.walkable = false;
            }
            else if (state == 1)
                type = TileType.Door;
            else
                type = TileType.Floor;
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle drawRect)
        {
            // Draw player
            spriteBatch.Draw(
                Game1.tileSheet,                // Player sprite
                drawRect,
                    new Rectangle((tileState * 50), 0, 50, 50), //TODO: Needs to be fixed
                Color.White);                   // Tint color
        }

    }
}