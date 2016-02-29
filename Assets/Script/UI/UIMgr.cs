using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMgr : MonoBehaviour
{

    #region Element
    public GameObject _StartBtn = null;
    public GameObject _TeachPage = null;
    public GameObject _GameText = null;
    public GameObject _Result = null;

    public Text _TopScore = null;
    public Text _Countdown = null;

    private UIGameText _UIGameText = null;
    
    #endregion

    #region Basic Method
    #endregion

    #region Method
    //---------------------------------------------------
    //Start
    public void ShowTitleUI(int topScore = 0)
    {
        if (!_StartBtn.activeInHierarchy)
        {
            _StartBtn.SetActive(true);
        }

        if (!_TopScore.gameObject.activeInHierarchy)
        {
            _TopScore.gameObject.SetActive(true);
        }

        _TopScore.text = "TOP SCORE : " + topScore.ToString("D5");
        _StartBtn.GetComponent<UIMove>().Enter();
    }

    //---------------------------------------------------
    public void HideTitleUI()
    {
        if (_StartBtn.activeInHierarchy)
        {
            _StartBtn.GetComponent<UIMove>().Exit( () => { _StartBtn.SetActive(false); }) ;
        }

        if (_TopScore.gameObject.activeInHierarchy)
        {
            _TopScore.gameObject.SetActive(false);
        }
    }


    //---------------------------------------------------
    //Teach
    public void ShowTeachPage()
    {
        if (!_TeachPage.activeInHierarchy)
        {
            _TeachPage.SetActive(true);
        }
        _TeachPage.GetComponent<UIMove>().Enter();
    }

    //---------------------------------------------------
    public void HideTeachPage()
    {
        if (_TeachPage.activeInHierarchy)
        {
            _TeachPage.GetComponent<UIMove>().Exit(() => { _TeachPage.SetActive(false); });
        }
    }

    //---------------------------------------------------
    public void ShowCountdown()
    {
        if (!_Countdown.gameObject.activeInHierarchy)
        {
            _Countdown.gameObject.SetActive(true);
            _Countdown.text = "3";
        }
    }

    //---------------------------------------------------
    public void HideCountdown()
    {
        if (_Countdown.gameObject.activeInHierarchy)
        {
            _Countdown.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------
    public void setCountdown(string val)
    {
        _Countdown.text = val;
    }

    //---------------------------------------------------
    //Game Text
    public void ShowGameText()
    {
        if (!_GameText.activeInHierarchy)
        {
            _GameText.SetActive(true);
            if (_UIGameText == null)
            {
                _UIGameText = _GameText.GetComponent<UIGameText>();
            }
        }
    }

    //---------------------------------------------------
    public void HideGameText()
    {
        if (_GameText.activeInHierarchy)
        {
            _GameText.SetActive(false);
        }
    }

    //---------------------------------------------------
    public void setLevel(int newLevel)
    {
        if (_UIGameText == null)
        {
            print("NULL");
            return;
        }
        if (!_GameText.activeInHierarchy)
        {
            return;
        }
        _UIGameText.setLevel(newLevel);
    }

    //---------------------------------------------------
    public void setTimer(int time)
    {
        if (_UIGameText == null)
        {
            print("NULL");
            return;
        }
        if (!_GameText.activeInHierarchy)
        {
            return;
        }
        _UIGameText.setTimer(time);
    }

    //---------------------------------------------------
    public void showAddScore(int value)
    {
        _UIGameText.showAddTimerMessage(value);
    }

    //---------------------------------------------------
    //Result
    public void ShowResult(int normal_num, int speical_num, int score, bool is_top_score)
    {
        if (!_Result.activeInHierarchy)
        {
            _Result.SetActive(true);
        }
        _Result.GetComponent<UIMove>().Enter();

        
        UIResult UIResult_ = _Result.GetComponent<UIResult>();
        UIResult_.setNormalBoxNum(normal_num);
        UIResult_.setNormalBoxScore(normal_num * constParameter.cNORMAL_ISLAND_SCORE);
        UIResult_.setSpecialBoxNum(speical_num);
        UIResult_.setSpecialBoxScore(speical_num * constParameter.cSPECIAL_ISLAND_SCORE);
        UIResult_.setScore(score);
        UIResult_.isTopScore(is_top_score);
    }

    //---------------------------------------------------
    public void HideResult()
    {
        if (_Result.activeInHierarchy)
        {
            _Result.GetComponent<UIMove>().Exit(() => { _Result.SetActive(false); });
        }
    }
    #endregion
    
}
