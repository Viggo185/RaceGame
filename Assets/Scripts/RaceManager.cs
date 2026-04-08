using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance;

    public LapTimer player1;
    public LapTimer player2;

    private int finishedPlayers = 0;
    public FinishUI finishUI;

    void Awake()
    {
        Instance = this;
    }

    public void PlayerFinished(LapTimer player)
    {
        finishedPlayers++;

        Debug.Log(player.playerName + " finished! Best lap: " + player.bestLapTime.ToString("F2"));

        if (finishedPlayers >= 2)
        {
            EndRace();
        }
    }

    void EndRace()
    {
        Debug.Log("🏁 RACE FINISHED");

        string winnerName;
        float bestLap;

        if (player1.bestLapTime < player2.bestLapTime)
        {
            winnerName = player1.playerName;
            bestLap = player1.bestLapTime;
        }
        else
        {
            winnerName = player2.playerName;
            bestLap = player2.bestLapTime;
        }

        finishUI.ShowWinner(winnerName, bestLap);
    }
}