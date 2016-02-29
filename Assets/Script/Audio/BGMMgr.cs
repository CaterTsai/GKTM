using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BGMMgr : MonoBehaviour
{
    #region BGM State
    public enum eBGM_TYPE : int
    {
        eBGM_OPEN   =   0
        ,eBGM_GAME
        ,eBGM_STOP
    };
    #endregion

    #region Element
    public AudioClip[] _BGMClip = new AudioClip[2];
    private AudioSource _AudioSource;

    private eBGM_TYPE _eState;
    #endregion

    #region Basic Method
    //---------------------------------------------------
    void Start()
    {
        if (_AudioSource == null)
        {
            _AudioSource = GetComponent<AudioSource>();
        }
        _eState = eBGM_TYPE.eBGM_STOP;
    }
    #endregion

    #region Method
    //---------------------------------------------------
    public void changeBGM(eBGM_TYPE eType)
    {
        if (eType == _eState)
        {
            return;
        }

        if (_eState != eBGM_TYPE.eBGM_STOP && _AudioSource.isPlaying)
        {
            Sequence fadeOutSeq_ = DOTween.Sequence();
            fadeOutSeq_.Append(_AudioSource.DOFade(0.0f, 1.0f));
            fadeOutSeq_.AppendCallback(
                () =>
                {
                    playBGM(eType);
                }
            );
        }
        else
        {
            playBGM(eType);
        }
    }

    //---------------------------------------------------
    public void StopBGM()
    {
        Sequence fadeOutSeq_ = DOTween.Sequence();
        fadeOutSeq_.Append(_AudioSource.DOFade(0.0f, 1.0f));
        fadeOutSeq_.AppendCallback(
            () =>
            {
                _AudioSource.Stop();
                _eState = eBGM_TYPE.eBGM_STOP;
            }
        );

    }

    //---------------------------------------------------
    private void playBGM(eBGM_TYPE eType)
    {
        _AudioSource.clip = _BGMClip[GetAudioClipIndex(eType)];
        _AudioSource.loop = true;
        _AudioSource.volume = 0.0f;
        _AudioSource.Play();

        Sequence playSeq_ = DOTween.Sequence();
        playSeq_.Append(_AudioSource.DOFade(0.5f, 1.0f));

        _eState = eType;
    }

    //---------------------------------------------------
    private int GetAudioClipIndex(eBGM_TYPE eType)
    {
        int result_ = 0;
        if (eType == eBGM_TYPE.eBGM_GAME)
        {
            result_ = 1;
        }
        else
        {
            //Default
            result_ = 0;
        }
        return result_;
    }
    #endregion


}
