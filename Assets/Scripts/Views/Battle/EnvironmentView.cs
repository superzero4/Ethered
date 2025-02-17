using System;
using System.Collections.Generic;
using BattleSystem;
using BattleSystem.TileSystem;
using Common.Events;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Environment = BattleSystem.Environment;

namespace Views.Battle
{
    public class EnvironmentView : AElementView<Environment>
    {
        [SerializeField, ReadOnly] private Tile _tile;
        public Tile Tile => _tile;
        public void SetTile(Tile tile) => _tile = tile;
        [SerializeField] protected Transform _root;
        private Renderer[] model = null;
        
        protected override void Init(Grid grid)
        {
            base.Init(grid);
            Assert.IsTrue(_root.childCount == Enum.GetValues(typeof(EAllowedMovement)).Length);
            int model = (int)Data.allowedMovement;
            for (int i = 0; i < _root.childCount; i++)
            {
                _root.GetChild(i).gameObject.SetActive(model == i);
                if (model == i)
                {
                    this.model = _root.GetChild(i).GetComponentsInChildren<Renderer>();
                }
            }
        }

        override protected void SetColor(Color color)
        {
            base.SetColor(color);
            foreach (var renderer in model)
            {
                renderer.material.color = color;
            }
        }

    }
}