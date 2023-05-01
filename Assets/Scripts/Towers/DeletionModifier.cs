namespace Towers
{
    public class DeletionModifier : PacketModifier
    {
        protected override void Modify(Packet packet)
        {
            Destroy(packet.gameObject);
        }
    }
}
