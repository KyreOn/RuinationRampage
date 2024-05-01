using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorTrail : MonoBehaviour
{
    [SerializeField] private float     refreshRate;
    [SerializeField] private Transform body;
    [SerializeField] private Material  material;
    
    private SkinnedMeshRenderer[] _skinnedMeshRenderers;
    private bool                  _isActive;

    public void StartTrail(float time)
    {
        StartCoroutine(ActivateTrail(time));
    }

    IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= refreshRate;

            if (_skinnedMeshRenderers is null)
                _skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            
            for (var i = 0; i < _skinnedMeshRenderers.Length; i++)
            {
                var gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(body.position, body.rotation);
                
                var mr = gObj.AddComponent<MeshRenderer>();
                var mf = gObj.AddComponent<MeshFilter>();

                var mesh = new Mesh();
                _skinnedMeshRenderers[i].BakeMesh(mesh);
                mf.mesh = mesh;
                mr.material = material;
                
                Destroy(gObj, 0.1f);
            }

            yield return new WaitForSeconds(refreshRate);
        }

        _isActive = false;
    }
}
