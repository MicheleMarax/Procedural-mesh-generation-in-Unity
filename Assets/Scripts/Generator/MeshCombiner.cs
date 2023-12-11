using System.Collections.Generic;
using UnityEngine;

public static class MeshCombiner
{
    private const int maxVertex = 65535;

    public static void CombineMesh(List<MeshMaterials> meshMaterials, Transform parent)
    {
        //Divido per Material
        if (meshMaterials == null)
            return;

        List<List<MeshMaterials>> matData = new List<List<MeshMaterials>>();
        List<MeshMaterials> tmp = new List<MeshMaterials>();

        for (int i = 0; i < meshMaterials.Count; i++)
        {
            tmp.Clear();
            tmp.Add(meshMaterials[i]);

            for (int j = i + 1; j < meshMaterials.Count; j++)
            {
                if (tmp[0].HasSameMaterials(meshMaterials[j].materials))
                    tmp.Add(meshMaterials[j]);
            }

            if (matData.Count <= 0)
            {
                matData.Add(new List<MeshMaterials>(tmp));
            }else
            {
                bool canAdd = true;
                for (int k = 0; k < matData.Count; k++)
                {
                    if (matData[k][0].HasSameMaterials(tmp[0].materials))
                    {
                        canAdd = false;
                        break;
                    }
                }

                if(canAdd)
                    matData.Add(new List<MeshMaterials>(tmp));
            }
        }

        //Create combine instance
        foreach (List<MeshMaterials> sameMatList in matData)
        {
            int vertexCount = 0;
            for (int index = 0; index < sameMatList.Count; index++)
            {
                vertexCount += sameMatList[index].meshFilter.mesh.vertexCount;
            }

            int iteration = (int)System.Math.Ceiling((double)vertexCount / maxVertex);

            //conta quanti giri fare per occupare tutti i vertici
            int lastIndex = 0;
            for (int round = 0; round < iteration; round++)
            {
                List<MeshMaterials> vertexSort = new List<MeshMaterials>();

                vertexCount = 0;
                for (int index = lastIndex; index < sameMatList.Count; index++)
                {
                    vertexCount += sameMatList[index].meshFilter.mesh.vertexCount;

                    if(vertexCount >= maxVertex)
                    {
                        lastIndex = index;
                        break;
                    }
                    else
                    {
                        vertexSort.Add(sameMatList[index]);
                    }
                }

                //Combino le mesh
                CombineInstance[] combine = new CombineInstance[vertexSort.Count];

                int i = 0;
                while (i < vertexSort.Count)
                {
                    combine[i].mesh = vertexSort[i].meshFilter.mesh;

                    combine[i].transform = vertexSort[i].meshFilter.transform.localToWorldMatrix;
                    GameObject.Destroy(vertexSort[i].meshFilter.gameObject);

                    i++;
                }

                Mesh combinedMesh = new Mesh();
                combinedMesh.CombineMeshes(combine, true);
                
                GameObject container = new GameObject("Props container " + round);
                container.transform.SetParent(parent);
                MeshRenderer renderer = container.AddComponent<MeshRenderer>();
                MeshFilter filter = container.AddComponent<MeshFilter>();

                filter.mesh = combinedMesh;
                renderer.material = vertexSort[0].materials[0];
            }       
        }
    }

    private static Mesh ExtractSubMeshes(Mesh meshToExtract)
    {
        Mesh newMesh = new Mesh();

        int[] oldTrianges = meshToExtract.GetTriangles(0);

        int count = 0;
        Dictionary<int, int> dictionary = new Dictionary<int, int>();
        for (int x = 0; x < oldTrianges.Length; x++)
        {
            int current = oldTrianges[x];

            if (!dictionary.ContainsKey(current))
            {
                dictionary.Add(current, count);
                count = count + 1;
            }
        }

        int[] newTriangles = new int[oldTrianges.Length];
        for (int x = 0; x < oldTrianges.Length; x++)
        {
            newTriangles[x] = dictionary[oldTrianges[x]];
        }

        Vector3[] oldVerts = meshToExtract.vertices;
        Vector3[] newVerts = new Vector3[count];
        foreach (KeyValuePair<int, int> pair in dictionary)
        {
            int oldVertIndex = pair.Key;
            int newVertIndex = pair.Value;
            newVerts[newVertIndex] = oldVerts[oldVertIndex];
        }

        newMesh.vertices = newVerts;
        newMesh.triangles = newTriangles;
        newMesh.uv = new Vector2[newVerts.Length];
        newMesh.RecalculateNormals();
        newMesh.Optimize();

        return newMesh;
    }
}

