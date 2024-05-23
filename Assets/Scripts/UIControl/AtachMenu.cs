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

    void OnEnable()
    {
        MenuCanvas = this.gameObject;
        MainMenuWindow = MenuCanvas.transform.Find("MainMenu").gameObject;
        OptionMenuWindow = MenuCanvas.transform.Find("OptionMenu").gameObject;
        ItemMenuWindow = MenuCanvas.transform.Find("ItemMenu").gameObject;

        MasterMenu masterMenu = GameObject.Find("MasterMenu").GetComponent<MasterMenu>();
        masterMenu.AtachGameObject(MenuCanvas, MainMenuWindow, OptionMenuWindow, ItemMenuWindow);
        masterMenu.Initialization();
    }
}
