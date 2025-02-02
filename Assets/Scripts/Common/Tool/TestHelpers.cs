using System;
using BattleSystem;
using BattleSystem.TileSystem;
using UnityEngine;

namespace Common.Events.Tool
{
    public class TestHelpers : MonoBehaviour
    {
        [SerializeField] private bool _runTests = true;

        public void Awake()
        {
            if (_runTests)
            {
                Debug.LogWarning("---------Tests---------");
                TestLOS();
                Debug.LogWarning("-------Tests ended-----");
            }
        }

        public static void TestLOS()
        {
            Debug.LogWarning("---Testing lines on some specific case---");
            PositionIndexer ori1 = new PositionIndexer(1, 1);
            PositionIndexer t1 = new PositionIndexer(2, 4);
            PositionIndexer t2 = new PositionIndexer(2, 2);
            PositionIndexer t3 = new PositionIndexer(2, 5);
            PositionIndexer t4 = new PositionIndexer(5, 4);
            PositionIndexer t5 = new PositionIndexer(5, -2);
            PositionIndexer t6 = new PositionIndexer(5, 2);
            //foreach (var (ori, target) in new(PositionIndexer,PositionIndexer)[] { (t3,ori1),(t1,t2) })
            foreach (var (ori, target) in new(PositionIndexer,PositionIndexer)[] { (ori1, t1),(ori1, t2), (ori1, t3),(t3,ori1),(t1,t2),(ori1,t4),(ori1,t5),(ori1,t6),(t2,t3) })
            {
                Debug.Log($"Line from {ori} to {target} : ");
                string ln = "";
                foreach (var pos in TilemapLineOfSightExtensions.StraightLine(ori, target))
                    ln += pos + " ";
                Debug.Log(ln);
            }
            Debug.LogWarning("---Testing lines on all distinct couples of a (3,1) centered 3-Manathan radius circle (including center---");
            (int x, int y)[] tab = new[]
            {
                (3, 1), (4, 4), (3, 4), (2, 4), (1, 3), (0, 2), (0, 1), (0, 0), (1, -1), (2, -2), (3, -2), (4, -2),
                    (5, -1), (6, 0), (6, 1), (6, 2)
            };
            for(int i = 0; i < tab.Length; i++)
            {
                for(int j = 0; j<tab.Length; j++)
                {
                    if (i == j)
                        continue;
                    var ori = new PositionIndexer(tab[i].x, tab[i].y);
                    var target = new PositionIndexer(tab[j].x, tab[j].y);
                    Debug.Log($"Line from {ori} to {target} : ");
                    string ln = "";
                    foreach (var pos in TilemapLineOfSightExtensions.StraightLine(ori, target))
                        ln += pos + " ";
                    Debug.Log(ln);
                }
            }
        }

    }
}