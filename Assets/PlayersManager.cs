using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine;
[RequireComponent(typeof(PlayerInput))]
public class PlayersManager : MonoBehaviour
{

    public GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        PlayerInput.GetPlayerByIndex(0).defaultControlScheme = "Keyboard/Mouse";
    }

    private void Update()
    {
        if(!PlayerInput.isSinglePlayer)
        {
            PlayerInput.GetPlayerByIndex(0).defaultControlScheme = "Keyboard/Mouse";
            PlayerInput.GetPlayerByIndex(1).defaultControlScheme = "Gamepad";
        }
     
    }
}
