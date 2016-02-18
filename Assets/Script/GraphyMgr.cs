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
    #region Enum
    public enum eGraphyState
    {
        eGraphy_Ready   =   0
        ,eGraphy_toKey
        ,eGraphy_toTreasure
        ,eGraphy_toMonster
        ,eGraphy_backtoGate
        ,eGraphy_Finish

        ,eGraph_GameOver
    }
    #endregion    
    
    #region Const Parameter
    private static readonly float cMOUSE_TRIGGER_DIST = 20.0f;
    #endregion

    
    #region Element
    public GameObject _IBaseP = null;
    public GameObject _IGateP = null;
    public GameObject _IKeyP = null;
    public GameObject _ITreasureP = null;
    public GameObject _IMonsterP = null;
    public GameObject _EBaseP = null;

    private GameObject _VertexSet = null;
    private GameObject _EdgeSet = null;

    
    private Dictionary<int, GameObject> _islandMap = new Dictionary<int,GameObject>();
    private Dictionary<int, List<GameObject>> _edgeMap = new Dictionary<int, List<GameObject>>();
    private GraphyData.GraphyData _nowGraphy;

    //Graphy Logic
    protected eGraphyState _eGraphyState = eGraphyState.eGraphy_Ready;
    private int _iNowIslandID = -1;

    //Control
    private Vector3 _fMousePos;
    
    #endregion

    #region Basic Method
    //---------------------------------------------------
    void Start()
    {
        _VertexSet = GameObject.Find("VertexSet");
        _EdgeSet = GameObject.Find("EdgeSet");

        _nowGraphy = new GraphyData.GraphyData();
        _nowGraphy.generalK33();
        resetGraphy();
    }

    //---------------------------------------------------
    void Update()
    {
        MouseCheck();
    }
    #endregion

    #region Method

    #region Graphy
    //---------------------------------------------------
    //Graphy
    private void resetGraphy()
    {
        clearGraphy();

        //Create Vertex
        List<int> indexList_ = new List<int>();
        for (int idx_ = 0; idx_ < _nowGraphy.VertexNum; idx_++)
        {
            indexList_.Add(idx_);
        }

        indexList_.shuffleList();
        
        for (int idx_ = 0; idx_ < _nowGraphy.VertexNum; idx_++)
        {
            GameObject newIsland_;
            switch (idx_)
            {
                case 0:
                    {
                        newIsland_ = Instantiate(_IGateP);
                        newIsland_.GetComponent<Island>().eType = GraphyData.eVertexType.eVertex_Order1;
                        
                        break;
                    }
                case 1:
                    {
                        newIsland_ = Instantiate(_IKeyP);
                        newIsland_.GetComponent<Island>().eType = GraphyData.eVertexType.eVertex_Order2;
                        break;
                    }
                case 2:
                    {
                        newIsland_ = Instantiate(_ITreasureP);
                        newIsland_.GetComponent<Island>().eType = GraphyData.eVertexType.eVertex_Order3;
                        break;
                    }
                case 3:
                    {
                        newIsland_ = Instantiate(_IMonsterP);
                        newIsland_.GetComponent<Island>().eType = GraphyData.eVertexType.eVertex_Order4;
                        break;
                    }
                default:
                    {
                        newIsland_ = Instantiate(_IBaseP);
                        newIsland_.GetComponent<Island>().eType = GraphyData.eVertexType.eVertex_Normal;
                        break;
                    }
            }
            newIsland_.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-3, 3), Random.Range(-5, 5));
            newIsland_.transform.parent = _VertexSet.transform;
            newIsland_.GetComponent<Island>().iID = indexList_[idx_];
            newIsland_.name = "Vertex_" + indexList_[idx_];

            _islandMap.Add(indexList_[idx_], newIsland_);
        }

        //Create Edge
        for(int idx_ = 0; idx_ < _nowGraphy.VertexNum; idx_++)
        {
            List<GameObject> EdgeList_ = new List<GameObject>();
            _edgeMap.Add(idx_, EdgeList_);
        }

        for (int idx_ = 0; idx_ < _nowGraphy.VertexNum; idx_++)
        {
            foreach (var toID_ in _nowGraphy._EdgeMap[idx_])
            {
                if (idx_ >= toID_)
                {
                    continue;
                }

                var newEdge_ = Instantiate(_EBaseP);
                var edgeScript_ = newEdge_.GetComponent<BaseLine>();
                edgeScript_.id1 = idx_;
                edgeScript_.id2 = toID_;
                edgeScript_.v1 = _islandMap[idx_];
                edgeScript_.v2 = _islandMap[toID_];
                newEdge_.transform.parent = _EdgeSet.transform;
                newEdge_.name = "Edge_" + idx_ + "_" + toID_;

                _edgeMap[idx_].Add(newEdge_);
                _edgeMap[toID_].Add(newEdge_);
            }
        }

        //Initial element
        _eGraphyState = eGraphyState.eGraphy_toKey;
        _iNowIslandID = indexList_[0];
        
    }

    //---------------------------------------------------
    private void clearGraphy()
    {
        //Clear vertex
        if (_islandMap.Count > 0)
        {
            foreach (var Iter_ in _islandMap)
            {
                Destroy(Iter_.Value);
            }
        }
        _islandMap.Clear();

    }

    //---------------------------------------------------
    private void disableVertex(int id)
    {
        //disable GameObject
        GameObject vertex_;
        
        if (_islandMap.TryGetValue(id, out vertex_))
        {
            vertex_.SetActive(false);
        }
    }

    //---------------------------------------------------
    private void disableEdge(int from, int to)
    {
        List<GameObject> EdgeList_;

        if (_edgeMap.TryGetValue(from, out EdgeList_))
        {
            foreach (var edge_ in EdgeList_)
            {
                var edgeScript_ = edge_.GetComponent<BaseLine>();
                if (edgeScript_.id1 == to || edgeScript_.id2 == to)
                {
                    edge_.SetActive(false);
                    break;
                }                
            }
        }
    }

    //---------------------------------------------------
    private void disableEdges(int id)
    {
        List<GameObject> EdgeList_;
        
        if (_edgeMap.TryGetValue(id, out EdgeList_))
        {
            foreach (var edge_ in EdgeList_)
            {
                edge_.SetActive(false);
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
        if(!IsIslandCanClick(IslandScript_.iID, IslandScript_.eType))
        {
            Debug.Log("[IslandCheck]Can't click this island");
            return;
        }

        var IslandNowScript_ = _islandMap[_iNowIslandID].GetComponent<Island>();
        if (IslandNowScript_.eType != GraphyData.eVertexType.eVertex_Order1)
        {
            disableEdges(_iNowIslandID);
            disableVertex(_iNowIslandID);
            _nowGraphy.removeVertex(_iNowIslandID);
        }
        else
        {
            disableEdge(_iNowIslandID, IslandScript_.iID);
            _nowGraphy.removeEdge(_iNowIslandID, IslandScript_.iID);
        }

        checkGraphyState(IslandScript_.eType);
        _iNowIslandID = IslandScript_.iID;
    }

    //---------------------------------------------------
    private bool IsIslandCanClick(int id, GraphyData.eVertexType eType)
    {
        if (id == -1 || id >= _islandMap.Count)
        {
            Debug.Log("[IsIslandCanClick]This island id failed : " + id);
            return false;
        }

        var edgeIDList_ = _nowGraphy._EdgeMap[_iNowIslandID];
        
        bool bResult_ = false;
        foreach (var Iter_ in edgeIDList_)
        {
            if (Iter_ == id)
            {
                switch (_eGraphyState)
                {
                    case eGraphyState.eGraphy_Ready:
                    case eGraphyState.eGraphy_toKey:
                        {
                            bResult_ = (eType == GraphyData.eVertexType.eVertex_Normal || eType == GraphyData.eVertexType.eVertex_Order2);
                            break;
                        }
                    case eGraphyState.eGraphy_toTreasure:
                        {
                            bResult_ = (eType == GraphyData.eVertexType.eVertex_Normal || eType == GraphyData.eVertexType.eVertex_Order3);
                            break;
                        }
                    case eGraphyState.eGraphy_toMonster:
                        {
                            bResult_ = (eType == GraphyData.eVertexType.eVertex_Normal || eType == GraphyData.eVertexType.eVertex_Order4);
                            break;
                        }
                    case eGraphyState.eGraphy_backtoGate:
                        {
                            bResult_ = (eType == GraphyData.eVertexType.eVertex_Normal || eType == GraphyData.eVertexType.eVertex_Order1);
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
        Debug.Log("[checkGraphyState]Change graphy state to :" + _eGraphyState);
    }
    #endregion //Graphy Logic

    #region Control
    //---------------------------------------------------
    //Control
    private void MouseCheck()
    {
        RaycastHit hit_ = new RaycastHit();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit_))
        {
            GameObject hitObj_ = hit_.transform.gameObject;

            //MouseOver
            hitObj_.GetComponent<HMouseOver>().MouseOver();

            //Check mouse click
            if (Input.GetMouseButtonDown(0))
            {
                _fMousePos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                float fMouseDelta_ = Vector3.Distance(_fMousePos, Input.mousePosition);
                if (fMouseDelta_ < cMOUSE_TRIGGER_DIST)
                {
                    IslandCheck(hitObj_);
                }
            }            
        }
    }
    #endregion //Control

    #endregion //Method
}
