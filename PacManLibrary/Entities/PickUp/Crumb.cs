using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PacManShared.LevelClasses;
using PacManShared.Entities.Player;
using PacManShared.LevelClasses.Cells;

namespace PacManShared.Entities.PickUp
{
    public class Crumb: PickUp, ICellEffect
    {
        public Crumb(String textureAsset, int score) :base(textureAsset)
        {
            this.Score = score;
        }

        public Crumb(String textureAsset):this(textureAsset, 100){}

        public override Vector2 Center
        {
            get
            {
                return new Vector2(position.X + (Texture.Width / 2), position.Y + (Texture.Height / 2));
            }
        }

        public override void ApplyEffect(MovableObject movObj)
        {
            if (movObj is PacMan)
            {
                if (this.IsActive)
                {
                    movObj.score += this.Score;
                }

                this.IsActive = false;
            }
        }

        public override void Update(IGameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch, int layer, Vector2 levelPosition)
        {
            if(this.IsActive)
            {
                spriteBatch.Draw(Texture, position + levelPosition, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, layer);   
            }
        }

        public override void Reset()
        {
            this.IsActive = true;
        }
    }
}
