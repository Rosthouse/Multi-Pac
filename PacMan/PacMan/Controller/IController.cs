using PacMan.Entities.Player;

namespace PacMan.Controller
{
    public interface IController
    {
        Direction Direction { get; set; }
        string Name { get; set; }
        void Update();
        bool Send();
        bool Recieve();

    }
}