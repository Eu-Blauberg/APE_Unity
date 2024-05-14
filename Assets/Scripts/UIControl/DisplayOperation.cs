using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class DisplayOperation : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Rover;
    [SerializeField] private GameObject OperationText;
    [SerializeField] private GameObject TradeCanvas; // クリックアクション
    [SerializeField] InputActionAsset inputActionAsset;

    private const float distanceThreshold = 2.0f;
    private float distance;
    private bool isDisplayed = false;
    private Vector2 PlayerPoint;
    private Vector2 RoverPoint;
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
        PlayerPoint = Player.GetComponent<Transform>().position;
        distance = Vector2.Distance(PlayerPoint, RoverPoint);

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
        if(isDisplayed)
        {
            GameObject instantedItemWindow = Instantiate(TradeCanvas);
            Time.timeScale = 0;
        }
    }
}
