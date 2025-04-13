using System;
using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using Common.Visuals;
using UnityEngine;
using UnityEngine.Serialization;
using Environment = BattleSystem.Environment;

namespace Common
{
    [CreateAssetMenu(fileName = "New map", menuName = "Battle/Map", order = 1)]
    public class MapInfo : ScriptableObject
    {
        [SerializeField] private Vector2Int _size;
        [SerializeField] private List<EnvironmentGroup> _specificEnvironments;

        [Serializable]
        public struct EnvironmentGroup
        {
            [SerializeField] public EnvironmentInfo environment;

            [FormerlySerializedAs("positions")] [SerializeField]
            public PositionData center;

            [SerializeField] public PositionIndexer min;
            [SerializeField] public PositionIndexer max;
        }

        public IEnumerable<EnvironmentGroup> Environments()
        {
            Dictionary<(PositionData center, PositionIndexer min, PositionIndexer max), EnvironmentInfo> dic = new();
            foreach (var env in _specificEnvironments)
            {
                var k = dic.Keys.FirstOrDefault(x =>
                    x.center.Position == env.center.Position && x.min.position == env.max.position && x.max.position == env.max.position);
                if (!k.Equals(default))
                {
                    var info = dic[k];
                    dic.Remove(k);
                    var center = k.center;
                    center.Phase = k.center.Phase | env.center.Phase;
                    dic.Add((center, env.min, env.max),
                        new EnvironmentInfo(info.VisualInformations,
                            info.AllowedMovement | env.environment.AllowedMovement));
                }
                else
                {
                    dic.Add((env.center, env.min,env.max), env.environment);
                }
            }

            return dic
                .Select(x => new EnvironmentGroup
                {
                    environment = x.Value,
                    center = x.Key.center,
                    min = x.Key.min,
                    max = x.Key.max
                });
        }

        public IEnumerable<Environment> GetSpecificEnvironments()
        {
            return _specificEnvironments
                .SelectMany<EnvironmentGroup, Environment>(ep =>
                {
                    List<Environment> environments = new();
                    for (int i = ep.min.position.x; i <= ep.max.position.x; i++)
                    {
                        for (int j = ep.min.position.y; j <= ep.max.position.y; j++)
                        {
                            var pos = new PositionData(ep.center.Position.x + i,
                                ep.center.Position.y + j ,
                                ep.center.Phase);
                            if (pos.Phase == EPhase.None)
                                continue;
                            var env = new Environment(ep.environment, pos);
                            environments.Add(env);
                        }
                    }

                    return environments;
                });
        }

        public Vector2Int Size => _size;
    }
}