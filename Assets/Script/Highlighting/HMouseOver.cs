using UnityEngine;
using System.Collections;

public class HMouseOver : HBase
{
    public Color _OverColor = new Color();

    public void MouseOver()
    {
        _Hightlighter.On(_OverColor);
    }
}
