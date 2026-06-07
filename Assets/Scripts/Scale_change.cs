using UnityEngine;

public class Scale_change : MonoBehaviour
{
    [Header("크기 설정")]
    public float minScale = 0.7f;   // 최소 크기
    public float maxScale = 2f;   // 최대 크기
    public float pulseSpeed = 1f;   // 변화 속도

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        transform.localScale = originalScale * scale;
    }
}