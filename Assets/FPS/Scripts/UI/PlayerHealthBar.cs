using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [Tooltip("Image component dispplaying current health")]
    public Image healthFillImage;

    Health m_PlayerHealth;
    public IS_PlayerCharacterController myPlayerCharacter;

    private void Start()
    {
        //myPlayerCharacter = GetComponentInParent<IS_PlayerCharacterController>();
        //DebugUtility.HandleErrorIfNullFindObject<IS_PlayerCharacterController, PlayerHealthBar>(myPlayerCharacter, this);

     //   m_PlayerHealth = myPlayerCharacter.GetComponent<Health>();
       // DebugUtility.HandleErrorIfNullGetComponent<Health, PlayerHealthBar>(m_PlayerHealth, this, myPlayerCharacter.gameObject);
    }

    void Update()
    {
        if(!m_PlayerHealth)
        {
              m_PlayerHealth = myPlayerCharacter.GetComponent<Health>();
              
        
        }
         // update health bar value
        healthFillImage.fillAmount = m_PlayerHealth.currentHealth / m_PlayerHealth.maxHealth;
       
    }
}
