using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarEngine : MonoBehaviour {

    //public Transform path;
    [Header("Wheel Colliders")]
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    [Header("Engine")]
    public float maxMotorTorque = 80f;
    public float maxBrakeTorque = 150f;
    public float maxSpeed = 100f;
    public float maxSteerAngle = 45f;
    public float turnSpeed = 5f;
    public Vector3 centerOfMass;

    [Header("Sensors")]
    public float sensorLength = 3f;
    public Vector3 frontSensorPosition = new Vector3(0f, 0.2f, 0.5f);
    public float frontSideSensorPosition = 0.2f;
    public float frontSensorAngle = 30f;

    [Header("Braking Visual")]
    public Texture2D textureNormal;
    public Texture2D textureBraking;
    public Renderer carRenderer;
    public bool _isBraking = false;


    public Vector3 _currentTargetPosition { get; private set; } = new Vector3(0, 0, 0);

    private float _currentSpeed;
    private bool _avoiding = false;
    private float _targetSteerAngle = 0;
    private bool _needStay = true;

    private void Start() {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
    }

    private void FixedUpdate() {
        Sensors();
        ApplySteer();
        Drive();
        Braking();
        LerpToSteerAngle();
    }

    private void Sensors() {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.up * frontSensorPosition.y;
		_avoiding = false;

		//front right sensor
		sensorStartPos += transform.right * frontSideSensorPosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)) {
				Debug.DrawLine(sensorStartPos, hit.point);
                _avoiding = true;
		}

        //front right angle sensor
        //else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)) {
        //        Debug.DrawLine(sensorStartPos, hit.point);
        //        _avoiding = true;
        //}

        //front left sensor
        sensorStartPos -= transform.right * frontSideSensorPosition * 2;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)) {
                Debug.DrawLine(sensorStartPos, hit.point);
                _avoiding = true;
        }

        //front left angle sensor
        //else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)) {
        //        Debug.DrawLine(sensorStartPos, hit.point);
        //        _avoiding = true;
        //}

		if (_avoiding)
		{
            _isBraking = true;
            maxBrakeTorque = 700;
		}
        else if (!_needStay)
		{
            _isBraking = false;
            maxBrakeTorque = 300;
		}

	}

    private void ApplySteer() {
		if (_avoiding) return;
		Vector3 relativeVector = transform.InverseTransformPoint(_currentTargetPosition);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        _targetSteerAngle = newSteer;
    }

    private void Drive() {
        _currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;

        if (_currentSpeed < maxSpeed && !_isBraking) {
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque;
        } else {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
        }
    }

    private void Braking() {
        if (_isBraking) {
            carRenderer.material.mainTexture = textureBraking;
            wheelRL.brakeTorque = maxBrakeTorque;
            wheelRR.brakeTorque = maxBrakeTorque;
        } else {
            carRenderer.material.mainTexture = textureNormal;
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
        }
    }
    private void LerpToSteerAngle() {
        wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, _targetSteerAngle, Time.deltaTime * turnSpeed);
        wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, _targetSteerAngle, Time.deltaTime * turnSpeed);
    }

    public void SetTarget(Vector3 targetPosition)
	{
        _currentTargetPosition = targetPosition;
	}

    public void SetBrake(bool brake)
	{
        _isBraking = brake;
        _needStay = brake;
	}
}
