using Microsoft.Xna.Framework;

namespace PacManShared.Controllers.Network
{
    public interface INetworkController
    {
        void Send();
        void Receive();
        void Update(IGameTime gameTime);
    }
}
