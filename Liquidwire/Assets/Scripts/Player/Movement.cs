using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public float speed = 6.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    public bool isLocked = false;

    private bool _hasMoved;
    private float _movementTutorialTimer;
    [SerializeField] private float _movementTutorialDelay;
    [SerializeField] private GameObject _movementTutorialObject;

    private void Start()
    {
        _movementTutorialObject.SetActive(false);
    }

    void Update()
    {
        
        // Debug.Log( "transform "+ "y: "+ transform.position.y + "x: " +  transform.position.x  + "z: " +  transform.position.z);
        if (!isLocked)
        {
            CharacterController controller = GetComponent<CharacterController>();
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            moveDirection.y -= gravity * Time.deltaTime;
            if (!_hasMoved && moveDirection != new Vector3(0, 0, 0))
            {
                _hasMoved = true;
                if (_movementTutorialObject.activeSelf)
                {
                    StartCoroutine(FadeTutorialHelp(4f));
                }
            }
            controller.Move(moveDirection * Time.deltaTime);
        }

        if (!_hasMoved && !isLocked)
        {
            _movementTutorialTimer += Time.deltaTime;
            if (_movementTutorialTimer >= _movementTutorialDelay && !_movementTutorialObject.activeSelf)
            {
                _movementTutorialObject.SetActive(true);
            }
        }
    }

    private IEnumerator FadeTutorialHelp(float time)
    {
        TextMeshProUGUI textMesh = _movementTutorialObject.GetComponent<TextMeshProUGUI>();
        float timeSpent = 0;
        Color newColour = textMesh.color;
        while (textMesh.color.a > 0)
        {
            timeSpent += Time.deltaTime;
         
            newColour.a = 1 - timeSpent / time;
            textMesh.color = newColour;
            yield return new WaitForEndOfFrame();
        }
        _movementTutorialObject.SetActive(false);
    }

    public void changeLock()
    {
        isLocked = !isLocked;
    }
    
}