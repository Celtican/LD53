using UnityEngine;

public class PacketSpawner : MonoBehaviour
{
    public float timeBetweenSpawns = 1;
    public float timeOffset = 0;
    public Vector2 direction = Vector2.left;
    public GameObject packetPrefab;
    public float rotationAfterShootingInDegrees = 0;

    private float timeUntilSpawn;

    private void Start()
    {
        timeUntilSpawn = timeBetweenSpawns + timeOffset;
    }

    public void Update()
    {
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn <= 0)
        {
            timeUntilSpawn += timeBetweenSpawns;
            if (!CameraController.instance.IsPosOutOfBounds(transform.position)) Shoot();
        }
    }
    
    public virtual void Shoot()
    {
        Packet.InstantiatePacket(packetPrefab, transform.position, direction);
        if (rotationAfterShootingInDegrees != 0) direction = Utils.Rotate(direction, rotationAfterShootingInDegrees * Mathf.Deg2Rad);
    }
}
