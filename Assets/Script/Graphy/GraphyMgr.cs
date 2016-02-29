using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CTExtension
{
    //---------------------------------------------------
    public static void shuffleList<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, list.Count);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public class GraphyMgr : MonoBehaviour
{
    #region Delegate
    public delegate void onGarphyEvent(string e);
    #endregion

    #region Enum
    public enum eGraphyState
    {
        eGraphy_Ready   =   0
        ,eGraphy_toKey
        ,eGraphy_toTreasure
        ,eGraphy_toMonster
        ,eGraphy_backtoGate
        ,eGraphy_Finish

        ,eGraphy_GameOver
        ,eGraphy_CanReset
    }

    public enum eGraphyLevel
    {
        eGraphy_K4 =   0
        ,eGraphy_K33
        ,eGraphy_Petersen
        ,eGraphy_Heawood
    }
    #endregion    
        
    #region Element
    public GameObject _IBaseP = null;
    public GameObject _IGateP = null;
    public GameObject _IKeyP = null;
    public GameObject _ITreasureP = null;
    public GameObject _IMonsterP = null;
    public GameObject _EBaseP = null;

    public GameObject _PlayerP = null;

    private VertexSet _VertexSet = null;
    private EdgeSet _EdgeSet = null;
    public SelectorCtrl _Selector = null;

    private GraphyData.GraphyData _nowGraphy;

    //Graphy Logic
    public onGarphyEvent _callback;

    private List<int> _IDList = new List<int>();
    protected eGraphyState _eGraphyState = eGraphyState.eGraphy_CanReset;
    private eGraphyLevel _eGraphyLevel = eGraphyLevel.eGraphy_K4;
    
    private int _iNowIslandID = -1;
    private int _iLevelCount = 0;
    private int _iNormalIslandCount = 0;
    private int _iSpecialIslandCount = 0;

    public GameObject _PlayerGO;
    private Player _Player = null;
    private bool _PlayerCanJump = true;

    //Control
    private bool _bStartControl = false;
    private Vector3 _fMousePos;
    
    #endregion

    #region Property
    public int Level { get {return _iLevelCount;} }
    public int NormalIsland { get { return _iNormalIslandCount; } }
    public int SpecialIsland { get { return _iSpecialIslandCount; } }
    #endregion

    #region Basic Method
    //---------------------------------------------------
    void Start()
    {
        _VertexSet = GameObject.Find("VertexSet").GetComponent<VertexSet>();
        _VertexSet.GetComponent<VertexSet>().callback += this.onVertexClear;
        _EdgeSet = GameObject.Find("EdgeSet").GetComponent<EdgeSet>();


        _Player = _PlayerGO.GetComponent<Player>();
        _Player._callback += onPlayerJumpFinish;

        _nowGraphy = new GraphyData.GraphyData();

        ResetGame();
    }

    //---------------------------------------------------
    void Update()
    {
        if(_bStartControl)
        {
            MouseCheck();
        }        
    }

    //---------------------------------------------------

    #endregion

    #region Method

    #region Game Logic
    //---------------------------------------------------
    public void StartGame()
    {
        if (_eGraphyState == eGraphyState.eGraphy_Ready)
        {
            _bStartControl = true;
            _iLevelCount = 1;
            _iNormalIslandCount = 0;
            _iSpecialIslandCount = 0;
            _eGraphyLevel = eGraphyLevel.eGraphy_K4;
            _nowGraphy.generateK4();
            generateGraphy();

            _eGraphyState = eGraphyState.eGraphy_toKey;
        }
    }

    //---------------------------------------------------
    public void ResetGame()
    {
        if (_eGraphyState == eGraphyState.eGraphy_CanReset)
        {
            if (!_PlayerGO.activeInHierarchy)
            {
                _PlayerGO.SetActive(true);
            }
            
            _eGraphyState = eGraphyState.eGraphy_Ready;

            _VertexSet.resetVertex();
        }
    }

    //---------------------------------------------------
    public void GameOver()
    {
        if (_eGraphyState == eGraphyState.eGraphy_GameOver)
        {
            return;
        }
        Debug.Log("[GraphyMgr]Game Over");
        _Selector.disableSelector();
        clearGraphy(true);
        _eGraphyState = eGraphyState.eGraphy_GameOver;
        _bStartControl = false;

        StartCoroutine(waitPlayerFollingdown());
    }

    //---------------------------------------------------
    IEnumerator waitPlayerFollingdown()
    {
        yield return new WaitForSeconds(3.0f);
        _PlayerGO.SetActive(false);
        _eGraphyState = eGraphyState.eGraphy_CanReset;

        _callback("E_GameOver");
    }
    #endregion

    #region Graphy
    //---------------------------------------------------
    //Graphy
    //---------------------------------------------------
    private void updateGraphyLevel()
    {
        if (_iLevelCount == 1 && _eGraphyLevel != eGraphyLevel.eGraphy_K4)
        {
            _eGraphyLevel = eGraphyLevel.eGraphy_K4;
        }
        else if (_iLevelCount == 2 && _eGraphyLevel != eGraphyLevel.eGraphy_K33)
        {
            _eGraphyLevel = eGraphyLevel.eGraphy_K33;
        }
        else if (_iLevelCount > 3 && _iLevelCount < 5 && _eGraphyLevel != eGraphyLevel.eGraphy_Petersen)
        {
            if (Random.Range(-1, 1) > 0)
            {
                _eGraphyLevel = eGraphyLevel.eGraphy_Petersen;
            }
        }
        else if (_iLevelCount == 5 && _eGraphyLevel != eGraphyLevel.eGraphy_Petersen)
        {
            _eGraphyLevel = eGraphyLevel.eGraphy_Petersen;
        }
        else if (_iLevelCount > 7 && _iLevelCount < 10 && _eGraphyLevel != eGraphyLevel.eGraphy_Heawood)
        {
            if (Random.Range(-1, 1) > 0)
            {
                _eGraphyLevel = eGraphyLevel.eGraphy_Heawood;
            }
        }
        else if (_iLevelCount == 10 && _eGraphyLevel != eGraphyLevel.eGraphy_Heawood)
        {
            _eGraphyLevel = eGraphyLevel.eGraphy_Heawood;
        }
    }

    //---------------------------------------------------
    private void nextLevel()
    {
        _iLevelCount++;
        _callback("E_NextLevel");
        updateGraphyLevel();
        switch (_eGraphyLevel)
        {
            case eGraphyLevel.eGraphy_K4:
                {
                    _nowGraphy.generateK4();
                }
                break;
            case eGraphyLevel.eGraphy_K33:
                {
                    _nowGraphy.generateK33();
                }
                break;
            case eGraphyLevel.eGraphy_Petersen:
                {
                    _nowGraphy.generatePetersen();
                }
                break;
            case eGraphyLevel.eGraphy_Heawood:
                {
                    _nowGraphy.generateHeawood();
                }
                break;
        }

        _eGraphyState = eGraphyState.eGraphy_toKey;
        generateGraphy();
    }

    //---------------------------------------------------
    private void generateGraphy()
    {
        //Initial id and position
        List<Vector3> _PosList = new List<Vector3>();
        float fUnitDegree_ = (360.0f) / (_nowGraphy.VertexNum);
        Vector3 vec_ = new Vector3(.0f, .0f, 1.0f);

        _IDList.Clear();
        for (int idx_ = 0; idx_ < _nowGraphy.VertexNum; idx_++)
        {
            Vector3 pos_ = vec_ * Random.Range(2.0f, 6.0f);
            _IDList.Add(idx_);
            _PosList.Add(pos_);

            vec_ = (Quaternion.AngleAxis(fUnitDegree_, Vector3.up) * vec_).normalized;
        }
        _IDList.shuffleList();
        _PosList.shuffleList();

        //Create Vertex
        for (int idx_ = 0; idx_ < _nowGraphy.VertexNum; idx_++)
        {
            var eType_ = GraphyData.eVertexType.eVertex_Normal;
            switch (idx_)
            {
                case 0:
                    {
                        eType_ = GraphyData.eVertexType.eVertex_Order1;
                        break;
                    }
                case 1:
                    {
                        eType_ = GraphyData.eVertexType.eVertex_Order2;
                        break;
                    }
                case 2:
                    {
                        eType_ = GraphyData.eVertexType.eVertex_Order3;
                        break;
                    }
                case 3:
                    {
                        eType_ = GraphyData.eVertexType.eVertex_Order4;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            _VertexSet.setVertex(eType_, _IDList[idx_], _PosList[idx_]);

        }

        //Create Edge
        for (int fromID_ = 0; fromID_ < _nowGraphy.VertexNum; fromID_++)
        {
            foreach (var toID_ in _nowGraphy._EdgeMap[fromID_])
            {
                if (fromID_ >= toID_)
                {
                    continue;
                }
                var From_ = _VertexSet.getVertexByID(fromID_);
                var To_ = _VertexSet.getVertexByID(toID_);
                _EdgeSet.setEdge(From_, To_);
            }
        }

        //Initial element
        _iNowIslandID = _IDList[0];
        hightlightEdge(true, _iNowIslandID);

    }

    //---------------------------------------------------
    private void clearGraphy(bool includeGate = false)
    {
        _EdgeSet.resetEdge();
        _VertexSet.clearVertex(includeGate);

        _nowGraphy.clearGraphy();
    }

    //---------------------------------------------------
    private void countIslandNum(GraphyData.eVertexType type)
    {
        switch (type)
        {
            case GraphyData.eVertexType.eVertex_Normal:
            {
                _iNormalIslandCount++;
                break;
            }
            default:
            {
                _iSpecialIslandCount++;
                break;
            }
        }
    }


    #endregion //Graphy

    #region Graphy Logic
    //---------------------------------------------------
    //Graphy Logic
    private void IslandCheck(GameObject island)
    {
        if (island.tag != "Island")
        {
            //This is not island
            return;
        }

        var IslandScript_ = island.GetComponent<Island>();
        if (!IsIslandCanClick(IslandScript_))
        {
            _Player.CanNotJump();
            Debug.Log("[IslandCheck]Can't click this island");
            return;
        }
        checkGraphyState(IslandScript_.eType);

        if (_eGraphyState != eGraphyState.eGraphy_Finish)
        {
            var IslandNowScript_ = _VertexSet.getVertexByID(_iNowIslandID).GetComponent<Island>();
            if (IslandNowScript_.eType != GraphyData.eVertexType.eVertex_Order1)
            {
                //Game Object
                _EdgeSet.removeEdges(_iNowIslandID);
                _VertexSet.removeVertex(_iNowIslandID);

                //Graphy
                _nowGraphy.removeVertex(_iNowIslandID);
            }
            else
            {
                //Game Object
                _EdgeSet.removeEdge(_iNowIslandID, IslandScript_.iID);

                //Graphy
                _nowGraphy.removeEdge(_iNowIslandID, IslandScript_.iID);
            }

            hightlightEdge(false, _iNowIslandID);
            _iNowIslandID = IslandScript_.iID;
            hightlightEdge(true, _iNowIslandID);

            countIslandNum(IslandScript_.eType);
        }

        //Player
        if (IslandScript_.eType == GraphyData.eVertexType.eVertex_Order1)
        {
            //Gate
            _Player.JumpTo(island.transform.position);
        }
        else
        {
            //Get the float position
            _Player.JumpTo(island.GetComponent<IslandFloat>().getPosition(constParameter.cPLAYER_JUMP_DURATION));
        }
        _PlayerCanJump = false;
    }

    //---------------------------------------------------
    private bool IsIslandCanClick(Island _island)
    {
        if (_island.iID == -1)
        {
            Debug.Log("[IsIslandCanClick]This island id failed : " + _island.iID);
            return false;
        }

        var edgeIDList_ = _nowGraphy._EdgeMap[_iNowIslandID];
        
        bool bResult_ = false;
        foreach (var Iter_ in edgeIDList_)
        {
            if (Iter_ == _island.iID)
            {
                switch (_eGraphyState)
                {
                    case eGraphyState.eGraphy_toKey:
                        {
                            bResult_ = (_island.eType == GraphyData.eVertexType.eVertex_Normal || _island.eType == GraphyData.eVertexType.eVertex_Order2);
                            break;
                        }
                    case eGraphyState.eGraphy_toTreasure:
                        {
                            bResult_ = (_island.eType == GraphyData.eVertexType.eVertex_Normal || _island.eType == GraphyData.eVertexType.eVertex_Order3);
                            break;
                        }
                    case eGraphyState.eGraphy_toMonster:
                        {
                            bResult_ = (_island.eType == GraphyData.eVertexType.eVertex_Normal || _island.eType == GraphyData.eVertexType.eVertex_Order4);
                            break;
                        }
                    case eGraphyState.eGraphy_backtoGate:
                        {
                            bResult_ = (_island.eType == GraphyData.eVertexType.eVertex_Normal || _island.eType == GraphyData.eVertexType.eVertex_Order1);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
        return bResult_;
    }

    //---------------------------------------------------
    private void checkGraphyState(GraphyData.eVertexType eType)
    {
        switch(eType)
        {
            case GraphyData.eVertexType.eVertex_Order1:
                {
                    if (_eGraphyState == eGraphyState.eGraphy_backtoGate)
                    {
                        _eGraphyState = eGraphyState.eGraphy_Finish;
                    }
                }
                break;
            case GraphyData.eVertexType.eVertex_Order2:
                {
                    if (_eGraphyState == eGraphyState.eGraphy_toKey)
                    {
                        _eGraphyState = eGraphyState.eGraphy_toTreasure;
                    }
                }
                break;
            case GraphyData.eVertexType.eVertex_Order3:
                {
                    if (_eGraphyState == eGraphyState.eGraphy_toTreasure)
                    {
                        _eGraphyState = eGraphyState.eGraphy_toMonster;
                    }
                }
                break;
            case GraphyData.eVertexType.eVertex_Order4:
                {
                    if (_eGraphyState == eGraphyState.eGraphy_toMonster)
                    {
                        _eGraphyState = eGraphyState.eGraphy_backtoGate;
                    }
                }
                break;
        }
        //Debug.Log("[checkGraphyState]Change graphy state to :" + _eGraphyState);
    }

    //---------------------------------------------------
    private void gameoverCheck()
    {
        if (_eGraphyState == eGraphyState.eGraphy_GameOver || _eGraphyState == eGraphyState.eGraphy_Finish || _eGraphyState == eGraphyState.eGraphy_Ready)
        {
            return;
        }

        ////Check 4-ordered islands
        //for (int idx_ = 0; idx_ < 4; idx_++)
        //{
        //    List<int>   islandEdge_;
        //    if (_nowGraphy._EdgeMap.TryGetValue(_IDList[idx_], out islandEdge_))
        //    {
        //        if (islandEdge_.Count <= 0)
        //        {
        //            GameOver();
        //            return;
        //        }
        //    }
        //}

        //Check now island
        var edgeList_ = _nowGraphy._EdgeMap[_iNowIslandID];
        if (edgeList_.Count <= 0)
        {
            GameOver();
            return;
        }
        else
        {
            bool bResult_ = false;
            foreach (var island_ in edgeList_)
            {
                bResult_ |= IsIslandCanClick(_VertexSet.getVertexByID(island_).GetComponent<Island>());
            }

            if (!bResult_)
            {
                GameOver();
                return;
            }
        }
    }

    //---------------------------------------------------
    private void roundFinish()
    {
        if (_eGraphyState != eGraphyState.eGraphy_Finish)
        {
            return;
        }
        clearGraphy();
    }

    //---------------------------------------------------
    private void onVertexClear()
    {
        if (_eGraphyState != eGraphyState.eGraphy_GameOver)
        {
            Debug.Log("[GraphyMgr]next level!!");
            nextLevel();
        }
    }

    //---------------------------------------------------
    private void hightlightEdge(bool bSetHightlight, int id)
    {
        var edgeSet_ = _EdgeSet.getEdges(id);

        foreach (var edgeIter_ in edgeSet_)
        {
            if (edgeIter_.activeInHierarchy)
            {
                var line_ = edgeIter_.GetComponent<BaseLine>();
                line_.setHightlight(bSetHightlight);
            }
        }
    }

    #endregion //Graphy Logic

    #region Player
    //---------------------------------------------------
    private void onPlayerJumpFinish()
    {
        _PlayerCanJump = true;

        if (_eGraphyState == eGraphyState.eGraphy_Finish)
        {
            roundFinish();
        }
        else
        {
            gameoverCheck();
        }
    }

    #endregion

    #region Control
    //---------------------------------------------------
    //Control
    private void MouseCheck()
    {
        RaycastHit hit_ = new RaycastHit();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit_))
        {
            GameObject hitObj_ = hit_.transform.gameObject;

            if (hitObj_.tag != "Island")
            {
                return;
            }

            _Selector.enableSelector(hitObj_.transform.position);

            //Check mouse click
            if (Input.GetMouseButtonDown(0))
            {
                _fMousePos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                float fMouseDelta_ = Vector3.Distance(_fMousePos, Input.mousePosition);
                if (fMouseDelta_ < constParameter.cMOUSE_TRIGGER_DIST && _PlayerCanJump)
                {
                    IslandCheck(hitObj_);
                }
            }
        }
        else
        _Selector.disableSelector();
        
    }
    #endregion //Control

    #endregion //Method
}
