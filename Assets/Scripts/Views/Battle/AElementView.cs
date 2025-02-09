using BattleSystem;
using NaughtyAttributes;
using UI;
using UnityEngine;
using UnityEngine.Serialization;


namespace Views.Battle
{
    public abstract class AElementView<T> : MonoBehaviour where T : BattleSystem.IBattleElement
    {
        [FormerlySerializedAs("_renderer")] [Header("View")] [SerializeField]
        protected Renderer _mainRenderer;

        [SerializeReference] [ReadOnly] protected T _data;

        [SerializeField] [ReadOnly] protected InfoUI _ui;

        public T Data => _data;

        public void Init(T data, Grid grid, InfoUI ui)
        {
            _data = data;
            _ui = ui;
            Init(grid);
            SetColor();
        }

        protected void SnapToCorrectPosition(Grid grid)
        {
            transform.position = grid.GetCellCenterWorld((Vector3Int)_data.Position.Position);
        }

        public virtual Color PickColor()
        {
            Color color = Color.grey;
            if (_data.Team == ETeam.Player)
            {
                switch (_data.Phase)
                {
                    case EPhase.Normal: color = Color.blue; break;
                    case EPhase.Ethered: color = Color.cyan; break;
                    case EPhase.Both: color = Color.green; break;
                }
            }
            else if (_data.Team == ETeam.Enemy)
            {
                switch (_data.Phase)
                {
                    case EPhase.Normal: color = Color.red; break;
                    case EPhase.Ethered: color = Color.magenta; break;
                    case EPhase.Both: color = Color.yellow; break;
                }
            }
            else if (_data.IsGround)
            {
                switch (_data.Phase)
                {
                    case EPhase.Normal: color = Color.white; break;
                    case EPhase.Ethered: color = Color.black; break;
                    case EPhase.Both: color = Color.grey; break;
                }
            }

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
            SnapToCorrectPosition(grid);
        }

        public void UpdateUI()
        {
            _ui.SetInfo(_data);
        }
    }
}