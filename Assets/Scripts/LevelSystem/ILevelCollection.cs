using Common;

namespace LevelSystem
{
    public interface ILevelCollection
    {
        public Level Current { get; }
        public Level Precedent { get; }
        EncounterInfo DynamicSquad { get; }
        public void Increment(int value = 1);
        public void Reset();
    }
}