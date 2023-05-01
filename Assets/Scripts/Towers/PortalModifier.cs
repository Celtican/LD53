using UnityEngine;

namespace Towers
{
    public class PortalModifier : PacketModifier
    {
        public GameObject target;

        protected override void Modify(Packet packet)
        {
            packet.gameObject.transform.position = target.transform.position;
        }
    }
}