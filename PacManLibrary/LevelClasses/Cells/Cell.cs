using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PacManShared.Entities.PickUp;
using PacManShared.LevelClasses.Effects;

namespace PacManShared.LevelClasses.Cells
{

    /// <summary>
    /// A cell of a level that can be either a way or a wall for the player objects
    /// </summary>
    public class Cell: Sprite
    {
        #region Members

        private Texture2D background;
        private ICellEffect cellEffect;
        private Point gridPosition;
        private bool isWall;

        private static Cell emptyCell = null;

        #endregion - Members

        #region Accessors

        /// <summary>
        /// Returns an empty Cell
        /// </summary>
        public static Cell Empty
        {
            get
            {
                if(emptyCell == null)
                {
                    emptyCell = new Cell();
                }

                return emptyCell;
            }
        }

        /// <summary>
        /// Returns the position on-screen
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        /// <summary>
        /// Returns the position this cell has within the grid
        /// </summary>
        public Point GridPosition
        {
            get
            {
                return gridPosition;
            }
        }

        /// <summary>
        /// Returns the effect this cell has on an object
        /// </summary>
        public ICellEffect CellEffect
        {
            get { return cellEffect; }
            set { this.cellEffect = value; }
        }

        /// <summary>
        /// Returns wether this cell is a wall
        /// </summary>
        public bool IsWall
        {
            get { return isWall; }
        }

        /// <summary>
        /// Returns the position of the center of this cell
        /// </summary>
        public override Vector2 Center
        {
            get { return new Vector2(position.X + (Texture.Width / 2) , position.Y + (Texture.Height / 2) ); }
        }

        /// <summary>
        /// Returns a rectangle to check intersections with
        /// </summary>
        public override Rectangle IntersectRectangle
        {
            get
            {
                return new Rectangle((int) this.position.X, (int) this.position.Y, (int) this.Size.X, (int) this.Size.Y);
            }
        }

        /// <summary>
        /// Gets the size of a sprite
        /// </summary>
        public override Vector2 Size
        {
            get
            {
                if(size != Vector2.Zero)
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

        #endregion - Accessors

        #region Constructors
		
        /// <summary>
        /// Generates an empty cell.
        /// </summary>
        public Cell(): base(@"Sprites\LevelSprites\Empty")
        {
            cellEffect = null;
            gridPosition = new Point(0, 0);
            isWall = false;
            size = new Vector2(50, 50);
            this.position = calculatePosition();

        }

        /// <summary>
        /// Creates a new cell with the specified parameters
        /// </summary>
        /// <param name="textureAsset">The path to the texture of this Cell</param>
        /// <param name="gridPosition">Which position this cell will be placed at</param>
        /// <param name="isWall">Defines if this cell is unpassable</param>
        public Cell(String textureAsset, Point gridPosition,  bool isWall): base(textureAsset)
        {
            this.gridPosition = gridPosition;
            this.isWall = isWall;
        }

        /// <summary>
        /// Creates a new cell
        /// </summary>
        /// <param name="gridPosition">the position inside the grid</param>
        /// <param name="isWall">if this cell is a wall</param>
        public Cell(Point gridPosition,  bool isWall): this(@"Sprites\LevelSprites\Empty", gridPosition, isWall){}

        #endregion  - Constructors

        /// <summary>
        /// Loads the texture of this cell
        /// </summary>
        /// <param name="contentManager">The content manager of the game class</param>
        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            
            this.size = new Vector2(Texture.Width, Texture.Height);
            this.position = calculatePosition();

            PickUp pU = cellEffect as PickUp;

            if(pU != null)
            {
                pU.LoadContent(contentManager);
                pU.SetCenter(this.Position);
            }
            
        }

        /// <summary>
        /// Updates the cell
        /// </summary>
        /// <param name="gameTime">GameTime from the game class</param>
        public override void Update(IGameTime gameTime)
        {
            PickUp pU = cellEffect as PickUp;

            if(pU != null)
            {
                pU.Update(gameTime);
            }
        }


        /// <summary>
        /// Draws this cell at its position
        /// </summary>
        /// <param name="spriteBatch">The spritebatch from the game class</param>
        /// <param name="layer">How deep this cell is</param>
        /// <param name="levelPosition">The offset of the level on-screen</param>
        public override void Draw(SpriteBatch spriteBatch, int layer, Vector2 levelPosition)
        {
            spriteBatch.Draw(Texture, Position + levelPosition, null, Color.White,  0, Vector2.Zero, 1, SpriteEffects.None, 0);

            PickUp pU = cellEffect as PickUp;

            if (pU != null)
            {
                if (pU.IsActive)
                {
                    pU.Draw(spriteBatch, layer + 1, levelPosition);
                }
            }
        }

        #region Helper Methods

        /// <summary>
        /// Calculates the onscreen position of this cell, depending on gridposition, the size and the scale
        /// </summary>
        /// <returns>the new Position</returns>
        private Vector2 calculatePosition()
        {
            return new Vector2(GridPosition.X*Size.X, GridPosition.Y*Size.Y);
        }
        
        /// <summary>
        /// Resets the celleffect of this cell
        /// </summary>
        public void Reset()
        {
            cellEffect.Reset();
        }
        #endregion - Helper Methods
    }
}
