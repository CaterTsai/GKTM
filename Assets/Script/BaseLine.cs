using UnityEngine;
using System.Collections;

public class BaseLine : MonoBehaviour {

    private GameObject _v1, _v2;
    private int _id1, _id2;

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
	
	// Update is called once per frame
	void Update () {
        var lineRenderer_ = GetComponent<LineRenderer>();

        lineRenderer_.SetPosition(0, _v1.transform.position);
        lineRenderer_.SetPosition(1, _v2.transform.position);
	}
}
