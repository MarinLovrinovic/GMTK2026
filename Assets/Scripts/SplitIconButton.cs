using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SplitIconButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private string playSceneName;
    
    [Header("UI References")]
    [SerializeField] private GameObject singleIconObject;
    [SerializeField] private RectTransform leftPartTransform;
    [SerializeField] private RectTransform rightPartTransform;
    [SerializeField] private GameObject revealedTextObject;

    [Header("Idle Sway Settings")]
    [SerializeField] private float swaySpeed = 2.5f;
    [SerializeField] private float maxSwayAngle = 12.0f;

    [Header("Split Animation Settings")]
    [SerializeField] private Vector2 leftPartOpenOffset = new Vector2(-60f, 0f);
    [SerializeField] private Vector2 rightPartOpenOffset = new Vector2(60f, 0f);
    [SerializeField] private float transitionDuration = 0.25f;
    
    [Header("Button Event")]
    public UnityEvent onClick = new UnityEvent();
    
    private Coroutine activeTransitionCoroutine;
    private bool isHovered = false;

    private void Start()
    {
        SetIdleState();
    }

    private void Update()
    {
        if (!isHovered && singleIconObject != null && singleIconObject.activeSelf)
        {
            float zAngle = Mathf.Sin(Time.time * swaySpeed) * maxSwayAngle;
            singleIconObject.transform.localRotation = Quaternion.Euler(0f, 0f, zAngle);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;

        if (activeTransitionCoroutine != null)
            StopCoroutine(activeTransitionCoroutine);

        singleIconObject.SetActive(false);
        leftPartTransform.gameObject.SetActive(true);
        rightPartTransform.gameObject.SetActive(true);

        activeTransitionCoroutine = StartCoroutine(AnimateSplit(true));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;

        if (activeTransitionCoroutine != null)
            StopCoroutine(activeTransitionCoroutine);

        activeTransitionCoroutine = StartCoroutine(AnimateSplit(false));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }

    private IEnumerator AnimateSplit(bool opening)
    {
        Vector2 leftStart = leftPartTransform.anchoredPosition;
        Vector2 rightStart = rightPartTransform.anchoredPosition;

        Vector2 leftTarget = opening ? leftPartOpenOffset : Vector2.zero;
        Vector2 rightTarget = opening ? rightPartOpenOffset : Vector2.zero;

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / transitionDuration);

            leftPartTransform.anchoredPosition = Vector2.Lerp(leftStart, leftTarget, t);
            rightPartTransform.anchoredPosition = Vector2.Lerp(rightStart, rightTarget, t);

            yield return null;
        }

        leftPartTransform.anchoredPosition = leftTarget;
        rightPartTransform.anchoredPosition = rightTarget;

        if (!opening)
            SetIdleState();
    }

    private void SetIdleState()
    {
        leftPartTransform.anchoredPosition = Vector2.zero;
        rightPartTransform.anchoredPosition = Vector2.zero;

        leftPartTransform.gameObject.SetActive(false);
        rightPartTransform.gameObject.SetActive(false);

        singleIconObject.SetActive(true);
    }

    public void LoadPlayScene()
    {
        SceneManager.LoadScene(playSceneName, LoadSceneMode.Additive);    
    }

    public void QuitApplication()
    {
        Application.Quit();
    }   
}
