using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TitleCube : MonoBehaviour {

	void Start () 
    {
        fixedUV();
	}

    #region Method

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
