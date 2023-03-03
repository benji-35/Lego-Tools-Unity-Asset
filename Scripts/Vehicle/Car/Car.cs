using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Car : Vehicle {
    
    [Header("Car settings")]
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float motorForce = 100f;
    [SerializeField] private float brakeForce = 100f;
    [SerializeField] private CarWheel[] wheels;
    [SerializeField] private bool invertVertical = true;
    
    [Header("Rescue settings")]
    [SerializeField] private bool haveGyro = false;
    [SerializeField] private CarGyro[] gyros;

    private bool isGyroOn = false;
    
    private string speedText = "Speed: ";
    
    protected override void OnMove() {
        if (Input.GetKeyDown(KeyCode.G) && haveGyro) {
            isGyroOn = !isGyroOn;
            if (isGyroOn) {
                StartCoroutine(gyroOn());
            }
        }
        
        float motor = 0f;
        float steer = 0f;
        float brake = 0f;
        
        float speed = GetComponent<Rigidbody>().velocity.magnitude;
        if (speed < maxSpeed) {
            if (invertVertical)
                motor = -Input.GetAxis("Vertical") * motorForce;
            else
                motor = Input.GetAxis("Vertical") * motorForce;
        }
        
        if (Input.GetKey(KeyCode.Space)) {
            brake = brakeForce;
        }
        
        steer = Input.GetAxis("Horizontal") * maxSteerAngle;
        
        foreach (var wheel in wheels) {
            if (wheel.isMotor) {
                wheel.collider.motorTorque = motor;
            }
            
            if (wheel.isSteering) {
                wheel.collider.steerAngle = steer;
            }
            
            wheel.collider.brakeTorque = brake;
            
            Quaternion quat;
            Vector3 position;
            wheel.collider.GetWorldPose(out position, out quat);
            if (wheel.wheel != null) {
                wheel.wheel.transform.position = position;
                wheel.wheel.transform.rotation = quat;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.R)) {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        
        speedText = "Speed: " + speed.ToString("0.00");
        Debug.Log(speedText);
    }

    IEnumerator gyroOn() {
        while (isGyroOn) {
            foreach (var gyro in gyros) {
                gyro.Toggle();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    protected override void OnStart() {
        foreach (var gyro in gyros) {
            gyro.SetOff();
        }        
    }
}

[Serializable]
public class CarGyro {
    public GameObject gyro;
    public Color colorOn;
    public Color colorOff;
    public bool isOn = false;
    
    public void SetOn() {
        isOn = true;
        gyro.GetComponent<Renderer>().material.color = colorOn;
    }
    
    public void SetOff() {
        isOn = false;
        gyro.GetComponent<Renderer>().material.color = colorOff;
    }
    
    public void Toggle() {
        if (isOn) {
            SetOff();
        } else {
            SetOn();
        }
    }
}

[Serializable]
public class CarWheel {
    public WheelCollider collider;
    public GameObject wheel;
    public bool isSteering = false;
    public bool isMotor = false;
}
