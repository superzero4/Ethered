namespace LevelSystem
{
    public interface ILevelCollection
    {
        public Level Current { get; }
        public Level Precedent { get; }
        public void Increment(int value = 1);
        public void Reset();
    }
}