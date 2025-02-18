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
        [SerializeField] protected Transform _root;
        private Renderer[] model = null;
        [SerializeField, ReadOnly] private Tile _tile;
        public Tile Tile => _tile;
        public void SetTile(Tile tile) => _tile = tile;

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

        protected override Color PickColor()
        {
            var color = base.PickColor();
            switch (_data.Position.Phase)
            {
                case EPhase.Normal: color = Color.white; break;
                case EPhase.Ethered: color = Color.blue; break;
                case EPhase.Both: color = (Color.blue)/2f; break;
            }
            return color;
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