using UnityEngine;

[CreateAssetMenu(fileName = "SquadMemberSO", menuName = "Scriptable Objects/SquadMemberSO")]
public class SquadMemberSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _class; // TODO : Replace string with enum
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _armor;
    [SerializeField] private int _coinsCost;
    [SerializeField] private int _etherCost;
}
