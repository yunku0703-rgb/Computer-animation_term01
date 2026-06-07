using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps; // 타일맵 시스템을 쓰기 위해 필요합니다!

public class BlinkTilemapOnly : MonoBehaviour
{
    [Header("시간 설정 (초)")]
    private float visibleTime = 3.0f;   // 타일맵이 보일 시간
    private float invisibleTime = 2.0f; // 타일맵이 안 보일 시간

    private TilemapRenderer tilemapRenderer;

    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
            tilemapRenderer.enabled = true;
            yield return new WaitForSeconds(visibleTime);
            tilemapRenderer.enabled = false;
            yield return new WaitForSeconds(invisibleTime);
        }
    }
}