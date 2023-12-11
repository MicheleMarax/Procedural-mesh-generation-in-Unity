using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Settings")]
public class SettingsData : ScriptableObject
{
    public bool useOcclusionCulling;
    public bool useDebugWindows;
    public int chunkRenderDistance;
    public int renderDistance; 
}

