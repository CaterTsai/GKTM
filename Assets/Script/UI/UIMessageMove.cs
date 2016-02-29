using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class UIMessageMove : MonoBehaviour
{
    private Color _StartColor = new Color(1.0f, 0.6f, 0.0f, 0.0f);
    private Vector3 _StartPos;

    private RectTransform _rectTransform = null;
    private Text _Message = null;

    #region Basic Method
    void OnEnable()
    {
        if (_Message == null)
        {
            _Message = GetComponent<Text>();
            _Message.text = "";
        }
        if(_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
            _StartPos = _rectTransform.position;
        }
        
    }
    #endregion

    #region Method
    public void showMessage(string Message)
    {  
        _Message.text = Message;
        Sequence showSeq_ = DOTween.Sequence();
        showSeq_.Append(_rectTransform.DOMoveY(_StartPos.y + 50.0f, 0.5f));
        showSeq_.Join(_Message.DOFade(1.0f, 0.25f));
        showSeq_.AppendCallback(
            () =>
            {
                _Message.color = _StartColor;
                _rectTransform.position = _StartPos;
            }
        );
    }

    #endregion
}
