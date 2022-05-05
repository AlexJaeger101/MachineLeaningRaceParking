using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheckpointManager : MonoBehaviour
{
    public event EventHandler mCorrectCheckpointEvent;
    public event EventHandler mWrongCheckpointEvent;


    [Header("Checkpoint Data")]
    private List<Checkpoint> mCheckpointList;
    private List<int> mNextCheckpointIndexList;
    public List<Transform> mCarList;
    public bool mShouldLap = false;


    void Awake()
    {
        Transform checkpointTrans = transform.Find("CheckpointHolder");
        mCheckpointList = new List<Checkpoint>();
        foreach(Transform checkpoint in checkpointTrans)
        {
            Checkpoint temp = checkpoint.GetComponent<Checkpoint>();

            temp.SetCheckpoints(this);
            mCheckpointList.Add(temp);
        }

        mNextCheckpointIndexList = new List<int>();
        foreach(Transform car in mCarList)
        {
            mNextCheckpointIndexList.Add(0);
        }
    }

    public void CarHitCheckpoint(Checkpoint hitCheckpoint, Transform car)
    {
        int index = mNextCheckpointIndexList[mCarList.IndexOf(car)];

        //Check if we hit the worng checkpoint
        if (mCheckpointList.IndexOf(hitCheckpoint) != index)
        {
            mWrongCheckpointEvent?.Invoke(this, EventArgs.Empty);
            return;
        }

        //Handle going through correct checkpoint
        mCorrectCheckpointEvent?.Invoke(this, EventArgs.Empty);

        ++mNextCheckpointIndexList[mCarList.IndexOf(car)];
        if (mShouldLap && (index > (mCheckpointList.Count - 1)))
        {
            mNextCheckpointIndexList[mCarList.IndexOf(car)] = 0;
        }
    }

    public void ResetCheckpoints()
    {
        mNextCheckpointIndexList.Clear();
        foreach (Transform car in mCarList)
        {
            mNextCheckpointIndexList.Add(0);
        }
    }

    public Transform GetNextCheckpointTranfrom(Transform car)
    {
        int index = mNextCheckpointIndexList[mCarList.IndexOf(car)];
        return mCheckpointList[index].transform;
    }

}
