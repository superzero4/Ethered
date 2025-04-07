using System.Collections.Generic;
using BattleSystem;
using Common.Events;
using Common.Events.UserInterface;
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

        [SerializeField, InfoBox("For movement")]
        protected Transform _root;

        public T Data => _data;

        public void Init(T data, Grid grid)
        {
            _data = data;
            Init(grid);
            SetColor();
        }

        protected void SnapToCorrectPosition(Grid grid, PositionIndexer lookAt)
        {
            SnapToPosition(grid, lookAt, _data.Position);
        }

        protected void SnapToPosition(Grid grid, PositionIndexer lookAt, PositionData dataPos)
        {
            SetPosition(grid, dataPos);
            SetRotation(lookAt);
        }

        //protected void SetPosition(Grid grid, Vector3 positionFloat)
        //{
        //    var pos = grid.CellToLocalInterpolated(positionFloat);
        //    pos.y -= grid.cellSize.y / 2;
        //    SetPosition(pos);
        //}

        private void SetPosition(Grid grid, PositionData dataPos)
        {
            SetPosition(grid.PhasedCellToWorld(dataPos));
        }

        private void SetPosition(Vector3 position)
        {
            _root.position = position;
        }


        protected virtual void RotationChanged(float newRot)
        {
        }

        protected float Rotation
        {
            get => _root.localRotation.eulerAngles.y;
            set
            {
                _root.localRotation = Quaternion.Euler(0, value, 0);
                RotationChanged(Rotation);
            }
        }

        protected float LookAtRotation(PositionIndexer lookAt) => LookAtRotation(new Vector2(lookAt.x, lookAt.y));
        protected float LookAtRotation(Vector2 lookAt) => Mathf.Atan2(lookAt.x, lookAt.y) * Mathf.Rad2Deg;

        protected Vector2 CurrentLookAt()
        {
            float yRotation = _root.localRotation.eulerAngles.y;
            float radians = yRotation * Mathf.Deg2Rad;
            return new Vector2(Mathf.Sin(radians), Mathf.Cos(radians));
        }

        protected void SetRotation(PositionIndexer lookAt)
        {
            Rotation = LookAtRotation(lookAt);
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
                _data.Team == ETeam.Player ? new PositionIndexer(0, 1) : new PositionIndexer(0, -1));
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