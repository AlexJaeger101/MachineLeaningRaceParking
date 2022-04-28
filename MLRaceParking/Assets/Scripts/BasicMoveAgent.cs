using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class BasicMoveAgent : Agent
{
    public float mMoveSpeed = 1.0f;

    [SerializeField] private Transform mTarget;
    [SerializeField] private Material mPositiveActionMat;
    [SerializeField] private Material mNegitiveActionMat;
    [SerializeField] private MeshRenderer mFloorMeshRenderer;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-1.5f, 2.8f), 11.0f, Random.Range(5.25f, 9.4f));
        mTarget.localPosition = new Vector3(Random.Range(-1.5f, 2.8f), 11.0f, Random.Range(5.25f, 9.4f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(mTarget.transform.localPosition);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        float moveX = vectorAction[0];
        float moveZ = vectorAction[1];

        transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * mMoveSpeed;
    }

    public override void Heuristic(float[] actionsOut)
    {
        float[] continuousActions = actionsOut;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            SetReward(10.0f);
            mFloorMeshRenderer.material = mPositiveActionMat;
            Debug.Log("REACHED GOAL YAY");
            EndEpisode();
        }
        else if (other.CompareTag("Wall"))
        {
            SetReward(-5.0f);
            mFloorMeshRenderer.material = mNegitiveActionMat;
            Debug.Log("no goal :(");
            EndEpisode();
        }
    }
}
