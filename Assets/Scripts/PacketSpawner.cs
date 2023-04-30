using UnityEngine;

public class PacketSpawner : MonoBehaviour
{
    public float timeBetweenSpawns = 1;
    public Vector2 direction = Vector2.left;
    public GameObject packetPrefab;

    private float timeUntilSpawn;

    private void Start()
    {
        timeUntilSpawn = timeBetweenSpawns;
    }

    public void Update()
    {
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn <= 0)
        {
            timeUntilSpawn += timeBetweenSpawns;
            if (!CameraController.instance.IsPosOutOfBounds(transform.position)) Packet.InstantiatePacket(packetPrefab, transform.position, direction);
        }
    }
}