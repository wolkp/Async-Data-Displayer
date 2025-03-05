using UnityEngine;
using DG.Tweening;

public class TweenRotator : MonoBehaviour
{
    [SerializeField] private RotateMode _rotateMode = RotateMode.LocalAxisAdd;
    [SerializeField] private float _rotationDuration = 1f;

    private void OnEnable()
    {
        Vector3 endValue = new Vector3(0, 0, -360);
        transform.DORotate(endValue, _rotationDuration, _rotateMode).SetLoops(-1, LoopType.Incremental);
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}