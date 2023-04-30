using UnityEngine;

namespace Towers
{
    public class CloneAndRotateRandomly : PacketModifier
    {
        protected override void Modify(Packet packet)
        {
            ClonePacket(packet);
            packet.Rotate(Random.Range(1, 4) * 90 * Mathf.Deg2Rad);
        }
    }
}