using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EdgeSet : MonoBehaviour {

    #region Element
    public GameObject _EBaseP = null;

    private List<GameObject> _EdgePool = new List<GameObject>();
    private Dictionary<int, List<GameObject>> _EdgeMap = new Dictionary<int, List<GameObject>>();

    private int _iEdgePoolSize = 0;
    #endregion

    #region Basic Method
    void Start()
    {
        //Initial
        for (int idx_ = 0; idx_ < constParameter.cEDGE_NUM_INIT; idx_++)
        {
            GameObject _newEdge = Instantiate(_EBaseP);
            _newEdge.transform.parent = transform;

            _EdgePool.Add(_newEdge);
        }

        _iEdgePoolSize = _EdgePool.Count;
    }
    #endregion

    #region Method
    //---------------------------------------------------
    public bool setEdge(GameObject from, GameObject to)
    {
        if (from == null || to == null)
        {
            Debug.LogError("[EdgeSet]setEdge vertex is null");
            return false;
        }
        
        int idFrom_ = from.GetComponent<Island>().iID;
        int idTo_ = to.GetComponent<Island>().iID;

        var newEdge_ = getEdgeFromPool();
        
        if (newEdge_)
        {
            newEdge_.SetActive(true);
            var EdgeScript_ = newEdge_.GetComponent<BaseLine>();
            EdgeScript_.id1 = idFrom_;
            EdgeScript_.id2 = idTo_;
            EdgeScript_.v1 = from;
            EdgeScript_.v2 = to;

            //newEdge_.name = "Edge_" + idFrom_ + "_" + idTo_;
            if (!_EdgeMap.ContainsKey(idFrom_))
            {
                _EdgeMap.Add(idFrom_, new List<GameObject>());
            }

            if (!_EdgeMap.ContainsKey(idTo_))
            {
                _EdgeMap.Add(idTo_, new List<GameObject>());
            }

            _EdgeMap[idFrom_].Add(newEdge_);
            _EdgeMap[idTo_].Add(newEdge_);
            return true;
        }
        else
        {
            Debug.LogError("[EdgeSet]setEdge failed");
            return false;
        }
    }

    //---------------------------------------------------
    public void removeEdges(int id)
    {
        List<GameObject> EdgeList_;

        if (_EdgeMap.TryGetValue(id, out EdgeList_))
        {
            foreach (var edge_ in EdgeList_)
            {
                edge_.SetActive(false);
                _iEdgePoolSize++;
            }
        }
    }

    //---------------------------------------------------
    public void removeEdge(int from, int to)
    {
        List<GameObject> EdgeList_;

        if (_EdgeMap.TryGetValue(from, out EdgeList_))
        {
            foreach (var edge_ in EdgeList_)
            {
                var edgeScript_ = edge_.GetComponent<BaseLine>();
                if (edgeScript_.id1 == to || edgeScript_.id2 == to)
                {
                    edge_.SetActive(false);
                    _iEdgePoolSize++;
                    break;
                }
            }
        }
    }

    //---------------------------------------------------
    private GameObject getEdgeFromPool()
    {
        if (_iEdgePoolSize == 0)
        {
            extandEdgePool();
        }

        GameObject edge_ = null;
        foreach (var Iter_ in _EdgePool)
        {
            if (!Iter_.activeInHierarchy)
            {
                edge_ = Iter_;
            }
        }

        return edge_;
    }

    //---------------------------------------------------
    private void extandEdgePool()
    {
        var edge_ = Instantiate(_EBaseP);
        edge_.transform.parent = transform;

        _EdgePool.Add(edge_);
        _iEdgePoolSize++;
    }

    //---------------------------------------------------
    public void resetEdge()
    {
        foreach (var edge_ in _EdgePool)
        {
            edge_.SetActive(false);
        }
        _iEdgePoolSize = _EdgePool.Count;

        _EdgeMap.Clear();
    }
    #endregion
}
