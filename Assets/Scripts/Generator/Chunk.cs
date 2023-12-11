using UnityEngine;
using System.Collections.Generic;

public partial class InfiniteTerrain
{
    public class Chunk
    {
        private int size;
        private Vector3 pos;
        private GameObject meshObject;
        private Bounds bounds;

        public GameObject mesh { get { return meshObject; } }
        public Bounds Bounds { get => bounds; }

        public Chunk(int size, Vector2 coord, float scale, MapGenerator generator)
        {
            this.size = size;
            pos = new Vector3(coord.x, 0, coord.y);

            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.position = pos * size * scale;
            meshObject.transform.localScale = Vector3.one * scale;
            meshObject.transform.parent = generator.transform;
            meshObject.isStatic = true;

            generator.RequestMapData(OnMapDataReceived, coord * size);

            bounds = new Bounds(pos * size * scale, Vector3.one * size * scale);
        }

        void OnMapDataReceived(TerrainData data)
        {
            mesh.GetComponent<MeshFilter>().mesh = data.MeshData.CreateMesh();

            Texture2D texture = new Texture2D(size + 1, size + 1);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;

            texture.SetPixels(data.ColorMap);
            texture.Apply();

            mesh.GetComponent<MeshRenderer>().material.mainTexture = texture;

            List<MeshMaterials> meshMaterials = new List<MeshMaterials>();

            if(data.PropsData != null)
            {
                foreach (PropData propData in data.PropsData)
                {
                    if (propData.PropModel == null)
                        continue;

                    GameObject prop = Instantiate(propData.PropModel, propData.Position, Quaternion.identity, meshObject.transform);
                    prop.transform.localScale = Vector3.one * data.PropsScale;
                    prop.transform.localPosition = propData.Position;
                    prop.isStatic = true;

                    meshMaterials.Add(new MeshMaterials(prop.GetComponent<MeshFilter>(), prop.GetComponent<MeshRenderer>().materials));
                }

                MeshCombiner.CombineMesh(meshMaterials, meshObject.transform);
            }       
        }

        public void DisableChunk()
        {
            meshObject.SetActive(false);
        }

        public void EnableChunk()
        {
            meshObject.SetActive(true);
        }
    }
}
