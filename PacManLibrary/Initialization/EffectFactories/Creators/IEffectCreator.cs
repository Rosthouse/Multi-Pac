using PacManShared.LevelClasses;

namespace PacManShared.Initialization.EffectFactories.Creators
{
    interface IEffectCreator
    {
        ICellEffect createEffect();
    }
}
