using UnityEngine;

public class RandomPacketSpawner : PacketSpawner
{
    public override void Shoot()
    {
        direction = Utils.Rotate(direction, Random.Range(0, 4) * Mathf.PI*0.5f);
        base.Shoot();
    }
}
