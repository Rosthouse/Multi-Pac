using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PacManClient.Components.GameScreens.GUIElements;

namespace PacManClient.Components.GameScreens.OptionScreens
{
    /// <summary>
    /// Allows to 
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        private readonly MenuEntry graphicOptions;

        public OptionsMenuScreen(string menuTitle) : base(menuTitle)
        {
            graphicOptions = new MenuEntry("Graphic Options");

            var back = new MenuEntry("Back");

            //Hook into event handlers
            graphicOptions.Selected += GraphicOptionsEntrySelected;

            back.Selected += OnCancel;

            MenuEntries.Add(graphicOptions);

            MenuEntries.Add(back);
        }

        public override void Initialize()
        {
            //throw new NotImplementedException();
        }

        private void GraphicOptionsEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new GraphicOptionsMenuScreen(), e.PlayerIndex);
        }
    }
}
