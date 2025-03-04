using Common.Events;
using Common.Events.UserInterface;

namespace Views.Battle
{
    public interface IPhaseView
    {
        void OnPhaseSelected(PhaseEventData arg0);
    }
}