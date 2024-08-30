
using System.Collections.Generic;
using CuaHang.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Core
{
    public class UISettingsMenu : UIPanel
    {
        [SerializeField] AudioMixer audioMixer;
        [SerializeField] bool _enableMenuSettings;
        [SerializeField] TMP_Dropdown resolutionDropdown;
        [SerializeField] TMP_Dropdown _dropDownGraphics;
        [SerializeField] Toggle _toggleFullScreen;
        [SerializeField] Slider _sliderVolume;
        [SerializeField] Resolution[] resolutions; // Array to store available screen resolutions

        private GameSettingsData _gameSettingsData = new GameSettingsData();


        void Start()
        {
            SetResolution();

            // bat event gamesettingstats
            GameSettingStats._OnDataChange += gameSettingsData =>
            {
                LoadSettings(gameSettingsData);
            };

            // tắt menu setting khi mới bắt đầu 
            _enableMenuSettings = false;
            _panelContent.gameObject.SetActive(_enableMenuSettings);
        }

        private void LoadSettings(GameSettingsData data)
        {
            SetVolume(data._masterVolume);
            SetQuality(data._qualityIndex);
            SetFullscreen(data._isFullScreen);
            SetResolution(data._currentResolutionIndex);

            // UI
            if (_toggleFullScreen) _toggleFullScreen.isOn = data._isFullScreen;
            if (_sliderVolume) _sliderVolume.value = data._masterVolume;
            if (_dropDownGraphics) _dropDownGraphics.value = data._qualityIndex;
        }

        private void SetResolution()
        {
            resolutions = Screen.resolutions; // Get all available screen resolutions from the system

            resolutionDropdown.ClearOptions(); // Clear any existing options in the dropdown

            List<string> options = new List<string>(); // Create a list to store resolution strings

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height; // Format resolution as a string

                options.Add(option); // Add the resolution string to the options list

                // if (resolutions[i].width == Screen.currentResolution.width &&
                //     resolutions[i].height == Screen.currentResolution.height)
                // {
                //     currentResolutionIndex = i; // Set the index of the current resolution
                // }
            }

            resolutionDropdown.AddOptions(options); // Add the resolution options to the dropdown
        }

        public void SetResolution(int currentResolutionIndex)
        {
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

            _gameSettingsData._currentResolutionIndex = currentResolutionIndex;
            SaveSettingsData();
        }

        public void SetActiveMenuSettings()
        {
            _enableMenuSettings = !_enableMenuSettings;
            _panelContent.gameObject.SetActive(_enableMenuSettings);
        }

        public void SetVolume(float volume)
        {
            audioMixer.SetFloat("volume", volume);

            _gameSettingsData._masterVolume = volume;
            SaveSettingsData();
        }

        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);

            _gameSettingsData._qualityIndex = qualityIndex;
            SaveSettingsData();
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;

            _gameSettingsData._isFullScreen = isFullscreen;
            SaveSettingsData();
        }

        private void SaveSettingsData()
        {
            GameSettings.Instance._gameSettingStats.SetGameSettingsData(_gameSettingsData);
        }
    }
}
