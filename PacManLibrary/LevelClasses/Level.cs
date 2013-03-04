using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using PacManShared.Entities.PickUp;
using PacManShared.Initialization.EffectFactories;
using PacManShared.LevelClasses.Cells;
using PacManShared.LevelClasses.Effects;
using PacManShared.Util.TimeStamps;

namespace PacManShared.LevelClasses
{
    /// <summary>
    /// This class hold the current level
    /// </summary>
    public class Level
    {
        #region Members

        //The grid holding the level
        private Cell[,] grid;
        private Dictionary<String, Cue> soundEffects;

        private Point size;

        //The position of the level inside the frame
        private Vector2 levelPosition;

        #endregion - Members

        #region Constructors

        /// <summary>
        /// Constructs a new Level
        /// </summary>
        /// <param name="width">How many cells wide the level is</param>
        /// <param name="height">How high the level is</param>
        /// <param name="levelPosition">The offsett of the level</param>
        public Level(int width, int height, Vector2 levelPosition)
        {
            size = new Point(width, height);
            grid = new Cell[width, height];

            //grid2 = new List<List<Cell>>();

            this.levelPosition = levelPosition;
        }
       

        #endregion - Constructors

        #region Accessors

        /// <summary>
        /// Gets or sets the position of the level onscreen
        /// </summary>
        public Vector2 LevelPosition
        {
            get { return levelPosition; }
            set { levelPosition = value; }
        }

        /// <summary>
        /// Gets the cell at the specified position
        /// </summary>
        /// <param name="x">The x coordinate of the cell</param>
        /// <param name="y">The y coordinate of the cell</param>
        /// <returns>The cell at the given coordinates or a emptycell at the requested position</returns>
        public Cell getCell(int x, int y)
        {

            if (!(x < 0 || x >= Size.X || y < 0 || y >= Size.Y))
            {
                return grid[x, y];
            }

            Cell returnCell = new Cell(new Point(x, y), false);

            return returnCell;
        }

        /// <summary>
        /// Gets a cell from the level. The vector 2 will be rounded to the nearest integer value
        /// </summary>
        /// <param name="position">A vector2 from where to get the cell</param>
        /// <returns></returns>
        public Cell getCell(Vector2 position)
        {
            int x = (int)Math.Round(position.X);
            int y = (int)Math.Round(position.Y);

            return getCell(x, y);
        }

        /// <summary>
        /// Gets a cell from the level
        /// </summary>
        /// <param name="point">A point</param>
        /// <returns></returns>
        public Cell getCell(Point point)
        {
            return getCell(point.X, point.Y);
        }

        /// <summary>
        /// Returns the whole grid
        /// </summary>
        public Cell[,] Grid
        {
            get { return this.grid; }
        }

        /// <summary>
        /// Gets the Size of the level (how many cells it's containing)
        /// </summary>
        public Point Size
        {
            get { return size; }
        }

        /// <summary>
        /// Replaces a cell at the specified coordinates
        /// </summary>
        /// <param name="cell">The replacement cell</param>
        /// <param name="x">The x coordinate of the cell</param>
        /// <param name="y">The y coordinate of the cell</param>
        public void setCell(Cell cell, int x, int y)
        {
            grid[y, x] = cell;
            //grid2[x][y] = cell;
        }

        #endregion - Accessors

        #region Logic

        /// <summary>
        /// Loads the content (textures, sounds, whatever) for the level
        /// </summary>
        /// <param name="contentManager">Content manager from the game class</param>
        public void LoadContent(ContentManager contentManager)
        {
            foreach (Cell cell in grid)
            {
                cell.LoadContent(contentManager);
            }
        }

        /// <summary>
        /// Draws the level
        /// </summary>
        /// <param name="spriteBatch">spritebatch from the game class</param>
        /// <param name="layer">The layer of the level</param>
        public void Draw(SpriteBatch spriteBatch, int layer)
        {
            foreach (Cell cell in grid)
            {
                cell.Draw(spriteBatch, layer - 1, LevelPosition);
            }

//            foreach (List<Cell> cells in Grid2)
//            {
//                foreach (Cell cell in cells)
//                {
//                    cell.Draw(spriteBatch, layer - 1, LevelPosition);
//                }
//            }
        }

        /// <summary>
        /// Updates the level
        /// </summary>
        /// <param name="gameTime">Game time from the game class</param>
        public void Update(IGameTime gameTime)
        {
            foreach (Cell cell in grid)
            {
                cell.Update(gameTime);
            }
        }


        #endregion - Logic


        #region Helper Methods

        /// <summary>
        /// Resets the level to it's starting state
        /// </summary>
        public void Reset()
        {
            foreach (Cell cell in grid)
            {

                cell.Reset();
            }
        }

        /// <summary>
        /// Applys a structure (from a timestamp for example) to the level
        /// </summary>
        /// <param name="levelStruct">The LevelStruct to apply</param>
        public void ApplyStructure(LevelStruct levelStruct)
        {
            foreach (Cell cell in Grid)
            {
                PickUp pickUp = cell.CellEffect as PickUp;

                if(pickUp != null)
                {
                    pickUp.SetIsActive(levelStruct.grid[cell.GridPosition.X, cell.GridPosition.Y]);
                }
            }
        }

        /// <summary>
        /// Gets a structure according to the current status of the level
        /// </summary>
        /// <returns>The levelStruct</returns>
        public LevelStruct GetLevelStruct()
        {
            bool[,] structGrid = new bool[this.Size.X,this.Size.Y];

            foreach (Cell cell in Grid)
            {
                PickUp pickUp = cell.CellEffect as PickUp;

                if(pickUp != null)
                {
                    structGrid[cell.GridPosition.X, cell.GridPosition.Y] = pickUp.IsActive;
                }
                else
                {
                    structGrid[cell.GridPosition.X, cell.GridPosition.Y] = false;
                }
            }

            return new LevelStruct(structGrid);
        }

        #endregion - Helper Methods
    }
}
