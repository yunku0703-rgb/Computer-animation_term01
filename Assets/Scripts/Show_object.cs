using System.Collections;
using UnityEngine;

public class Show_object: MonoBehaviour
{
    [Header("시간 설정 (초)")]
    public float visibleTime = 2.0f;   // 보일 시간
    public float invisibleTime = 2.0f; // 안 보일 시간

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
            //보이게 설정
            SetVisibility(true);
            yield return new WaitForSeconds(visibleTime);

            //안 보이게 설정
            SetVisibility(false);
            yield return new WaitForSeconds(invisibleTime);
        }
    }

    void SetVisibility(bool isVisible)
    {
        if (spriteRenderer != null)
            spriteRenderer.enabled = isVisible;
    }
}