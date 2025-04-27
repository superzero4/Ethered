using NaughtyAttributes;
using SquadSystem;
using UnitSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common
{
    [CreateAssetMenu(fileName = "New encounter", menuName = "Battle/Encounter", order = 1)]
    public class EncounterInfo : ScriptableObject
    {
        [SerializeField] private Squad _units;
        [SerializeField] private UnitInfo _defaultUnit;
        public Squad Units => _units;

        public UnitInfo DefaultUnit => _defaultUnit;

        public int Coins
        {
            get { return _units.Coins; }
            set { _units.Coins = value; }
        }
        public int Ether
        {
            get { return _units.Ether; }
            set { _units.Ether = value; }
        }

        public void Fill(Squad squad)
        {
            _units = squad;
        }

        [Button]
        private void CreateDefault()
        {
            _units.Init(5, _defaultUnit);
        }

        public static void FillRandomNames(Squad squad)
        {
            string[] names = new string[]
            {
                "Sacha", "Flo", "John", "Doe", "Jane", "Smith", "Alice", "Bob", "Charlie", "David", "Eve", "Frank",
                "Grace", "Heidi",
                "Ivan", "Judy", "Kevin", "Linda", "Mallory", "Oscar", "Peggy", "Romeo", "Trent", "Ursula", "Victor",
                "Walter", "Xander", "Yvonne", "Zelda"
            };
            for (int i = 0; i < squad.Units.Count; i++)
            {
                var info = squad.Units[i].VisualInformations;
                info.Name = names[UnityEngine.Random.Range(0, names.Length)] + " " +
                            names[UnityEngine.Random.Range(0, names.Length)];
                squad.Units[i].VisualInformations = info;
            }
        }

        [Button]
        private void FillRandomNames()
        {
            FillRandomNames(_units);
        }

        [Button]
        private void SetAllWhite()
        {
            SetColors(Color.white);
        }

        private void SetColors(Color color)
        {
            foreach (var unit in _units.Units)
            {
                var info = unit.VisualInformations;
                info.Color = color;
                unit.VisualInformations = info;
            }
        }
    }
}