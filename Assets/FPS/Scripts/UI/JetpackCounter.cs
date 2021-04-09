using UnityEngine;
using UnityEngine.UI;

public class JetpackCounter : MonoBehaviour
{
    [Tooltip("Image component representing jetpack fuel")]
    public Image jetpackFillImage;
    [Tooltip("Canvas group that contains the whole UI for the jetack")]
    public CanvasGroup mainCanvasGroup;
    [Tooltip("Component to animate the color when empty or full")]
    public FillBarColorChange fillBarColorChange;

    Jetpack m_Jetpack;
    //The player we're pulling data from
    public GameObject desiredPlayer;

    void Awake()
    {
        
       // m_Jetpack = FindObjectOfType<Jetpack>();
       // DebugUtility.HandleErrorIfNullFindObject<Jetpack, JetpackCounter>(m_Jetpack, this);

        fillBarColorChange.Initialize(1f, 0f);
    }

    void Update()
    {
        if(desiredPlayer != null)
        {
            m_Jetpack = desiredPlayer.GetComponent<Jetpack>();
        }
        mainCanvasGroup.gameObject.SetActive(m_Jetpack.isJetpackUnlocked);

        if (m_Jetpack.isJetpackUnlocked)
        {
            jetpackFillImage.fillAmount = m_Jetpack.currentFillRatio;
            fillBarColorChange.UpdateVisual(m_Jetpack.currentFillRatio);
        }
    }
}
