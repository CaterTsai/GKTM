using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIGameText : MonoBehaviour 
{
    public GameObject _Level = null;
    public GameObject _Timer = null;
    public UIMessageMove _Message = null;


    private Text _DisplayLevel = null;
    private Text _DisplayTimer = null;

    private int _NowTime = 0;
    private bool _TimeLessTen = false;
    private Color _NormalColor = new Color(0.66f, 0.81f, 0.66f);
    private Color _LessColor = new Color(1.0f, 0.0f, 0.0f);

    //---------------------------------------------
    void Awake()
    {
        if (_DisplayLevel == null)
        {
            _DisplayLevel = _Level.GetComponent<Text>();
        }

        if (_DisplayTimer == null)
        {
            _DisplayTimer = _Timer.GetComponent<Text>();
        }
    }

    //---------------------------------------------
    void OnEnable()
    {
        setLevel(1);
        setTimer(30);
        _NowTime = 30;
    }

    //---------------------------------------------
    public void setLevel(int iLevel)
    {
        _DisplayLevel.text = "LEVEL " + iLevel.ToString("D2");
    }

    //---------------------------------------------
    public void setTimer(int time)
    {
        if (time == _NowTime)
        {
            return;
        }
        _NowTime = time;
        
        _DisplayTimer.text = time.ToString("D2");

        if (time > 10 && _TimeLessTen)
        {
            _TimeLessTen = false;
            _DisplayTimer.color = _NormalColor;
        }
        else if(time <= 10 && !_TimeLessTen)
        {
            _TimeLessTen = true;
            _DisplayTimer.color = _LessColor;
        }
    }

    //---------------------------------------------
    public void showAddTimerMessage(int value)
    {
        string message_ = "+ " + value.ToString();
        _Message.showMessage(message_);
    }
}
