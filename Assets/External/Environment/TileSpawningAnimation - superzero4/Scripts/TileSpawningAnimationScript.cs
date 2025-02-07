using TileSpawning;
using UnityEngine;
using static TileSpawning.CompleteAnimation;

public class TileSpawningAnimationScript : MonoBehaviour
{
    private Vector3 targetPos;
    private Quaternion targetRot;
    //private CompleteAnimation[] animations = { randomLeaning, droppingFromOverOrBelow };

    public void startPlayingAnimation(CompleteAnimation animationToPlay)
    {
        setTargets();
        animationToPlay.playAnimation(gameObject, targetPos, targetRot, timeOfStartingAnimations*(2f/3f));
        //Destroy(this, GlobalVariables.timeOfStartingAnimations);
    }

    internal void setTargets()
    {
        targetPos = transform.position;
        targetRot = transform.rotation;
        //Debug.Log("targets set to : " + targetPos + "," + targetRot);
    }

}
