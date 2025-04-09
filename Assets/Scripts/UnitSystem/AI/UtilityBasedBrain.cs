using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using BattleSystem.Actions;
using BattleSystem.TileSystem;

namespace UnitSystem.AI
{
    public class UtilityBasedBrain : IBrain
    {
        private IComparer<Action> _priorityComparer;

        public UtilityBasedBrain(IComparer<Action> priorityComparer)
        {
            _priorityComparer = priorityComparer;
        }

        public Action GetDecision(Unit source, Tilemap map)
        {
            var all = map.GetAllValidActions(source, 0f);
            return all.OrderBy(a => a, _priorityComparer).FirstOrDefault();
        }
    }
}