using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VertexSet : MonoBehaviour
{
    public delegate void onVertexClear();

    #region Element
    public onVertexClear callback;
    private bool _checkClear = false;

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
    void Awake()
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

        //resetVertex();
    }

    void Update()
    {
        if (_checkClear)
        {
            if (checkVertexClear())
            {
                callback();
                _checkClear = false;
            }
        }
    }
    #endregion

    #region Method
    //---------------------------------------------------
    public void resetVertex()
    {
        _VertexGate.SetActive(true);
        _VertexGate.GetComponent<IslandMove>().Enter(
            () =>
            {
                _VertexGate.transform.localRotation = new Quaternion();
            }
        );
        _VertexGate.transform.position = default(Vector3);
    }
    
    //---------------------------------------------------
    public bool setVertex(GraphyData.eVertexType eType, int id, Vector3 pos = default(Vector3))
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
            vertex_.GetComponent<Island>().eType = eType;

            if (pos != default(Vector3) && eType != GraphyData.eVertexType.eVertex_Order1)
            {
                vertex_.GetComponent<IslandMove>().Enter(
                    () =>
                    {
                        vertex_.transform.localRotation = new Quaternion();
                    }
                );
                vertex_.transform.position = pos;
            }

            vertex_.name = "Vertex_" + id;

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
            vertex_.GetComponent<IslandMove>().Exit(
                () => {onVertexExit(vertex_);}
            );
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
    public void clearVertex(bool includeGate = false)
    {
        foreach (var vertex_ in _VertexPool)
        {
            if (vertex_.activeInHierarchy)
            {
                removeVertex(vertex_.GetComponent<Island>().iID);
            }
        }

        if (_VertexKey.activeInHierarchy)
        {
            _VertexKey.GetComponent<IslandMove>().Exit(
                () =>{onVertexExit(_VertexKey);}
            );
        }
        if (_VertexTreasure.activeInHierarchy)
        {
            _VertexTreasure.GetComponent<IslandMove>().Exit(
                () => { onVertexExit(_VertexTreasure); }
            );
        }
        if (_VertexMonster.activeInHierarchy)
        {
            _VertexMonster.GetComponent<IslandMove>().Exit(
                () => { onVertexExit(_VertexMonster); }
            );
        }

        if (includeGate)
        {
            _VertexGate.GetComponent<IslandMove>().Exit(
                () => { onVertexExit(_VertexGate); }
            );
        }

        _VertexMap.Clear();
        _checkClear = true;
    }
    
    //---------------------------------------------------
    private bool checkVertexClear()
    {
        bool result_ = true;
        //Check map
        result_ &= (_VertexMap.Count == 0);

        //Check key, treasure, and monster
        result_ &= (!_VertexKey.activeInHierarchy);
        result_ &= (!_VertexTreasure.activeInHierarchy);
        result_ &= (!_VertexMonster.activeInHierarchy);

        if (!result_)
        {
            return result_;
        }

        //Check Vertex Pool
        foreach (var vertex_ in _VertexPool)
        {
            if (vertex_.activeInHierarchy)
            {
                result_ = false;
                break;
            }
        }
        return result_;
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

    //---------------------------------------------------
    private void onVertexExit(GameObject vertex)
    {
        vertex.SetActive(false);
        
        if (vertex.GetComponent<Island>().eType == GraphyData.eVertexType.eVertex_Normal)
        {
            _iVertexPoolSize++;
        }
    }
    #endregion

}
