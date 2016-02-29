using UnityEngine;
using System.Collections;
using DG.Tweening;

public class UIMove : MonoBehaviour
{

    #region Method
    //---------------------------------------------------
    public void Enter(TweenCallback callback = null)
    {  
        Sequence enterSeq_ = DOTween.Sequence();
        enterSeq_.Append(transform.DOScaleX(1.0f, 0.5f).SetEase(Ease.InOutExpo));
        if (callback != null)
        {
            enterSeq_.AppendCallback(callback);
        }
    }

    //---------------------------------------------------
    public void Exit(TweenCallback callback = null)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        Sequence exitSeq_ = DOTween.Sequence();
        exitSeq_.Append(transform.DOScaleX(0.0f, 0.5f).SetEase(Ease.InOutExpo));
        if (callback != null)
        {
            exitSeq_.AppendCallback(callback);
        }
    }
    #endregion
    
}
