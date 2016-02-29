using UnityEngine;
using System.Collections;

//Audio for UI
public class UIAudioMgr: MonoBehaviour
{
    #region enum
    public enum eAUDIO_TYPE
    {
        eAUDIO_BUTTON   =   0
        ,eAUDIO_COUTDOWN
        ,eAUDIO_START
        ,eAUDIO_TOP
        ,eAUDIO_PASSLEVEL
    }
    #endregion

    #region Element
    public AudioClip[] _AudioClip = new AudioClip[4];
    private AudioSource _AudioSource;

    #endregion

    #region Basic Method
    //---------------------------------------------------
    void Start()
    {
        if (_AudioSource == null)
        {
            _AudioSource = GetComponent<AudioSource>();
        }

    }
    #endregion

    #region Method
    //---------------------------------------------------
    public void playAudio(eAUDIO_TYPE eType)
    {
        _AudioSource.PlayOneShot(_AudioClip[GetAudioClipIndex(eType)]);
    }

    //---------------------------------------------------
    private int GetAudioClipIndex(eAUDIO_TYPE eType)
    {
        int result_ = 0;
        switch (eType)
        {
            case eAUDIO_TYPE.eAUDIO_BUTTON:
                {
                    result_ = 0;
                }
                break;
            case eAUDIO_TYPE.eAUDIO_COUTDOWN:
                {
                    result_ = 1;
                }
                break;
            case eAUDIO_TYPE.eAUDIO_START:
                {
                    result_ = 2;
                }
                break;
            case eAUDIO_TYPE.eAUDIO_TOP:
                {
                    result_ = 3;
                }
                break;
            case eAUDIO_TYPE.eAUDIO_PASSLEVEL:
                {
                    result_ = 4;
                }
                break;
        }
        return result_;
    }
    #endregion


}
