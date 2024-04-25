using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEChain : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;

    private Transform _start;
    private Transform _end;
    private float     _lifespan;
    private float     _timer;

    public void Init(Transform startPoint, Transform endPoint, float lifespan)
    {
        _start = startPoint;
        _end = endPoint;
        _lifespan = lifespan;
    }

    private void Update()
    {
        start.position = _start.position + Vector3.up;
        end.position = _end.position     + Vector3.up;
        _timer += Time.deltaTime;
        if (_timer >= _lifespan)
            Destroy(gameObject);
    }
}
