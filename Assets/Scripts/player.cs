using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도

    [Header("Jump Settings")]
    public float jumpHeight = 6.5f; // 점프 높이 (Y 방향 속도)

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isGrounded = false; // 현재 바닥에 닿아 있는지 여부

    [Header("Animation Controller")]
    public RuntimeAnimatorController idleController; // 대기 애니메이션
    public RuntimeAnimatorController jumpController; // 점프 애니메이션
    public RuntimeAnimatorController runController;  // 달리기 애니메이션
    private Animator animator;

    [Header("Knockback Settings")]
    private bool isKnockback = false;       // 현재 넉백 상태 여부
    private float knockbackForceX = 17f;    // 넉백 수평 힘
    private float knockbackForceY = 4f;     // 넉백 수직 힘
    private float knockbackDuration = 3f;   // 넉백 지속 시간 (초)

    void Start()
    {
        // 필요한 컴포넌트 초기화
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // 물리 회전 고정 (캐릭터가 넘어지지 않도록)
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 시작 시 대기 애니메이션 적용
        if (idleController != null)
            animator.runtimeAnimatorController = idleController;
    }

    void Update()
    {
        float moveInput = 0f;

        // 넉백 중에는 플레이어 조작 불가
        if (!isKnockback)
        {
            // 좌우 방향키 입력 감지
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveInput = -1f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveInput = 1f;
            }

            // 이동 방향에 따라 스프라이트 좌우 반전
            if (moveInput > 0f)
            {
                spriteRenderer.flipX = false; // 오른쪽
            }
            else if (moveInput < 0f)
            {
                spriteRenderer.flipX = true;  // 왼쪽
            }

            // 수평 이동 속도 적용 (수직 속도는 유지)
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            // 스페이스바 점프 (바닥에 있을 때만)
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
                isGrounded = false;
                animator.runtimeAnimatorController = jumpController;
            }
        }

        // 바닥에 있고 넉백 상태가 아닐 때 애니메이션 전환
        if (isGrounded && !isKnockback)
        {
            if (moveInput != 0f)
            {
                // 움직이는 중이면 달리기 애니메이션
                if (animator.runtimeAnimatorController != runController)
                    animator.runtimeAnimatorController = runController;
            }
            else
            {
                // 정지 중이면 대기 애니메이션
                if (animator.runtimeAnimatorController != idleController)
                    animator.runtimeAnimatorController = idleController;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ground 태그이거나 충돌 법선이 위쪽을 향하면 착지로 판정
        if (collision.gameObject.CompareTag("Ground") || collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;

            // 넉백 중이 아닐 때만 대기 애니메이션으로 복귀
            if (!isKnockback && animator != null && idleController != null)
            {
                animator.runtimeAnimatorController = idleController;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 스파이크에 닿으면 넉백 코루틴 실행 (넉백 중 중복 실행 방지)
        if (other.CompareTag("Spike") && !isKnockback)
        {
            StartCoroutine(KnockbackRoutine(other.transform.position));
        }
        // 결승 지점에 도달하면 엔딩 씬으로 이동
        else if (other.CompareTag("Finish"))
        {
            SceneManager.LoadScene("Scene_7");
        }
    }

    IEnumerator KnockbackRoutine(Vector3 spikePosition)
    {
        isKnockback = true;
        isGrounded = false;

        // 스파이크 위치 기준으로 플레이어가 어느 방향으로 튕길지 결정
        float knockbackDirection = transform.position.x > spikePosition.x ? 1f : -1f;

        // 기존 속도 초기화 후 넉백 힘 적용
        rb.linearVelocity = Vector2.zero;
        rb.linearVelocity = new Vector2(knockbackDirection * knockbackForceX, knockbackForceY);

        // 넉백 지속 시간만큼 대기
        yield return new WaitForSeconds(knockbackDuration);

        // 넉백 상태 해제
        isKnockback = false;
    }
}