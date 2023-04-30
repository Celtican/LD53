using UnityEngine;

public class RandomRotationModifier : BasicPacketModifier
{
    protected override void Modify(Packet packet)
    {
        rotationInDegrees = Random.Range(0, 2) == 0 ? 90 : -90;
        base.Modify(packet);
    }
}