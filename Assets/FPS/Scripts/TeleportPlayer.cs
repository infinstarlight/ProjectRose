using UnityEngine;

// Debug script, teleports the player across the map for faster testing
public class TeleportPlayer : MonoBehaviour
{
    public KeyCode activateKey = KeyCode.F12;

    IS_PlayerCharacterController m_IS_PlayerCharacterController;

    void Awake()
    {
        m_IS_PlayerCharacterController = FindObjectOfType<IS_PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullFindObject<IS_PlayerCharacterController, TeleportPlayer>(m_IS_PlayerCharacterController, this);
    }

    void Update()
    {
        if (Input.GetKeyDown(activateKey))
        {
            m_IS_PlayerCharacterController.transform.SetPositionAndRotation(transform.position, transform.rotation);
            Health playerHealth = m_IS_PlayerCharacterController.GetComponent<Health>();
            if(playerHealth)
            {
                playerHealth.Heal(999);
            }
        }
    }

}
