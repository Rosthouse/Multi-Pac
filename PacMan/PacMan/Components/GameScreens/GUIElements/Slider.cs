using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PacManClient.Components.GameScreens.GUIElements
{
    /// <summary>
    /// Allows creating a slider for options that need a slider
    /// </summary>
    class Slider
    {
        private Vector2 position;
        private int steps;

        private Texture2D dot;
        private Texture2D lineDot;
        private Texture2D endDot;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="steps"></param>
        public Slider(Vector2 position, int steps)
        {
            this.position = position;
        }

        /// <summary>
        /// Loads the content for the slider
        /// </summary>
        /// <param name="contentManager">The content manager</param>
        public void LoadContent(ContentManager contentManager)
        {
            
        }

        /// <summary>
        /// Updates this slider
        /// </summary>
        /// <param name="gameTime">the current gametime</param>
        public void Update(GameTime gameTime)
        {
            
        }

        /// <summary>
        /// Draws this slider
        /// </summary>
        /// <param name="spriteBatch">the spritebatch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            
        }


    }
}
