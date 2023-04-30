public class AlternatingRotationModifier : BasicPacketModifier
{
    protected override void Modify(Packet packet)
    {
        rotationInDegrees = rotationInDegrees < 0 ? 90 : -90;
        base.Modify(packet);
    }
}