using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PacManShared.Entities.Player;
using PacManShared.LevelClasses;

namespace PacManShared.Entities.PickUp
{
    public abstract class PickUp : Sprite, ICellEffect
    {
        private bool isActive = true;

        /// <summary>
        /// Creates an empty sprite
        /// </summary>
//        public Sprite()
//        {
//            size = Vector2.Zero;
//            textureAsset = string.Empty;
//        }

        public PickUp(string textureAsset): base(textureAsset){}

        public bool IsActive
        {
            get {return isActive; }
            set { this.isActive = value; }
        }

        private int score;
        
        public int Score
        {
            get { return this.score; }
            set { this.score = value; }
        }

        public override Rectangle IntersectRectangle
        {
            get
            {
                return new Rectangle((int) position.X, (int) position.Y, (int) Size.X, (int) Size.Y);
                // new Rectangle((int) this.position.X, (int) this.position.Y, (int) (Texture.Width*Scale), (int) (Texture.Height*Scale));
            }
        }

        public void SetCenter(Vector2 Position)
        {
            this.position = Position;
        }

        public override Vector2 Center
        {
            get { return new Vector2(position.X + (size.X/2f), position.Y + (size.Y / 2f)); }
        }

        /// <summary>
        /// Gets the size of a sprite
        /// </summary>
        public override Vector2 Size
        {
            get
            {
                if (size != Vector2.Zero)
                {
                    return size;
                }
                else
                {
                    return Vector2.Zero;
                }
            }
            set { size = value; }
        }

        public abstract void ApplyEffect(MovableObject movObj);
        
        public void SetIsActive(bool isActive)
        {
            this.isActive = isActive;
        }

        public abstract void Reset();
    }
}
