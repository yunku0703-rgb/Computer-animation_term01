using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform player;

    public float upperBoundary = 8.5f;
    public float lowerBoundary = -7f;

    private bool isTransitioning = false;

    void Update()
    {
        if (player == null || isTransitioning) return;

        if (player.position.y >= upperBoundary)
        {
            LoadSceneByOffset(1, lowerBoundary + 0.5f);
        }
        else if (player.position.y <= lowerBoundary)
        {
            LoadSceneByOffset(-1, upperBoundary - 0.5f);
        }
    }

    void LoadSceneByOffset(int indexOffset, float nextPlayerY)
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + indexOffset;

        if (nextSceneIndex >= 0 && nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            isTransitioning = true;

            PlayerPrefs.SetFloat("NextPlayerY", nextPlayerY);
            PlayerPrefs.SetFloat("NextPlayerX", player.position.x);

            SceneManager.LoadScene(nextSceneIndex);
        }
    }

}