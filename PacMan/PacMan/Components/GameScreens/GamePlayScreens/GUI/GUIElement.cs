using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PacManShared;
using PacManShared.Entities.Player;

namespace PacManClient.Components.GameScreens.GamePlayScreens.GUI
{
    public abstract class GUIElement
    {
        protected MovableObject watchee;
        protected Texture2D texture;
        protected Vector2 position;


        public GUIElement(MovableObject watchee, Vector2 position)
        {
            this.watchee = watchee;
        }

        public Vector2 Size
        {
            get
            {
                if(Texture != null)
                {
                    return new Vector2(Texture.Width, Texture.Height);
                }
                return Vector2.Zero;
            }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public abstract void LoadContent(ContentManager contentManager);


        public abstract void Draw(SpriteBatch spriteBatch, int layer);
    }
}
