using UnityEngine;
using System.Collections;
using HighlightingSystem;

public class HBase : MonoBehaviour
{
    protected Highlighter _Hightlighter;
    void Awake()
    {
        _Hightlighter = GetComponent<Highlighter>();
        if (_Hightlighter == null) { _Hightlighter = gameObject.AddComponent<Highlighter>(); }
    }

    void Start()
    {
        
    }
}
