using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PacManShared.Initialization.EffectFactories.Creators;
using PacManShared.LevelClasses.Cells;

namespace PacManShared.Initialization.CellFactory.Creators
{
    
    /// <summary>
    /// Creates a new Cell, facing rightdown
    /// </summary>
    public class RightDownCellCreator:ICellCreator
    {
        public Cell GetCell(int x, int y)
        {
            Cell cell = new Cell(@"Sprites\LevelSprites\RightDown", new Point(x, y), true);
            cell.CellEffect = new NullEffectCreator().createEffect();
            return cell;
        }
    }

    /// <summary>
    /// Creates a new Cell, facing rightup
    /// </summary>
    public class RightUpCellCreator:ICellCreator
    {
        public Cell GetCell(int x, int y)
        {
            Cell cell = new Cell(@"Sprites\LevelSprites\RightUp", new Point(x, y), true);
            cell.CellEffect = new NullEffectCreator().createEffect();
            return cell;
        }
    }

    /// <summary>
    /// Creates a new cell, facing leftdown
    /// </summary>
    public class LeftDownCellCreator:ICellCreator
    {
        public Cell GetCell(int x, int y)
        {
            Cell cell = new Cell(@"Sprites\LevelSprites\LeftDown", new Point(x, y), true);
            cell.CellEffect = new NullEffectCreator().createEffect();
            return cell;
        }
    }

    /// <summary>
    /// Creates a new cell, facing leftup
    /// </summary>
    public class LeftUpCellCreator : ICellCreator
    {
        public Cell GetCell(int x, int y)
        {
            Cell cell = new Cell(@"Sprites\LevelSprites\LeftUP", new Point(x, y), true);
            cell.CellEffect = new NullEffectCreator().createEffect();
            return cell;
        }
    }

    /// <summary>
    /// Creates a new cell, facing horizontal
    /// </summary>
    public class HorizontalCellCreator : ICellCreator
    {
        public Cell GetCell(int x, int y)
        {
            Cell cell = new Cell(@"Sprites\LevelSprites\Horizontal", new Point(x, y), true);
            cell.CellEffect = new NullEffectCreator().createEffect();
            return cell;
        }
    }

    /// <summary>
    /// Creates a new cell, facing vertical
    /// </summary>
    public class VerticalCellCreator : ICellCreator
    {
        public Cell GetCell(int x, int y)
        {
            Cell cell = new Cell(@"Sprites\LevelSprites\Vertical", new Point(x, y), true);
            cell.CellEffect = new NullEffectCreator().createEffect();
            return cell;
        }
    }
    
}
