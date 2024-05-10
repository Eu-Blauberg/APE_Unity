using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterMenu : MonoBehaviour
{
    [SerializeField] GameObject MainMenuWindow;
    [SerializeField] GameObject OptionMenuWindow;


    void Start()
    {
        MainMenuWindow.SetActive(true);
        OptionMenuWindow.SetActive(false);
        return;
    }

    public void OpenOptionMenu()
    {
        Debug.Log("OpenOptionMenu");
        MainMenuWindow.SetActive(false);
        OptionMenuWindow.SetActive(true);
        return;
    }

    public IEnumerator CloseOptionMenu()
    {
        Debug.Log("CloseOptionMenu");
        OptionMenuWindow.SetActive(false);
        yield return new WaitForSecondsRealtime(0.01f);
        MainMenuWindow.SetActive(true);
        yield return null;
    }
    
    public void CloseMainMenu()
    {
        Debug.Log("CloseMainMenu");
        Destroy(transform.parent.gameObject);
        Time.timeScale = 1;
    }
}
