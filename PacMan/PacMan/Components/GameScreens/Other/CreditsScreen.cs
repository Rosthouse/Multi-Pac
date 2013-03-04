using System;
using System.Collections.Generic;
using System.IO;

namespace PacManClient.Components.GameScreens.Other
{
    class CreditsScreen: MenuScreen
    {
        private List<String> entries;

        public CreditsScreen() : base("Credits")
        {
            entries = new List<string>();
        }

        public override void LoadContent()
        {
            TextReader reader = new StreamReader(@"Other/credits.txt");
            String input;
            while((input = reader.ReadLine()) != null)
            {
                entries.Add(input);
            }

            base.LoadContent();
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
