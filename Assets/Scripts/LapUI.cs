using UnityEngine;
using TMPro;

public class LapUI : MonoBehaviour
{
    public TextMeshProUGUI lapTimeText;
    public TextMeshProUGUI sectorText;
    public TextMeshProUGUI lapCounterText;

    private float lapStartTime;
    private float currentLapTime;

    public int currentLap = 1;
    
    


    void Start()
    {
        lapStartTime = Time.time;
    }

    void Update()
    {
        currentLapTime = Time.time - lapStartTime;
        lapTimeText.text = "Lap: " + currentLapTime.ToString("F2");
    }

    public void UpdateSector(float sectorTime)
    {
        sectorText.text = "Sector: " + sectorTime.ToString("F2");
    }

    public void NewLap(float lapTime)
    {
        currentLap++;
        lapCounterText.text = "Lap " + currentLap;
        lapStartTime = Time.time;
    }
}