using UnityEngine;
using System.Collections;

public class GameMgr : MonoBehaviour
{
    #region enum
    enum eGameState
    {
        eGame_toTitle = 0
        ,eGame_Title
        ,eGame_toTeach
        ,eGame_Teach
        ,eGame_toGame
        ,eGame_Game
        ,eGame_toResult
        ,eGame_Result
    }
    #endregion

    #region Element
    public GraphyMgr _GraphyMgr = null;
    public TitleMgr _TitleMgr = null;
    public UIMgr _UIMgr = null;
    public CameraControl _CameraCtrl = null;
    public UIAudioMgr _UIAudioMgr = null;
    public BGMMgr _BGMMgr = null;

    private float _Timer = 0.0f;
    private int _TopScore = 0;

    private bool _StartCountdown = false;
    private float _CountdownTimer = 0.0f;

    private eGameState _gameState = eGameState.eGame_toTitle;
    #endregion

    #region Basic Method
    //---------------------------------------------------
    void Start()
    {
        _GraphyMgr._callback += onGraphyMgrEvent;

        _TopScore = PlayerPrefs.GetInt(constParameter.cPPREF_SCORE, 0);

        initTitle();
    }

    //---------------------------------------------------
    void Update()
    {
        if (_gameState == eGameState.eGame_toGame && _StartCountdown)
        {
            float tmpT_ = _CountdownTimer - Time.deltaTime;
            if (tmpT_ < 0.0f)
            {
                _StartCountdown = false;
                //Start Game
                StartCoroutine(WaitToGame());

                _UIMgr.setCountdown("GO!");
                _UIAudioMgr.playAudio(UIAudioMgr.eAUDIO_TYPE.eAUDIO_START);
            }
            else
            {
                if (Mathf.CeilToInt(_CountdownTimer) - Mathf.CeilToInt(tmpT_) > 0)
                {
                    _UIAudioMgr.playAudio(UIAudioMgr.eAUDIO_TYPE.eAUDIO_COUTDOWN);
                    _UIMgr.setCountdown(Mathf.CeilToInt(tmpT_).ToString());
                }
            }
            _CountdownTimer = tmpT_;
        }
        else if (_gameState == eGameState.eGame_Game)
        {
            _Timer -= Time.deltaTime;
            if (_Timer <= 0.0)
            {
                //Timeout
                _GraphyMgr.GameOver();
            }
            else
            {
                _UIMgr.setTimer((int)_Timer);
            }
        }

        //Exit the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    #endregion

    #region Method

    #region Flow
    //---------------------------------------------------
    private void initTitle()
    {
        _TitleMgr.DisplayTitle();
        _BGMMgr.changeBGM(BGMMgr.eBGM_TYPE.eBGM_OPEN);
        StartCoroutine(WaitToTitle());
    }

    //---------------------------------------------------
    private int ComputeScore(int normal, int special)
    {
        return (normal * constParameter.cNORMAL_ISLAND_SCORE + special * constParameter.cSPECIAL_ISLAND_SCORE);
    }

    //---------------------------------------------------
    private void toTitle()
    {
        _UIMgr.HideResult();
        _gameState = eGameState.eGame_toTitle;

        _CameraCtrl.MoveToTitle();
        _TitleMgr.DisplayTitle();
        _BGMMgr.changeBGM(BGMMgr.eBGM_TYPE.eBGM_OPEN);
        _GraphyMgr.ResetGame();
        StartCoroutine(WaitToTitle());
    }

    //---------------------------------------------------
    private void toTeach()
    {
        _gameState = eGameState.eGame_toTeach;
        _TitleMgr.HideTitle();
        _UIMgr.HideTitleUI();
        _CameraCtrl.MoveToGameStart();

        StartCoroutine(WaitToTeach());
    }

    //---------------------------------------------------
    private void toGame()
    {
        _gameState = eGameState.eGame_toGame;
        _UIMgr.HideTeachPage();
        _UIMgr.ShowCountdown();
        _StartCountdown = true;
        _CountdownTimer = 3.0f;

        _BGMMgr.StopBGM();
        _UIAudioMgr.playAudio(UIAudioMgr.eAUDIO_TYPE.eAUDIO_COUTDOWN);
    }

    //---------------------------------------------------
    private void toResult()
    {
        _gameState = eGameState.eGame_toResult;
        int Score_ = ComputeScore(_GraphyMgr.NormalIsland, _GraphyMgr.SpecialIsland);

        //Higher Score
        bool isTopScore_ = (Score_ > _TopScore);
        if (isTopScore_)
        {
            PlayerPrefs.SetInt(constParameter.cPPREF_SCORE, Score_);
            _TopScore = Score_;

            PlayerPrefs.Save();

            _UIAudioMgr.playAudio(UIAudioMgr.eAUDIO_TYPE.eAUDIO_TOP);
        }
        _UIMgr.HideGameText();
        _UIMgr.ShowResult(_GraphyMgr.NormalIsland, _GraphyMgr.SpecialIsland, Score_, isTopScore_);
        StartCoroutine(WaitToResult());
    }

    //---------------------------------------------------
    //Wait
    IEnumerator WaitToTitle()
    {
        yield return new WaitForSeconds(1.0f);
        _UIMgr.ShowTitleUI(_TopScore);

        _gameState = eGameState.eGame_Title;
    }

    //---------------------------------------------------
    IEnumerator WaitToTeach()
    {
        yield return new WaitForSeconds(1.0f);
        _gameState = eGameState.eGame_Teach;
        _UIMgr.ShowTeachPage();
    }

    //---------------------------------------------------
    IEnumerator WaitToGame()
    {
        yield return new WaitForSeconds(1.0f);
        _gameState = eGameState.eGame_Game;
        _GraphyMgr.StartGame();
        _UIMgr.ShowGameText();
        _Timer = 30.0f;

        _UIMgr.HideCountdown();
        _BGMMgr.changeBGM(BGMMgr.eBGM_TYPE.eBGM_GAME);
    }

    //---------------------------------------------------
    IEnumerator WaitToResult()
    {
        yield return new WaitForSeconds(1.0f);
        _gameState = eGameState.eGame_Result;
    }
    #endregion
    
    #region EVENT
    //---------------------------------------------------
    //GraphyMgr Event
    public void onGraphyMgrEvent(string e)
    {
        if (e == "E_NextLevel")
        {
            _UIMgr.setLevel(_GraphyMgr.Level);

            _Timer += 5.0f;
            _UIMgr.showAddScore(5);

            _UIAudioMgr.playAudio(UIAudioMgr.eAUDIO_TYPE.eAUDIO_PASSLEVEL);
        }
        else if (e == "E_GameOver")
        {
            _Timer = 0;
            toResult();
        }
    }

    //---------------------------------------------------
    //UIMgr Event
    public void onStartBtnClick()
    {
        if (_gameState != eGameState.eGame_Title)
        {
            return;
        }
        toTeach();

        _UIAudioMgr.playAudio(UIAudioMgr.eAUDIO_TYPE.eAUDIO_BUTTON);
    }

    //---------------------------------------------------
    public void onGoBtnClick()
    {
        if (_gameState != eGameState.eGame_Teach)
        {
            return;
        }
        toGame();

        _UIAudioMgr.playAudio(UIAudioMgr.eAUDIO_TYPE.eAUDIO_BUTTON);
    }

    //---------------------------------------------------
    public void onGoTitleClick()
    {
        if (_gameState != eGameState.eGame_Result)
        {
            return;
        }
        toTitle();

        _UIAudioMgr.playAudio(UIAudioMgr.eAUDIO_TYPE.eAUDIO_BUTTON);
    }
    #endregion
    
    #endregion

}
