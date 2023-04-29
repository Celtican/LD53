using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Packet : MonoBehaviour
{
    [SerializeField] private GameObject enemyColliderObject;
    
    [SerializeField] private float initialSpeed = 5;
    public float damage = 1;
    
    private Rigidbody2D body;
    private readonly List<PacketModifier> collidedModifiers = new List<PacketModifier>();

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
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

    public void MultiplyScale(float scaleMultiplier)
    {
        enemyColliderObject.transform.DOScale(transform.localScale * scaleMultiplier, 0.2f);
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
