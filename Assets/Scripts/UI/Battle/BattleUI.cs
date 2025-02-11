using Common.Visuals;
using NaughtyAttributes;
using UnityEngine;

namespace UI.Battle
{
    public class BattleUI : MonoBehaviour
    {
        [SerializeField] private UnitUI _unitUI;
        [SerializeField] private InfoUI _tileUI;
        [SerializeField] private InfoUI _targetUI;
        [SerializeField] private VisualInformations _default;
        [SerializeField,InfoBox("Reusable, changable and even reassignable action button")] private ClickableUI _actionButton;
        public UnitUI UnitUI => _unitUI;

        public InfoUI TileUI => _tileUI;

        public InfoUI TargetUI => _targetUI;

        public ClickableUI ActionButton => _actionButton;

        public void Initialize()
        {
            VisualInformations.Default = _default;
        }
    }
}