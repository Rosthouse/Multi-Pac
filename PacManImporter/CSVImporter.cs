using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;

// TODO: replace this with the type you want to import.
using TImport = System.String;

namespace PacManImporter
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    /// 
    /// This should be part of a Content Pipeline Extension Library project.
    /// 
    /// TODO: change the ContentImporter attribute to specify the correct file
    /// extension, display name, and default processor for this importer.
    /// </summary>
    [ContentImporter(".csv", DisplayName = "CSV Importer", DefaultProcessor = "LevelProcessor")]
    public class CSVImporter : ContentImporter<List<String[]>>
    {
        public override List<String[]> Import(string filename, ContentImporterContext context)
        {

            var parsedData = new List<String[]>();

            try
            {
                using (StreamReader reader = new StreamReader(filename))
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
                
            }


            return parsedData;
        }
    }
}
