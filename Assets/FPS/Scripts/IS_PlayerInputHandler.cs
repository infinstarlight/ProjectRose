using UnityEngine;
using UnityEngine.InputSystem;


public class IS_PlayerInputHandler : MonoBehaviour
{
    [Tooltip("Sensitivity multiplier for moving the camera around")]
    public float lookSensitivity = 1f;
    [Tooltip("Additional sensitivity multiplier for WebGL")]
    public float webglLookSensitivityMultiplier = 0.25f;
    [Tooltip("Limit to consider an input when using a trigger on a controller")]
    public float triggerAxisThreshold = 0.4f;
    [Tooltip("Used to flip the vertical input axis")]
    public bool invertYAxis = false;
    [Tooltip("Used to flip the horizontal input axis")]
    public bool invertXAxis = false;

    RoseInputSettings controls;

    GameFlowManager m_GameFlowManager;
    IS_PlayerCharacterController m_IS_PlayerCharacterController;
    bool m_FireInputWasHeld;
    public Vector2 mouseLook;
    public float SmoothingRate = 2.0f;
    private Vector2 lookInput;
    private Vector2 moveInput;
    private Vector2 smoothingVector;

    Keyboard currentKeyboard = Keyboard.current;
    Gamepad currentGamepad = Gamepad.current;

    private bool bIsJumping = false;
    private bool bIsFiring = false;
    private bool bIsAiming = false;
    private bool bIsCrouching = false;

    private bool bIsSprinting = false;

    private void Awake()
    {
        controls = new RoseInputSettings();

    }

    private void Start()
    {
        m_IS_PlayerCharacterController = GetComponent<IS_PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullGetComponent<IS_PlayerCharacterController, IS_PlayerInputHandler>(m_IS_PlayerCharacterController, this, gameObject);
        m_GameFlowManager = FindObjectOfType<GameFlowManager>();
        DebugUtility.HandleErrorIfNullFindObject<GameFlowManager, IS_PlayerInputHandler>(m_GameFlowManager, this);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        InputSystem.onDeviceChange +=
      (device, change) =>
      {
          switch (change)
          {
              case InputDeviceChange.Added:
                  // New Device
                  if (device != null)
                  {
                      Debug.LogWarning("New device added: " + device);
                      InputSystem.AddDevice(device);
                      for (int i = 0; i < Gamepad.all.Count; ++i)
                      {
                          if (device == Gamepad.all.ToArray()[i])
                          {
                              currentGamepad = Gamepad.all.ToArray()[i];
                          }
                      }
                      //If on touchscreen and gamepad is connected
                      //Disable touch controls and use gamepad
                      //Do vice versa
                  }

                  break;
              case InputDeviceChange.Disconnected:
                  // Device got unplugged
                  Debug.LogWarning("Device is disconnected: " + device);
                  //InputSystem.Dis
                  break;
              case InputDeviceChange.Reconnected:
                  // Plugged back in
                  Debug.LogWarning("Device reconnected: " + device);
                  break;
              case InputDeviceChange.Removed:
                  // Remove from Input System entirely; by default, devices stay in the system once discovered
                  //Debug.LogWarning("Device removed: " + device);
                  InputSystem.RemoveDevice(device);
                  break;
              default:
                  // See InputDeviceChange reference for other event types.
                  break;
          }
      };
    }

    private void LateUpdate()
    {
        m_FireInputWasHeld = GetFireInputHeld();
    }

    public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked && !m_GameFlowManager.gameIsEnding;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                {

                    bIsJumping = true;
                }

                break;

            case InputActionPhase.Started:
                {
                    {


                    }
                }
                break;
            case InputActionPhase.Canceled:
                {

                    bIsJumping = false;
                }
                break;
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                {

                    bIsCrouching = !bIsCrouching;
                }

                break;

            case InputActionPhase.Started:
                {
                    {


                    }
                }
                break;
            case InputActionPhase.Canceled:
                {

                    bIsCrouching = false;
                }
                break;
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                {

                    bIsFiring = true;
                }

                break;

            case InputActionPhase.Started:
                {
                    {


                    }
                }
                break;
            case InputActionPhase.Canceled:
                {

                    bIsFiring = false;
                }
                break;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                {

                    bIsSprinting = true;
                }

                break;

            case InputActionPhase.Started:
                {
                    {


                    }
                }
                break;
            case InputActionPhase.Canceled:
                {

                    bIsSprinting = false;
                }
                break;
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                {

                    bIsAiming = true;
                }

                break;

            case InputActionPhase.Started:
                {
                    {


                    }
                }
                break;
            case InputActionPhase.Canceled:
                {

                    bIsAiming = false;
                }
                break;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var moveValue = context.ReadValue<Vector2>();
        moveInput = moveValue;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        var lookValue = context.ReadValue<Vector2>();
        lookInput = lookValue;
        Vector2 LookDirection = new Vector2(lookInput.x, lookInput.y);
       smoothingVector.x = Mathf.Lerp(smoothingVector.x, LookDirection.x, 1f / SmoothingRate);
            smoothingVector.y = Mathf.Lerp(smoothingVector.y, LookDirection.y, 1f / SmoothingRate);
            mouseLook += smoothingVector;
            mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

    }

    public Vector3 GetMoveInput()
    {
        if (CanProcessInput())
        {
            Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

            // constrain move input to a maximum magnitude of 1, otherwise diagonal movement might exceed the max move speed defined
            move = Vector3.ClampMagnitude(move, 1);

            return move;
        }

        return Vector3.zero;
    }

    public float GetLookInputsHorizontal()
    {
        return lookInput.x;
    }

    public float GetLookInputsVertical()
    {
        return lookInput.y;
    }

    public bool GetJumpInputDown()
    {
        if (CanProcessInput())
        {
            return bIsJumping;
        }

        return false;
    }

    public bool GetJumpInputHeld()
    {
        if (CanProcessInput())
        {
            return bIsJumping;
        }

        return false;
    }

    public bool GetFireInputDown()
    {
        return GetFireInputHeld() && !m_FireInputWasHeld;
    }

    public bool GetFireInputReleased()
    {
        return !GetFireInputHeld() && m_FireInputWasHeld;
    }

    public bool GetFireInputHeld()
    {
        if (CanProcessInput())
        {
            bool isGamepad = currentGamepad.rightTrigger.ReadValue() != 0f;
            if (isGamepad)
            {
                return Input.GetAxis(GameConstants.k_ButtonNameGamepadFire) >= triggerAxisThreshold;
            }
            else
            {
                return bIsFiring;
            }
        }

        return false;
    }

    public bool GetAimInputHeld()
    {
        if (CanProcessInput())
        {
            bool isGamepad = currentGamepad.leftTrigger.ReadValue() != 0f;
            bool i = isGamepad ? (currentGamepad.rightTrigger.ReadValue() > 0f) : bIsAiming;
            return i;
        }

        return false;
    }

    public bool GetSprintInputHeld()
    {
        if (CanProcessInput())
        {
            return bIsSprinting;
        }

        return false;
    }

    public bool GetCrouchInputDown()
    {
        if (CanProcessInput())
        {
            return bIsCrouching;
        }

        return false;
    }

    public bool GetCrouchInputReleased()
    {
        if (CanProcessInput())
        {
            return !bIsCrouching;
        }

        return false;
    }

    public int GetSwitchWeaponInput()
    {
        // if (CanProcessInput())
        // {

        //     bool isGamepad = Input.GetAxis(GameConstants.k_ButtonNameGamepadSwitchWeapon) != 0f;
        //     string axisName = isGamepad ? GameConstants.k_ButtonNameGamepadSwitchWeapon : GameConstants.k_ButtonNameSwitchWeapon;

        //     if (Input.GetAxis(axisName) > 0f)
        //         return -1;
        //     else if (Input.GetAxis(axisName) < 0f)
        //         return 1;
        //     else if (Input.GetAxis(GameConstants.k_ButtonNameNextWeapon) > 0f)
        //         return -1;
        //     else if (Input.GetAxis(GameConstants.k_ButtonNameNextWeapon) < 0f)
        //         return 1;
        // }

        return 0;
    }

    public int GetSelectWeaponInput()
    {
        if (CanProcessInput())
        {
            // if (Input.GetKeyDown(KeyCode.Alpha1))
            //     return 1;
            // else if (Input.GetKeyDown(KeyCode.Alpha2))
            //     return 2;
            // else if (Input.GetKeyDown(KeyCode.Alpha3))
            //     return 3;
            // else if (Input.GetKeyDown(KeyCode.Alpha4))
            //     return 4;
            // else if (Input.GetKeyDown(KeyCode.Alpha5))
            //     return 5;
            // else if (Input.GetKeyDown(KeyCode.Alpha6))
            //     return 6;
            // else
            //     return 0;
        }

        return 0;
    }



    float GetMouseOrStickLookAxis(string mouseInputName, string stickInputName)
    {
        if (CanProcessInput())
        {

            // Check if this look input is coming from the mouse
            bool isGamepad = currentGamepad.leftStick.ReadValue() != Vector2.zero;
            float i = isGamepad ? Input.GetAxis(stickInputName) : Input.GetAxisRaw(mouseInputName);

            // handle inverting vertical input
            if (invertYAxis)
                i *= -1f;

            // apply sensitivity multiplier
            i *= lookSensitivity;

            if (isGamepad)
            {
                // since mouse input is already deltaTime-dependant, only scale input with frame time if it's coming from sticks
                i *= Time.deltaTime;
            }
            else
            {
                // reduce mouse input amount to be equivalent to stick movement
                i *= 0.01f;
#if UNITY_WEBGL
                // Mouse tends to be even more sensitive in WebGL due to mouse acceleration, so reduce it even more
                i *= webglLookSensitivityMultiplier;
#endif
            }

            return i;
        }

        return 0f;
    }
}
