using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PacManShared.Controllers;
using PacManShared.Entities.Player;
using PacManShared.Enums;
using PacManShared.Util.TimeStamps;

namespace PacManShared.Entities
{
    public class LoadObject : MovableObject
    {
        public LoadObject(String textureAsset, Point frameSize) : base(textureAsset, null, null, Direction.None, 0f, new EmptyController(0), frameSize)
        {
        }

        public override Vector2 Center
        {
            get { return new Vector2(position.X + (frameSize.X/2f), position.Y + (frameSize.Y/2f)); }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public override Direction Direction
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
        }

        public override void Update(IGameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override MovObjStruct GetStruct()
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch, int layer, Vector2 levelPosition)
        {
            spriteBatch.Draw(Texture, position, Color.White);
        }
    }
}