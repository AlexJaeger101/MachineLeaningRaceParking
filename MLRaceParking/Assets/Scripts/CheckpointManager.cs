using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheckpointManager : MonoBehaviour
{
    private List<Checkpoint> mCheckpointList;
    private List<int> mNextCheckpointIndexList;
    public List<Transform> mCarList;
    public bool mShouldLap = false;


    void Awake()
    {
        Transform checkpointTrans = transform.Find("CheckpointHolder");
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
            Debug.Log("Wrong Checkpoint!!!! :(");
            return;
        }

        Debug.Log("Hit correct checkpoint!!! :)");

        ++mNextCheckpointIndexList[mCarList.IndexOf(car)];
        if (mShouldLap && (index > (mCheckpointList.Count - 1)))
        {
            mNextCheckpointIndexList[mCarList.IndexOf(car)] = 0;
        }
    }

}
