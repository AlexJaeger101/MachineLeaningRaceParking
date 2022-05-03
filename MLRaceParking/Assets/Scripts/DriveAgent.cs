using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class DriveAgent : Agent
{
    [SerializeField] private Transform mParkingSpot;
    [SerializeField] private DrivingController mCar;

    public override void OnEpisodeBegin()
    {
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.transform);
        sensor.AddObservation(mParkingSpot.transform);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        float accelInput = vectorAction[0];
        float turnInput = vectorAction[1];

        mCar.SetCarInput(accelInput, turnInput);
    }

    public override void Heuristic(float[] actionsOut)
    {
        float[] continuousActions = actionsOut;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ParkingSpot"))
        {
            //Check if the AI is in bounds of the spot
            //Reward and end episode if it is

            //SetReward(100.0f);
            //EndEpisode();
        }
        else if (other.CompareTag("Checkpoint"))
        {
            SetReward(1.0f);
        }
        else if (other.CompareTag("FallTrigger"))
        {
            SetReward(-5.0f);
            EndEpisode();
        }
    }
}
