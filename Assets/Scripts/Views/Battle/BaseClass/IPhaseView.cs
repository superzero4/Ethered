using Common.Events;

namespace Views.Battle
{
    public interface IPhaseView
    {
        void OnPhaseSelected(PhaseEventData arg0);
    }
}