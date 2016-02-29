using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIResult : MonoBehaviour {

    public Text _NormalBoxNum = null;
    public Text _SpecialBoxNum = null;
    public Text _NormalBoxScore = null;
    public Text _SpecialBoxScore = null;
    public Text _Score = null;
    public Text _TopScore = null;

    void OnEnable()
    {

    }

    //---------------------------------------------------
    public void setNormalBoxNum(int num)
    {
        _NormalBoxNum.text = num.ToString("D3");
    }

    //---------------------------------------------------
    public void setSpecialBoxNum(int num)
    {
        _SpecialBoxNum.text = num.ToString("D3");
    }

    //---------------------------------------------------
    public void setNormalBoxScore(int score)
    {
        _NormalBoxScore.text = score.ToString("D4");
    }

    //---------------------------------------------------
    public void setSpecialBoxScore(int score)
    {
        _SpecialBoxScore.text = score.ToString("D4");
    }

    //---------------------------------------------------
    public void setScore(int score)
    {
        _Score.text = score.ToString("D5");
    }

    //---------------------------------------------------
    public void isTopScore(bool val)
    {
        if (val)
        {
            _TopScore.gameObject.SetActive(true);
        }
        else
        {
            _TopScore.gameObject.SetActive(false);
        }
    }

}
