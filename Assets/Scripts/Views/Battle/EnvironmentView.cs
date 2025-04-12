using System;
using System.Collections.Generic;
using BattleSystem;
using BattleSystem.TileSystem;
using Common.Events;
using Common.Events.UserInterface;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Views.Battle.Selection;
using Environment = BattleSystem.Environment;

namespace Views.Battle
{
    public class EnvironmentView : AElementView<Environment>
    {
        [SerializeField] private Transform _modelsParent = null;
        [SerializeField] private Selectable _selectable = null;
        private Renderer[] model = null;
        [SerializeField, ReadOnly] private Tile _tile;
        private Renderer _mainRenderer1;
        public Tile Tile => _tile;

        public Selectable Selectable => _selectable;

        public void SetTile(Tile tile) => _tile = tile;

        protected override void Init(Grid grid)
        {
            base.Init(grid);
            Assert.IsTrue(_modelsParent.childCount == Enum.GetValues(typeof(EAllowedMovement)).Length);
            int model = (int)Data.allowedMovement;
            for (int i = 0; i < _modelsParent.childCount; i++)
            {
                _modelsParent.GetChild(i).gameObject.SetActive(model == i);
                if (model == i)
                {
                    this.model = _modelsParent.GetChild(i).GetComponentsInChildren<Renderer>();
                }
            }
        }

        public void DisableModels()
        {
            foreach (Transform models in _modelsParent)
                models.gameObject.SetActive(false);
        }


        protected override Color GetColor()
        {
            var color = base.GetColor();
            return Color.white;
            switch (_data.Position.Phase)
            {
                case EPhase.Normal: color = Color.white; break;
                case EPhase.Ethered: color = Color.blue; break;
                case EPhase.Both: color = (Color.blue) / 2f; break;
            }

            return color;
        }

        protected override void SetColor(Color color)
        {
            foreach (var renderer in model)
            {
                renderer.material.color = color;
            }
        }

        public override void ToggleVisibility(bool state)
        {
            foreach (var renderer in model)
                renderer.enabled = state;
            _selectable.Hint.gameObject.SetActive(state);
        }
    }
}