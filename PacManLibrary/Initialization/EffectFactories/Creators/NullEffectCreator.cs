using PacManShared.LevelClasses;

namespace PacManShared.Initialization.EffectFactories.Creators
{
    class NullEffectCreator: IEffectCreator 
    {
        public ICellEffect createEffect()
        {
            return new NullEffect();
        }
    }
}
