using UI;
using UnityEngine;

namespace UI.Battle
{
    public class BattleUI : MonoBehaviour
    {
        [SerializeField] private InfoUI _unitUI;
        [SerializeField] private InfoUI _tileUI;
        [SerializeField] private InfoUI _targetUI;
        [SerializeField] private ActionUI _actionUI;
        [SerializeField] private VisualInformations _default;
        public InfoUI UnitUI => _unitUI;

        public InfoUI TileUI => _tileUI;

        public InfoUI TargetUI => _targetUI;

        public void Initialize()
        {
            VisualInformations.Default = _default;
        }
    }
}