using System;
using System.Collections;
using System.Collections.Generic;
using kap35.lego;
using Unity.VisualScripting;
using UnityEngine;

public class Vehicle : MonoBehaviour {
    [SerializeField] protected GameObject[] cameras;
    [SerializeField] protected VehicleSeat[] seats;
    [SerializeField] protected ControlType controlType = ControlType.ARCADE;
    
    private GameObject player;
    private int currentCameraIndex = 0;

    private bool protectToTp = true;
    // Start is called before the first frame update
    void Start() {
        HideAllCameras();
        player = GameObject.FindWithTag("Player");
        if (player == null) {
            Debug.LogError("Player not found");
            enabled = false;
        }
        
        bool driverFound = false;
        foreach (var seat in seats) {
            seat.SetVehicle(this);
            seat.GetOut();
            if (seat.isDriver) {
                driverFound = true;
            }
        }

        if (!driverFound) {
            Debug.LogWarning("No driver seat found");
        }

        protectToTp = false;
        OnStart();
    }

    // Update is called once per frame
    void Update() {
        if (isSeated()) {
            if (IsDriver()) OnMove();
            if (Input.GetKeyDown(KeyCode.Plus)) {
                NextCamera();
            } else if (Input.GetKeyDown(KeyCode.Minus)) {
                PreviousCamera();
            }

            if (Input.GetKeyDown(KeyCode.Return)) {
                foreach (var seat in seats) {
                    if (seat.isOccupied) seat.GetOut();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        OnFixedMove();
    }

    protected virtual void OnMove() { }
    protected virtual void OnStart() { }
    
    protected virtual void OnFixedMove() { }

    public void GetIn() {
        player.SetActive(false);
        cameras[currentCameraIndex].SetActive(true);
    }

    public void GetOut(Transform point) {
        if (!protectToTp) {
            player.SetActive(true);
            player.transform.position = point.position;
        }
        HideAllCameras();
    }

    public void NextCamera()
    {
        currentCameraIndex++;
        if (currentCameraIndex >= cameras.Length) {
            currentCameraIndex = 0;
        }
        HideAllCameras();
        cameras[currentCameraIndex].SetActive(true);
    }
    
    public void PreviousCamera()
    {
        currentCameraIndex--;
        if (currentCameraIndex < 0) {
            currentCameraIndex = cameras.Length - 1;
        }
        HideAllCameras();
        cameras[currentCameraIndex].SetActive(true);
    }

    private void HideAllCameras() {
        foreach (var camera in cameras) {
            camera.SetActive(false);
        }
    }

    public bool IsDriver() {
        foreach (var seat in seats) {
            if (seat.isDriver) {
                return seat.isOccupied;
            }
        }

        return false;
    }
    
    public bool isSeated() {
        foreach (var seat in seats) {
            if (seat.isOccupied) {
                return true;
            }
        }

        return false;
    }
    
    public ControlType GetControlType() {
        return controlType;
    }
}

[Serializable]
public class VehicleSeat {
    public Interactable enterPoint;
    public Transform exitPoint;
    public bool isOccupied = false;
    public bool isDriver = false;

    private Vehicle vehicle;

    public void SetVehicle(Vehicle vehicle) {
        this.vehicle = vehicle;
        enterPoint.AddOnInteractEvent(GetIn);
    }
    
    public void GetIn() {
        isOccupied = true;
        vehicle.GetIn();
    }
    
    public void GetOut() {
        isOccupied = false;
        vehicle.GetOut(exitPoint);
    }
}

[Serializable]
public enum ControlType {
    ARCADE,
    REALISTIC,
}