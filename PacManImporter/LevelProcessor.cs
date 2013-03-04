using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using PacManShared;
// TODO: replace these with the processor input and output types.
using PacManShared.LevelClasses;
using TInput = System.String;
using TOutput = System.String;

namespace PacManImporter
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "PacManImporter.LevelProcessor")]
    public class LevelProcessor : ContentProcessor<List<TInput[]>, List<TOutput[]>>
    {
        /// <summary>
        /// Processes a list of strings
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override List<String[]> Process(List<String[]> input, ContentProcessorContext context)
        {
            
            return input;
        }

    }
}