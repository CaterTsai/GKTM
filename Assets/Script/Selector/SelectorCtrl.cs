using UnityEngine;
using System.Collections;


public class SelectorCtrl : MonoBehaviour
{
    
    #region Basic Method
    //---------------------------------------------------
    void Update()
    {

    }
	#endregion


    #region Method
    //---------------------------------------------------
    public void enableSelector(Vector3 pos)
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        transform.position = pos;
    }

    //---------------------------------------------------
    public void disableSelector()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion
}
