using UnityEngine;
using UnityEngine.Serialization;

public class PacketModifier : MonoBehaviour
{

    public float speedMultiplier = 1;
    public float damageMultiplier = 1;
    public float scaleMultiplier = 1;
    public float rotationInDegrees = 0;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.TryGetComponent(out Packet packet))
        {
            if (!packet.Modify(this)) return;

            if (speedMultiplier != 1) packet.MultiplySpeed(speedMultiplier);
            packet.damage *= damageMultiplier;
            if (scaleMultiplier != 1) packet.MultiplyScale(scaleMultiplier);
            if (rotationInDegrees != 0)
            {
                packet.transform.position = transform.position;
                packet.Rotate(rotationInDegrees * Mathf.Deg2Rad);
            }
        }
    }
}