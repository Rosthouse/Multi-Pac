using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PacManShared.Entities.Player;

namespace PacManClient.Components.GameScreens.GamePlayScreens.GUI
{
    class GUIString:GUIElement
    {
        private Vector2 stringOffset;
        private Color stringColor;

        private SpriteFont font;

        public GUIString(MovableObject watchee, Color stringColor, Vector2 position, Vector2 stringOffset) : base(watchee, position)
        {
            this.stringColor = stringColor;
            this.stringOffset = stringOffset;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            this.font = contentManager.Load<SpriteFont>(@"SpriteFonts\gamefont");
        }

        public override void Draw(SpriteBatch spriteBatch, int layer)
        {
            spriteBatch.DrawString(font, "Score: " + watchee.score.ToString(), position + stringOffset, stringColor, 0, Vector2.Zero, 1,
                                   SpriteEffects.None, layer + 6);
            spriteBatch.DrawString(font, "Lives: " + watchee.lives.ToString(), position + stringOffset + new Vector2(0, 40), stringColor, 0, Vector2.Zero, 1,
                       SpriteEffects.None, layer + 7);
            
        }
    }
}
