using Common.Events;
using Common.Events.UserInteraction;

namespace Common
{
    public interface IReset
    {
        public void Reset();
        public void Reset(EmptyEvenData data) => Reset();
    }
}