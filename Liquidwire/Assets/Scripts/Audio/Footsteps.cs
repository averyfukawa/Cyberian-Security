using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Footsteps : MonoBehaviour
{
    [EventRef] public string regularFootstepSound;
    [EventRef] public string carpetFootstepSound;
    private bool _playerIsMoving;
    private bool _playerOnCarpet;
    public float walkingSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(FootstepSounds), 0, walkingSpeed);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetAxis("Vertical") >= 0.01f || Input.GetAxis("Horizontal") >= 0.01f ||
            Input.GetAxis("Vertical") <= -0.01f || Input.GetAxis("Horizontal") <= -0.01f)
        {
            _playerIsMoving = true;
        }
        else if (Input.GetAxis("Vertical") == 0.0f || Input.GetAxis("Horizontal") == 0.0f)
        {
            _playerIsMoving = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "carpet")
        {
            _playerOnCarpet = true;
            Debug.Log("Enter Carpet");
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "carpet")
        {
            _playerOnCarpet = true;
            Debug.Log("On Carpet");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "carpet")
        {
            _playerOnCarpet = false;
            Debug.Log("Out of Carpet");
        }
    }

    private void RegularFootsteps()
    {
        if (_playerIsMoving)
        {
            RuntimeManager.PlayOneShot(regularFootstepSound);
        }
    }

    private void CarpetFootsteps()
    {
        if (_playerIsMoving)
        {
            RuntimeManager.PlayOneShot(carpetFootstepSound);
        }
    }

    private void FootstepSounds()
    {
        if (_playerOnCarpet)
        {
            CarpetFootsteps();
        }
        else
        {
            RegularFootsteps();
        }
    }

    private void OnDisable()
    {
        _playerIsMoving = false;
    }
}
