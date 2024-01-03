using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuScript : MonoBehaviour
{
    public GameObject gameMenu;
    public GameObject hotbarMenu;


    public void OnButtonClick()
    {
        ToggleGameMenu();
    }

     void ToggleGameMenu()
    {
        if (gameMenu != null)
        {
            gameMenu.SetActive(!gameMenu.activeSelf);
        }
    }

     public void ToggleHotbarMenu()
    {
        if (hotbarMenu  != null)
        {
            hotbarMenu.SetActive(!hotbarMenu.activeSelf);
        }
    }
}