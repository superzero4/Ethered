using System.Collections.Generic;
using BattleSystem;
using NUnit.Framework;

namespace Views.Battle.Selection
{
    public class TileHints : IHints
    {
        private Dictionary<PositionData, Selectable> _cache;

        public TileHints(IEnumerable<Selectable> selectables = null)
        {
            _cache = new();
            if (selectables != null)
                foreach (var s in selectables)
                    if (!_cache.TryAdd(s.Tile.Base.Position, s))
                        Assert.IsTrue(
                            s.Tile.Base.Position.Phase != EPhase.Both && s.Tile.Base.Position.Phase != EPhase.Normal &&
                            s.Tile.Base.Position.Phase != EPhase.Ethered, "A non-ignored tile has been added twice");
        }

        public void Clear()
        {
            foreach (var s in _cache.Values)
                s.Hint.Deactivate();
        }


        public void Hint(PositionData s)
        {
            if (_cache.TryGetValue(s, out var selectable))
                selectable.Hint.TogglePartial();
            else
                Assert.IsTrue(false);
        }

        public void HintMultiple(IEnumerable<PositionData> positions)
        {
            foreach (var pos in positions)
                Hint(pos);
        }
    }
}