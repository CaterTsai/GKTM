using UnityEngine;
using System.Collections;

public class BaseLine : MonoBehaviour {

    private GameObject _v1, _v2;
    private int _id1, _id2;

    private Color normalColor_ = new Color(.0f, 1.0f, .5f, .5f);
    private Color hightlightColor_ = new Color(.5f, 1.0f, .0f, 1.0f);

    private LineRenderer _render = null;
    public GameObject v1
    {
        get { return this._v1; }
        set { this._v1 = value; }
    }

    public GameObject v2
    {
        get { return this._v2; }
        set { this._v2 = value; }
    }

    public int id1
    {
        get { return this._id1; }
        set { this._id1 = value; }
    }

    public int id2
    {
        get { return this._id2; }
        set { this._id2 = value; }
    }

    //---------------------------------------------------    
    void Awake()
    {
        if (_render == null)
        {
            _render = GetComponent<LineRenderer>();
        }
        init();
    }

    //---------------------------------------------------    
	void Update () 
    {
        if (_v1 != null || _v2 != null)
        {
            _render.SetPosition(0, _v1.transform.position);
            _render.SetPosition(1, _v2.transform.position);
        }
	}

    //---------------------------------------------------    
    void OnEnable()
    {
        init();
    }

    //---------------------------------------------------    
    void OnDisable()
    {
        name = "Edge_Disable";
    }

    private void init()
    {
        _v1 = _v2 = null;
        _id1 = _id2 = -1;
        _render.SetPosition(0, new Vector3(0, 0, 0));
        _render.SetPosition(1, new Vector3(0, 0, 0));
        _render.SetColors(normalColor_, normalColor_);

    }

    public void setHightlight(bool val)
    {
        if (val)
        {
            _render.SetColors(hightlightColor_, hightlightColor_);
        }
        else
        {
            _render.SetColors(normalColor_, normalColor_);
        }
    }
}
