using System;
using System.Collections.Generic;
using BattleSystem;
using BattleSystem.TileSystem;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Environment = BattleSystem.Environment;

public class EnvironmentView : AElementView<Environment>
{
    [SerializeField] protected Transform _root;
    private Renderer[] model = null;
    protected override void Init(Grid grid)
    {
        base.Init(grid);
        Assert.IsTrue(_root.childCount == Enum.GetValues(typeof(EAllowedMovement)).Length);
        int model = (int)_data.allowedMovement;
        for (int i = 0; i < _root.childCount; i++)
        {
            _root.GetChild(i).gameObject.SetActive(model == i);
            if(model == i)
            {
                this.model = _root.GetChild(i).GetComponentsInChildren<Renderer>();
            }
        }
    }
    override protected void SetColor(Color color)
    {
        base.SetColor(color);
        foreach(var renderer in model)
        {
            renderer.material.color = color;
        }
    }
}