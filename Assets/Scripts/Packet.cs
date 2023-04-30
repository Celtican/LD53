using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Packet : MonoBehaviour
{
    [SerializeField] private GameObject enemyColliderObject;
    [SerializeField] private float initialSpeed = 5;
    public float damage = 1;

    [NonSerialized] public UnityEvent onDestroy = new UnityEvent();
    
    private Rigidbody2D body;
    private readonly List<PacketModifier> collidedModifiers = new List<PacketModifier>();

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent != null && other.transform.parent.TryGetComponent(out EnemyController enemy))
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            onDestroy.Invoke();
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
}
