using System;
using Microsoft.Xna.Framework.Input;

namespace Nyctophobia
{
    /// <summary>
    /// Class that handles all of the game input
    /// </summary>
    public static class InputManager
    {
        #region Fields

        // ////////////////////////////////////////////
        // Field Information
        // ////////////////////////////////////////////

        private static KeyboardState _curKeyboardState;     // Current keyboard state
        private static KeyboardState _prevKeyboardState;    // Previous keyboard state

        #endregion

        #region Methods

        // ////////////////////////////////////////////
        // Method Information
        // ////////////////////////////////////////////

        /// <summary>
        /// Sets the previous keyboard state to the current and updates the current state
        /// </summary>
        public static void Update()
        {
            _prevKeyboardState = _curKeyboardState;
            _curKeyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Checks to see if a key is being pressed down
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if the key is down</returns>
        public static bool isKeyDown(Keys key)
        {
            return _curKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks to see if a key is up
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if the key is up</returns>
        public static bool isKeyUp(Keys key)
        {
            return _curKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Checks to see if a key has just been pressed
        /// Does not return true if the key is being held down
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if the key has been pressed</returns>
        public static bool onKeyPress(Keys key)
        {
            return _curKeyboardState.IsKeyDown(key) &&
                _prevKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Checks to see if a key has just been released
        /// Does not return true if the key is already up
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if the key has been released</returns>
        public static bool onKeyRelease(Keys key)
        {
            return _curKeyboardState.IsKeyUp(key) &&
                _prevKeyboardState.IsKeyDown(key);
        }

        #endregion
    }
}
