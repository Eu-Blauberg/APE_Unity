using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtachMenu : MonoBehaviour
{
    private GameObject MenuCanvas;
    private GameObject MainMenuWindow;
    private GameObject OptionMenuWindow;
    private GameObject ItemMenuWindow;
    private MasterMenu masterMenu;

    void OnEnable()
    {
        MenuCanvas = this.gameObject;
        MainMenuWindow = MenuCanvas.transform.Find("MainMenu").gameObject;
        OptionMenuWindow = MenuCanvas.transform.Find("OptionMenu").gameObject;
        ItemMenuWindow = MenuCanvas.transform.Find("ItemMenu").gameObject;

        masterMenu = new MasterMenu();
        masterMenu.AtachGameObject(MenuCanvas, MainMenuWindow, OptionMenuWindow, ItemMenuWindow);
        masterMenu.Initialization();
    }
}
