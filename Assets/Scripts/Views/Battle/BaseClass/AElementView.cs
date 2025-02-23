using System.Collections.Generic;
using BattleSystem;
using Common.Events;
using NaughtyAttributes;
using UI;
using UI.Battle;
using UnityEngine;
using UnityEngine.Serialization;


namespace Views.Battle
{
    public abstract class AElementView<T> : MonoBehaviour, IPhaseView where T : BattleSystem.IBattleElement
    {
        [SerializeReference] [ReadOnly] protected T _data;
        [SerializeField] protected Transform _root;
        public T Data => _data;

        public void Init(T data, Grid grid)
        {
            _data = data;
            Init(grid);
            SetColor();
        }

        protected void SnapToCorrectPosition(Grid grid, PositionIndexer lookAt)
        {
            var pos = grid.GetCellCenterWorld((Vector3Int)_data.Position.Position);
            pos.y -= grid.cellSize.y / 2;
            _root.position = pos;
            float angle = Mathf.Atan2(lookAt.y, lookAt.x) * Mathf.Rad2Deg;
            _root.localRotation = Quaternion.Euler(0, angle, 0);
        }

        protected virtual Color GetColor()
        {
            Color color = Color.grey;
            return color;
        }

        protected abstract void SetColor(Color color);

        public abstract void ToggleVisibility(bool state);

        //protected abstract IEnumerable<Renderer> Renderers { get; }
        public void SetColor()
        {
            SetColor(GetColor());
        }

        protected virtual void Init(Grid grid)
        {
            SnapToCorrectPosition(grid,
                _data.Team == ETeam.Player ? new PositionIndexer(1, 0) : new PositionIndexer(-1, 0));
        }

        public virtual void OnPhaseSelected(PhaseEventData arg0)
        {
            //The default implementation of an element view does nothing, but we could decide to call some other helper methods here
        }

        protected void ToggleVisibiltyFromPhase(EPhase phase)
        {
            ToggleVisibility(_data.Position.Phase.HasFlag(phase));
        }
    }
}