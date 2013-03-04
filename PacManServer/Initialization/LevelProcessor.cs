using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PacManShared.Initialization;
using PacManShared.LevelClasses;

namespace PacManServer.Initialization
{
    public class LevelProcessor
    {
        private LevelParser parser;

        /// <summary>
        /// Loads a Level
        /// </summary>
        /// <param name="filepath">the filepath of the level</param>
        /// <returns>A generated level</returns>
        public Level LoadMap(string filepath)
        {
            List<String[]> parsedFile = Import(filepath);

            parser = new LevelParser();

            Level level = parser.GenerateLevel(parsedFile);

            return level;
        }

        /// <summary>
        /// Creates a list of stringarrays for further processing
        /// </summary>
        /// <param name="filename">the filepath</param>
        /// <returns>A list of stringarrays</returns>
        private List<String[]> Import(string filename)
        {

            var parsedData = new List<String[]>();

            string finalFileName = Directory.GetCurrentDirectory() + filename;
            try
            {
                Console.Write(finalFileName);
                using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + filename))
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
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Error: File " + filename + " not found.");
            }


            return parsedData;
        }
    }
}
