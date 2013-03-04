using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using PacManShared.Initialization.EffectFactories;
using PacManShared.LevelClasses;
using PacManShared.LevelClasses.Cells;

namespace PacManShared.Initialization
{
    public class LevelParser
    {
        /// <summary>
        /// Creates a new level
        /// </summary>
        /// <param name="path">a path to a level file</param>
        /// <returns>a generated level</returns>
        public Level GenerateLevel(string path)
        {
            List<string[]> parsedData = parseCSV(path);
            return parseLevel(parsedData);

        }

        /// <summary>
        /// Creates a new level from a list of stringarrays
        /// </summary>
        /// <param name="data">The stringarrays</param>
        /// <returns>A generated level</returns>
        public Level GenerateLevel(List<string[]> data)
        {
            return parseLevel(data);
        }

        /// <summary>
        /// Parses a csv into a List of stringarrays
        /// </summary>
        /// <param name="path">The path to the csv file</param>
        /// <returns>A pares list of stringarrays</returns>
        public List<string[]> parseCSV(string path)
        {
            List<string[]> parsedData = new List<string[]>();

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string line;
                    string[] row;

                    while ((line = reader.ReadLine()) != null)
                    {
                        row = line.Split(';');
                        parsedData.Add(row);
                    }
                }
            }
            catch(FileNotFoundException e)
            {
                //TODO: implement error handeling
            }
            

            return parsedData;
        }

        /// <summary>
        /// Parses a level from a list of stringarrays
        /// </summary>
        /// <param name="parsedData">the stringarrays to parse</param>
        /// <returns>A generated level</returns>
        private Level parseLevel(List<string[]> parsedData)
        {
            int height = parsedData.Count;
            int width = parsedData[0].Length;

            Level level = new Level(width, height, new Vector2(0, 0));

            CellFactory.CellFactory cellFactory = new CellFactory.CellFactory();
            

            for(int i=0; i<height; i++)
            {
                for(int j=0; j<width; j++)
                {
                    Cell setcell = cellFactory.CreateCell(j, i, parsedData[i].ElementAt(j));
                    level.setCell(setcell, i, j);
                }
            }

            return level;
        }
    }
}
