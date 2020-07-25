using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerJoinManager : MonoBehaviour
{

    public GameFlowManager myGameFlowManager;
    public GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }
    
    // public void OnJoin(InputAction.CallbackContext context)
    // {
    //     myGameFlowManager.m_Players = FindObjectsOfType<IS_PlayerCharacterController>();
    //     Debug.Log("We have " + myGameFlowManager.m_Players.Length + " number of players!");
    // }

    // private void FixedUpdate()
    // {
    //     // Listening must be enabled explicitly.
    //     ++InputUser.listenForUnpairedDeviceActivity;

    //     // Example of how to spawn a new player automatically when a button
    //     // is pressed on an unpaired device.
    //     InputUser.onUnpairedDeviceUsed +=
    //         (control, eventPtr) =>
    //         {
    //     // Ignore anything but button presses.
    //     if (!(control is ButtonControl))
    //                 return;

    //     // Spawn player and pair device. If the player's actions have control schemes
    //     // defined in them, PlayerInput will look for a compatible scheme automatically.
    //     // PlayerInput.Instantiate(playerPrefab, device: control.device);
    //     //     };
    // }
}
