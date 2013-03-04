using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using PacManShared.Enums;

namespace PacManShared.LevelClasses.Effects
{
    /// <summary>
    /// A delegate for a PowerUpEffectEvent
    /// </summary>
    /// <param name="sender">The powerup that fired the event</param>
    /// <param name="e">the arguments for that event</param>
    public delegate void PowerUpEffectEvent(object sender, PowerUpEffectEventArgs e);

    /// <summary>
    /// Contains arguments for this event
    /// </summary>
    public class PowerUpEffectEventArgs:EventArgs
    {
        public readonly int timer;
        public readonly float speed;
        public readonly EGhostBehaviour ghostBehaviour;

        /// <summary>
        /// Creates a new PowerUpEffectEventArgs
        /// </summary>
        /// <param name="timer">How long the powerup will have an effect</param>
        /// <param name="speed">How much the speed will be altered</param>
        /// <param name="ghostBehaviour">The behaviour ghosts have after this effect</param>
        public PowerUpEffectEventArgs(int timer, float speed, EGhostBehaviour ghostBehaviour)
        {
            this.timer = timer;
            this.speed = speed;
            this.ghostBehaviour = ghostBehaviour;
        }
    }
}
