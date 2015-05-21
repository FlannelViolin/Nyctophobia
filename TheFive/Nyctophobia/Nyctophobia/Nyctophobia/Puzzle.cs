// Written by Tyler Wozniak

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Nyctophobia
{
    /// <summary>
    /// This is the base class for Puzzles.  It will be held in each room
    /// </summary>
    /// <typeparam name="T">The type of tile being used by this puzzle</typeparam>
    public abstract class Puzzle<T> where T:Tile, new()
    {
        private MapReader _mapRead;
        private T _baseTile;
        private Room<T> _parent;
        private string _map;
        public Room<T> parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        /// <summary>
        /// Stub
        /// </summary>
        public Puzzle()
        {
            _mapRead = new MapReader();
        }

        /// <summary>
        /// Initializes the Room with the correct tiles and layout
        /// </summary>
        public void initialize(Room<T> parentRoom, string map)
        {
            _parent = parentRoom;
            _map = map;
            int index = parentRoom.index;
            int[,] temp = _mapRead.ReadMap(index + 1, map);
            parentRoom.solnArray = _mapRead.ReadMap(index + 1, "S_" + map);
            parentRoom.roomArray = new T[temp.GetLength(0), temp.GetLength(1)];
            parentRoom.resetArray = new T[temp.GetLength(0), temp.GetLength(1)];
            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {
                    parentRoom.roomArray[i, j] = new T();
                    parentRoom.roomArray[i, j].initTile(j, i);
                    parentRoom.roomArray[i, j].setState(temp[i, j]);

                    parentRoom.resetArray[i, j] = new T();
                    parentRoom.resetArray[i, j].initTile(j, i);
                    parentRoom.resetArray[i, j].setState(temp[i, j]);
                }
            }

        }

        public void resetPuzzle()
        {
            int index = _parent.index;
            int[,] temp = _mapRead.ReadMap(index + 1, _map);
            _parent.roomArray = new T[temp.GetLength(0), temp.GetLength(1)];
            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {
                    _parent.roomArray[i, j] = new T();
                    _parent.roomArray[i, j].initTile(j, i);
                    _parent.roomArray[i, j].setState(temp[i, j]);
                }
            }
            
        }

        

        /// <summary>
        /// Stub
        /// </summary>
        /// <returns>Whether or not the puzzle is solved</returns>
        abstract public bool checkSol();

        abstract public bool handleStep(Player play, int newX, int newY);
    }
}
