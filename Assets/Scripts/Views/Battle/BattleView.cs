using System.Linq;
using BattleSystem;
using Common;
using UnityEngine;
using UnityEngine.Serialization;

public class BattleView : MonoBehaviour
{
    [SerializeField] private BattleInfo _battleInfo;
    [SerializeField] private UnitView _unitViewPrefab;
    [SerializeField] private EnvironmentView _environmentViewPrefab;
    [SerializeField] private Grid _grid;
    [SerializeField] private InfoUI _unitUI;
    [SerializeField] private InfoUI _tileUI;
    [SerializeField] private InfoUI _targetUI;
    private Battle _battle;

    public Battle Battle
    {
        get => _battle;
    }

    private void Awake()
    {
        _battle = new Battle();
        _battle.Init(_battleInfo);
        foreach (var unit in _battle.Units)
        {
            var unitView = Instantiate(_unitViewPrefab, transform);
            unitView.Init(unit, _grid, _unitUI);
        }

        foreach (var tile in _battle.Tiles.TilesFlat.Select(t => t.Base))
        {
            var tileView = Instantiate(_environmentViewPrefab, transform);
            tileView.Init(tile, _grid, _tileUI);
        }
    }
}