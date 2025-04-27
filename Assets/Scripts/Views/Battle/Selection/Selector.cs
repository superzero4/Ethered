using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using Common;
using Common.Events.UserInteraction;
using Common.Events.UserInterface;
using NaughtyAttributes;
using UnityEngine;

namespace Views.Battle.Selection
{
    public class Selector : MonoBehaviour, IReset, IPhaseView
    {
        [SerializeField] private PooledHints _cursor;
        [Header("Events")] [SerializeField] private SelectionEvent _hoverChanged = new();

        [SerializeField] private SelectionEvent _selectionUpdated = new();


        [Header("ReadOnly")] [SerializeField, ReadOnly]
        private EPhase _phase;

        [SerializeField] [ReadOnly] private Selectable _current;
        [SerializeField, ReadOnly] private bool _showCursor;
        [Header("Set externally")] private LayerMask _selectionMask;

        //[Header("References")] [SerializeField]
        private Camera _camera;
        private RaycastHit[] _results;
        private Dictionary<GameObject, Selectable> _selectables;

        public SelectionEvent HoverChanged => _hoverChanged;
        public SelectionEvent SelectionUpdated => _selectionUpdated;

        public bool ShowCursor
        {
            get => _showCursor;
            set => _showCursor = value;
        }

        public void Initialize(IEnumerable<Selectable> selectables, LayerMask mask, Camera camera, Grid grid)
        {
            _cursor.Init(2, grid);
            _camera = camera;
            ShowCursor = false;
            //We have a quick mapping from a gameObject to it's selectable component without the need of a GetComponent on every selection
            _selectables = new(selectables.Select(s => new KeyValuePair<GameObject, Selectable>(s.gameObject, s)));
            _results = new RaycastHit[8];
            _current = _selectables.First().Value;
            RaiseCurrentHover();
            _selectionMask = mask;
            StartCoroutine(CheckSelection());
        }

        public void Select()
        {
            if (_current == null || _current.Selection.unit != _unsafeHover.unit ||
                _current.Selection.environment != _unsafeHover.environment) return;
            _selectionUpdated.Invoke(_current.Selection);
        }

        private IEnumerator CheckSelection(float delay = 0.016f)
        {
            while (true)
            {
                CastRays();
                yield return new WaitForSeconds(delay);
            }
        }


        /// <summary>
        /// Compared to the even which is raised containing the last not null, usable information, the unsafe hover is correspoding in real time to what's under the mouse, including null
        /// </summary>
        private SelectionEventData _unsafeHover;
        //public SelectionEventData UnsafeHover => _unsafeHover;

        private void CastRays(bool smartRefresh = true)
        {
            int result = Physics.RaycastNonAlloc(_camera.ScreenPointToRay(Input.mousePosition), _results,
                Mathf.Infinity, _selectionMask);
            //Possible result = 0 => we don't enter
            _unsafeHover = new SelectionEventData(null, null);
            for (int i = 0; i < result; i++)
            {
                var selectable = _selectables[_results[i].transform.gameObject];
                if (_phase.Intersects(selectable.Tile.Phase))
                {
                    _unsafeHover = selectable.Selection;
                    if (!smartRefresh || selectable != _current)
                    {
                        UpdateCurrent(selectable);
                    }
                }
            }
        }

        private void UpdateCurrent(Selectable selectable, bool erase = true)
        {
            _current = selectable;
            RaiseCurrentHover();
        }

        public void RaiseCurrentHover()
        {
            if (_current != null)
            {
                if (_showCursor)
                {
                    _cursor.HintMultiple(new[] { _current.Tile.Base.Position });
                    _hoverChanged.Invoke(_current.Selection);
                }
            }
        }

        public void Reset()
        {
            //_cursor.Reset();
            //_current = null;
            ShowCursor = true;
            RaiseCurrentHover();
        }

        public void OnPhaseChanged(PhaseEventData arg0)
        {
            if (_phase != arg0.targetPhase)
            {
                _phase = arg0.targetPhase;
                UpdateCurrent(_current.Other, false);
            }
        }

        public float Progress
        {
            set
            {
                //No continuous updates required, this object works only with the discrete event to fetch the target phase
            }
        }

        public const int SelectableLayer = 6;

        public static LayerMask GetLayerMask()
        {
            return 0b1 << Layer();
        }

        private static int Layer()
        {
            return SelectableLayer;
        }

        public static void SetLayer<T>(AElementView<T> element) where T : IBattleElement
        {
            element.gameObject.layer = Layer();
        }
    }
}