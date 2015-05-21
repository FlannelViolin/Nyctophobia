using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileTest
{
    /// <summary>
    /// Parent sprite class used for drawable objects
    /// All drawable objects derive from Sprite
    /// </summary>
    class Sprite
    {
        // ///////////////////////////////
        // Fields
        // ///////////////////////////////

        Texture2D _texture;         // Object texture image
        Vector2 _coordPosition;     // Vector position in respect to coordinate system

        List<Rectangle> _frames;    // Drawable frames in texture
        int width;                  // Width of frames
        int height;                 // Height of frames

        Color tint;                 // Tint color of object

        bool collidable;            // Determines if object can collide with other objects

    }
}
