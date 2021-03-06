﻿using UnityEngine;

public class CameraManager : MonoBehaviour {

    public static CameraManager instance;

    [SerializeField] Camera camera;
    public float cameraSensitivity = 5f;

    public CameraState state;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start () {
        if (!camera)
            camera = Camera.main;
	}

    /// <summary>
    /// Move camera by given delta value.
    /// </summary>
    /// <param name="delta"></param>
    public void MoveCamera(Vector3 delta)
    {
        camera.transform.position += (camera.transform.right * -delta.x + camera.transform.up * -delta.z) * Time.deltaTime * cameraSensitivity;
    }
}
