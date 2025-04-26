using System.Collections.Generic;
using BattleSystem;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnitSystem.AI
{
    public class PriorityComparer : MonoBehaviour, IComparer<Action>
    {
        private enum PriorityType
        {
            Attack,
            Support,
            Move,
        }

        [SerializeField] private PriorityType _priority;

        [FormerlySerializedAs("_safeDistance")] [SerializeField, UnityEngine.Range(0, 15)]
        private int _maxMove;

        public int Compare(Action x, Action y)
        {
            Assert.AreEqual(x.Origin, y.Origin);
            var xTar = x.MainTarget;
            var yTar = y.MainTarget;
            if (_priority != PriorityType.Move)
            {
                var team = x.Origin.Team;
                if (xTar.Team != ETeam.None && yTar.Team != ETeam.None)
                {
                    if (xTar.Team == ETeam.None)
                        return -1;
                    if (yTar.Team == ETeam.None)
                        return 1;
                    if (xTar.Team != yTar.Team)
                    {
                        if (_priority == PriorityType.Attack)
                            return xTar.Team != team ? 1 : -1;
                        if (_priority == PriorityType.Support)
                            return xTar.Team == team ? 1 : -1;
                        else
                            Assert.IsTrue(false);
                    }
                }
            }

            var origin = x.Origin.Position;
            if (xTar.Team != ETeam.None)
                return -1;
            if (yTar.Team != ETeam.None)
                return 1;
            Assert.IsTrue(xTar.Team == ETeam.None && yTar.Team == xTar.Team);
            var d1 = origin.DistanceTo(xTar.Position);
            var d2 = origin.DistanceTo(yTar.Position);
            if (d1 > _maxMove && d2 > _maxMove)
                return d1.CompareTo(d2);
            if (d1 < _maxMove && d2 < _maxMove)
                return -d1.CompareTo(d2);
            else
                return d1 < _maxMove ? 1 : -1;
        }
    }
}