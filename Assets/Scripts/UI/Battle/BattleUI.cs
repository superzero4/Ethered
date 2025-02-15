using Common.Visuals;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Battle
{
    public class BattleUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UnitUI _unitUI;
        [SerializeField] private InfoUI _tileUI;
        [SerializeField] private InfoUI _targetUI;
        [SerializeField] private TimelineUI _timelineUI;
        [SerializeField] private PhaseUI _phaseUI;

        [SerializeField, InfoBox("Reusable, changable and event reassignable action button")]
        private ClickableUI _confirmButton;
        [SerializeField]
        private ClickableUI _endTurnButton;

        [SerializeField] private VisualInformations _default;
        public UnitUI UnitUI => _unitUI;

        public InfoUI TileUI => _tileUI;

        public InfoUI TargetUI => _targetUI;

        public ClickableUI ConfirmButton => _confirmButton;

        public TimelineUI TimelineUI1 => _timelineUI;

        public PhaseUI PhaseUI => _phaseUI;

        public ClickableUI EndTurnButton => _endTurnButton;

        public void Initialize()
        {
            VisualInformations.Default = _default;
        }
    }
}