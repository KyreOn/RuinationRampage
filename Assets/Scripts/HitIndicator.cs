using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitIndicator : MonoBehaviour
{
    [SerializeField] private GameObject aoe;
    [SerializeField] private Transform  progressCircle;
    [SerializeField] private float      time;
    [SerializeField] private float      maxSize;
    
    private float _timer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer >= time)
        {
            if (aoe is not null)
            {
                var aoeInst = Instantiate(aoe, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        _timer += Time.deltaTime;
        progressCircle.localScale = Vector3.one * (maxSize * (_timer / time));
    }
}
