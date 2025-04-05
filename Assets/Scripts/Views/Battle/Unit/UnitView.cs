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

        [SerializeField] private EPhase _displayedPhase;
        [SerializeField] private UnitAnimations _unitAnimations;

        [Header("ReadOnly")] [SerializeField] [ReadOnly]
        private Grid _grid;

        protected override void Init(Grid grid)
        {
            _skinIndex = Mathf.Clamp(_skinIndex, 0, _skin.Length - 1);
            for (int i = 0; i < _skin.Length; i++)
                _skin[i].gameObject.SetActive(i == _skinIndex);
            SetColor();
            SyncVisibility();
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
                    SyncVisibility();
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

        public override void OnPhaseSelected(PhaseEventData arg0)
        {
            _displayedPhase = arg0.phase;
            SyncVisibility();
        }

        protected override void RotationChanged(float newRot)
        {
            _healthUI.transform.localRotation = Quaternion.Euler(0, -newRot, 0);
        }

        public void SyncVisibility()
        {
            if (_showOnlyOnCorrectPhase)
                ToggleVisibiltyFromPhase(_displayedPhase);
        }

        public void Move(UnitMovementData arg0)
        {
            var last = arg0.path.Path[0];
            Vector2 lastDir = CurrentLookAt();
            bool running = true;
            bool turning = false;
            var seq = LeanTween.sequence();
            foreach (var pos in arg0.path.Path.Skip(1))
            {
                var dir = (Vector2)pos.Position - last.Position;

                if (dir != lastDir)
                {
                    seq.append(() =>
                    {
                        running = false;
                        turning = true;
                        _unitAnimations.Turn(LookAtRotation(lastDir) > LookAtRotation(dir), () => !turning);
                    });
                    TweenTurn(seq, lastDir, dir);
                    seq.append(() => { turning = false; });
                }
                seq.append(0.01f);
                seq.append(() =>
                {
                    SetColor();
                    SyncVisibility();
                });
                seq.append(() =>
                {
                    running = true;
                    _unitAnimations.Move(() => !running);
                });
                seq.append(LeanTween.move(_root.gameObject, WorldPosition(_grid, pos),
                    _unitAnimations.MoveTime));
                //Safe in case of rounding errors in tween
                last = pos;
                lastDir = dir;
            }
            seq.append(() =>
            {
                running = false;
            });
        }

        private void Attack(UnitAttackData arg0)
        {
            bool running = true;
            var seq = LeanTween.sequence();
            var origin = CurrentLookAt();
            var targ = new Vector2(arg0.direction.x, arg0.direction.y);
            //seq.append(() => { Debug.Log($"Attack {origin} {targ}"); });
            TweenTurn(seq, origin, targ);
            //seq.append(() => { Debug.Log($"Attack {origin} {targ}"); });
            seq.append(() => _unitAnimations.Attack(arg0, () =>
            {
                seq.append(_unitAnimations.Delay(targ.magnitude));
                seq.append(EventQueue.ProcessAll);
            }));
        }

        private void TweenTurn(LTSeq seq, Vector2 origin, Vector2 dest)
        {
            if (Mathf.Abs(LookAtRotation(origin) - LookAtRotation(dest)) > 5f)
            {
                seq.append(LeanTween.value(_root.gameObject,
                    d => Rotation = LookAtRotation(d), origin, dest, _unitAnimations.RotationTime));
            }
            else
            {
                seq.append(() => Rotation = LookAtRotation(dest));
            }
        }
        // ReSharper disable Unity.PerformanceAnalysis
    }
}