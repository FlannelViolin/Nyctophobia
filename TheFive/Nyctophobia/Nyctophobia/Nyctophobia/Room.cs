//Tyler Wozniak

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nyctophobia
{
    public class Room<T> where T: Tile, new()
    {
        //Array
        private T[,] _roomArray;
        private T[,] _resetArray;
        private int[,] _solnArray;
        private int _index;
        MapReader mapRead = new MapReader();       
        private string _map;
        public string map
        { get { return _map; } }
        public int index
        {
            get { return _index; }
        }
        private Puzzle<T> _puzzle;
        public Puzzle<T> puzzle
        {
            get { return _puzzle; }
        }

        private int _numLinks;
        public int numLinks
        {
            get { return _numLinks; }
        }
        private Room<T> _north;
        private Room<T> _south;
        private Room<T> _east;
        private Room<T> _west;

        public Room<T> north
        {
            get { return _north; }
            set { _north = value; }
        }
        public Room<T> south
        {
            get { return _south; }
            set { _south = value; }
        }
        public Room<T> east
        {
            get { return _east; }
            set { _east = value; }
        }
        public Room<T> west
        {
            get { return _west; }
            set { _west = value; }
        }

        public T[,] roomArray
        {
            get { return _roomArray; }
            set { _roomArray = value; }
        }

        public int[,] solnArray
        {
            get { return _solnArray; }
            set { _solnArray = value; }
        }
        public T[,] resetArray
        {
            get { return _resetArray; }
            set { _resetArray = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">Tiles across</param>
        /// <param name="height">Tiles down</param>
        /// <param name="puzzle">Puzzle & Tile type</param>
        public Room(Puzzle<T> puzzle,int i, string map)
        {
            _index = i;
            _map = map;
            _puzzle = puzzle;
            _puzzle.initialize(this, map);
        }

        public void Draw(SpriteBatch sb, Rectangle drawRect)
        {
            int rows = _roomArray.GetLength(0);
            int columns = _roomArray.GetLength(1);
            //int solnRows = _solnArray.GetLength(0);
            //int solnCol = _solnArray.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    _roomArray[i, j].Draw(sb, new Rectangle(
                        drawRect.X + (j * Game1._spritePx),
                        drawRect.Y + (i * Game1._spritePx),
                        Game1._spritePx, Game1._spritePx));
                    
                    
                }
            }

        }


        public void DrawSoln(SpriteBatch sb, Texture2D _tileMap)
        {
            int tileSource = 50;
            int solnRows = _solnArray.GetLength(0);
            int solnCol = _solnArray.GetLength(1);
            for (int i = 0; i < solnRows; i++)
            {
                for (int j = 0; j < solnCol; j++)
                {
                    sb.Draw(_tileMap,
                        new Rectangle(525 + (j * 20), 30 + (i * 20), 20, 20),
                        new Rectangle(_solnArray[i, j] * tileSource, 0, tileSource, tileSource),
                        Color.Beige);
                }
            }
        }

        public void DrawStory(SpriteBatch sb, SpriteFont sf)
        {
            sb.DrawString(
                sf, 
                mapRead.ReadStory(index + 1, "../../../../NyctophobiaContent/NycStory.txt"),
                new Vector2(500, 270),
                Color.DarkOrange);
        }


        public void MoveRoom(int x, int y)
        {
            if (x == 0 && east != null)
            {
                Game1.curRoom = east._index;
              //  Game1.playedSound = false; // there's a better way to do thiss, but just checking
                Game1.player.xPos = east._roomArray.GetLength(1)-1;
                             
            }
            else if (y == 0 && north != null)
            {
                Game1.curRoom = north._index;
                //Game1.playedSound = false;
                Game1.player.yPos = north._roomArray.GetLength(0)-1;
            }
            else if (x == roomArray.GetLength(1)-1 && west != null)
            {
                Game1.curRoom = west._index;
               // Game1.playedSound = false;
                Game1.player.xPos = 0;
            }
            else if (y == roomArray.GetLength(0)-1 && south != null)
            {
                Game1.player.yPos = 0;
                Game1.curRoom = south._index;
               // Game1.playedSound = false;
            }
        }

        public void ResetRoom()
        {
            Game1._timer = 15;
            _puzzle.resetPuzzle();
        }


    }
}
