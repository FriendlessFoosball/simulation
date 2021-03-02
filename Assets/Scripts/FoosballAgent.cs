using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class FoosballAgent : Agent {
    public GameObject ball;
    public Transform goal;
    public Transform opponentGoal;

    public bool player1;
    public GameObject offense;
    public GameObject goalie;

    public GameObject opponentOffense;
    public GameObject opponentGoalie;

    //TODO: Tune these
    const float linMoveMultiplier = 2f;//0.25f;
    const float angMoveMultiplier = 20f; //10f;

    const float maxVelocity = 16f;

    const float maxLinMove = 4.64f;

    //float frozenTime = 0f;
    //const float maxFrozenTime = 10f;

    const float maxAngularVelocity = 30f;
    const float fieldWidth = 12.835f;
    const float fieldLength = 18.47f;

    const float maxBallVelocity = 30f;
    
    float invert;
    float existentialPenalty;
    
    [HideInInspector]
    public float timePenalty;

    Rigidbody offenseRb;
    Rigidbody goalieRb;
    Rigidbody ballRb;
    
    Rigidbody oOffenseRb;
    Rigidbody oGoalieRb;

    BallScorer bs;

    // Start is called before the first frame update
    void Start() {
        existentialPenalty = 1f / MaxStep;

        offenseRb = offense.GetComponent<Rigidbody>();
        goalieRb = goalie.GetComponent<Rigidbody>();
        ballRb = ball.GetComponent<Rigidbody>();

        oOffenseRb = opponentOffense.GetComponent<Rigidbody>();
        oGoalieRb = opponentGoalie.GetComponent<Rigidbody>();

        bs = ball.GetComponent<BallScorer>();

        invert = player1 ? 1f : -1f;
    }

    //Reset ball to middle with random force
    public override void OnEpisodeBegin() {
        timePenalty = 0f;
        // backup for max timestep
        bs.ResetBall();
    }

    public override void CollectObservations(VectorSensor sensor) {
        //Ball Observation (4)
        sensor.AddObservation(invert * ball.transform.localPosition.x / fieldLength);
        sensor.AddObservation(invert * ball.transform.localPosition.z / fieldWidth);
        sensor.AddObservation(invert * ballRb.velocity.x / maxBallVelocity);
        sensor.AddObservation(invert * ballRb.velocity.z / maxBallVelocity);
        //sensor.AddObservation(ball.GetComponent<Rigidbody>().velocity.z);
        //Possibly add observe goal position?

        //Paddle Observations (8)
        sensor.AddObservation(offense.transform.localPosition.z / maxLinMove);
        sensor.AddObservation(invert * offenseRb.velocity.z / maxVelocity);
        sensor.AddObservation(offense.transform.localRotation.z / 180f);
        sensor.AddObservation(invert * offenseRb.angularVelocity.z / maxAngularVelocity);
        sensor.AddObservation(goalie.transform.localPosition.z / 180f);
        sensor.AddObservation(invert * goalieRb.velocity.z / maxVelocity);
        sensor.AddObservation(goalie.transform.localRotation.z / 180f);
        sensor.AddObservation(invert * goalieRb.angularVelocity.z / maxAngularVelocity);

        //Opponent Observations (4)
        sensor.AddObservation(-1f * opponentOffense.transform.localPosition.z / maxLinMove);
        sensor.AddObservation(-1f * opponentOffense.transform.localRotation.z / 180f);
        sensor.AddObservation(-1f * opponentGoalie.transform.localPosition.z / maxLinMove);
        sensor.AddObservation(-1f * opponentGoalie.transform.localRotation.z / 180f);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        //Actions, size = 4
        Vector3 offenseLinMove = Vector3.forward * actions.ContinuousActions[0] * linMoveMultiplier;

        if (Mathf.Abs(invert * offenseRb.velocity.z + actions.ContinuousActions[0] * linMoveMultiplier) > maxVelocity) {
            Vector3 maxSpeed = Vector3.forward * maxVelocity;
            if (invert * offenseRb.velocity.z < 0)
                maxSpeed *= -1f;
            offenseLinMove = maxSpeed - invert * offenseRb.velocity;
        }
        offenseRb.AddRelativeForce(offenseLinMove, ForceMode.VelocityChange);

        Vector3 offenseAngMove = Vector3.forward * actions.ContinuousActions[1] * angMoveMultiplier;
        offenseRb.AddRelativeTorque(offenseAngMove, ForceMode.VelocityChange);

        Vector3 goalieLinMove = Vector3.forward * actions.ContinuousActions[2] * linMoveMultiplier;

        if (Mathf.Abs(invert * goalieRb.velocity.z + actions.ContinuousActions[2] * linMoveMultiplier) > maxVelocity) {
            Vector3 maxSpeed = Vector3.forward * maxVelocity;
            if (invert * goalieRb.velocity.z < 0)
                maxSpeed *= -1f;
            goalieLinMove = maxSpeed - invert * goalieRb.velocity;
        }
        goalieRb.AddRelativeForce(goalieLinMove, ForceMode.VelocityChange);

        Vector3 goalieAngMove = Vector3.forward * actions.ContinuousActions[3] * angMoveMultiplier;
        goalieRb.AddRelativeTorque(goalieAngMove, ForceMode.VelocityChange);

        float spin = Mathf.Abs(offenseRb.angularVelocity.z / maxAngularVelocity) + Mathf.Abs(goalieRb.angularVelocity.z / maxAngularVelocity);

        AddReward(-0.01f * 0.50f * spin);
        //AddReward(-0.001f);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");

        continuousActionsOut[2] = Input.GetAxis("Vertical");
        continuousActionsOut[3] = Input.GetAxis("Horizontal");

        if (!player1) {
            continuousActionsOut[0] *= -1f;
            continuousActionsOut[1] *= -1f;
            continuousActionsOut[2] *= -1f;
            continuousActionsOut[3] *= -1f;
        }
    }
}
