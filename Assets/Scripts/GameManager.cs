using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_Big : MonoBehaviour
{
    public Transform player;
    public float upperBoundary = 8f;   // 위쪽 경계선 Y 좌표
    public float lowerBoundary = -6f;  // 아래쪽 경계선 Y 좌표

    private bool isTransitioning = false; // 씬 전환 중복 방지 플래그

    void Update()
    {
        // 플레이어가 없거나 이미 전환 중이면 실행 안 함
        if (player == null || isTransitioning) return;

        // 플레이어가 위쪽 경계를 넘으면 다음 씬(위)으로 이동
        if (player.position.y >= upperBoundary)
        {
            LoadSceneByOffset(1, lowerBoundary + 0.5f); // 다음 씬의 아래쪽에서 등장
        }
        // 플레이어가 아래쪽 경계를 넘으면 이전 씬(아래)으로 이동
        else if (player.position.y <= lowerBoundary)
        {
            LoadSceneByOffset(-1, upperBoundary - 0.5f); // 이전 씬의 위쪽에서 등장
        }
    }

    void LoadSceneByOffset(int indexOffset, float nextPlayerY)
    {
        // 현재 씬 인덱스에 오프셋을 더해 이동할 씬 인덱스 계산
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + indexOffset;

        // 이동할 씬이 유효한 범위 내에 있는지 확인
        if (nextSceneIndex >= 0 && nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            isTransitioning = true;

            // 다음 씬에서 플레이어가 등장할 Y, X 위치를 저장
            PlayerPrefs.SetFloat("NextPlayerY", nextPlayerY);
            PlayerPrefs.SetFloat("NextPlayerX", player.position.x);

            // 씬 전환 실행
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}