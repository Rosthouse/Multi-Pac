using PacManShared.Enums;

namespace PacManShared.Controllers.AI
{
    /// <summary>
    /// Interface for an object that observers a ghosts state
    /// </summary>
    public interface IGhostStateObserver
    {
        /// <summary>
        /// Sets the state of the ghost
        /// </summary>
        /// <param name="eGhostStateBehaviour">the new ghoststate</param>
        void SetGhostState(EGhostBehaviour eGhostStateBehaviour);
    }
}
