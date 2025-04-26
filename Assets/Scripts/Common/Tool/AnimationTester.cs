using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using Common.Events.Combat;
using UnitSystem;
using UnityEngine;
using Views.Battle;
using Views.Battle.Animation;

public class AnimationTester : MonoBehaviour
{
    public Grid grid;
    public UnitView _unit;
    public UnitInfo _unitInfo;
    public UnitView _target;
    public Vector2Int _targetPosition;
    public UnitInfo _targetInof;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        _unit.Init(new Unit(_unitInfo, ETeam.Player, new Vector2Int(0, 0), EPhase.Normal), grid);
        _target.Init(new Unit(_targetInof, ETeam.Enemy, _targetPosition, EPhase.Normal), grid);
        InitSkin(_unit);
        InitSkin(_target);
        while (true)
        {
            yield return new WaitForSeconds(1f);
            bool left = UnityEngine.Random.value > .5f;
            var sequence = LeanTween.sequence();
            _unit.Data.Attack(new[] { _target.Data }, 1, true);
            if (_unit.Data.HealthInfo.CurrentHealth <= 0)
            {
                _unit.Data.HealthInfo.Heal(_unit.Data.HealthInfo.MaxHealth, _unit.Data);
            }
        }
    }

    private void InitSkin(UnitView _unit)
    {
        var skin = _unit.GetComponentInChildren<UnitSkin>();
        skin.SetSkin(0, _unit.Data.Team == ETeam.Enemy ? Color.red : Color.blue, UnitSkin.WeaponType.Pistol);
        _unit.GetComponent<UnitAnimations>().Init(skin);
    }

    // Update is called once per frame
    void Update()
    {
    }
}