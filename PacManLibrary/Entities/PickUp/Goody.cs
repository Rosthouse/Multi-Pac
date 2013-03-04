using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PacManShared.LevelClasses;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PacManShared.Entities.Player;

namespace PacManShared.Entities.PickUp
{
    internal class Goody: PickUp
    {
        private int goodyRounds = 1;
        float secAmount = 0f;

        public Goody(String textureAsset, int Score, bool isActive): base(textureAsset)
        {
            this.IsActive = isActive;
            this.Score = Score;
        }

        public Goody(String textureAsset): this(textureAsset, 500, false) {}

        public override void Update(IGameTime gameTime)
        {
            secAmount += gameTime.ElapsedGameTime.Milliseconds;
            if (!IsActive)
            {
                if (secAmount > goodyRounds * 3500)
                {
                    IsActive = true;
                    goodyRounds++;
                    secAmount = 0;
                }
            }
            else
            {
                if (secAmount > 2500)
                {
                    this.IsActive = false;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, int layer, Vector2 levelPosition)
        {
                spriteBatch.Draw(Texture, position + levelPosition, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, layer);
        }

        public override void ApplyEffect(MovableObject movObj)
        {
            if(movObj is PacMan)
            {
                movObj.score += this.Score;
                Reset();
            }
        }

        public override void Reset()
        {
            this.IsActive = false;
            secAmount = 0;
        }
    }
}
