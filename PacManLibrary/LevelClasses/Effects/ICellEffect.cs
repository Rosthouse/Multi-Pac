using PacManShared.Entities.Player;
namespace PacManShared.LevelClasses
{
    public interface ICellEffect
    {
        void ApplyEffect(MovableObject movObj);
        void Reset();
    }
}