using BattleSystem;
using NaughtyAttributes;
using UI;
using UI.Battle;
using UnityEngine;
using UnityEngine.Serialization;


namespace Views.Battle
{
    public abstract class AElementView<T> : MonoBehaviour where T : BattleSystem.IBattleElement
    {
        [FormerlySerializedAs("_renderer")] [Header("View")] [SerializeField]
        protected Renderer _mainRenderer;

        [SerializeReference] [ReadOnly] protected T _data;

        //[SerializeField] [ReadOnly] protected UnitUI _ui;

        public T Data => _data;

        public void Init(T data, Grid grid)
        {
            _data = data;
            //_ui = ui;
            Init(grid);
            SetColor();
        }

        protected void SnapToCorrectPosition(Grid grid, PositionIndexer lookAt)
        {
            var pos = grid.GetCellCenterWorld((Vector3Int)_data.Position.Position);
            pos.y -= grid.cellSize.y / 2;
            transform.position = pos;
            float angle = Mathf.Atan2(lookAt.y, lookAt.x) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(0, angle, 0);
        }

        protected virtual Color PickColor()
        {
            Color color = Color.grey;
            return color;
        }

        protected virtual void SetColor(Color color)
        {
            _mainRenderer.material.color = color;
        }

        public void SetColor()
        {
            SetColor(PickColor());
        }

        protected virtual void Init(Grid grid)
        {
            SnapToCorrectPosition(grid,
                _data.Team == ETeam.Player ? new PositionIndexer(1, 0) : new PositionIndexer(-1, 0));
        }

        //public void UpdateUI()
        //{
        //    _ui.SetInfo(_data);
        //}
    }
}