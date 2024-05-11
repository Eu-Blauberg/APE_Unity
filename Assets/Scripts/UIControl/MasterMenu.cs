using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterMenu : MonoBehaviour
{
    [SerializeField] GameObject MainMenuWindow;
    [SerializeField] GameObject OptionMenuWindow;
    [SerializeField] GameObject ItemMenuWindow;


    void Start()
    {
        MainMenuWindow.SetActive(true);
        OptionMenuWindow.SetActive(false);
        ItemMenuWindow.SetActive(false);
        return;
    }

    public IEnumerator OpenOptionMenu()
    {
        Debug.Log("OpenOptionMenu");
        MainMenuWindow.SetActive(false);
        OptionMenuWindow.SetActive(true);
        ItemMenuWindow.SetActive(false);
        yield return null;
    }

    public IEnumerator CloseOptionMenu()
    {
        Debug.Log("CloseOptionMenu");
        OptionMenuWindow.SetActive(false);
        MainMenuWindow.SetActive(true);
        ItemMenuWindow.SetActive(false);
        yield return null;
    }

    public IEnumerator OpenItemMenu()
    {
        Debug.Log("OpenItemMenu");
        MainMenuWindow.SetActive(false);
        OptionMenuWindow.SetActive(false);
        ItemMenuWindow.SetActive(true);
        yield return null;
    }

    public IEnumerator CloseItemMenu()
    {
        Debug.Log("CloseItemMenu");
        MainMenuWindow.SetActive(true);
        OptionMenuWindow.SetActive(false);
        ItemMenuWindow.SetActive(false);
        yield return null;
    }
    
    public void CloseMainMenu()
    {
        Debug.Log("CloseMainMenu");
        Destroy(transform.parent.gameObject);
        Time.timeScale = 1;
    }
}
