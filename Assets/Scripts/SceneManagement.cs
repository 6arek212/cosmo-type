using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance;
   [SerializeField] private TMP_Dropdown dropdown;
    private void Awake()
    {
        Instance = this;
    }
    public void LoadScene(ModeType mode)
    {
       string sceneName = mode.DisplayName();
        SceneManager.LoadScene(sceneName);
    }
    public void LoadScene(string mode)
    {
        SceneManager.LoadScene(mode);
    }

    public void StartModeMainMenu()
    {
        int selectedValue = dropdown.value;
        string mode = dropdown.options[selectedValue].text.Replace(" ", "");
        SceneManager.LoadScene(mode);
    }

}
