using UnityEngine;

public class Moving_object : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveDistance = 2f;    // 좌우 이동 거리
    public float moveSpeed = 2f;       // 이동 속도

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = new Vector3(startPosition.x + offset, startPosition.y, startPosition.z);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}