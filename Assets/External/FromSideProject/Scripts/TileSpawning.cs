using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TileSpawning
{
    public class SpawnOrderer : IComparer<(int, int)>
    {
        public IComparer<(int, int)> orderer;
        public float delayBeetweenspawns;
        public Vector2Int dimensions;
        Func<(int, int), (int, int), Vector2Int, bool> predicateAInfB;

        public static readonly SpawnOrderer DefaultOrderer = new SpawnOrderer((a, b, v) => true,0);
        public static readonly SpawnOrderer RandomOrderer = new SpawnOrderer((a, b, v) => Random.value < .5f);
        public static readonly SpawnOrderer IThenJOrderer = new SpawnOrderer((a, b, v) => true);
        public static readonly SpawnOrderer JThenIOrderer = new SpawnOrderer((a, b, v) => a.Item2 < b.Item2 || a.Item1 < b.Item1);
        public static readonly SpawnOrderer SpiralOrderer = new SpawnOrderer((a, b, v) =>
        {
            //float x = v.x / 2f; float y = v.y / 2f;
            //var vv = v/2;
            //Debug.Log(MathHelper.dist(v, (2,3)) +" "+ MathHelper.dist(v, (1, 3)));
            var vv = (v.x, v.y);
            var distsToA = (Mathf.Abs(vv.x - a.Item1), Mathf.Abs(vv.y - a.Item2));
            var distsToB = (Mathf.Abs(vv.x - b.Item1), Mathf.Abs(vv.y - b.Item2));
            float totalDistToA = distsToA.Item1 + distsToA.Item2;
            float totalDistToB = distsToB.Item1 + distsToB.Item2;
            if (totalDistToA == totalDistToB)
            {
                float sum1 = distsToA.Item1 + distsToA.Item2;
                float sum2 = distsToB.Item1 + distsToB.Item2;
                if (sum1 == sum2)
                {
                    return Math.Abs(distsToA.Item1) < Math.Abs(distsToB.Item1);
                }
                else
                {
                    return (sum1 < sum2);
                }
            }
            else
            {
                return totalDistToA < totalDistToB;
            }           
        });


        public int Compare((int, int) a, (int, int) b)
        {
            return predicateAInfB.Invoke(a, b, dimensions/2) ? -1 : 1;
        }
        /*        public SpawnOrderer(Func<(int, int), (int, int), bool> predicateAInfB, float delayBeetweenspawns = .1f)
                {
                    this.orderer = Comparer<(int, int)>.Create((a, b) => predicateAInfB.Invoke(a, b) ? -1 : 1);
                    this.delayBeetweenspawns = delayBeetweenspawns;
                }*/
        public SpawnOrderer(Func<(int, int), (int, int), Vector2Int, bool> predicateAInfB, float delayBeetweenspawns = .1f)
        {
            this.delayBeetweenspawns = delayBeetweenspawns;
            this.predicateAInfB = predicateAInfB;
        }
    }
    public class CompleteAnimation
    {
        public const float timeOfStartingAnimations = 1f;
        public ISpawningPosition spawning;
        public IMovementAnimation movement;
        private SpawnOrderer coordOrderer;
        public List<(int, int)> sortedTiles;

        private static List<CompleteAnimation> animations = new List<CompleteAnimation>();//= { randomLeaning, droppingFromOverOrBelow, droppingCheckBoard, delayedCheckBoard, delayedRandomAbove };

        #region animationStorage
        private static CompleteAnimation randomLeaning = new CompleteAnimation(new randomRotAndPosInUnitCircle());
        private static CompleteAnimation delayedRandomLeaning = new CompleteAnimation(new randomRotAndPosInUnitCircle(),SpawnOrderer.RandomOrderer);
        private static CompleteAnimation droppingFromOverOrBelow = new CompleteAnimation(new Parametrizable0ForBelow1ForAboveSpawn(v => Random.Range(0, 2)));
        private static CompleteAnimation droppingCheckBoard = new CompleteAnimation(new Parametrizable0ForBelow1ForAboveSpawn(v => (int)(v.x + v.z) % 2));
        private static CompleteAnimation delayedCheckBoard = new CompleteAnimation(new Parametrizable0ForBelow1ForAboveSpawn(v => (int)(v.x + v.z) % 2), SpawnOrderer.IThenJOrderer);
       
        private static CompleteAnimation delayedNormal = new CompleteAnimation(new Parametrizable0ForBelow1ForAboveSpawn(v => 1), SpawnOrderer.IThenJOrderer);
        private static CompleteAnimation delayedRandomAbove = new CompleteAnimation(new Parametrizable0ForBelow1ForAboveSpawn(v => 1), SpawnOrderer.RandomOrderer);
        private static CompleteAnimation delayedRandomBelow = new CompleteAnimation(new Parametrizable0ForBelow1ForAboveSpawn(v => 0), SpawnOrderer.RandomOrderer);
        private static CompleteAnimation delayedSpiralAbove = new CompleteAnimation(new Parametrizable0ForBelow1ForAboveSpawn(v => 1), SpawnOrderer.SpiralOrderer);

        #endregion
        public static CompleteAnimation getRandomAnimation(Vector2Int dimensions)
        {
            int lenght = animations.Count;
            //Debug.Log(animations[0].spawning);
            CompleteAnimation animation = loadAndReturnAnimation(dimensions, Random.Range(0, lenght));
            return animation;
        }

        private static CompleteAnimation loadAndReturnAnimation(Vector2Int dimensions, int indexOfAnimation)
        {
            CompleteAnimation animation = animations[indexOfAnimation];
            //Debug.Log("Playing spawning animation : " + nameof(animations[indexOfAnimation]));
            animation.setSizeAndInitList(dimensions);
            return animation;
        }
        private void setSizeAndInitList(Vector2Int dimensions)
        {
            sortedTiles = new List<(int, int)>();
            coordOrderer.dimensions = dimensions;
            //sortedTiles.Add((0, 0));
            for (int i = 0; i < dimensions.x; i++)
            {
                for (int j = 0; j < dimensions.y; j++)
                {
                    int cpt = 0;
                    foreach (var ij in sortedTiles)
                    {
                        /*Debug.Log(coordOrderer == null);
                        Debug.Log(coordOrderer.orderer == null);*/

                        if (coordOrderer.Compare((i, j), ij) < 0)
                        {
                            sortedTiles.Insert(cpt, (i, j));
                            break;
                        }
                        cpt++;
                    }
                    if (cpt == sortedTiles.Count)
                    {
                        sortedTiles.Add((i, j));
                    }
                }
            }
            /*String s = "list : ";
            foreach (var item in sortedTiles)
            {
                s += item + "; ";
            }
            Debug.Log(s);*/
            //sortedTiles.Sort(coordOrderer.orderer);
        }
        internal float getDelayBeetweenspawns()
        {
            return coordOrderer.delayBeetweenspawns;
        }
        public static CompleteAnimation getFixedAnimation(int indexOfTransition, Vector2Int dimensions)
        {
            return loadAndReturnAnimation(dimensions, indexOfTransition);
        }
        public void playAnimation(GameObject objectToAnimate, Vector3 targetPos, Quaternion targetRot, float timeOfAnimation)
        {
            spawning.initPosition(objectToAnimate, targetPos, targetRot);
            movement.doMovement(objectToAnimate, targetPos, targetRot, timeOfAnimation);
        }
        public CompleteAnimation(ISpawningPosition spawning, SpawnOrderer coordOrderer = null, IMovementAnimation movement = null)
        {
            this.spawning = spawning;
            this.movement = movement ?? new NormalLeaning();
            this.coordOrderer = coordOrderer ?? SpawnOrderer.DefaultOrderer;
            animations.Add(this);
        }
    }
    #region movementAnimations
    public interface IMovementAnimation
    {
        public abstract void doMovement(GameObject objectToAnimate, Vector3 targetPos, Quaternion targetRot, float timeOfAnimation = CompleteAnimation.timeOfStartingAnimations);
    }
    class NormalLeaning : IMovementAnimation
    {
        public void doMovement(GameObject objectToAnimate, Vector3 targetPos, Quaternion targetRot, float timeOfAnimation)
        {
            objectToAnimate.LeanMove(targetPos, timeOfAnimation);
            objectToAnimate.LeanRotate(targetRot.eulerAngles, timeOfAnimation);
        }
    }
    class DroppingFromHeight : IMovementAnimation
    {
        public void doMovement(GameObject objectToAnimate, Vector3 targetPos, Quaternion targetRot, float timeOfAnimation)
        {
            new NormalLeaning().doMovement(objectToAnimate, targetPos, targetRot, timeOfAnimation);
        }
    }
    #endregion
    #region spawningAnimation
    public interface ISpawningPosition
    {
        public abstract void initPosition(GameObject objectToPlace, Vector3 targetPos, Quaternion targetRot);
    }
    class randomRotAndPosInUnitCircle : ISpawningPosition
    {
        public void initPosition(GameObject objectToPlace, Vector3 targetPos, Quaternion targetRot)
        {
            objectToPlace.transform.position = targetPos + UnityEngine.Random.insideUnitSphere;
            objectToPlace.transform.Rotate(UnityEngine.Random.rotationUniform.eulerAngles);
        }
    }
    /*class RightAboveOrBelowSpawnPosition : ISpawningPosition
    {
        public void initPosition(GameObject objectToPlace, Vector3 targetPos, Quaternion targetRot)
        {
            objectToPlace.transform.position = targetPos + 2 * (1 - 2 * UnityEngine.Random.Range(0, 2)) * Vector3.up;
            objectToPlace.transform.rotation = targetRot;
            //objectToPlace.transform.RotateAround(objectToPlace.transform.position,Vector3.up,UnityEngine.Random.value*360);
        }
    }*/
    /*class RightAboveOrBelowSpawnPositionCheckBoardVariation : ISpawningPosition
    {
        public void initPosition(GameObject objectToPlace, Vector3 targetPos, Quaternion targetRot)
        {
            objectToPlace.transform.position = targetPos + 2 * (1 - 2 * ((int)(targetPos.x + targetPos.z) % 2)) * Vector3.up;
            objectToPlace.transform.rotation = targetRot;
            //objectToPlace.transform.RotateAround(objectToPlace.transform.position,Vector3.up,UnityEngine.Random.value*360);
        }
    }*/
    class Parametrizable0ForBelow1ForAboveSpawn : ISpawningPosition
    {
        Func<Vector3, int> funcReturning0ForBelowAndOneForUp;
        public Parametrizable0ForBelow1ForAboveSpawn(Func<Vector3, int> parameter)
        {
            this.funcReturning0ForBelowAndOneForUp = parameter;
        }
        public void initPosition(GameObject objectToPlace, Vector3 targetPos, Quaternion targetRot)
        {
            objectToPlace.transform.position = targetPos + 2 * (2 * funcReturning0ForBelowAndOneForUp.Invoke(targetPos) - 1) * Vector3.up;
            objectToPlace.transform.rotation = targetRot;
        }
    }
    #endregion
}
