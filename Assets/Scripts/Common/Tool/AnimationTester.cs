using System.Collections;
using System.Collections.Generic;
using Common.Events.Combat;
using UnityEngine;
using Views.Battle;
using Views.Battle.Animation;

public class AnimationTester : MonoBehaviour
{
    public UnitAnimations _unit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        var skin = _unit.GetComponentInChildren<UnitSkin>();
        skin.SetSkin(0,Color.red, UnitSkin.WeaponType.Pistol);
        _unit.Init(skin);
        while (true)
        {
            yield return new WaitForSeconds(1f);
            _unit.Attack(new UnitAttackData()
            {
                direction = new Vector2Int(0, 1)
            });
            //_unit._animationPlayer.Play(AnimationType.Healed,null, ()=>Debug.Log("Trigger"),null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
