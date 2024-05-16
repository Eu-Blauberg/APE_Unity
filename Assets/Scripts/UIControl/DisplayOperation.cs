using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class DisplayOperation : MonoBehaviour
{
    [SerializeField] private GameObject Rover;
    [SerializeField] private GameObject OperationText;
    [SerializeField] private GameObject TradeCanvas; // クリックアクション
    [SerializeField] InputActionAsset inputActionAsset;
    [SerializeField] private PlayerData playerData;

    private const float distanceThreshold = 2.0f;
    private float distance;
    private bool isDisplayed = false;
    private Vector3 PlayerPoint;
    private Vector3 RoverPoint;
    private GameInputs gameInputs;


    void Awake()
    {
        gameInputs = new GameInputs();
        gameInputs.UIControls.Click.performed += OnClickPerformed;
        gameInputs.Enable();
    }

    void Start()
    {
        RoverPoint = Rover.GetComponent<Transform>().position;
    }
    // Update is called once per frame
    void Update()
    {
        PlayerPoint = playerData._position;
        distance = Vector3.Distance(PlayerPoint, RoverPoint);

        if(distance < distanceThreshold && !isDisplayed)
        {
            ChangeDisplay();
            isDisplayed = true;
        }
        else if(distance > distanceThreshold && isDisplayed)
        {
            ChangeDisplay();
            isDisplayed = false;
        }
    }

    private void ChangeDisplay()
    {
        OperationText.SetActive(!isDisplayed);
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        if(isDisplayed && Time.timeScale != 0)
        {
            GameObject instantedItemWindow = Instantiate(TradeCanvas);
            instantedItemWindow.name = ("TradeCanvas");
            Time.timeScale = 0;
        }
    }
}
