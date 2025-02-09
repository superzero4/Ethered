using Common.Visuals;
using UnityEngine;

namespace UI.Battle
{
    public class BattleUI : MonoBehaviour
    {
        [SerializeField] private UnitUI _unitUI;
        [SerializeField] private InfoUI _tileUI;
        [SerializeField] private InfoUI _targetUI;
        [SerializeField] private VisualInformations _default;
        public UnitUI UnitUI => _unitUI;

        public InfoUI TileUI => _tileUI;

        public InfoUI TargetUI => _targetUI;

        public void Initialize()
        {
            VisualInformations.Default = _default;
        }
    }
}