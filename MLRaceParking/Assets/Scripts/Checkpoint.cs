using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public CheckpointManager mCheckpointManager;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Car"))
        {
            mCheckpointManager.CarHitCheckpoint(this, other.transform);
        }
    }

    public void SetCheckpoints(CheckpointManager checkpoints)
    {
        this.mCheckpointManager = checkpoints;
    }
}
