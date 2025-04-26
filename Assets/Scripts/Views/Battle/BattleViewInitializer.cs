using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using Common;
using LevelSystem;
using NUnit.Framework;
using SquadSystem;
using UnitSystem;
using UnitSystem.AI;
using UnitSystem.AI.Dev;
using UnityEngine;
using Views.Battle.Selection;
using Selectable = Views.Battle.Selection.Selectable;

namespace Views.Battle
{
    public class BattleViewInitializer : MonoBehaviour
    {
        private IBrainCollection _brains;
        [SerializeField] private EnvironmentProps _props;
        [SerializeField] private EnvironmentInfo _defaultEnvironment;
        [SerializeField] private EnvironmentInfo _defaultObstacle;
        [Header("Prefabs")] [SerializeField] private UnitView _unitViewPrefab;
        [SerializeField] private EnvironmentView _environmentViewPrefab;


        public void Init(Level level, Squad squad, PhaseSelector phaseSelector, Grid _grid,
            out List<Selectable> selectables,
            out BattleSystem.Battle battle)
        {
            selectables = new();
            battle = new BattleSystem.Battle();
            battle.Init(level.Battle, level.Map, squad, _defaultEnvironment, _defaultObstacle,
                // ReSharper disable once CoVariantArrayConversion
                new RandomBrainCollection(GetComponentsInChildren<IComparer<Action>>()
                    .Select(comp => new UtilityBasedBrain(comp)).ToArray()));
            foreach (var unit in battle.Units)
            {
                var unitView = Instantiate(_unitViewPrefab, transform);
                unitView.Init(unit, _grid);
                phaseSelector.Subscribe(unitView.phaseViews);
                Assert.IsTrue((int)unit.Position.Phase >= 0 && (int)unit.Position.Phase <= (int)EPhase.Both,
                    " Enum values seems corrupted, probably due to unity automatically converting ticking everything and converting all bit to 1 for a negative value, avoid using everything in serialized fields");
            }

            Dictionary<PositionData, EnvironmentView> envs = new();
            foreach (var t in battle.Tiles.TilesFlat)
            {
                EnvironmentView env = Instantiate(_environmentViewPrefab, transform);
                env.Init(t.Base, _grid);
                var pos = t.Base.Position;
                pos.Phase = ERelativePhase.Opposite.ToPhase(pos.Phase);
                envs.TryGetValue(pos, out var other);
                env.Init(t, other);
                if (!level.ShowTileModels)
                    env.DisableModels();
                phaseSelector.Subscribe(env.phaseViews);
                Selector.SetLayer(env);
                env.gameObject.name = "Tile " + t.Base.Position.ToString();
                selectables.Add(env.Selectable);
                Assert.IsTrue((int)t.Base.Position.Phase >= 0 && (int)t.Base.Position.Phase < (int)EPhase.Both,
                    " Enum values seems corrupted, probably due to unity automatically converting ticking everything and converting all bit to 1 for a negative value, avoid using everything in serialized fields");
                envs.Add(t.Base.Position, env);
            }

            foreach (var env in level.Map.Environments())
            {
                var size = env.max - env.min + Vector2Int.one;
                bool turn = size.x < size.y;
                var prefab = _props[size];
                var pos = _grid.PhasedCellToWorld(env.center);
                var go = Instantiate(prefab, pos,
                    Quaternion.identity,
                    _grid.transform);
                go.transform.localRotation = Quaternion.Euler(0, turn ? -90 : 0, 0);
                go.name = "Prop " + env.center.ToString() + ", " + size.ToString();
                go.Phase = env.center.Phase;
                phaseSelector.Subscribe(go);
            }
        }
    }
}