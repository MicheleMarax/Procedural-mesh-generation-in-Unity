using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class UIHandler : MonoBehaviour
{
    public GameObject mainPage;
    public GameObject settingsPage;
    public GameObject creditsPage;

    public CinemachineVirtualCamera startCam;
    public CinemachineVirtualCamera settingsCam;
    public CinemachineVirtualCamera creditsCam;

    public Button settingsBtn;
    public Button exitBtn;
    public Button backBtn;
    public Button creditsBackBtn;
    public Button creditsBtn;

    public MapGenerator gen;

    void Start()
    {
        mainPage.SetActive(true);
        settingsPage.SetActive(false);
        creditsPage.SetActive(false);

        startCam.Priority = 10;
        settingsCam.Priority = 0;
        creditsCam.Priority = 0;

        settingsBtn.onClick.AddListener(OnSettingsBtnPressed);
        exitBtn.onClick.AddListener(OnExitBtnPressed);
        backBtn.onClick.AddListener(OnBackBtnPressed);
        creditsBackBtn.onClick.AddListener(OnBackBtnPressed);
        creditsBtn.onClick.AddListener(OpenCreditPage);
    }

    private void OnSettingsBtnPressed()
    {
        mainPage.SetActive(false);
        settingsPage.SetActive(true);
        creditsPage.SetActive(false);

        startCam.Priority = 0;
        settingsCam.Priority = 10;
        creditsCam.Priority = 0;

        gen.GeneratePreview();
    }

    private void OnExitBtnPressed()
    {
        Application.Quit();
    }

    private void OpenCreditPage()
    {
        mainPage.SetActive(false);
        settingsPage.SetActive(false);
        creditsPage.SetActive(true);

        startCam.Priority = 0;
        settingsCam.Priority = 0;
        creditsCam.Priority = 10;
    }

    private void OnBackBtnPressed()
    {
        mainPage.SetActive(true);
        settingsPage.SetActive(false);
        creditsPage.SetActive(false);

        startCam.Priority = 10;
        settingsCam.Priority = 0;
        creditsCam.Priority = 0;
    }
}
