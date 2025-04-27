using System.Collections.Generic;
using BattleSystem;
using Common;

namespace Views.Battle.Selection
{
    public interface IHints : IReset
    {
        void HintMultiple(IEnumerable<PositionData> positions, PositionData? main = default);
    }
}