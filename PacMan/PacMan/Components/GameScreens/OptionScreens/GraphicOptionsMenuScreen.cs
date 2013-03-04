#region File Description

//-----------------------------------------------------------------------------
// GraphicOptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion

#region Using Statements

#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PacManClient.Components.GameScreens.GUIElements;

namespace PacManClient.Components.GameScreens
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class GraphicOptionsMenuScreen : MenuScreen
    {
        #region Fields

        private List<Vector2> resolutions;

        private int selectedScreenSize;

        

        private bool isFullScreen;


        private readonly MenuEntry fullScreenMenuEntry;
        private readonly MenuEntry applyMenuEntry;
        private readonly MenuEntry resolutionMenuEntry;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public GraphicOptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.

            fullScreenMenuEntry = new MenuEntry(string.Empty);
            applyMenuEntry = new MenuEntry(string.Empty);
            resolutionMenuEntry = new MenuEntry(string.Empty);

            

            var back = new MenuEntry("Back");

            // Hook up menu event handlers.

            fullScreenMenuEntry.Selected += FullScreenMenuEntrySelected;
            applyMenuEntry.Selected += ApplyMenuEntrySelected;
            resolutionMenuEntry.Selected += ResolutionMenuEntrySelected;

            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(fullScreenMenuEntry);
            MenuEntries.Add(resolutionMenuEntry);
            MenuEntries.Add(applyMenuEntry);
            
            MenuEntries.Add(back);
        }

        /// <summary>
        /// Loads the content for this screen
        /// </summary>
        public override void LoadContent()
        {
            resolutions = new List<Vector2>();

            foreach(DisplayMode mode in ScreenManager.GraphicsDevice.Adapter.SupportedDisplayModes)
            {
                resolutions.Add(new Vector2(mode.Width, mode.Height));
            }

            

            base.LoadContent();

            SetMenuEntryText();
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        private void SetMenuEntryText()
        {

            applyMenuEntry.Text = "Apply changes";
            if(ScreenManager != null)
            {
                fullScreenMenuEntry.Text = "Fullscreen: " + isFullScreen;
            } else
            {
                fullScreenMenuEntry.Text = "Fullscreen: " + false;
            }

            resolutionMenuEntry.Text = "Resolution: " + resolutions[selectedScreenSize].X + "x" +
                                       resolutions[selectedScreenSize].Y;

        }

        #endregion

        #region Handle Input




        #endregion

        /// <summary>
        /// Initializes this screen
        /// </summary>
        public override void Initialize()
        {
            //throw new NotImplementedException();
        }

        
        /// <summary>
        /// Creates the default parameters for the screen
        /// </summary>
        /// <returns>the default parameters</returns>
        private PresentationParameters DefaultParameters()
        {
            PresentationParameters parameters = new PresentationParameters();

            parameters.DisplayOrientation = DisplayOrientation.Default;
            parameters.DepthStencilFormat = DepthFormat.Depth24;
            parameters.MultiSampleCount = 0;

            parameters.BackBufferFormat = SurfaceFormat.Color;

            return parameters;
        }

        /// <summary>
        /// Gets fired when the fullscren option is seleced on the screen
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The playerindexargs</param>
        private void FullScreenMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            isFullScreen = (isFullScreen == false) ? true : false;   
     
            SetMenuEntryText();
        }

        /// <summary>
        /// Gets fired as soon as the user wants to apply his changes
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The playerindexargs</param>
        private void ApplyMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            PresentationParameters parameters = ScreenManager.GraphicsDevice.PresentationParameters.Clone();

            parameters.IsFullScreen = isFullScreen;
            parameters.BackBufferHeight = (int)resolutions[selectedScreenSize].Y;
            parameters.BackBufferWidth = (int)resolutions[selectedScreenSize].X;


#if !DEBUG

            ScreenManager.GraphicsDevice.Reset(parameters);

#endif
        }

        /// <summary>
        /// Gets fired when the user wants to select a different screen resolution
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The playerindexargs</param>
        private void ResolutionMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            selectedScreenSize++;
            if(selectedScreenSize == resolutions.Count)
            {
                selectedScreenSize = 0;
            }

            SetMenuEntryText();
        }
    }
}