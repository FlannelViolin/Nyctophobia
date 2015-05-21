//Tyler Wozniak WUZ HERE
//Haydon Clore

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Nyctophobia
{
    public class LightPuzzle<T> : Puzzle<T> where T:Tile, new()
    {
        private MapReader readS = new MapReader();
        private string _solvedStates;
        private string _curStates;
       
       
        public override bool checkSol()
        {
            // First time through reads in the solution, once set it won't change for this puzzle
            if (_solvedStates == null)
            {
                _solvedStates = readS.MapString(parent.index + 1, parent.map, true);
                
            }
            _curStates = "";
            foreach (T tile in parent.roomArray)
            {
                _curStates += tile.tileState.ToString();
                
            }
            
            return _curStates == _solvedStates;
        }

        //Right now this can return a boolean regarding whether or not the player
        //--position is in a wall but can be changed back to void, just delete the "move" references
        //--and change the Puzzle method to be void as well
        public override bool handleStep(Player player, int newX, int newY)
        {
            if (newX < 0 || newX >= parent.roomArray.GetLength(1)
                || newY < 0 ||  newY >= parent.roomArray.GetLength(0)) { return false; }

            bool solved = checkSol();
            if (newX == 9)
            {
                return false; // quick fix for the off right side bug
            }
            T curTile = parent.roomArray[newY, newX];

            if (!checkSol())
            {
                if (curTile.tileState == 3)
                {
                    curTile.setState(4);
                    return true;
                }
                else if (curTile.tileState == 4)
                {
                    curTile.setState(3);
                    return true;
                }
            }

            if (curTile.tileState == 0)
            {
                return false;
            }
            else if (curTile.tileState == 1 && !checkSol())
            {
                if (newY == parent.roomArray.GetLength(0) - 1)
                {
                    parent.MoveRoom(newX, newY);
                    return true;
                }
                return false;
            }
            else if (curTile.tileState == 1 && checkSol())
            {
                if (curTile.tileState == 1 && checkSol() && Game1.curRoom == (Game1.floor1.Length - 1))
                {
                    Game1.win = true;
                }
                else
                    parent.MoveRoom(newX, newY);
            }

            return true;
        }

        
    }
}
