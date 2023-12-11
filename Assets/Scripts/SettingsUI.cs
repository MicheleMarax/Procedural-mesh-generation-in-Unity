using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class SettingsUI : MonoBehaviour
{
    public SettingsData data;
    public CameraController cameraController;

    public InfiniteTerrain infiniteTerrain;
    public GameObject panel;
    public GameObject debugPanel;

    [Header("UI")]
    public Slider chunkRender;
    public TextMeshProUGUI chunkRenderTxt;

    public Slider renderSlider;
    public TextMeshProUGUI renderTxt;

    public Toggle occlusionToggle;
    public Toggle debugToggle;

    public Button closeBtn;
    public Button exitBtn;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        Cursor.visible = false;

        panel.SetActive(false);

        debugPanel.SetActive(data.useDebugWindows);

        chunkRender.minValue = 100;
        chunkRender.maxValue = 500;
        chunkRender.wholeNumbers = true;
        chunkRender.value = data.chunkRenderDistance;
        chunkRenderTxt.text = data.chunkRenderDistance.ToString();

        renderSlider.minValue = 10;
        renderSlider.maxValue = 50;
        renderSlider.wholeNumbers = true;
        renderSlider.value = data.renderDistance;
        renderTxt.text = data.renderDistance.ToString();

        occlusionToggle.isOn = data.useOcclusionCulling;
        debugToggle.isOn = data.useDebugWindows;

        chunkRender.onValueChanged.AddListener(RenderSldierChange);
        renderSlider.onValueChanged.AddListener(RenderDistanceChange);
        occlusionToggle.onValueChanged.AddListener(OcclusionToggleChange);
        debugToggle.onValueChanged.AddListener(DebugToggleChange);
        closeBtn.onClick.AddListener(CloseBtnPressed);
        exitBtn.onClick.AddListener(ExitBtnPressed);
    }

    private void RenderDistanceChange(float arg0)
    {
        cam.farClipPlane = arg0 * 100;
        data.renderDistance = (int)arg0;
        renderTxt.text = arg0.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            panel.SetActive(!panel.activeSelf);
            cameraController.enabled = !panel.activeSelf;

            if (Cursor.visible)
                Cursor.visible = false;
            else
                Cursor.visible = true;
        }
    }

    private void ExitBtnPressed()
    {
        SceneManager.LoadScene(0);
        Cursor.visible = true;
    }

    private void CloseBtnPressed()
    {
        panel.SetActive(false);
        cameraController.enabled = true;
        Cursor.visible = false;
    }

    private void DebugToggleChange(bool arg0)
    {
        debugPanel.SetActive(arg0);
        data.useDebugWindows = arg0;
    }

    private void OcclusionToggleChange(bool arg0)
    {
        infiniteTerrain.UseOcclusionCulling = arg0;
        data.useOcclusionCulling = arg0;
    }

    private void RenderSldierChange(float arg0)
    {
        data.chunkRenderDistance = (int)arg0;
        infiniteTerrain.maxViewDistance = arg0;
        chunkRenderTxt.text = arg0.ToString();
        infiniteTerrain.CalculateRenderDistance();
    }

   

}
