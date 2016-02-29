using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Player : MonoBehaviour {

    #region Element
    public TweenCallback _callback = null;
    private Rigidbody _body = null;
    private Vector3 _StartPos = new Vector3(0, 10, 0);
    private PlayerAudioMgr _AudioMgr = null;
    private bool _isDrop = false;
    #endregion

    #region Basic Method
    //---------------------------------------------------
    void Start()
    {
        if (_body == null)
        {
            _body = GetComponent<Rigidbody>();
        }

        if (_AudioMgr == null)
        {
            _AudioMgr = GetComponent<PlayerAudioMgr>();
        }
        _StartPos = transform.position;
    }

    //---------------------------------------------------
    void Update()
    {
        if (!_isDrop && transform.position.y < constParameter.cPLAYER_DROP_TRIGGER)
        {
            _AudioMgr.playAudio(PlayerAudioMgr.eP_AUDIO_TYPE.eAUDIO_DROP);
            _isDrop = true;
        }
    }

    //---------------------------------------------------
    void OnEnable()
    {
        transform.position = _StartPos;
        _isDrop = false;
    }

    #endregion

    #region Method
    //---------------------------------------------------
    public void JumpTo(Vector3 pos)
    {
        //Rotate -> Jump -> Rotate
        var from_ = new Vector2(0.0f, 1.0f);
        var to_ = new Vector2(pos.x -transform.position.x, pos.z - transform.position.z).normalized;
        float fdegree_ = Vector2.Angle(from_, to_);
        fdegree_ = (to_.x > from_.x)?fdegree_:-fdegree_;

        //Audio
        _AudioMgr.playAudio(PlayerAudioMgr.eP_AUDIO_TYPE.eAUDIO_JUMP);

        //Set Animation Sequence
        Sequence JumpSeq_ = DOTween.Sequence();
        JumpSeq_.Append(_body.DORotate(new Vector3(.0f, fdegree_, .0f), constParameter.cPLAYER_JUMP_DURATION * 0.1f, RotateMode.FastBeyond360));
        JumpSeq_.Append(_body.DOJump(pos, constParameter.cPLAYER_JUMP_HEIGHT, 1, constParameter.cPLAYER_JUMP_DURATION * 0.8f));
        JumpSeq_.Append(_body.DORotate(new Vector3(.0f, .0f, .0f), constParameter.cPLAYER_JUMP_DURATION * 0.1f, RotateMode.FastBeyond360));
        JumpSeq_.AppendCallback(
            () =>
            {
                _AudioMgr.playAudio(PlayerAudioMgr.eP_AUDIO_TYPE.eAUDIO_LAND);
            }
        );
        if (_callback != null)
        {
            JumpSeq_.AppendCallback(_callback);
        }
    }

    //---------------------------------------------------
    public void CanNotJump()
    {
        _AudioMgr.playAudio(PlayerAudioMgr.eP_AUDIO_TYPE.eAUDIO_WRONG);
    }
    #endregion
}
