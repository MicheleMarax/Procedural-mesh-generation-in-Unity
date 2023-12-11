using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Biome", menuName = "Biomes")]
public class Biome : ScriptableObject
{
    public Color color;

    [Range(0, 1)] public float minTemperature;
    [Range(0, 1)] public float maxTemperature;

    [Range(0, 1)] public float minHeight;
    [Range(0, 1)] public float maxHeight;

    [Range(0, 1)] public float minLevelForProps;
    [Range(0, 1)] public float maxLevelForProps;

    public Props[] biomeProps;

    public bool CanFit(float temperature, float height)
    {
        if (temperature >= 1 && maxTemperature >= 1)
            if (height >= minHeight && height <= maxHeight)
                return true;

        if (height >= 1 && maxHeight >= 1)
            if (temperature >= minTemperature && temperature <= maxTemperature)
                return true;

        if (temperature >= minTemperature && temperature <= maxTemperature)
            if (height >= minHeight && height < maxHeight)
                return true;

        return false;

    }

    public GameObject GetProp(float level)
    {
        if (biomeProps == null || biomeProps.Length == 0)
            return null;

        System.Random rdr = new System.Random();

        int start = rdr.Next(0, biomeProps.Length);

        int counter = 0;
        for (int i = start; counter < biomeProps.Length; i++, counter++)
        {
            if (i >= biomeProps.Length)
                i = 0;

            if (minLevelForProps <= level && maxLevelForProps >= level)
            {
                if ((rdr.Next(0, 100) / 100) <= biomeProps[i].probability)
                    return biomeProps[i].propModel;
            }
                
        }

        return null;
    }
}

[Serializable]
public struct Props
{
    public GameObject propModel;

    [Range(0,1)]
    public float probability;
}

public struct PropData
{
    private GameObject propModel;
    private Vector3 position;

    public PropData(GameObject propModel, Vector3 position)
    {
        this.propModel = propModel;
        this.position = position;
    }

    public Vector3 Position { get => position;}
    public GameObject PropModel { get => propModel;}
}
