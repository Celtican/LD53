using UnityEngine;
using UnityEngine.Serialization;

public class BasicPacketModifier : PacketModifier
{
    public float speedMultiplier = 1;
    public float damageMultiplier = 1;
    public float damageAdder = 0;
    [FormerlySerializedAs("scaleMultiplier")] public float scaleAdder = 0;
    public float rotationInDegrees = 0;
    public float newExplosionSize = 0;
    
    protected override void Modify(Packet packet)
    {
        if (speedMultiplier != 1) packet.MultiplySpeed(speedMultiplier);
        packet.damage *= damageMultiplier;
        packet.damage += damageAdder;
        if (scaleAdder != 0) packet.AddScale(scaleAdder);
        if (rotationInDegrees != 0)
        {
            packet.transform.position = transform.position;
            packet.Rotate(rotationInDegrees * Mathf.Deg2Rad);
        }

        if (packet.explosionSize < newExplosionSize) packet.explosionSize = newExplosionSize;
    }
}