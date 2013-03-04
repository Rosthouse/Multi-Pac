using System;
using Microsoft.Xna.Framework;
using PacManShared.Enums;
using PacManShared.LevelClasses;
using PacManShared.LevelClasses.Cells;
using PacManShared.LevelClasses.Effects;

namespace PacManShared.Initialization.EffectFactories.Creators
{
    class TeleportEffectCreator: IEffectCreator
    {
        private Point target;
        private Direction direction;

        public TeleportEffectCreator(String[] effectOptions)
        {

            Point targetPoint = Point.Zero;

            try
            {
                target = new Point(Convert.ToInt32(effectOptions[1]), Convert.ToInt32(effectOptions[2]));

                direction = DirectionExtension.StringToDirection(effectOptions[3]);
            } 
            catch(Exception e)
            {
                Environment.Exit(-1);
            }

            
        }

        public ICellEffect createEffect()
        {
            return new TeleportEffect(target, direction); 
        }
    }
}
