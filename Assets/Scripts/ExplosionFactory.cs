using System;
using UnityEngine;

public class ExplosionFactory : MonoBehaviour
{
    public static ExplosionFactory instance;
    
    public GameObject explosionPrefab;

    private void Awake()
    {
        instance = this;
    }

    public Explosion CreateExplosion(Vector3 position, float size, float damage)
    {
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity).GetComponent<Explosion>();
        explosion.finalSize = size;
        explosion.damage = damage;
        return explosion;
    }

    public void OnDestroy()
    {
        if (instance == this) instance = null;
    }
}