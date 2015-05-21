using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Nyctophobia
{
    /// <summary>
    /// TileMap class used to draw the tile-based map
    /// </summary>
    class TileMap
    {
        // ///////////////////////////////////////////
        // Tile Types
        //
        // Wall - Player cannot move onto tile
        // Floor - Normal tile, no effect
        // On - Toggle to off if stepped on
        // Off - Toggle to on if stepped on
        // NumTypes - Used to define the number of different tile types
        //
        // ///////////////////////////////////////////

        enum TileType { Wall, Floor, On, Off, NumTypes };

        // ///////////////////////////////////////////
        // Fields
        // ///////////////////////////////////////////

        Texture2D _texture;         // Tile sprite texture
        Rectangle[] _tiles;         // Rectangles used to determine source rectangle
        int[,] _tileInfo;           // 2D array to determine types of tiles at the tile coordinates
        bool _solved;               // Determines whether the puzzle is solved


        // ///////////////////////////////////////////
        // Constructors
        // ///////////////////////////////////////////

        /// <summary>
        /// Initialize a new instance of TileMap
        /// </summary>
        /// <param name="texture">Sprite sheet of tiles</param>
        /// <param name="width">Tile source width</param>
        /// <param name="height">Tile source height</param>
        /// <param name="maxWidth">Max tiles columns</param>
        /// <param name="maxHeight">Max tile rows</param>
        public TileMap(Texture2D texture, int width, int height, int maxWidth, int maxHeight)
        {
            // Store the texture
            _texture = texture;

            // Initialize the array of source rectangles
            _tiles = new Rectangle[(int)TileType.NumTypes];

            // Find and store source rectangles based on texture width
            // All tiles are stored in a single row, so no need to check for height
            for (int i = 0; i < (int)TileType.NumTypes; i++)
            {
                // Check to see if the rectangle is located on the source image
                // Do not read in data if rectangle is larger that image
                if (i * width > texture.Width || height > texture.Height) { return; }

                // Store source rectangle information
                _tiles[i] = new Rectangle(i * width, 0, width, height);
            }

            // Initialize array that stores the tile at each element in the array
            _tileInfo = new int[maxHeight, maxWidth];

            // Foreach element in the array, set the default to the floor tile
            for (int i = 0; i < maxHeight; i++)
            {
                for (int j = 0; j < maxWidth; j++)
                {
                    _tileInfo[i, j] = (int)TileType.Floor;
                }
            }
        }

        /// <summary>
        /// Initialize a new instance of TileMap
        /// Creates TileMap based on information passed in
        /// </summary>
        /// <param name="texture">Sprite sheet of tiles</param>
        /// <param name="tileInfo">2D array of tile index</param>
        /// <param name="width">Tile source width</param>
        /// <param name="height">Tile source height</param>
        /// <param name="maxWidth">Max tiles columns</param>
        /// <param name="maxHeight">Max tile rows</param>
        public TileMap(Texture2D texture, int[,] tileInfo, int width, int height, int maxWidth, int maxHeight)
            : this(texture, width, height, maxWidth, maxHeight)
        {
            // Store the number of rows and columns for passed in tile index array
            int rows = tileInfo.GetUpperBound(0) + 1;
            int columns = tileInfo.GetUpperBound(1) + 1;

            // Store each element in the array
            for (int i = 0; i < maxHeight; i++)
            {
                for (int j = 0; j < maxWidth; j++)
                {
                    // Check to see if the index corresponds to a location in the array
                    // and to see if the tile index is less than the number of tile types
                    if (i < rows && j < columns && tileInfo[i, j] < (int)TileType.NumTypes)
                    {
                        this._tileInfo[i, j] = tileInfo[i, j];
                    }
                }
            }
        }

        /// <summary>
        /// Draws the tiles onto the game screen
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw image on screen</param>
        /// <param name="width">Destination rectangle width</param>
        /// <param name="height">Destination rectangle height</param>
        public void Draw(SpriteBatch spriteBatch, int width, int height)
        {
            // Draw each tile on screen
            for (int i = 0; i <= _tileInfo.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= _tileInfo.GetUpperBound(1); j++)
                {
                    spriteBatch.Draw(
                        _texture,                       // Tile sprite map
                        new Rectangle(                  // Destination rectangle
                            i * width,
                            j * height,
                            width,
                            height),
                            _tiles[_tileInfo[j, i]],    // Source rectangle
                        Color.White);                   // Tint color
                }
            }
        }        
        
    }
}
