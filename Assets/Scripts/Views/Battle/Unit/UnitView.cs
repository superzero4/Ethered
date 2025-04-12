using System.Collections;
using System.Linq;
using BattleSystem;
using Common.Events;
using Common.Events.Combat;
using Common.Events.UserInterface;
using NaughtyAttributes;
using UI.Battle;
using UnitSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace Views.Battle
{
    public class UnitView : AElementView<Unit>
    {
        [Header("Unit")] [SerializeField] private HealthUI _healthUI;
        [Header("Visuals")] [SerializeField] private bool _showOnlyOnCorrectPhase = false;
        [SerializeField] private UnitSkin[] _skin;
        [SerializeField, Range(0, 3)] private int _skinIndex;
        public UnitSkin currentSkin => _skin[_skinIndex];

        [SerializeField] private UnitAnimations _unitAnimations;

        [Header("ReadOnly")] [SerializeField] [ReadOnly]
        private Grid _grid;

        protected override void Init(Grid grid)
        {
            _skinIndex = Mathf.Clamp(_skinIndex, 0, _skin.Length - 1);
            for (int i = 0; i < _skin.Length; i++)
                _skin[i].gameObject.SetActive(i == _skinIndex);
            SetColor();
            Assert.IsNotNull(_healthUI, "No HealthUI assigned");
            base.Init(grid);
            _grid = grid;
            _unitAnimations.Init(currentSkin);
            Data.OnUnitHealthChange.AddListener(d =>
            {
                EventQueue.QueueEvent(() =>
                {
                    _healthUI.UpdateHealth(d);
                    _unitAnimations.UpdateHealth(d);
                });
            });
            Data.OnUnitMoves.AddListener(Move);
            Data.OnUnitAttack.AddListener(Attack);
        }


        protected override Color GetColor()
        {
            var color = base.GetColor();
            if (_data.Team == ETeam.Player)
            {
                switch (_data.Position.Phase)
                {
                    default: color = Color.green; break;
                    //case EPhase.Normal: color = Color.blue; break;
                    //case EPhase.Ethered: color = Color.cyan; break;
                    //case EPhase.Both: color = Color.green; break;
                }
            }
            else if (_data.Team == ETeam.Enemy)
            {
                switch (_data.Position.Phase)
                {
                    default: color = Color.red; break;
                    //case EPhase.Normal: color = Color.red; break;
                    //case EPhase.Ethered: color = Color.magenta; break;
                    //case EPhase.Both: color = Color.yellow; break;
                }
            }

            return color;
        }

        protected override void SetColor(Color color)
        {
            currentSkin.SetColor(color);
        }

        public override void ToggleVisibility(bool state)
        {
            currentSkin.ToggleVisibility(state);
            _healthUI.ToggleVisibility(state);
        }

        protected override void RotationChanged(float newRot)
        {
            _healthUI.transform.localRotation = Quaternion.Euler(0, -newRot, 0);
        }

        

        public void Move(UnitMovementData arg0)
        {
            var last = arg0.path.Path[0];
            Vector2 lastDir = CurrentLookAt();
            var seq = LeanTween.sequence();
            foreach (var pos in arg0.path.Path.Skip(1))
            {
                var dir = (Vector2)pos.Position - last.Position;
                var turn = TweenTurn(lastDir, dir, out bool snap, out bool left);
                if (!snap)
                    seq.append(() => { _unitAnimations.Turn(left); });
                seq.append(turn);
                seq.append(() =>
                {
                    SetColor();
                    SyncPhase();
                });
                seq.append(() => { _unitAnimations.Move(); });
                seq.append(LeanTween.move(_root.gameObject, _grid.PhasedCellToWorld(pos),
                    _unitAnimations.MoveTime));
                //Safe in case of rounding errors in tween
                last = pos;
                lastDir = dir;
            }
        }

        private void Attack(UnitAttackData arg0)
        {
            var seq = LeanTween.sequence();
            var origin = CurrentLookAt();
            var targ = new Vector2(arg0.direction.x, arg0.direction.y);
            //seq.append(() => { Debug.Log($"Attack {origin} {targ}"); });
            seq.append(TweenTurn(origin, targ, out _, out _));
            //seq.append(() => { Debug.Log($"Attack {origin} {targ}"); });
            seq.append(() => _unitAnimations.Attack(arg0,
                _grid.PhasedCellToWorld(arg0.unit.Position.Position + arg0.direction), () =>
                {
                    seq.append(_unitAnimations.Delay(targ.magnitude));
                    seq.append(EventQueue.ProcessAll);
                }));
        }

        private LTDescr TweenTurn(Vector2 origin, Vector2 dest, out bool snap, out bool isLeft)
        {
            snap = false;
            float diff = LookAtRotation(origin) - LookAtRotation(dest);
            isLeft = diff > 0;
            if (Mathf.Abs(diff) > 5f)
            {
                return LeanTween.value(_root.gameObject,
                    d => Rotation = LookAtRotation(d), origin, dest, _unitAnimations.RotationTime);
            }
            else
            {
                snap = true;
                return LeanTween.delayedCall(0, () => Rotation = LookAtRotation(dest));
            }
        }
// ReSharper disable Unity.PerformanceAnalysis
    }
}