using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nyctophobia
{
    public class LightTile : Tile
    {
        //Attributes
        public enum LightState
        {
            Lit,
            Unlit
        }

        private LightState _lightState;
        public LightState lightState
        {
            get { return _lightState; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public LightTile()
        {

        }

        /// <summary>
        /// LightTile Constructor - Light puzzle
        /// </summary>
        /// <param name="x">Tile's X Position</param>
        /// <param name="y">Tile's Y Position</param>
        /// <param name="state">Tile State</param>
        /// <param name="image">Tile Image</param>
        public LightTile(int x, int y, TileType type, Texture2D image)
            : base(x, y)
        {
            _type = type;
        }

        public override void setState(int state)
        {
            base.setState(state);
            if (state == 3)
            {
                _lightState = LightState.Unlit;
            }
            else if (state == 4)
            {
                _lightState = LightState.Lit;
            }
        }
    }
}
