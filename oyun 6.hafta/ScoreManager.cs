using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Skor metin alaný
    public GameObject gameOverSceen;
    PickupManager pickups;

    int totalPickups;
    int score;

    private void Start()
    {
        pickups = FindFirstObjectByType<PickupManager>();
        totalPickups = pickups.amount;
        UpdateScore();
    }

    public void CollectPickup()
    {
        score++;
        UpdateScore();

        if(score >= totalPickups)
        {
            gameOverSceen.SetActive(true);
            Time.timeScale = 0f; // Oyunu durdur
        }
    }

    public void UpdateScore()
    {
        scoreText.text = "Skor : " + score.ToString();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}