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
        skin.SetSkin(0, Color.red, UnitSkin.WeaponType.Pistol);
        _unit.Init(skin);
        while (true)
        {
            yield return new WaitForSeconds(1f);
            bool left = UnityEngine.Random.value > .5f;
            var sequence = LeanTween.sequence();
            sequence.append(()=>_unit.Turn(left));
            sequence.append(
                LeanTween.rotateLocal(gameObject,
                    transform.localRotation.eulerAngles + new Vector3(0, left ? 90 : -90, 0), _unit.RotationTime));
            sequence.append(()=>_unit.Move());
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}