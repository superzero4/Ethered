using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using BattleSystem.TileSystem;
using NUnit.Framework;
using UnityEngine;
using Views.Battle;

namespace Common.Events.Tool
{
    public class TestHelpers : MonoBehaviour
    {
        [SerializeField] private BattleView _battleView;
        [SerializeField] private bool _runTests = true;

        public IEnumerator Start()
        {
            if (_runTests)
            {
                Debug.LogWarning("---------Tests---------");
                TestLOS();
            }

            yield return new WaitForSeconds(1);
            TestInLos();
            Debug.LogWarning("-------Tests ended-----");
        }

        public void TestInLos()
        {
            PositionData center = new(new PositionIndexer(2, 0), 0);
            PositionData left = new(new PositionIndexer(0, 2), 0);
            PositionData right = new(new PositionIndexer(4, 2), 0);
            var map = _battleView.Battle.Tiles;
            LogLine(center.Position, left.Position);
            LogLine(center.Position, right.Position);
            Assert.IsTrue(map.HasLOS(center, right));
            Assert.IsTrue(map.HasLOS(center, left));
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
            foreach (var (ori, target) in new (PositionIndexer, PositionIndexer)[]
                     {
                         (ori1, t1), (ori1, t2), (ori1, t3), (t3, ori1), (t1, t2), (ori1, t4), (ori1, t5), (ori1, t6),
                         (t2, t3)
                     })
            {
                LogLine(ori, target);
            }

            Debug.LogWarning(
                "---Testing lines on all distinct couples of a (3,1) centered 3-Manathan radius circle (including center---");
            (int x, int y)[] tab = new[]
            {
                (3, 1), (4, 4), (3, 4), (2, 4), (1, 3), (0, 2), (0, 1), (0, 0), (1, -1), (2, -2), (3, -2), (4, -2),
                (5, -1), (6, 0), (6, 1), (6, 2)
            };
            //TestAllCouples(tab);
            Debug.LogWarning(
                "---Testing lines on all distinct couples of a 5 sized squard and it's center on (2,2)---");

            TestAllCouples(new []
            {
                (2, 2), (0,0), (4,4), (0,4), (4,0)
            });
        }

        private static void TestAllCouples((int x, int y)[] tab)
        {
            for (int i = 0; i < tab.Length; i++)
            {
                for (int j = 0; j < tab.Length; j++)
                {
                    if (i == j)
                        continue;
                    var ori = new PositionIndexer(tab[i].x, tab[i].y);
                    var target = new PositionIndexer(tab[j].x, tab[j].y);
                    LogLine(ori, target);
                }
            }
        }

        private static void LogLine(PositionIndexer ori, PositionIndexer target)
        {
            string ln = $"Line from {ori} to {target} : \n";
            foreach (var pos in TilemapLineOfSightExtensions.StraightLine(ori, target))
                ln += pos + " ";
            Debug.Log(ln);
        }
    }
}