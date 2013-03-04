using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PacManShared.Initialization.EffectFactories.Creators;
using PacManShared.LevelClasses.Cells;

namespace PacManShared.Initialization.CellFactory.Creators
{
    public class CrumbCellCreator: ICellCreator
    {
        public Cell GetCell(int x, int y)
        {
            Cell cell = new Cell(new Point(x, y), false);
            cell.CellEffect = new CrumbCreator().createEffect();
            return cell;
        }
    }

    public class PowerUpCellCreator:ICellCreator
    {
        public Cell GetCell(int x, int y)
        {
            Cell cell = new Cell(new Point(x, y), false);
            cell.CellEffect = new PowerUpCreator().createEffect();
            return cell;
        }
    }

    public class GoodyCellCreator:ICellCreator
    {
        public Cell GetCell(int x, int y)
        {
            Cell cell = new Cell(new Point(x, y), false);
            cell.CellEffect = new GoodyCreator().createEffect();
            return cell;
        }
    }
}
