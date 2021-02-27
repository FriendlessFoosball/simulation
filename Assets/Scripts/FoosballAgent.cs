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

    //TODO: Tune these
    float linMoveMultiplier = 2f;//0.25f;
    float angMoveMultiplier = 10f;

    float ballStartMultiplier = 20f; //20f;

    float maxLinMove = 4.64f;

    float frozenTime = 0f;
    float maxFrozenTime = 10f;
    
    float invert;

    // Start is called before the first frame update
    void Start() {

    }

    //Reset ball to middle with random force
    public override void OnEpisodeBegin() {
        frozenTime = 0f;
        ball.transform.localPosition = Vector3.zero;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().AddRelativeForce(Random.onUnitSphere * ballStartMultiplier);
    }

    public override void CollectObservations(VectorSensor sensor) {
        //Ball Observation
        invert = player1 ? 1f : -1f;

        sensor.AddObservation(invert * ball.transform.localPosition.x);
        sensor.AddObservation(invert * ball.transform.localPosition.y);
        sensor.AddObservation(invert * ball.GetComponent<Rigidbody>().velocity.x);
        sensor.AddObservation(invert * ball.GetComponent<Rigidbody>().velocity.y);
        //sensor.AddObservation(ball.GetComponent<Rigidbody>().velocity.z);
        //Possibly add observe goal position?

        //Paddle Observations
        sensor.AddObservation(offense.transform.localPosition.z);
        sensor.AddObservation(offense.GetComponent<Rigidbody>().velocity.z);
        sensor.AddObservation(offense.transform.localRotation.z);
        sensor.AddObservation(offense.GetComponent<Rigidbody>().angularVelocity.z);
        sensor.AddObservation(goalie.transform.localPosition.z);
        sensor.AddObservation(goalie.GetComponent<Rigidbody>().velocity.z);
        sensor.AddObservation(goalie.transform.localRotation.z);
        sensor.AddObservation(goalie.GetComponent<Rigidbody>().angularVelocity.z);

        float spin = Mathf.Abs(offense.GetComponent<Rigidbody>().angularVelocity.z / angMoveMultiplier) + Mathf.Abs(goalie.GetComponent<Rigidbody>().angularVelocity.z / angMoveMultiplier);

        AddReward(-0.01f * 0.50f * spin);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        //Actions, size = 4
        Vector3 offenseLinMove = Vector3.forward * actions.ContinuousActions[0] * linMoveMultiplier;
        
        // if (player1 && Mathf.Abs(offense.transform.localPosition.z + offenseLinMove.z) < maxLinMove) {
        //     offense.transform.Translate(offenseLinMove);
        // } else if (!player1 && Mathf.Abs(offense.transform.localPosition.z + offenseLinMove.z) < maxLinMove) {
        //     offense.transform.Translate(offenseLinMove);
        // }

        offense.GetComponent<Rigidbody>().AddRelativeForce(offenseLinMove, ForceMode.VelocityChange);

        Vector3 offenseAngMove = Vector3.forward * actions.ContinuousActions[1] * angMoveMultiplier;
        //offense.transform.Rotate(offenseAngMove);
        offense.GetComponent<Rigidbody>().AddRelativeTorque(offenseAngMove, ForceMode.VelocityChange);

        Vector3 goalieLinMove = Vector3.forward * actions.ContinuousActions[2] * linMoveMultiplier;
        // if (player1 && Mathf.Abs(goalie.transform.localPosition.z + goalieLinMove.z) < maxLinMove) {
        //     goalie.transform.Translate(goalieLinMove);
        // } else if (!player1 && Mathf.Abs(goalie.transform.localPosition.z + goalieLinMove.z) < maxLinMove) {
        //     goalie.transform.Translate(goalieLinMove);
        // }

        goalie.GetComponent<Rigidbody>().AddRelativeForce(goalieLinMove, ForceMode.VelocityChange);

        Vector3 goalieAngMove = Vector3.forward * actions.ContinuousActions[3] * angMoveMultiplier;
        // goalie.transform.Rotate(goalieAngMove);
        goalie.GetComponent<Rigidbody>().AddRelativeTorque(goalieAngMove, ForceMode.VelocityChange);

        //Rewards
        float distanceToGoal = Vector3.Distance(ball.transform.localPosition, goal.localPosition);
        //Reached Target. TODO: Change this to work with the trigger  instead:
        if (distanceToGoal <  3f) {
            SetReward(1.0f);
            EndEpisode();
        }

        float distanceToOpponentGoal = Vector3.Distance(ball.transform.localPosition, opponentGoal.localPosition);
        if (distanceToOpponentGoal <  3f) {
            SetReward(-1.0f);
            //EndEpisode();
        }

        //TODO: neg reward for owngoal? Reset episode if ball falls off?
        //Reset if ball stops moving for 10 secs or falls off plane
        if (ball.GetComponent<Rigidbody>().IsSleeping() || ball.transform.position.y < -5) {
            if (frozenTime != 0f && Time.time - frozenTime > maxFrozenTime) {
                SetReward(-1.0f);
                EndEpisode();
            } else if (frozenTime == 0f) {
                frozenTime = Time.time;
            }
        }
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
