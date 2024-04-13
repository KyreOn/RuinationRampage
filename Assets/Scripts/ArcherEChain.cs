using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEChain : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;

    private float _lifespan;
    private float _timer;

    public void Init(Vector3 startPoint, Vector3 endPoint, float lifespan)
    {
        start.position = startPoint;
        end.position = endPoint;
        _lifespan = lifespan;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _lifespan)
            Destroy(gameObject);
    }
}
