using UnityEngine;
using System.Collections;


public class IslandFloat : MonoBehaviour
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
        _rVec = Mathf.PI * Random.Range(0.05f, 0.15f); 
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

    public Vector3 getPosition(float time)
    {
        var tRadian_ = _radian + time * _rVec;
        var pos_ = transform.position;
        var returnPos_ = new Vector3(pos_.x, _startY + Mathf.Sin((float)tRadian_) * _radius, pos_.z);

        return returnPos_;
    }
}
