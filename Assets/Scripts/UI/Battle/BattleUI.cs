using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Events.Combat;
using Common.Visuals;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace UI.Battle
{
    public class BattleUI : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private UnitUI _unitUI;

        [SerializeField] private InfoUI _tileUI;
        [SerializeField] private InfoUI _targetUI;
        [SerializeField] private TimelineUI _timelineUI;
        [SerializeField] private PhaseUI _phaseUI;

        [SerializeField, InfoBox("Reusable, changable and event reassignable action button")]
        private ClickableUI _confirmButton;

        [SerializeField] private ClickableUI _endTurnButton;
        [Header("Global")] [SerializeField] private VisualInformations _default;

        [Serializable]
        private struct Icons
        {
            public IIcon.IconType iconType;
            public IIcon.IconText icon;
        }

        [SerializeField] private List<Icons> _icons;

        public UnitUI UnitUI => _unitUI;

        public InfoUI TileUI => _tileUI;

        public InfoUI TargetUI => _targetUI;

        public ClickableUI ConfirmButton => _confirmButton;

        public TimelineUI TimelineUI => _timelineUI;

        public PhaseUI PhaseUI => _phaseUI;

        public ClickableUI EndTurnButton => _endTurnButton;

        public void Initialize(UserInput userInput)
        {
            if (!_targetUI.isActiveAndEnabled)
                _targetUI = null;
            userInput.EndTurn.AddListener(_endTurnButton.Click);
            VisualInformations.Default = _default;
            IIcon.Icons = _icons.ToDictionary(i => i.iconType, i => i.icon);
            _unitUI.Initialize();
            _phaseUI.Initialize(false);
        }
    }
}