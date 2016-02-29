using UnityEngine;
using System.Collections;
using DG.Tweening;

public class IslandMove : MonoBehaviour {

    public float _targetScale = 1.0f;
    public float _duration = 0.5f;
    private BoxCollider _collider = null;


    #region Method
    //---------------------------------------------------
    public void Enter(TweenCallback callback = null)
    {
        if (_collider == null)
        {
            _collider = GetComponent<BoxCollider>();
        }
        _collider.enabled = true;
        Sequence enterSeq_ = DOTween.Sequence();
        enterSeq_.Append(transform.DORotate(new Vector3(0, 360, 0), _duration, RotateMode.FastBeyond360));
        enterSeq_.Join(transform.DOScale(_targetScale, _duration));
        if (callback != null)
        {
            enterSeq_.AppendCallback(callback);
        }
        
    }

    //---------------------------------------------------
    public void Exit(TweenCallback callback = null)
    {
        if (gameObject.activeInHierarchy)
        {
            if (_collider == null)
            {
                _collider = GetComponent<BoxCollider>();
            }
            _collider.enabled = false;
            Sequence exitSeq_ = DOTween.Sequence();
            exitSeq_.Append(transform.DORotate(new Vector3(0, 360, 0), _duration, RotateMode.FastBeyond360));
            exitSeq_.Join(transform.DOScale(0, _duration));

            if (callback != null)
            {
                exitSeq_.AppendCallback(callback);
            }
        }
    }
    #endregion


}
