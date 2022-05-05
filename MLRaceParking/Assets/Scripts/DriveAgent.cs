using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class DriveAgent : Agent
{
    [SerializeField] private Transform mParkingSpot;
    [SerializeField] private DrivingController mCar;
    [SerializeField] private CheckpointManager mCheckpointManager;
    [SerializeField] private Transform mStartPoint;

    void Start()
    {
        mCheckpointManager.mCorrectCheckpointEvent += CorrectCheckpointEvent;
        mCheckpointManager.mWrongCheckpointEvent += WrongCheckpointEvent;
    }

    void Awake()
    {
        mCar = gameObject.GetComponent<DrivingController>();
    }

    private void CorrectCheckpointEvent(object sender, System.EventArgs ev)
    {
        AddReward(1.0f);
    }

    private void WrongCheckpointEvent(object sender, System.EventArgs ev)
    {
        AddReward(-1.0f);
    }

    public override void OnEpisodeBegin()
    {
        transform.position = mStartPoint.position;
        transform.forward = mStartPoint.forward;
        mCheckpointManager.ResetCheckpoints();
        mCar.StopCar();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(mParkingSpot.transform);

        Vector3 checkpointForward = mCheckpointManager.GetNextCheckpointTranfrom(transform).forward;
        float dir = Vector3.Dot(transform.forward, checkpointForward);
        sensor.AddObservation(dir);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        float accelInput = 0.0f;
        float turnInput = 0.0f;

        //Assign given acceleration from AI
        switch (vectorAction[0])
        {
            //Dont move
            case 0:
                accelInput = 0.0f;
                break;

            //Go forward
            case 1:
                accelInput = 1.0f;
                break;
            
            //Go backwards
            case 2:
                accelInput = -1.0f;
                break;
        }

        //Assign given rotation from AI
        switch (vectorAction[1])
        {

            //Dont turn
            case 0:
                turnInput = 0.0f;
                break;

            //turn right
            case 1:
                turnInput = 1.0f;
                break;

            //turn left
            case 2:
                turnInput = -1.0f;
                break;
        }


        mCar.SetCarInput(accelInput, turnInput);
    }

    public override void Heuristic(float[] actionsOut)
    {
        float[] continueousAction = actionsOut;

        int accelInput = 0;
        int turnInput = 0;
        
        //Forward and Backwards
        if (Input.GetKey(KeyCode.UpArrow)) { accelInput = 1; }
        if (Input.GetKey(KeyCode.DownArrow)) { accelInput = 2; }

        //Right and Left
        if (Input.GetKey(KeyCode.RightArrow)) { turnInput = 1; }
        if (Input.GetKey(KeyCode.LeftArrow)) { turnInput = 2; }

        continueousAction[0] = accelInput;
        continueousAction[1] = turnInput;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallTrigger"))
        {
            SetReward(-5.0f);
            EndEpisode();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ParkingSpot"))
        {
            SetReward(5.0f);
            EndEpisode();
        }
    }
}
