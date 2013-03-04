using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PacManClient.Components.GameScreens.GUIElements;

namespace PacManClient.Components.GameScreens.OptionScreens
{
    /// <summary>
    /// Allows to edit the audio options for the game
    /// </summary>
    class AudioOptionsMenuScreen: MenuScreen
    {
        
        /// <summary>
        /// Constructor
        /// </summary>
        public AudioOptionsMenuScreen() : base("Audio Options")
        {
            var ActivateSound = new MenuEntry(string.Empty);

            var back = new MenuEntry("back");

            ActivateSound.Selected += ActivateSoundMenuEntrySelected;

            back.Selected += OnCancel;

        }

        /// <summary>
        /// Initializes this screen
        /// </summary>
        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets fired when the user selects a menu entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ActivateSoundMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            
        }
    }
}
