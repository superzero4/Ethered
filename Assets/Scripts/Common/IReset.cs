using Common.Events;

namespace Common
{
    public interface IReset
    {
        public void Reset();
        public void Reset(EmptyEvenData data) => Reset();
    }
}