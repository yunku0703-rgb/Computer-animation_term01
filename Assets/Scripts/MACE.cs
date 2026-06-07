using UnityEngine;
using System.Collections;

public class MaceObstacle2D : MonoBehaviour
{
    [Header("Movement Speed")]
    [SerializeField] private float fallSpeed = 200f;    // 하강 속도 (엄청 빠르게)
    [SerializeField] private float riseSpeed = 2f;     // 상승 속도 (서서히)

    [Header("Distance & Timing")]
    [SerializeField] private float dropDistance = 5f;  // 내려갈 총 거리
    [SerializeField] private float waitTimeOnGround = 1f; // 바닥에서 멈춰있을 시간
    [SerializeField] private float waitTimeAtTop = 1.5f;  // 위에서 다음 하강까지 대기 시간

    private float startY;   // 처음 시작(최고) 높이
    private float targetY;  // 바닥(최저) 높이

    // 철퇴의 상태를 정의
    private enum MaceState { IDLE, FALLING, GROUND_WAIT, RISING }
    private MaceState currentState = MaceState.FALLING;

    private float timer = 0f;

    void Start()
    {
        startY = transform.position.y;
        targetY = startY - dropDistance; // 시작 위치에서 dropDistance만큼 내려간 곳이 바닥
    }

    void Update()
    {
        switch (currentState)
        {
            case MaceState.FALLING:
                // 빠르게 수직 하강
                transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

                // 바닥 목표치에 도달하면
                if (transform.position.y <= targetY)
                {
                    transform.position = new Vector2(transform.position.x, targetY);
                    timer = 0f;
                    currentState = MaceState.GROUND_WAIT; // 바닥 대기 상태로 전환
                }
                break;

            case MaceState.GROUND_WAIT:
                // 지체시간
                timer += Time.deltaTime;
                if (timer >= waitTimeOnGround)
                {
                    currentState = MaceState.RISING; // 상승 상태로 전환
                }
                break;

            case MaceState.RISING:
                // 원래 위치로 천천히 상승
                transform.Translate(Vector2.up * riseSpeed * Time.deltaTime);

                // 원래 높이에 도달하면
                if (transform.position.y >= startY)
                {
                    transform.position = new Vector2(transform.position.x, startY);
                    timer = 0f;
                    currentState = MaceState.IDLE; // 최상단 대기 상태로 전환
                }
                break;

            case MaceState.IDLE:
                // 위에서 잠시 대기 후 다시 낙하 준비
                timer += Time.deltaTime;
                if (timer >= waitTimeAtTop)
                {
                    currentState = MaceState.FALLING; // 다시 하강
                }
                break;
        }
    }
}