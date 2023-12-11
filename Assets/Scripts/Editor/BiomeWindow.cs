using UnityEditor;
using UnityEngine;

public class BiomeWindow : EditorWindow
{
    private Biome[] biomes;
    private Biome selectedBiome;

    [MenuItem("Window/BiomeWindow")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(BiomeWindow));

        window.maxSize = new Vector2(570, 600);
        window.minSize = new Vector2(570, 600);
    }

    private void OnEnable()
    {
        biomes = Resources.FindObjectsOfTypeAll<Biome>();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

        DrawButtons();

        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(400), GUILayout.MaxHeight(180), GUILayout.ExpandHeight(true));

        if(selectedBiome != null)
            DrawProperties();

        DrawTexture();

        EditorGUILayout.EndVertical();
    }

    private void DrawProperties()
    {
        selectedBiome.color = EditorGUILayout.ColorField(selectedBiome.color);

        EditorGUILayout.LabelField("Max height");
        selectedBiome.maxHeight = EditorGUILayout.Slider(selectedBiome.maxHeight, selectedBiome.minHeight, 1);

        EditorGUILayout.LabelField("Min height");
        selectedBiome.minHeight = EditorGUILayout.Slider(selectedBiome.minHeight, 0, selectedBiome.maxHeight);

        EditorGUILayout.LabelField("Max temperature");
        selectedBiome.maxTemperature = EditorGUILayout.Slider(selectedBiome.maxTemperature, selectedBiome.minTemperature, 1);

        EditorGUILayout.LabelField("Min temperature");
        selectedBiome.minTemperature = EditorGUILayout.Slider(selectedBiome.minTemperature, 0, selectedBiome.maxTemperature);
    }

    private void DrawButtons()
    {
        foreach (Biome item in biomes)
        {
            if (GUILayout.Button(item.name))
            {
                selectedBiome = item;
            }
        }

        if(GUILayout.Button("==SAVE=="))
        {
            foreach (Biome item in biomes)
            {
                EditorUtility.SetDirty(item);
            }
        }
    }

    private void DrawArrayList()
    {
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty biomeProperty = so.FindProperty("biomes");

        EditorGUILayout.PropertyField(biomeProperty, true); // True means show children
        so.ApplyModifiedProperties(); // Remember to apply modified properties
    }

    private void DrawTexture()
    {
        Texture2D texture = new Texture2D(100, 100);
        Color[] colorMap = new Color[100 * 100];

        for (int x = 0; x < 100; x++)
        {
            for (int y = 0; y < 100; y++)
            {
                colorMap[y * 100 + x] = Color.black;
            }
        }

        foreach (Biome biome in biomes)
        {
            for (int x = (int)(biome.minTemperature * 100); x < (int)(biome.maxTemperature * 100); x++)
            {
                for (int y = (int)(biome.minHeight * 100); y < (int)(biome.maxHeight * 100); y++)
                {
                    if (colorMap[y * 100 + x] == Color.black)
                        colorMap[y * 100 + x] = new Color(biome.color.r, biome.color.g, biome.color.b);
                    else
                        colorMap[y * 100 + x] = Color.red;
                }
            }
        }

        texture.SetPixels(colorMap);
        texture.Apply();

        GUI.DrawTexture(new Rect(160, 200, 400, 400), texture, ScaleMode.StretchToFill);
        
    }
}