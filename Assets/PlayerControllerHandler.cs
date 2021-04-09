using UnityEngine.InputSystem;
using System.Linq;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine;

public class PlayerControllerHandler : MonoBehaviour
{

    PlayerInput playerInput;
    IS_PlayerInputHandler InputHandler;

    private void Awake()   
    {
        playerInput = GetComponent<PlayerInput>();    
        
        var players = FindObjectsOfType<IS_PlayerInputHandler>();
        var index = playerInput.playerIndex;
        InputHandler = players.FirstOrDefault(p => p.GetPlayerIndex() == index);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

}


