using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VertexSet : MonoBehaviour
{    
    #region Element
    public GameObject _IBaseP = null;
    public GameObject _IGateP = null;
    public GameObject _IKeyP = null;
    public GameObject _ITreasureP = null;
    public GameObject _IMonsterP = null;

    private GameObject _VertexGate = null; //Always enable
    private GameObject _VertexKey = null;
    private GameObject _VertexTreasure = null;
    private GameObject _VertexMonster = null;
    private List<GameObject> _VertexPool = new List<GameObject>();
    private Dictionary<int, GameObject> _VertexMap = new Dictionary<int, GameObject>();

    private int _iVertexPoolSize = 0;
    #endregion

    #region Property
    public int PoolSize
    {
        get { return _iVertexPoolSize; }
    }
    #endregion

    #region Basic Method
    void Start()
    {
        //Initial
        _VertexGate = Instantiate(_IGateP);
        _VertexGate.transform.parent = transform;

        _VertexKey = Instantiate(_IKeyP);
        _VertexKey.transform.parent = transform;

        _VertexTreasure = Instantiate(_ITreasureP);
        _VertexTreasure.transform.parent = transform;

        _VertexMonster = Instantiate(_IMonsterP);
        _VertexMonster.transform.parent = transform;

        for (int idx_ = 0; idx_ < constParameter.cVERTEX_NUM_INIT - 4; idx_++)
        {
            GameObject _newVertex = Instantiate(_IBaseP);
            _newVertex.transform.parent = transform;

            //_VertexPool
            _VertexPool.Add(_newVertex);
        }

        _iVertexPoolSize = _VertexPool.Count;
    }
    #endregion

    #region Method
    //---------------------------------------------------
    public bool setVertex(GraphyData.eVertexType eType, int id)
    {
        GameObject vertex_ = null;
        switch (eType)
        {
            case GraphyData.eVertexType.eVertex_Normal:
                {
                    vertex_ = getVertexFromPool();
                    break;
                }
            case GraphyData.eVertexType.eVertex_Order1:
                {
                    vertex_ = _VertexGate;
                    break;
                }
            case GraphyData.eVertexType.eVertex_Order2:
                {
                    if (_VertexKey.activeInHierarchy)
                    {
                        Debug.LogWarning("[VertexSet]Key Vertex has been set");
                        return false;
                    }
                    vertex_ = _VertexKey;
                    break;
                }
            case GraphyData.eVertexType.eVertex_Order3:
                {
                    if (_VertexTreasure.activeInHierarchy)
                    {
                        Debug.LogWarning("[VertexSet]Treasure Vertex has been set");
                        return false;
                    }
                    vertex_ = _VertexTreasure;
                    break;
                }
            case GraphyData.eVertexType.eVertex_Order4:
                {
                    if (_VertexMonster.activeInHierarchy)
                    {
                        Debug.LogWarning("[VertexSet]Monster Vertex has been set");
                        return false;
                    }
                    vertex_ = _VertexMonster;
                    break;
                }
        }

        if (vertex_)
        {
            vertex_.SetActive(true);
            vertex_.GetComponent<Island>().iID = id;
            //vertex_.name = "Vertex_" + id;

            if (_VertexMap.ContainsKey(id))
            {
                _VertexMap[id] = vertex_;
            }
            else
            {
                _VertexMap.Add(id, vertex_);
            }
            
            return true;
        }
        else
        {
            Debug.LogError("[VertexSet]setVertex failed");
            return false;
        }        
    }
    
    //---------------------------------------------------
    public void removeVertex(int id)
    {
        GameObject vertex_ = null;
        if (_VertexMap.TryGetValue(id, out vertex_))
        {  
            vertex_.SetActive(false);
            vertex_.GetComponent<Island>().iID = -1;
            if (vertex_.GetComponent<Island>().eType == GraphyData.eVertexType.eVertex_Normal)
            {
                _iVertexPoolSize++;
            }
            //_VertexMap.Remove(id);
        }
    }

    //---------------------------------------------------
    private GameObject getVertexFromPool()
    {
        if (_iVertexPoolSize == 0)
        {
            extandVertexPool();
        }

        GameObject vertex_ = null;
        foreach (var Iter_ in _VertexPool)
        {
            if (!Iter_.activeInHierarchy)
            {
                vertex_ = Iter_;
            }
        }

        if (vertex_ == null)
        {
            Debug.LogWarning("[VertexSet]getVertexFromPool null");
        }

        _iVertexPoolSize--;
        return vertex_;
    }

    //---------------------------------------------------
    private void extandVertexPool()
    {
        var vertex_ = Instantiate(_IBaseP);
        vertex_.transform.parent = transform;

        //_VertexPool
        _VertexPool.Add(vertex_);
        _iVertexPoolSize++;
    }

    //---------------------------------------------------
    public void resetVertex()
    {
        foreach (var vertex_ in _VertexPool)
        {
            if (vertex_.activeInHierarchy)
            {
                vertex_.SetActive(false);
            }
        }
        _iVertexPoolSize = _VertexPool.Count;

        if (_VertexKey.activeInHierarchy)
        {
            _VertexKey.SetActive(false);
        }
        if (_VertexTreasure.activeInHierarchy)
        {
            _VertexTreasure.SetActive(false);
        }
        if (_VertexMonster.activeInHierarchy)
        {
            _VertexMonster.SetActive(false);
        }

        _VertexMap.Clear();
    }
    //---------------------------------------------------
    public GameObject getVertexByID(int id)
    {
        GameObject vertex_ = null;
        if (!_VertexMap.TryGetValue(id, out vertex_))
        {
            Debug.LogWarning("[VertexSet]getVertexByID null");
        }

        return vertex_;
    }


    #endregion

}
