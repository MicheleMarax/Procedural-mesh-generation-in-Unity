using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public partial class InfiniteTerrain : MonoBehaviour
{
    [SerializeField] private SettingsData data;
    [SerializeField] private MapGenerator generator;

    public float maxViewDistance;
    public Transform viewer;
    public bool UseOcclusionCulling = true;
    public Camera viewerCamera;

    private float scale;

    Dictionary<Vector2, Chunk> chunksLoaded;
    List<Chunk> lastLoadedChunks;

    private int chunkSize;
    private int chunksVisible;

    public int ChunksVisible
    {
        get => chunksVisible;
        set
        {
            if (value >= 1)
                chunksVisible = value;
            else
                chunksVisible = 1;
        }
    }

    private void Start()
    {
        generator = FindObjectOfType<MapGenerator>(false);

        UseOcclusionCulling = data.useOcclusionCulling;
        maxViewDistance = data.chunkRenderDistance;

        chunkSize = generator.chunkSize - 1;

        scale = generator.chunkScale;
        ChunksVisible = Mathf.RoundToInt(maxViewDistance / chunkSize);
        chunksLoaded = new Dictionary<Vector2, Chunk>();
        lastLoadedChunks = new List<Chunk>();
    }

    public void CalculateRenderDistance()
    {
        ChunksVisible = Mathf.RoundToInt(maxViewDistance / chunkSize);
        UpdateChunks();
    }

    private void Update()
    {
        UpdateChunks();

        if(UseOcclusionCulling)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(viewerCamera);

            List<Chunk> chunks = chunksLoaded.Select(kvp => kvp.Value).ToList();

            for (int i = 0; i < chunksLoaded.Count; i++)
            {
                if(GeometryUtility.TestPlanesAABB(planes, chunks[i].Bounds))
                {
                    chunks[i].mesh.SetActive(true);
                }
                else
                {
                    chunks[i].mesh.SetActive(false);
                }
            }
        }
    }

    private void UpdateChunks()
    {
        foreach (Chunk item in lastLoadedChunks)
        {
            item.DisableChunk();
        }

        lastLoadedChunks.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewer.position.x / chunkSize / scale);
        int currentChunkCoordY = Mathf.RoundToInt(viewer.position.z / chunkSize / scale);

        for (int yOffset = -ChunksVisible; yOffset <= ChunksVisible; yOffset++)
        {
            for (int xOffset = -ChunksVisible; xOffset <= ChunksVisible; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (chunksLoaded.ContainsKey(viewedChunkCoord))
                {
                    chunksLoaded[viewedChunkCoord].EnableChunk();
                }
                else
                {
                    Chunk chunk = new Chunk(chunkSize, viewedChunkCoord, scale, generator);
                    chunksLoaded.Add(viewedChunkCoord, chunk);
                }

                lastLoadedChunks.Add(chunksLoaded[viewedChunkCoord]);
            }
        }
    }
}


