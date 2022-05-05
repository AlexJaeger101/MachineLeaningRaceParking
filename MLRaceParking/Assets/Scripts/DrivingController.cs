using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingController : MonoBehaviour
{
    private Rigidbody mRB;

    public float mForwardAcc = 25.0f;
    public float mBackAcc = 15.0f;
    public float mMaxSpeed;

    public float mTurnSpeed = 5.0f;
    public float mGravity = 10.0f;

    private float mDragGround;

    private float mForwardInput = 0.0f;
    private float mTurnInput = 0.0f;

    public bool mIsAIControlled = false;
    private bool mIsGrounded = false;

    public LayerMask mGroundMask;
    public float mGroundRayLength = 0.5f;
    public Transform mGroundRayStart;

    // Start is called before the first frame update
    void Start()
    {
        mRB = gameObject.GetComponent<Rigidbody>();
        mDragGround = mRB.drag;
    }

    // Update is called once per frame
    void Update()
    {
        if (!mIsAIControlled)
        {
            mForwardInput = Input.GetAxisRaw("Vertical");
            mTurnInput = Input.GetAxisRaw("Horizontal");
        }
    }

    void FixedUpdate()
    {
        mIsGrounded = false;
        mRB.drag = 0.1f;

        RaycastHit hit;
        if (Physics.Raycast(mGroundRayStart.position, -transform.up, out hit, mGroundRayLength, mGroundMask))
        {
            mIsGrounded = true;
            mRB.drag = mDragGround;

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) *transform.rotation;
        }

        CarMovement();
    }

    private void CarMovement()
    {
        if (!mIsGrounded)
        {
            mRB.AddForce(Vector3.up * -mGravity);
            return;
        }

        float accel = mForwardInput > 0 ? mForwardAcc : mBackAcc;
        float currentForward = mForwardInput * accel;

        //Accelerate the car
        if (Mathf.Abs(currentForward) > 0)
        {
            mRB.AddForce(transform.forward * currentForward, ForceMode.Acceleration);
        }

        //Rotate the car
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0.0f, mTurnInput * mTurnSpeed * Time.deltaTime * mForwardInput, 0.0f));
    }

    public void SetCarInput(float forward, float turn)
    {
        mForwardInput = forward;
        mTurnInput = turn;
    }

    public void StopCar()
    {
        if (mRB.velocity.magnitude < .01)
        {
            mRB.velocity = Vector3.zero;
            mRB.angularVelocity = Vector3.zero;
        }
    }
}
