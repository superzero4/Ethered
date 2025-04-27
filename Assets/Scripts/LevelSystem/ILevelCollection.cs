using Common;

namespace LevelSystem
{
    public interface ILevelCollection
    {
        public Level Current { get; }
        public Level Precedent { get; }
        public void Increment(int value, out bool reset);
        public void Increment(int value = 1)
        {
            Increment(value, out bool _);
        }
        public void Reset();
    }
}