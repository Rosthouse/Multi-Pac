#region File Description

//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion

#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using PacManClient.Components.GameScreens.GamePlayScreens;
using PacManClient.Components.GameScreens.GUIElements;
using PacManClient.Components.GameScreens.OptionScreens;
//using PacManClient.Components.GameScreens.Other;
using PacManClient.Components.Workers;
using PacManShared;

#endregion

namespace PacManClient.Components.GameScreens
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Initialization

        private Dictionary<String, Cue> mainMenuSound;

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("Main Menu")
        {
            // Create our menu entries.
            var playMultiplayerGameMenuEntry = new MenuEntry("Play multiplayer Game");
            var playLocalGameMenuEntry = new MenuEntry("Play local Game");
            var optionsMenuEntry = new MenuEntry("Options");
            var creditsScreenMenuEntry = new MenuEntry("Credits");
            var exitMenuEntry = new MenuEntry("Exit");



            // Hook up menu event handlers.
            playMultiplayerGameMenuEntry.Selected += PlayMultiplayerGameMenuEntrySelected;
            playLocalGameMenuEntry.Selected += PlayLocalGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            creditsScreenMenuEntry.Selected += CreditsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playMultiplayerGameMenuEntry);
            MenuEntries.Add(playLocalGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);

            mainMenuSound = new Dictionary<string, Cue>();
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        private void PlayMultiplayerGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            EndSounds();
            ScreenManager.AddScreen(new ConfigMultiplayerScreen(), e.PlayerIndex);
            //LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new ConnectServerLoader(),new ConfigMultiplayerScreen());
        }

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        private void PlayLocalGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //Console.Console.GetInstance().WriteString("Loading Local Game");
            EndSounds();
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new LocalLoader(),
                               new SingleplayerScreen());
        }


        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        private void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen("Options"), e.PlayerIndex);
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you scared?";

            var confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        /// <summary>
        /// Event handler for when the user selects the credts entry
        /// </summary>
        private void CreditsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //ScreenManager.AddScreen(new CreditsScreen(), e.PlayerIndex);
        }

        #endregion

        public override void Initialize()
        {
            
        }

        public override void LoadContent()
        {
            base.LoadContent();


            mainMenuSound.Add("MenuSong", ScreenManager.SoundBank.GetCue("MainMenu"));
            mainMenuSound.Add("StageSelect", ScreenManager.SoundBank.GetCue("StageSelect"));
            //mainMenuSound["MenuSong"].Play();
            //Console.Console.GetInstance().WriteString("MainMenuScreen loaded");
        }

        private void EndSounds()
        {
            mainMenuSound["MenuSong"].Stop(AudioStopOptions.Immediate);
            //mainMenuSound["StageSelect"].Play();
            //mainMenuSound["StageSelect"].Stop(AudioStopOptions.AsAuthored);

            while (true)
            {
                if(!mainMenuSound["StageSelect"].IsPlaying)
                {
                    return;
                }

            }
        }
    }
}
