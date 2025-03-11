using SquadSystem.Enums;
using UnitSystem;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SquadMemberSO", menuName = "Scriptable Objects/SquadMemberSO")]
public class SquadMemberSO : ScriptableObject
{
    [SerializeField] private string _name;
    [FormerlySerializedAs("_class")] [SerializeField] private ESquadMemberClass memberClass;
    [SerializeField] private UnitInfo _unitInfo;
    [SerializeField] private int _coinsCost;
    [SerializeField] private int _etherCost;
}
