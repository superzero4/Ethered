using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using Common;
using LevelSystem;
using NUnit.Framework;
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
        [SerializeField] private Grid _grid;
        [SerializeField] private EnvironmentInfo _defaultEnvironment;
        [Header("Prefabs")] [SerializeField] private UnitView _unitViewPrefab;
        [SerializeField] private EnvironmentView _environmentViewPrefab;

        public Grid Grid => _grid;

        public void Init(Level level, EncounterInfo squad, PhaseSelector phaseSelector,
            out List<Selectable> selectables,
            out BattleSystem.Battle battle)
        {
            selectables = new();
            battle = new BattleSystem.Battle();
            battle.Init(level.Battle, level.Map, squad, _defaultEnvironment,
                new RandomBrainCollection(GetComponentsInChildren<IComparer<Action>>()
                    .Select(comp => new UtilityBasedBrain(comp)).ToArray()));
            _grid.transform.position = level.Position;
            _grid.transform.eulerAngles = level.Rotation;
            foreach (var unit in battle.Units)
            {
                var unitView = Instantiate(_unitViewPrefab, transform);
                unitView.Init(unit, _grid);
                phaseSelector.Subscribe(unitView);
                Assert.IsTrue((int)unit.Position.Phase >= 0 && (int)unit.Position.Phase <= (int)EPhase.Both,
                    " Enum values seems corrupted, probably due to unity automatically converting ticking everything and converting all bit to 1 for a negative value, avoid using everything in serialized fields");
            }

            foreach (var t in battle.Tiles.TilesFlat)
            {
                EnvironmentView env = Instantiate(_environmentViewPrefab, transform);
                env.Init(t.Base, _grid);
                env.SetTile(t);
                if (!level.ShowTileModels)
                    env.DisableModels();
                phaseSelector.Subscribe(env);
                phaseSelector.SetLayer(env);
                env.gameObject.name = "Tile " + t.Base.Position.ToString();
                selectables.Add(env.Selectable);
                Assert.IsTrue((int)t.Base.Position.Phase >= 0 && (int)t.Base.Position.Phase < (int)EPhase.Both,
                    " Enum values seems corrupted, probably due to unity automatically converting ticking everything and converting all bit to 1 for a negative value, avoid using everything in serialized fields");
            }
        }
    }
}