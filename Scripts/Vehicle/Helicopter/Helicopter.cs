using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Helicopter : Vehicle {
    
    [SerializeField] private HelicopterRotor[] rotors;
    [SerializeField] private float maxSpeed = 150f;
    [SerializeField] private bool keepAltitude = true;
    
    [Header("Arcade settings")]
    [SerializeField] private float arcadeUpForce = 100f;
    [SerializeField] private float turningSpeed = 50f;
    [SerializeField] private float arcadeForwardForce = 1000f;
    [SerializeField] private float arcadeBackwardForce = 1000f;
    [SerializeField] private float arcadeLeftForce = 1000f;
    [SerializeField] private float arcadeRightForce = 1000f;

    private void Awake()
    {
        if (keepAltitude)
            GetComponent<Rigidbody>().useGravity = false;
        if (controlType == ControlType.ARCADE) {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    protected override void OnMove() {
        if (controlType == ControlType.ARCADE) {
            ArcadeControl();            
        } else if (controlType == ControlType.REALISTIC) {
            RealisticControl();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void ArcadeControl() {
        var speed = GetComponent<Rigidbody>().velocity.magnitude;
        var upSpeed = GetUpSpeed();
        var turnSpeed = GetTurnSpeed();
        var forwardSpeed = GetForwardSpeed();
        var rightSpeed = GetRightSpeed();
        
        foreach (var rotor in rotors) {
            if (rotor.axe == AxeRotor.X) {
                if (rotor.invertRotation)
                    rotor.rotor.transform.Rotate(-Vector3.right * Time.deltaTime * (1000f + (5f * upSpeed)));
                else
                    rotor.rotor.transform.Rotate(Vector3.right * Time.deltaTime * (1000f + (5f * upSpeed)));
            } else if (rotor.axe == AxeRotor.Y) {
                if (rotor.invertRotation)
                    rotor.rotor.transform.Rotate(-Vector3.up * Time.deltaTime * (1000f + (5f * upSpeed)));
                else
                    rotor.rotor.transform.Rotate(Vector3.up * Time.deltaTime * (1000f + (5f * upSpeed)));
            } else if (rotor.axe == AxeRotor.Z) {
                if (rotor.invertRotation)
                    rotor.rotor.transform.Rotate(-Vector3.forward * Time.deltaTime * (1000f + (5f * upSpeed)));
                else
                    rotor.rotor.transform.Rotate(Vector3.forward * Time.deltaTime * (1000f + (5f * upSpeed)));
            }
        }
    }
    
    private void RealisticControl() {
        
    }
    
    private float GetUpSpeed() {
        bool isFlyingUp = false;
        bool isFlyingDown = false;
        
        if (Input.GetKey(KeyCode.LeftShift)) {
            isFlyingUp = true;
        }
        
        if (Input.GetKey(KeyCode.LeftControl)) {
            isFlyingDown = true;
        }
        
        if (isFlyingUp) {
            GetComponent<Rigidbody>().AddForce(transform.up * arcadeUpForce);
        }
        
        if (isFlyingDown) {
            GetComponent<Rigidbody>().AddForce(-transform.up * arcadeUpForce);
        }
        
        return Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.up);
    }
    
    private float GetTurnSpeed() {
        
        bool canTurn = true;
        bool turningLeft = false;
        bool turningRight = false;
        float turnSpeed = 0f;
        
        if (Input.GetKey(KeyCode.A) && canTurn) {
            turningLeft = true;
        }
        if (Input.GetKey(KeyCode.E) && canTurn) {
            turningRight = true;
        }
        
        if (turningLeft) {
            transform.Rotate(Vector3.up * Time.deltaTime * turningSpeed);
            turnSpeed = turningSpeed;
        }

        if (!turningRight) return turnSpeed;
        transform.Rotate(-Vector3.up * Time.deltaTime * turningSpeed);
        turnSpeed = -turningSpeed;

        return turnSpeed;
    }
    
    private float GetForwardSpeed() {
        float vertical = -Input.GetAxis("Horizontal");
        if (vertical > 0) {
            GetComponent<Rigidbody>().AddForce(transform.forward * vertical * arcadeForwardForce);
        } else if (vertical < 0) {
            GetComponent<Rigidbody>().AddForce(transform.forward * vertical * arcadeBackwardForce);
        }
        if (maxSpeed < GetComponent<Rigidbody>().velocity.magnitude)
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * maxSpeed;

        return Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.forward);
    }
    
    private float GetRightSpeed() {
        float horizontal = Input.GetAxis("Vertical");
        if (horizontal > 0) {
            GetComponent<Rigidbody>().AddForce(transform.right * horizontal * arcadeRightForce);
        } else if (horizontal < 0) {
            GetComponent<Rigidbody>().AddForce(transform.right * horizontal * arcadeLeftForce);
        }
        return Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.right);
    }
}

[Serializable]
public class HelicopterRotor {
    public GameObject rotor;
    public bool invertRotation = false;
    public AxeRotor axe;
}

[Serializable]
public enum AxeRotor {
    X,
    Y,
    Z
}