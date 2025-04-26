using System.Collections.Generic;
using BattleSystem;
using Common.Events;
using Common.Events.UserInterface;
using NaughtyAttributes;
using UI;
using UI.Battle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Views.Phase;


namespace Views.Battle
{
    public abstract class AElementView<T> : MonoBehaviour where T : BattleSystem.IBattleElement
    {
        [SerializeReference] [ReadOnly] protected T _data;

        [SerializeField, InfoBox("For movement")]
        protected Transform _root;

        [SerializeField] protected ScalingPhaseView _scalingPhaseView;
        public T Data => _data;
        public IPhaseView[] phaseViews => new[] { _scalingPhaseView };

        public void Init(T data, Grid grid)
        {
            _data = data;
            _scalingPhaseView.Root = _root;
            SyncPhase();
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
        
        //protected abstract IEnumerable<Renderer> Renderers { get; }
        public void SetColor()
        {
            SetColor(GetColor());
        }

        protected void SyncPhase()
        {
            _scalingPhaseView.Phase = _data.Position.Phase;
        }

        protected virtual void Init(Grid grid)
        {
            SnapToCorrectPosition(grid,
                _data.Team == ETeam.Player ? new PositionIndexer(0, 1) : new PositionIndexer(0, -1));
        }
    }
}