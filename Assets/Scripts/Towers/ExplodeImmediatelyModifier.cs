namespace Towers
{
    public class ExplodeImmediatelyModifier : PacketModifier
    {
        public float explosionSize = 3;
        
        protected override void Modify(Packet packet)
        {
            packet.Explode(explosionSize);
        }
    }
}