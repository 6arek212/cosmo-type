using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
   [SerializeField] private string menuName;
   [SerializeField] private bool open;
    public void Open()
    {
        open = true;
        gameObject.SetActive(true);
 
    }


    public void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }

    public string GetMenuName()
    {
        return menuName;
    }


    public bool IsOpen()
    {
        return open;
    }



}
