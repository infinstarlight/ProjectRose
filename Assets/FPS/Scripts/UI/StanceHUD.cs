using UnityEngine;
using UnityEngine.UI;

public class StanceHUD : MonoBehaviour
{
    [Tooltip("Image component for the stance sprites")]
    public Image stanceImage;
    [Tooltip("Sprite to display when standing")]
    public Sprite standingSprite;
    [Tooltip("Sprite to display when crouching")]
    public Sprite crouchingSprite;

    private void Start()
    {
        IS_PlayerCharacterController character = FindObjectOfType<IS_PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullFindObject<IS_PlayerCharacterController, StanceHUD>(character, this);
        character.onStanceChanged += OnStanceChanged;

        OnStanceChanged(character.isCrouching);
    }

    void OnStanceChanged(bool crouched)
    {
        stanceImage.sprite = crouched ? crouchingSprite : standingSprite;
    }
}
