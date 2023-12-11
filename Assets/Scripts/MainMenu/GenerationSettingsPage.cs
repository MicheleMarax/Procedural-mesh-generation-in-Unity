using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GenerationSettingsPage : MonoBehaviour
{
    public GameObject options;

    public Toggle hideUI;
    private MapGenerator generator;
    public Button startBtn;

    [Header("===HEIGHT MAP===")]
    public TMP_InputField heightField;
    public Slider h_scale;
    public Slider h_persistance;
    public Slider h_octaves;
    public Slider h_lacunarity;
    public TMP_InputField heightMultiplier;
    public TMP_InputField xOffset;
    public TMP_InputField yOffset;

    public TextMeshProUGUI h_scaleTxt;
    public TextMeshProUGUI h_persistanceTxt;
    public TextMeshProUGUI h_octavesTxt;
    public TextMeshProUGUI h_lacunarityTxt;

    [Header("===TEMPERATURE MAP===")]
    public TMP_InputField tempField;
    public Slider t_scale;
    public Slider t_persistance;
    public Slider t_octaves;
    public Slider t_lacunarity;
    public Slider propsScale;

    public TextMeshProUGUI t_scaleTxt;
    public TextMeshProUGUI t_persistanceTxt;
    public TextMeshProUGUI t_octavesTxt;
    public TextMeshProUGUI t_lacunarityTxt;
    public TextMeshProUGUI t_pScale;


    public Toggle spawnProps;

    void Start()
    {
        generator = MapGenerator.Instance;

        hideUI.onValueChanged.AddListener(OnToggleValueChange);

        startBtn.onClick.AddListener(LoadMap);

        #region HEIGHT MAP
        heightField.SetTextWithoutNotify(generator.heightSeed.ToString());
        heightField.characterLimit = 9;
        heightField.contentType = TMP_InputField.ContentType.IntegerNumber;
        heightField.onValueChanged.AddListener(heightFieldChange);

        h_scale.minValue = 1;
        h_scale.maxValue = 300;
        h_scale.wholeNumbers = false;
        h_scale.value = generator.heightMapScale;
        h_scaleTxt.text = generator.heightMapScale.ToString();
        h_scale.onValueChanged.AddListener(heightScaleChange);

        h_octaves.minValue = 1;
        h_octaves.maxValue = 4;
        h_octaves.wholeNumbers = true;
        h_octaves.value = generator.heightOctaves;
        h_octavesTxt.text = generator.heightOctaves.ToString();
        h_octaves.onValueChanged.AddListener(heightOctavesChange);

        h_persistance.minValue = 0;
        h_persistance.maxValue = 1;
        h_persistance.wholeNumbers = false;
        h_persistance.value = generator.heightPersistance;
        h_persistanceTxt.text = generator.heightPersistance.ToString();
        h_persistance.onValueChanged.AddListener(heightPersistanceChange);


        h_lacunarity.minValue = 0;
        h_lacunarity.maxValue = 100;
        h_lacunarity.wholeNumbers = false;
        h_lacunarity.value = generator.heightLacunarity;
        h_lacunarityTxt.text = generator.heightLacunarity.ToString();
        h_lacunarity.onValueChanged.AddListener(heightLacunarityChange);

        heightMultiplier.SetTextWithoutNotify(generator.heightMultiplier.ToString());
        heightMultiplier.characterLimit = 3;
        heightMultiplier.contentType = TMP_InputField.ContentType.DecimalNumber;
        heightMultiplier.onValueChanged.AddListener(heightMultiplierChange);

        xOffset.SetTextWithoutNotify(generator.noiseOffset.x.ToString());
        xOffset.characterLimit = 3;
        xOffset.contentType = TMP_InputField.ContentType.DecimalNumber;
        xOffset.onValueChanged.AddListener(xOffsetChange);

        yOffset.SetTextWithoutNotify(generator.noiseOffset.y.ToString());
        yOffset.characterLimit = 3;
        yOffset.contentType = TMP_InputField.ContentType.DecimalNumber;
        yOffset.onValueChanged.AddListener(yOffsetChange);
        #endregion

        #region TEMPERATURE MAP

        tempField.SetTextWithoutNotify(generator.temperatureSeed.ToString());
        tempField.characterLimit = 9;
        tempField.contentType = TMP_InputField.ContentType.IntegerNumber;
        tempField.onValueChanged.AddListener(temperatureFieldChange);

        t_scale.minValue = 1;
        t_scale.maxValue = 300;
        t_scale.wholeNumbers = false;
        t_scale.value = generator.temperatureMapScale;
        t_scaleTxt.text = generator.temperatureMapScale.ToString();
        t_scale.onValueChanged.AddListener(temperatureScaleChange);

        t_octaves.minValue = 1;
        t_octaves.maxValue = 4;
        t_octaves.wholeNumbers = true;
        t_octaves.value = generator.temperatureOctaves;
        t_octavesTxt.text = generator.temperatureOctaves.ToString();
        t_octaves.onValueChanged.AddListener(temperatureOctavesChange);

        t_persistance.minValue = 0;
        t_persistance.maxValue = 1;
        t_persistance.wholeNumbers = false;
        t_persistance.value = generator.temperaturePersistance;
        t_persistanceTxt.text = generator.temperaturePersistance.ToString();
        t_persistance.onValueChanged.AddListener(temperaturePersistanceChange);

        t_lacunarity.minValue = 0;
        t_lacunarity.maxValue = 100;
        t_lacunarity.wholeNumbers = false;
        t_lacunarity.value = generator.temperatureLacunarity;
        t_lacunarityTxt.text = generator.temperatureLacunarity.ToString();
        t_lacunarity.onValueChanged.AddListener(temperatureLacunarityChange);

        spawnProps.isOn = generator.generateProps;
        spawnProps.onValueChanged.AddListener(SpawnPropsChange);

        propsScale.minValue = 0.1f;
        propsScale.maxValue = 3;
        propsScale.wholeNumbers = false;
        propsScale.value = generator.propsScale;
        t_pScale.text = Math.Round(generator.propsScale, 2).ToString();
        propsScale.onValueChanged.AddListener(PropsScaleChange);
        #endregion
    }

    #region TEMPERATURE MAP EVENTS
    private void temperatureLacunarityChange(float arg0)
    {
        generator.temperatureLacunarity = (int)arg0;
        t_lacunarityTxt.text = generator.temperatureLacunarity.ToString();
        generator.GeneratePreview();
    }

    private void temperaturePersistanceChange(float arg0)
    {
        generator.temperaturePersistance = arg0;
        t_persistanceTxt.text = Math.Round(generator.temperaturePersistance, 2).ToString();
        generator.GeneratePreview();
    }

    private void temperatureOctavesChange(float arg0)
    {
        generator.temperatureOctaves = (int)arg0;
        t_octavesTxt.text = generator.temperatureOctaves.ToString();
        generator.GeneratePreview();
    }

    private void temperatureScaleChange(float arg0)
    {
        generator.temperatureMapScale = (int)arg0;
        t_scaleTxt.text = generator.temperatureMapScale.ToString();
        generator.GeneratePreview();
    }

    private void temperatureFieldChange(string arg0)
    {
        if (arg0 == null || arg0 == "" || int.Parse(arg0) < 0)
            arg0 = "1";

        generator.temperatureSeed = int.Parse(arg0);
        generator.GeneratePreview();
    }

    #endregion

    #region HEIGHT MAP EVENTS
    private void yOffsetChange(string arg0)
    {
        if (arg0 == null || arg0 == "")
            arg0 = "0";

        generator.noiseOffset = new Vector2(generator.noiseOffset.x, int.Parse(arg0));
        generator.GeneratePreview();
    }

    private void xOffsetChange(string arg0)
    {
        if (arg0 == null || arg0 == "")
            arg0 = "0";

        generator.noiseOffset = new Vector2(int.Parse(arg0), generator.noiseOffset.y);
        generator.GeneratePreview();
    }

    private void heightMultiplierChange(string arg0)
    {
        if (arg0 == null || arg0 == "" || int.Parse(arg0) < 0)
            arg0 = "1";

        generator.heightMultiplier = int.Parse(arg0);
        generator.GeneratePreview();
    }

    private void heightOctavesChange(float arg0)
    {
        generator.heightOctaves = (int)arg0;
        h_octavesTxt.text = generator.heightOctaves.ToString();
        generator.GeneratePreview();
    }

    private void heightScaleChange(float arg0)
    {
        generator.heightMapScale = (int)arg0;
        h_scaleTxt.text = generator.heightMapScale.ToString();
        generator.GeneratePreview();
    }

    private void heightLacunarityChange(float arg0)
    {
        generator.heightLacunarity = (int)arg0;
        h_lacunarityTxt.text = generator.heightLacunarity.ToString();
        generator.GeneratePreview();
    }

    private void heightPersistanceChange(float arg0)
    {
        generator.heightPersistance = arg0;
        h_persistanceTxt.text = Math.Round(generator.heightPersistance, 2).ToString();
        generator.GeneratePreview();
    }

    private void heightFieldChange(string arg0)
    {
        if (arg0 == null || arg0 == "" || int.Parse(arg0) < 0)
            arg0 = "1";

        generator.heightSeed = int.Parse(arg0);
        generator.GeneratePreview();
    }

    private void SpawnPropsChange(bool value)
    {
        generator.generateProps = !generator.generateProps;
    }

    private void PropsScaleChange(float arg0)
    {
        generator.propsScale = arg0;
        t_pScale.text = Math.Round(generator.propsScale, 2).ToString();
    }

    #endregion


    private void OnToggleValueChange(bool value)
    {
        options.SetActive(!value);
    }

    private void LoadMap()
    {
        
        SceneManager.LoadScene(1);
    }
}
