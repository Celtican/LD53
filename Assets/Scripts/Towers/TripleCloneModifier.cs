using UnityEngine;

namespace Towers
{
    public class TripleCloneModifier : PacketModifier
    {
        protected override void Modify(Packet packet)
        {
            ClonePacket(packet).Rotate(Mathf.Deg2Rad *  90);
            ClonePacket(packet).Rotate(Mathf.Deg2Rad * -90);
        }
    }
}