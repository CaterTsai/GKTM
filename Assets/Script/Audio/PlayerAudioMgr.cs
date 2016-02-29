using UnityEngine;
using System.Collections;

//Audio for UI
public class PlayerAudioMgr : MonoBehaviour
{
    #region enum
    public enum eP_AUDIO_TYPE
    {
        eAUDIO_JUMP = 0
        ,eAUDIO_LAND
        ,eAUDIO_WRONG
        ,eAUDIO_DROP
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
    public void playAudio(eP_AUDIO_TYPE eType)
    {
        _AudioSource.PlayOneShot(_AudioClip[GetAudioClipIndex(eType)]);
    }

    //---------------------------------------------------
    private int GetAudioClipIndex(eP_AUDIO_TYPE eType)
    {
        int result_ = 0;
        switch (eType)
        {
            case eP_AUDIO_TYPE.eAUDIO_JUMP:
                {
                    result_ = 0;
                }
                break;
            case eP_AUDIO_TYPE.eAUDIO_LAND:
                {
                    result_ = 1;
                }
                break;
            case eP_AUDIO_TYPE.eAUDIO_WRONG:
                {
                    result_ = 2;
                }
                break;
            case eP_AUDIO_TYPE.eAUDIO_DROP:
                {
                    result_ = 3;
                }
                break;
        }
        return result_;
    }
    #endregion


}
