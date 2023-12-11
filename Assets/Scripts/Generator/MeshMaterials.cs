using System;
using UnityEngine;

public class MeshMaterials
{
    public readonly MeshFilter meshFilter;
    public readonly Material[] materials;

    public MeshMaterials(MeshFilter mesh, Material[] materials)
    {
        this.meshFilter = mesh ?? throw new ArgumentNullException(nameof(mesh));
        this.materials = materials ?? throw new ArgumentNullException(nameof(materials));
    }

    public bool HasSameMaterials(Material[] mat)
    {
        if (mat.Length != materials.Length)
            return false;

        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].name != mat[i].name)
                return false;
        }

        return true;
    }

}

