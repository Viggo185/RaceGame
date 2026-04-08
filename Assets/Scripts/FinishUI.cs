using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinishUI : MonoBehaviour
{
    public GameObject finishPanel;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI bestLapText;
    

    public void ShowWinner(string winnerName, float bestLap)
    {
        finishPanel.SetActive(true);

        winnerText.text = "WINNER: " + winnerName;
        bestLapText.text = "Best Lap: " + bestLap.ToString("F2") + "s";

        Time.timeScale = 0f; 
    }
    public void RestartRace()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}