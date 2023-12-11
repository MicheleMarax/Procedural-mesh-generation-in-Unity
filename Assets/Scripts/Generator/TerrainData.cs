using UnityEngine;

public class TerrainData
{
    private float[,] heightMap;
    private MeshData meshData;
    private Color[] colorMap;
    private PropData[] propsData;
    private float propsScale;

    public TerrainData(float[,] heightMap, MeshData meshData, Color[] colorMap, PropData[] propsData, float pScale)
    {
        this.heightMap = heightMap;
        this.meshData = meshData;
        this.colorMap = colorMap;
        this.propsData = propsData;
        propsScale = pScale;
    }

    public float[,] HeightMap { get => heightMap; }
    public MeshData MeshData { get => meshData; }
    public Color[] ColorMap { get => colorMap; }
    public PropData[] PropsData { get => propsData; }
    public float PropsScale { get => propsScale; }
}