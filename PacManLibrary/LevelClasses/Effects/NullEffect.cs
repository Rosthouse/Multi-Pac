using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PacManShared.Entities.Player;

namespace PacManShared.LevelClasses
{
    public class NullEffect : ICellEffect
    {
        public NullEffect()
        {

        }

        public void ApplyEffect(MovableObject movObj)
        {
            //do nothing
        }

        public void Reset()
        {
            //reset nothing
        }

        public void LoadEffect(Level level)
        {
            //load nothing
        }
    }
}
