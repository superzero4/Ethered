using System.Collections;
using System.Linq;
using BattleSystem;
using Common.Events;
using Common.Events.Combat;
using Common.Events.UserInteraction;
using Common.Events.UserInterface;
using NaughtyAttributes;
using UI.Battle;
using UnitSystem;
using UnityEngine;
using UnityEngine.Assertions;
using Views.Battle.Animation;

namespace Views.Battle
{
    public class UnitView : AElementView<Unit>
    {
        [Header("Unit")] [SerializeField] private HealthUI _healthUI;
        [Header("Visuals")] [SerializeField] private bool _showOnlyOnCorrectPhase = false;
        [SerializeField] private UnitSkin[] _skin;
        [SerializeField, Range(0, 3)] private int _skinIndex;
        public UnitSkin currentSkin => _skin[_skinIndex];

        public ResetEvent OnActionViewEnded => _onActionViewEnded;

        [SerializeField] private UnitAnimations _unitAnimations;
        [SerializeField] private ResetEvent _onActionViewEnded = new();

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
                EventQueue<UnitView>.QueueEvent(() =>
                {
                    _healthUI.UpdateHealth(d);
                    _unitAnimations.UpdateHealth(d, _root);
                });
            });
            Data.OnUnitMoves.AddListener(Move);
            Data.OnUnitAttack.AddListener(Attack);
            Data.OnCancel.AddListener(Cancel);
        }

        private void Cancel(UnitCancelEventData arg0)
        {
            if (!Data.HealthInfo.Alive)
                return;
            System.Action onEnd = _onActionViewEnded.Invoke;
            if (arg0.isCancelTarget)
                _unitAnimations._animationPlayer.Play(AnimationType.Cancel, false, null, onEnd);
            else
                _unitAnimations._animationPlayer.Play(AnimationType.Celebrate, false, null, onEnd);
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

        protected override void RotationChanged(float newRot)
        {
            _healthUI.transform.localRotation = Quaternion.Euler(0, -newRot, 0);
        }


        public void Move(UnitMovementData arg0)
        {
            var last = arg0.oldPosition;
            Vector2 lastDir = CurrentLookAt();
            var seq = LeanTween.sequence();
            foreach (var pos in arg0.path.Path)
            {
                if (pos == arg0.oldPosition) //If we are on the start
                    continue;
                var dir = (Vector2)pos.Position - last.Position;
                //Assert.IsTrue((dir.magnitude == 1 && pos.Phase == last.Phase) || (dir.magnitude == 0 && pos.Phase != last.Phase), $"Invalid movement {dir} {last} to {pos}  with a magnitude higher than 1 or switching phase with a magnitude higher than 0");
                var turn = TweenTurn(lastDir, dir, out bool snap, out bool left);
                if (!snap)
                    seq.append(() => { _unitAnimations.Turn(left); });
                seq.append(turn);
                seq.append(() => { SetColor(); });
                if (pos.Phase != last.Phase)
                    seq.append(() => SyncPhase());
                if (dir.x != 0 || dir.y != 0)
                    seq.append(() => { _unitAnimations.Move(); });
                seq.append(LeanTween.move(_root.gameObject, _grid.PhasedCellToWorld(pos),
                    _unitAnimations.MoveTime));
                //Safe in case of rounding errors in tween
                last = pos;
                lastDir = dir;
            }

            seq.append(_onActionViewEnded.Invoke);
        }

        private void Attack(UnitAttackData arg0)
        {
            var seq = LeanTween.sequence();
            var origin = CurrentLookAt();
            var dir = new Vector2(arg0.direction.x, arg0.direction.y);
            //seq.append(() => { Debug.Log($"Attack {origin} {targ}"); });
            seq.append(TweenTurn(origin, dir, out _, out _));
            //seq.append(() => { Debug.Log($"Attack {origin} {targ}"); });
            seq.append(() =>
            {
                var del = _unitAnimations.Delay(dir.magnitude);
                _unitAnimations.Attack(arg0,
                    _grid.PhasedCellToWorld(arg0.unit.Position.Position + arg0.direction, 1f), del, () =>
                    {
                        seq.append(del);
                        seq.append(EventQueue<UnitView>.ProcessAll);
                    }
                );
            });
            seq.append(_onActionViewEnded.Invoke);
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