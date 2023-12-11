using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateMesh(float[,] heightMap, AnimationCurve heightCurve, float multiplier, bool flatShader)
    {
        AnimationCurve _heightCurve = new AnimationCurve(heightCurve.keys);
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        float offsetX = width / 2f;
        float offsetZ = height / 2f;

        MeshData data = new MeshData(width, height, flatShader);

        int i = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                data.vertices[i] = new Vector3(x - offsetX, multiplier * _heightCurve.Evaluate(heightMap[x, y]), y - offsetZ);
                data.uvs[i] = new Vector2(x / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1)
                {
                    data.AddTriangle(i, i + width + 1, i + width);
                    data.AddTriangle(i + width + 1, i, i + 1);
                }

                i++;
            }
        }

        //if (flatShader)
        //    data.FlatShading();

        return data;
    }
}
