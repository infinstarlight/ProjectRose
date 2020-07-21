using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform m_PlayerTransform;
    private Vector3 m_OriginalOffset;

    void Start()
    {
        IS_PlayerCharacterController IS_PlayerCharacterController = GameObject.FindObjectOfType<IS_PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullFindObject<IS_PlayerCharacterController, FollowPlayer>(IS_PlayerCharacterController, this);

        m_PlayerTransform = IS_PlayerCharacterController.transform;

        m_OriginalOffset = transform.position - m_PlayerTransform.position;
    }

    void LateUpdate()
    {
        transform.position = m_PlayerTransform.position + m_OriginalOffset;
    }
}
