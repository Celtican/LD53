using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Packet : MonoBehaviour
{
    public GameObject prefab;
    [SerializeField] private GameObject enemyColliderObject;
    [SerializeField] private float initialSpeed = 5;
    public float damage = 1;
    public float explosionSize = 0;

    [NonSerialized] public UnityEvent onDestroy = new UnityEvent();
    
    private Rigidbody2D body;
    private List<PacketModifier> collidedModifiers = new List<PacketModifier>();
    private bool hasExploded = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void CopyAttributes(Packet other)
    {
        body.velocity = other.body.velocity;
        enemyColliderObject.transform.localScale = other.enemyColliderObject.transform.localScale;
        damage = other.damage;
        collidedModifiers = new List<PacketModifier>(other.collidedModifiers);
        explosionSize = other.explosionSize;
    }

    private void Update()
    {
        if (CameraController.instance.IsPosOutOfBounds(transform.position))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent != null && other.transform.parent.TryGetComponent(out EnemyController enemy))
        {
            if (explosionSize > 0)
            {
                Explode(explosionSize);
                return;
            }
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public static Packet InstantiatePacket(GameObject packetPrefab, Vector2 position, Vector2 direction)
    {
        GameObject packetInstance = Instantiate(packetPrefab);
        Packet packet = packetInstance.GetComponent<Packet>();
        Rigidbody2D body = packetInstance.GetComponent<Rigidbody2D>();

        body.velocity = direction.normalized * packet.initialSpeed;
        packetInstance.transform.position = position;

        return packet;
    }

    public bool Modify(PacketModifier modifier)
    {
        if (collidedModifiers.Contains(modifier)) return false;
        
        collidedModifiers.Add(modifier);

        return true;
    }

    public void AddScale(float scaleToAdd)
    {
        Tween tween = enemyColliderObject.transform.DOScale(enemyColliderObject.transform.localScale + Vector3.one*scaleToAdd, 0.2f);
        onDestroy.AddListener(() => tween.Kill());
    }

    public void MultiplySpeed(float speedMultiplier)
    {
        body.velocity *= speedMultiplier;
    }

    public void Rotate(float radians)
    {
        body.velocity = Utils.Rotate(body.velocity, radians);
    }

    public void Explode(float explosionSize)
    {
        if (hasExploded) return;
        hasExploded = true;
        ExplosionFactory.instance.CreateExplosion(transform.position, explosionSize, damage);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        onDestroy.Invoke();
    }
}
