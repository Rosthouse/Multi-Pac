using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PacManShared.Initialization.CellFactory.Creators;
using PacManShared.LevelClasses.Cells;

namespace PacManShared.Initialization.CellFactory
{
    public class CellFactory
    {
        /// <summary>
        /// Creates a new cell
        /// </summary>
        /// <param name="x">the x position in the grid</param>
        /// <param name="y">the y position in the grid</param>
        /// <param name="element">the arguments for this cell</param>
        /// <returns>The finished cell</returns>
        public Cell CreateCell(int x, int y, String element)
        {
            Cell CellToReturn = Cell.Empty;
            String[] evaluatedElement = evaluateString(element);

            string switchString = evaluatedElement[0];

            switch(switchString)
            {
                
                    //return new Cell(CellType.Empty, new Point(i, j),  false);
                case "1":
                    return new HorizontalCellCreator().GetCell(x, y);
                case "2":
                    return new VerticalCellCreator().GetCell(x,y);
                case "3":
                    return new LeftDownCellCreator().GetCell(x, y);
                case "4":
                    return new LeftUpCellCreator().GetCell(x, y);
                case "5":
                    return new RightUpCellCreator().GetCell(x, y);
                case "6":
                    return new RightDownCellCreator().GetCell(x, y);
                case "10":
                    return new TeleportCreator(evaluatedElement).GetCell(x, y);
                case "20":
                    return new CrumbCellCreator().GetCell(x, y);
                case "21":
                    return new PowerUpCellCreator().GetCell(x, y);
                case "22":
                    return new GoodyCellCreator().GetCell(x, y);
                case "30":
                case "31":
                case "32":
                case "33":
                case "34":
                case "40":
                case "41":
                case "42":
                case "43":
                case "":
                    return new EmptyCellCreator().GetCell(x, y);
                default:
                    throw new Exception("Cell Type not found");
            }
        }

        /// <summary>
        /// Evaluates a string
        /// </summary>
        /// <param name="elementAt">the string to evaluate</param>
        /// <returns>An array of strings</returns>
        private String[] evaluateString(string elementAt)
        {
            String evaluate = elementAt;

            String[] splited = null;

            Char[] splitChar = {','};
            splited = evaluate.Split(splitChar);
            

            return splited;
        }
    }
}
