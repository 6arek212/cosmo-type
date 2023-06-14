using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardMenu : MonoBehaviour
{
    [SerializeField] private GameObject scoreboardPanel;
    private bool MenuIsActive = false;
    private bool isGameOver = false;
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Tab)) return;

        if (MenuIsActive)
        {
            HideMenu();
        }
        else
        {
            ShowMenu();
        }
    }
 

    public void ShowMenu()
    {
 
        MenuIsActive = true;
        scoreboardPanel.SetActive(true);
    }

    public void HideMenu()
    {
    
        MenuIsActive = false;
        scoreboardPanel.SetActive(false);
    }



}
