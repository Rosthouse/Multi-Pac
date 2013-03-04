using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace PacManClient.Controller.Local
{
    class Texteingabe
    {
        private int Lastposition;
        public int MaxAnzahlZeichen = 9; //ES WIRD VON 0 AN GEZÄHLT, DAS HEISST WENN MAXPOSITION 9 IST,SIND 10 MAXIMAL 10EINGEBBAR 
        public bool SchreibModus = false;
        public bool Tastendruck;
        public string Buchstabe;
        public bool Großschreiben;
        private string[] AllowedChars = new string[36] 
        { 
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" ,"1", "2", "3", "4", "5", "6", "7", "8", "9", "0" 
        };
        public void Update(KeyboardState Tastenstatus, ref string Text)
        {
            if (SchreibModus == true)
            {
                Lastposition = Text.Length;

                if (Tastenstatus.IsKeyDown(Keys.LeftShift))
                {
                    Großschreiben = true;
                    if (Tastendruck == true && Tastenstatus.GetPressedKeys().Length == 1)
                    {
                        Tastendruck = false;
                    }
                }
                else
                {
                    Großschreiben = false;
                    if (Tastendruck == true && Tastenstatus.GetPressedKeys().Length == 0)
                    {
                        Tastendruck = false;
                    }
                }
                if (Tastenstatus.IsKeyDown(Keys.Back) && Lastposition > 0 && Tastendruck == false)
                {
                    Text = Text.Remove(Lastposition - 1, 1);
                    Tastendruck = true;
                }
                if (Lastposition <= MaxAnzahlZeichen)
                {
                    Keys[] currentlyPressed = Tastenstatus.GetPressedKeys();
                    List<string> allowedChars = new List<string>(AllowedChars);
                    foreach (Keys key in currentlyPressed)
                    {

                        Buchstabe = key.ToString();
                        if (Buchstabe.Length == 2)
                        {
                            Buchstabe = Buchstabe.Remove(0, 1);
                        }
                        if (allowedChars.Contains(Buchstabe.ToLower()) && Tastendruck == false)
                        {
                            Tastendruck = true;
                            if (Großschreiben == true)
                            {
                                Text += Buchstabe.ToUpper();
                            }
                            else
                            {
                                Text += Buchstabe.ToLower();
                            }
                        }
                    }
                }
            }
        }

    }
}
