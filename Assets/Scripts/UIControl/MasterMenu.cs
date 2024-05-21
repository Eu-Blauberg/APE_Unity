using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterMenu: MonoBehaviour
{
    [SerializeField] GameObject MenuCanvas;
    [SerializeField] GameObject MainMenuWindow;
    [SerializeField] GameObject OptionMenuWindow;
    [SerializeField] GameObject ItemMenuWindow;

    public void AtachGameObject(GameObject menuCanvas, GameObject mainMenuWindow, GameObject optionMenuWindow, GameObject itemMenuWindow)
    {
        Debug.Log("AtachGameObject");
        MenuCanvas = menuCanvas;
        MainMenuWindow = mainMenuWindow;
        OptionMenuWindow = optionMenuWindow;
        ItemMenuWindow = itemMenuWindow;
    }
    public void Initialization()
    {
        Debug.Log("Initialization");
        MainMenuWindow.SetActive(true);
        OptionMenuWindow.SetActive(false);
        ItemMenuWindow.SetActive(false);
    }

    public void CloseMainMenu()
    {
        Debug.Log("CloseMainMenu");
        MenuCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenOptionMenu()
    {
        Debug.Log("OpenOptionMenu");
        MainMenuWindow.SetActive(false);
        OptionMenuWindow.SetActive(true);
        ItemMenuWindow.SetActive(false);
    }

    public void CloseOptionMenu()
    {
        Debug.Log("CloseOptionMenu");
        OptionMenuWindow.SetActive(false);
        MainMenuWindow.SetActive(true);
        ItemMenuWindow.SetActive(false);
    }

    public void OpenItemMenu()
    {
        Debug.Log("OpenItemMenu");
        MainMenuWindow.SetActive(false);
        OptionMenuWindow.SetActive(false);
        ItemMenuWindow.SetActive(true);
    }

    public void CloseItemMenu()
    {
        Debug.Log("CloseItemMenu");
        MainMenuWindow.SetActive(true);
        OptionMenuWindow.SetActive(false);
        ItemMenuWindow.SetActive(false);
    }
}
