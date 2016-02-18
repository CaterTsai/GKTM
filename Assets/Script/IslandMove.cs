using UnityEngine;
using System.Collections;


public class IslandMove : MonoBehaviour
{
    private float _startY   =   0;
    private float _radius = 2.0f;
    private double _rVec = Mathf.PI * 0.25f;
    private double _radian = 0.0f;
    
	// Use this for initialization
	void Start () 
    {
        _startY = transform.position.y;
        _radian = Random.Range(0, Mathf.PI * 2);
        _rVec = Mathf.PI * Random.Range(0.1f, 0.3f); 
	}
	
	// Update is called once per frame
	void Update () 
    {
        _radian += Time.deltaTime * _rVec;
        if(_radian > Mathf.PI * 2)
        {
            _radian -= Mathf.PI * 2;
        }

        var pos_ = transform.position;
        transform.position = new Vector3(pos_.x, _startY + Mathf.Sin((float)_radian) * _radius, pos_.z);
	}
}
