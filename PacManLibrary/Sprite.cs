using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PacManShared
{
    /// <summary>
    /// A basic class for sprites
    /// </summary>
    public abstract class Sprite
    {
        protected Vector2 position;
        protected Vector2 size;
        protected string textureAsset;
        private Texture2D texture;

        #region Accessors

        /// <summary>
        /// Gets or sets the texture of this sprite
        /// </summary>
        public Texture2D Texture { 
            get
            {
                return texture;
            }
            
            set
            {
                this.texture = value;
            } 
        }

        /// <summary>
        /// Gets the center of a sprite
        /// </summary>
        public abstract Vector2 Center { get; }

        /// <summary>
        /// Generates an Intersect Rectangle
        /// </summary>
        public abstract Rectangle IntersectRectangle { get; }

        /// <summary>
        /// Gets the size of a sprite
        /// </summary>
        public abstract Vector2 Size { get; set; }

        #endregion - Accessors

        #region Constructors

        /// <summary>
        /// Creates an empty sprite
        /// </summary>

        public Sprite(string textureAsset)
        {
            this.textureAsset = textureAsset;
        }

        /// <summary>
        /// The path to the texture of this sprite object
        /// </summary>
        public string TextureAsset
        {
            get { return this.textureAsset; }
        }

        /// <summary>
        /// Loads the content for this sprite
        /// </summary>
        /// <param name="contentManager">The contentmangaer</param>
        public virtual void LoadContent(ContentManager contentManager)
        {
            this.Texture = contentManager.Load<Texture2D>(TextureAsset);
        }

        /// <summary>
        /// Updates this sprite
        /// </summary>
        /// <param name="gameTime">the current gametime</param>
        public abstract void Update(IGameTime gameTime);

        /// <summary>
        /// Draws this sprite
        /// </summary>
        /// <param name="spriteBatch">the spritebatch</param>
        /// <param name="layer">the layer we want to render into</param>
        /// <param name="levelPosition">The position of the level onscreen</param>
        public abstract void Draw(SpriteBatch spriteBatch, int layer, Vector2 levelPosition);

        #endregion - Contructors
    }
}