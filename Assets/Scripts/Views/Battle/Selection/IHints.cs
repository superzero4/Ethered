using System.Collections.Generic;
using BattleSystem;

namespace Views.Battle.Selection
{
    public interface IHints
    {
        void Clear();
        void Hint(PositionData s);
        void HintMultiple(IEnumerable<PositionData> positions);
    }
}