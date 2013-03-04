using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PacManShared.Entities.PickUp;
using PacManShared.LevelClasses;

namespace PacManShared.Initialization.EffectFactories.Creators
{
    class PowerUpCreator: IEffectCreator
    {
        public ICellEffect createEffect()
        {
            PowerUp powerUp = new PowerUp(@"Sprites\LevelSprites\PowerUp");
            return powerUp;
        }
    }

    class GoodyCreator:IEffectCreator
    {
        public ICellEffect createEffect()
        {
            Goody goody = new Goody(@"Sprites\LevelSprites\Cherry");
            return goody;
        }
    }

    class CrumbCreator:IEffectCreator
    {
        public ICellEffect createEffect()
        {
            Crumb crumb = new Crumb(@"Sprites\LevelSprites\Crumb");
            return crumb;
        }
    }
}
