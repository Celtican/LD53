using UnityEngine;

namespace Towers
{
    public class TCloneModifier : PacketModifier
    {
        protected override void Modify(Packet packet)
        {
            ClonePacket(packet).Rotate(-90*Mathf.Deg2Rad);
            packet.Rotate(90*Mathf.Deg2Rad);
        }
    }
}