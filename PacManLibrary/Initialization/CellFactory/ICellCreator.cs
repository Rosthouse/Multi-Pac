using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PacManShared.LevelClasses.Cells;

namespace PacManShared.Initialization.CellFactory
{
    interface ICellCreator
    {
         Cell GetCell(int x, int y);
    }
}
