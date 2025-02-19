using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{

    public Transform player;
    public GameObject[] mapChunks;
    public GameObject[] decorPrefabs;

    public float chunkSize = 50f;
    public int renderDistance = 3;

    private Dictionary<Vector2, GameObject> activeChunks = new Dictionary<Vector2, GameObject>();
    private Dictionary<Vector2, List<GameObject>> activeDecor = new Dictionary<Vector2, List<GameObject>>();

    private Vector2 currentChunkPosition;
    private Queue<GameObject> chunkPool = new Queue<GameObject>(); // Pool để tái sử dụng chunk

    void Update()
    {
        Vector2 newChunkPosition = new Vector2(
            Mathf.FloorToInt(player.position.x / chunkSize),
            Mathf.FloorToInt(player.position.z / chunkSize)
        );

        if (newChunkPosition != currentChunkPosition)
        {
            currentChunkPosition = newChunkPosition;
            UpdateMapChunks();
        }
    }

    void UpdateMapChunks()
    {
        HashSet<Vector2> neededChunks = new HashSet<Vector2>();

        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                Vector2 chunkPosition = new Vector2(currentChunkPosition.x + x, currentChunkPosition.y + z);
                neededChunks.Add(chunkPosition);

                if (!activeChunks.ContainsKey(chunkPosition))
                {
                    SpawnChunk(chunkPosition);
                }
            }
        }

        List<Vector2> chunksToRemove = new List<Vector2>();
        foreach (var chunk in activeChunks)
        {
            if (!neededChunks.Contains(chunk.Key))
            {
                chunksToRemove.Add(chunk.Key);
            }
        }

        foreach (var chunkPosition in chunksToRemove)
        {
            RecycleChunk(chunkPosition);
        }
    }

    void SpawnChunk(Vector2 chunkPosition)
    {
        GameObject newChunk;
        if (chunkPool.Count > 0)
        {
            newChunk = chunkPool.Dequeue();
            newChunk.transform.position = new Vector3(chunkPosition.x * chunkSize, 0, chunkPosition.y * chunkSize);
            newChunk.SetActive(true);
        }
        else
        {
            GameObject chunkPrefab = mapChunks[Random.Range(0, mapChunks.Length)];
            Vector3 position = new Vector3(chunkPosition.x * chunkSize, 0, chunkPosition.y * chunkSize);
            newChunk = Instantiate(chunkPrefab, position, Quaternion.identity);
        }

        activeChunks.Add(chunkPosition, newChunk);

        if (!activeDecor.ContainsKey(chunkPosition))
        {
            SpawnDecor(chunkPosition);
        }
    }

    void SpawnDecor(Vector2 chunkPosition)
    {
        List<GameObject> decorList = new List<GameObject>();
        int decorCount = Random.Range(5, 6);

        for (int i = 0; i < decorCount; i++)
        {
            GameObject decorPrefab = decorPrefabs[Random.Range(0, decorPrefabs.Length)];
            float offsetX = Random.Range(-chunkSize / 2, chunkSize / 2);
            float offsetZ = Random.Range(-chunkSize / 2, chunkSize / 2);
            Vector3 decorPosition = new Vector3(
                chunkPosition.x * chunkSize + offsetX,
                50f, // Bắt đầu từ trên cao
                chunkPosition.y * chunkSize + offsetZ
            );

            if (NavMesh.SamplePosition(decorPosition, out NavMeshHit hit, 100f, NavMesh.AllAreas))
            {
                decorPosition = hit.position;
                GameObject newDecor = Instantiate(decorPrefab, decorPosition, Quaternion.identity);
                decorList.Add(newDecor);
            }
        }

        activeDecor[chunkPosition] = decorList;
    }

    void RecycleChunk(Vector2 chunkPosition)
    {
        if (activeChunks.ContainsKey(chunkPosition))
        {
            GameObject chunk = activeChunks[chunkPosition];
            chunk.SetActive(false);
            chunkPool.Enqueue(chunk);
            activeChunks.Remove(chunkPosition);
        }

        if (activeDecor.ContainsKey(chunkPosition))
        {
            foreach (var obj in activeDecor[chunkPosition])
            {
                Destroy(obj);
            }
            activeDecor.Remove(chunkPosition);
        }
    }


}
