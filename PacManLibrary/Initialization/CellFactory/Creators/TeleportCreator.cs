using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PacManShared.Initialization.EffectFactories.Creators;
using PacManShared.LevelClasses.Cells;

namespace PacManShared.Initialization.CellFactory.Creators
{
    class TeleportCreator: ICellCreator
    {
        private String[] options;

        public TeleportCreator(String[] options)
        {
            this.options = options;
        }

        public Cell GetCell(int x, int y)
        {
            Cell cell =  new Cell(new Point(x, y), false);
            cell.CellEffect = new TeleportEffectCreator(options).createEffect();
            return cell;
        }
    }
}
