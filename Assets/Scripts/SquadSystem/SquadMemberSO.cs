using SquadSystem.Enums;
using UnitSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "SquadMemberSO", menuName = "Scriptable Objects/SquadMemberSO")]
public class SquadMemberSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private ESquadClass _class;
    [SerializeField] private UnitInfo _unitInfo;
    [SerializeField] private int _coinsCost;
    [SerializeField] private int _etherCost;
}
