using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitIndicator : MonoBehaviour
{
    [SerializeField] private Transform progressCircle;
    [SerializeField] private float     time;
    [SerializeField] private float     maxSize;
    
    private float _timer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer >= time) 
            Destroy(gameObject);
        _timer += Time.deltaTime;
        progressCircle.localScale = Vector3.one * (maxSize * (_timer / time));
    }
}
