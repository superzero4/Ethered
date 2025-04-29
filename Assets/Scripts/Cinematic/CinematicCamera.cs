using System.Linq;
using UnityEngine;

public class CinematicCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform[] _targets; // The target object to follow

    [SerializeField]
    private LeanTweenType _rotationEeasing = LeanTweenType.easeInOutQuad;
    [SerializeField]
    private LeanTweenType _positionEeasing = LeanTweenType.easeInOutQuad;

    [SerializeField, Range(0, 10f)] private float _speed = 1f; // The speed of the camera movement

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var seq = LeanTween.sequence();
        //LeanTween.moveSpline(_camera.gameObject, _targets.Select(t => t.position).ToArray(), _speed).setEase(_rotationEeasing);
        foreach (var target in _targets)
        {
            seq.append(() =>
            {
                LeanTween.move(_camera.gameObject, target.position, _speed).setEase(_positionEeasing);
                LeanTween.rotate(_camera.gameObject, target.rotation.eulerAngles, _speed).setEase(_rotationEeasing);
            });
            seq.append(_speed);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}