using Common;
using UnityEngine;
using Views.Battle.Selection;

namespace Views.Battle
{
    public class BattleViewInitializer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BattleInfo _battleInfo;
        [SerializeField] private Grid _grid;

        [Header("Prefabs")]
        [SerializeField] private UnitView _unitViewPrefab;
        [SerializeField] private EnvironmentView _environmentViewPrefab;

        public BattleSystem.Battle Init()
        {
            var battle =new BattleSystem.Battle();
            battle.Init(_battleInfo);
            SpanwPrefabs(battle);
            return battle;
        }

        private void SpanwPrefabs(BattleSystem.Battle battle)
        {
            foreach (var unit in battle.Units)
            {
                var unitView = Instantiate(_unitViewPrefab, transform);
                unitView.Init(unit, _grid);
            }

            foreach (var t in battle.Tiles.TilesFlat)
            {
                EnvironmentView env = Instantiate(_environmentViewPrefab, transform);
                env.Init(t.Base, _grid);
                env.SetTile(t);
                PhaseSelector.SetLayer(env);
                env.gameObject.name = "Tile " + t.Base.Position.ToString();
            }
        }
    }
}