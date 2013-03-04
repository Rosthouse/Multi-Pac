using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PacManShared.Entities.PickUp;
using PacManShared.Entities.Player;

namespace PacManShared.LevelClasses
{
    class EmptyPickup : PickUp, ICellEffect
    {
        public EmptyPickup() : base(@"Sprites\LevelSprites\Empty") {}

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
        }

        public override void Update(IGameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch, int layer, Vector2 levelPosition)
        {
            
        }

        public override void ApplyEffect(MovableObject movObj)
        {
            throw new NotImplementedException();
        }

        public override void Reset()
        {
            
        }
    }
}
