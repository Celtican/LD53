public class CloneAndModify : BasicPacketModifier
{
    protected override void Modify(Packet packet)
    {
        base.Modify(ClonePacket(packet));
    }
}
