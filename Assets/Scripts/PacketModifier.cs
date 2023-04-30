using UnityEngine;

public abstract class PacketModifier : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.TryGetComponent(out Packet packet))
        {
            if (!packet.Modify(this)) return;
            Modify(packet);
        }
    }

    protected Packet ClonePacket(Packet packet)
    {
        Packet newPacket = Instantiate(packet.prefab).GetComponent<Packet>();
        newPacket.CopyAttributes(packet);
        return newPacket;
    }

    protected virtual void Modify(Packet packet)
    {
        Debug.LogWarning("PacketModifier.Modify was not overriden!");
    }
}