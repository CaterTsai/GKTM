using UnityEngine;
using System.Collections;
using GraphyData;

public class Island : MonoBehaviour
{
    #region Element
    private GraphyData.eVertexType _eType = GraphyData.eVertexType.eVertex_Normal;
    private int _iID = -1;
    #endregion

    #region Proproty
    public GraphyData.eVertexType eType
    {
        get {return _eType;}
        set { _eType = value; }
    }

    public int iID
    {
        get { return _iID; }
        set { _iID = value; }
    }
    #endregion

    #region Basic Method
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Method
    //---------------------------------------------------

    #endregion
}
