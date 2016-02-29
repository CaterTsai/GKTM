using UnityEngine;
using System.Collections;

public class UIButton : MonoBehaviour
{
    #region Delegate
    public delegate void onButtonClick();
    #endregion

    onButtonClick _callback = null;

    #region Method
    public void OnClick()
    {
        if (_callback == null)
        {
            _callback();
        }
    }
    #endregion
}
