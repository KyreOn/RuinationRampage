using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSpellQProjectile : MonoBehaviour
{
    [SerializeField] private float     lifeSpan;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float     timeBetweenTicks;

    [Header("Sounds")] [SerializeField] private AudioClip[] hitSfx;
    
    private float _tickTimer;
    private float _damage;
    private float _slowPower;
    
    public void Init(float damage, float slowPower)
    {
        _damage = damage;
        _slowPower = slowPower;
        if (PlayerPrefs.GetString($"ChosenPerks0").Contains('3'))
            transform.localScale = new Vector3(1.5f, 0.06f, 1.5f);
        if (PlayerPrefs.GetString($"ChosenPerks0").Contains('4'))
            lifeSpan *= 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0)
            Destroy(gameObject);
        _tickTimer -= Time.deltaTime;
        if (_tickTimer >= 0) return;
        var enemiesInRange = Physics.OverlapSphere(transform.position, 2, enemyLayer);
        var choice         = Random.Range(0, 2);
        AudioManager.PlaySFX(hitSfx[choice]);
        foreach (var enemy in enemiesInRange)
        {
            if (enemy.gameObject.GetComponent<DamageSystem>().ApplyDamage(_damage, transform))
                enemy.gameObject.GetComponent<EffectSystem>().AddEffect(new SlowEffect(timeBetweenTicks,_slowPower), false);
        }

        _tickTimer = timeBetweenTicks;
    }
}
