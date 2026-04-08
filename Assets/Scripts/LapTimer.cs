using Unity.Mathematics;
using UnityEngine;

public class LapTimer : MonoBehaviour
{
    public string playerName = "Player";

    public int nextCheckpoint = 0;
    public int lap = 0;
    public int totalCheckpoints = 4;
    public int maxLaps = 5;
    private Vector3 lastCheckpointPos;
    public Vector3 GetLastCheckpointPos()
    {
        return lastCheckpointPos;
    }

    private float lapStartTime;
    private float sectorStartTime;
    private quaternion lastCheckpointRot;
    public quaternion GetLastCheckpointRot()
    {
        return lastCheckpointRot;
    }

    public float bestLapTime = Mathf.Infinity;

    public LapUI lapUI;

    void Start()
    {
        totalCheckpoints = totalCheckpoints = FindObjectsByType<Checkpoint>().Length;
        lastCheckpointPos = transform.position;

        lapStartTime = Time.time;
        sectorStartTime = Time.time;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Checkpoint cp = other.GetComponent<Checkpoint>();

        if (cp != null && cp.checkpointIndex == nextCheckpoint)
        {
            float sectorTime = Time.time - sectorStartTime;
            lapUI.UpdateSector(sectorTime);

            sectorStartTime = Time.time;
            nextCheckpoint++;
            lastCheckpointPos = cp.transform.position;
            lastCheckpointRot = cp.transform.rotation;

            if (nextCheckpoint == totalCheckpoints)
            {
                float lapTime = Time.time - lapStartTime;
                lap++;

                // 🏆 BEST LAP CHECK
                if (lapTime < bestLapTime)
                {
                    bestLapTime = lapTime;
                }

                lapUI.NewLap(lapTime);

                Debug.Log(playerName + " Lap " + lap + ": " + lapTime.ToString("F2"));

                nextCheckpoint = 0;
                lapStartTime = Time.time;
                sectorStartTime = Time.time;

                // 🏁 Race finished
                if (lap >= maxLaps)
                {
                    RaceManager.Instance.PlayerFinished(this);
                }
            }
        }
    }
}
