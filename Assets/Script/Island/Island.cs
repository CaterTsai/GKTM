using UnityEngine;
using System.Collections;
using GraphyData;

public class Island : MonoBehaviour
{
    #region Element
    public bool _isStatic = false;
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
    //---------------------------------------------------
    void Start()
    {
        fixedUV();
    }

    //---------------------------------------------------    
    void Update()
    {
    }

    //---------------------------------------------------    
    void OnEnable()
    {
        init();
    }

    //---------------------------------------------------    
    void OnDisable()
    {
        name = "Vertex_Disable";
    }

    #endregion

    #region Method
    //---------------------------------------------------
    private void init()
    {
        if (_isStatic)
        {
            transform.position = new Vector3(.0f, .0f, .0f);
        }
        else
        {
            transform.position = new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), Random.Range(-5f, 5f));
            transform.localScale = new Vector3(.0f, .0f, .0f);
        }
        transform.localRotation = new Quaternion();
        _iID = -1;
    }

    //---------------------------------------------------
    private void fixedUV()
    {
        var mf = GetComponent<MeshFilter>();
        var mesh = GetComponent<Mesh>();
        if (mf != null)
            mesh = mf.mesh;

        if (mesh == null || mesh.uv.Length != 24)
        {
            Debug.Log("Script needs to be attached to built-in cube");
            return;
        }

        var uvs = mesh.uv;

        // Back
        uvs[7].Set(0.0f, 0.0f);
        uvs[6].Set(1.0f, 0.0f);
        uvs[11].Set(0.0f, 1.0f);
        uvs[10].Set(1.0f, 1.0f);

        mesh.uv = uvs;
    }
    #endregion
}
