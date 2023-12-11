using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine.SceneManagement;

public enum generationType { heightMap, temperatureMap, biomeMap, meshMap, propMap }

public class MapGenerator : MonoBehaviour
{
    public int chunkSize;
    public float chunkScale;
    public bool autoUpdate;
    public generationType genType;
    public Noise.NormalizeMode mode;
    private MapDisplay display;

    public List<Biome> biomes;

    public Vector2 noiseOffset;

    static MapGenerator instance;

    [Header("======Height map settings======")]
    public float heightMapScale;
    public int heightSeed;
    [Range(1, 10)] public int heightOctaves;
    [Range(0, 1)] public float heightPersistance;
    [Range(0, 100)] public float heightLacunarity;

    [Header("======Temperature map settings======")]
    public float temperatureMapScale;
    public int temperatureSeed;
    [Range(1, 10)] public int temperatureOctaves;
    [Range(0, 1)] public float temperaturePersistance;
    [Range(0, 100)] public float temperatureLacunarity;

    [Header("======Props map settings======")]
    public float propsMapScale;
    public int propsSeed;
    public bool generateProps;
    [Range(1, 10)] public int propsOctaves;
    [Range(0, 1)] public float propsPersistance;
    [Range(0, 100)] public float propsLacunarity;
    [Range(0, 3)] public float propsScale;

    [Header("======Mesh map settings======")]
    public AnimationCurve heightCurve;
    public float heightMultiplier;
    public bool useFlatShader;

    Queue<MapThreadInfo<TerrainData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<TerrainData>>();

    public static MapGenerator Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        //display.gameObject.SetActive(false);
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += onSceneChange;
        display = FindObjectOfType<MapDisplay>();
    }

    private void onSceneChange(Scene arg0, Scene arg1)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        if (arg1.buildIndex == 0)
        {
            display = FindObjectOfType<MapDisplay>();
            mode = Noise.NormalizeMode.Local;
        }
        else
            mode = Noise.NormalizeMode.Global;
    }

    //#if UNITY_EDITOR
    public void GeneratePreview()
    {
        TerrainData data = GenerateTerrain(Vector2.zero);

        if (genType == generationType.heightMap)
        {
            display.DrawNoiseMap(data.HeightMap);
        }

        if (genType == generationType.temperatureMap)
        {
            float[,] tempMap = Noise.GenerateNoiseMap(chunkSize, chunkSize, temperatureSeed, temperatureMapScale, temperatureOctaves, temperaturePersistance, temperatureLacunarity, noiseOffset, mode);

            display.DrawNoiseMap(tempMap);
        }

        if (genType == generationType.biomeMap)
        {
            display.DrawColorMap(data.ColorMap, chunkSize, chunkSize);
        }

        if (genType == generationType.meshMap)
        {
            display.DrawMesh(data.MeshData, data.ColorMap, chunkSize, chunkSize);
        }

        if (genType == generationType.propMap)
        {
            float[,] propsMap = Noise.GenerateNoiseMap(chunkSize, chunkSize, propsSeed, propsMapScale, propsOctaves, propsPersistance, propsLacunarity, noiseOffset, mode);

            display.DrawNoiseMap(propsMap);
        }
    }
//#endif

    public void RequestMapData(Action<TerrainData> callback, Vector2 position)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(callback, position);
        };

        new Thread(threadStart).Start();
    }

    void MapDataThread(Action<TerrainData> callback, Vector2 position)
    {
        TerrainData data = GenerateTerrain(position);
        lock (mapDataThreadInfoQueue)
        {
            mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<TerrainData>(callback, data));
        }
    }

    private void Update()
    {
        if (mapDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < mapDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<TerrainData> threadInfo = mapDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    public TerrainData GenerateTerrain(Vector2 coord)
    {
        float[,] heightMap = Noise.GenerateNoiseMap(chunkSize, chunkSize, heightSeed, heightMapScale, heightOctaves, heightPersistance, heightLacunarity, coord + noiseOffset, mode);
        float[,] tempMap = Noise.GenerateNoiseMap(chunkSize, chunkSize, temperatureSeed, temperatureMapScale, temperatureOctaves, temperaturePersistance, temperatureLacunarity, coord + noiseOffset, mode);
        float[,] propsMap = Noise.GenerateNoiseMap(chunkSize, chunkSize, propsSeed, propsMapScale, propsOctaves, propsPersistance, propsLacunarity, coord + noiseOffset, mode);
        List<PropData> propsData = new List<PropData>();

        Color[] colorMap = new Color[chunkSize * chunkSize];

        int halfChunk = Mathf.FloorToInt(chunkSize / 2);
        int hX;
        int hY;

        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                foreach (Biome biome in biomes)
                {
                    if (biome.CanFit(tempMap[x, y], heightMap[x, y]))
                    {
                        colorMap[y * chunkSize + x] = biome.color;

                        hX = x - halfChunk;
                        hY = y - halfChunk;

                        GameObject prop = biome.GetProp(propsMap[x, y]);

                        lock (heightCurve)
                        {
                            PropData propData = new PropData(prop, new Vector3(hX, heightMultiplier * heightCurve.Evaluate(heightMap[x, y]), hY));
                            propsData.Add(propData);
                        }

                        break;
                    }
                }
            }
        }

        MeshData data = MeshGenerator.GenerateMesh(heightMap, heightCurve, heightMultiplier, useFlatShader);

        if (!generateProps)
            propsData.Clear();

        if(generateProps)
            return new TerrainData(heightMap, data, colorMap, propsData.ToArray(), propsScale);
        else
            return new TerrainData(heightMap, data, colorMap, null, propsScale);
    }


    void OnValidate()
    {
        if (heightLacunarity < 1)
        {
            heightLacunarity = 1;
        }
        if (heightOctaves < 0)
        {
            heightOctaves = 0;
        }
    }

    
}



struct MapThreadInfo<T>
{
    public readonly Action<T> callback;
    public readonly T parameter;

    public MapThreadInfo(Action<T> callback, T parameter)
    {
        this.callback = callback;
        this.parameter = parameter;
    }

}
