using TMPro;
using UnityEngine;

public class ScrollText : MonoBehaviour
{
    public float scrollSpeed = 50f; // 每秒移动的像素数
    public float startYOffset = 200f; // 起始偏移
    public float endYOffset = -200f; // 终点偏移
    public bool autoStart = true;

    private RectTransform rectTransform;
    private float startY;
    private float endY;
    private bool isScrolling;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        startY = Screen.height + startYOffset;
        endY = endYOffset;

        // 设置初始位置在屏幕外上方
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, startY);

        if (autoStart)
            StartScrolling();
    }

    void Update()
    {
        var pos = rectTransform.anchoredPosition;
        pos.y -= scrollSpeed * Time.deltaTime;
        rectTransform.anchoredPosition = pos;
    }

    public void StartScrolling()
    {
        isScrolling = true;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, startY);
    }

    public void StopScrolling()
    {
        isScrolling = false;
    }
}