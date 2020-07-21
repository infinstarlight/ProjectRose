using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [Tooltip("Image component dispplaying current health")]
    public Image healthFillImage;

    Health m_PlayerHealth;

    private void Start()
    {
        IS_PlayerCharacterController IS_PlayerCharacterController = GameObject.FindObjectOfType<IS_PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullFindObject<IS_PlayerCharacterController, PlayerHealthBar>(IS_PlayerCharacterController, this);

        m_PlayerHealth = IS_PlayerCharacterController.GetComponent<Health>();
        DebugUtility.HandleErrorIfNullGetComponent<Health, PlayerHealthBar>(m_PlayerHealth, this, IS_PlayerCharacterController.gameObject);
    }

    void Update()
    {
        // update health bar value
        healthFillImage.fillAmount = m_PlayerHealth.currentHealth / m_PlayerHealth.maxHealth;
    }
}
