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
    public IS_PlayerCharacterController character;

    private void Start()
    {
        //character = GetComponentInParent<IS_PlayerCharacterController>();
       // DebugUtility.HandleErrorIfNullFindObject<IS_PlayerCharacterController, StanceHUD>(character, this);
        character.onStanceChanged += OnStanceChanged;

        OnStanceChanged(character.isCrouching);
    }

    void OnStanceChanged(bool crouched)
    {
        stanceImage.sprite = crouched ? crouchingSprite : standingSprite;
    }
}
