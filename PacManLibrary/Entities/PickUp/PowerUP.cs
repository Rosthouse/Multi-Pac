using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PacManShared.Entities.Player;
using PacManShared.Enums;
using PacManShared.LevelClasses.Effects;

namespace PacManShared.Entities.PickUp
{
    /// <summary>
    /// This class is a PowerUp that allows Pacman to move faster and eat the ghosts
    /// </summary>
    public class PowerUp : PickUp
    {

        private int powerUpTimer;

        public event PowerUpEffectEvent OnPickedUp;
        
        /// <summary>
        /// Creates a new PowerUp
        /// </summary>
        /// <param name="textureAsset">The path to the texture of this PowerUP</param>
        public PowerUp(String textureAsset): base(textureAsset)
        {
        }

        /// <summary>
        /// Updates this PowerUp
        /// </summary>
        /// <param name="gameTime">the current gameTime</param>
        public override void Update(IGameTime gameTime)
        {
        }

        /// <summary>
        /// Draws this PowerUp
        /// </summary>
        /// <param name="spriteBatch">The spritebatch</param>
        /// <param name="layer">The layer we want to render into</param>
        /// <param name="levelPosition">The position of the level onscreen</param>
        public override void Draw(SpriteBatch spriteBatch, int layer, Vector2 levelPosition)
        {
            spriteBatch.Draw(Texture, position + levelPosition, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, layer);
        }

        /// <summary>
        /// Applys an effect to a MovableObject if that object  is a PacMan
        /// </summary>
        /// <param name="movObj">the movable object that hit this PowerUp</param>
        public override void ApplyEffect(MovableObject movObj)
        {
            if (movObj is PacMan)
            {
                if (this.IsActive)
                {
                    movObj.CellsPerSecond *= 1.5f;
                    movObj.PowerUpTimer += powerUpTimer;

                   this.IsActive = false;
                   InvokeOnPickedUp(new PowerUpEffectEventArgs(5000, 0.8f, EGhostBehaviour.Fright));

                }
            }
        }

        /// <summary>
        /// Sets IsActive to true
        /// </summary>
        public override void Reset()
        {
            this.IsActive = true;
        }

        /// <summary>
        /// Fires an PwerUpEffectEvent when it is hit by a Pacman
        /// </summary>
        /// <param name="e"></param>
        public void InvokeOnPickedUp(PowerUpEffectEventArgs e)
        {
            PowerUpEffectEvent handler = OnPickedUp;
            if (handler != null) handler(this, e);
        }
    }
}