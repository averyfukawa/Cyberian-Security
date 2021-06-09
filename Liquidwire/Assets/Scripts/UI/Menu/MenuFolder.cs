using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFolder : MonoBehaviour
{
    public FolderState currentState = FolderState.Down;
    private Vector3 _basePosition;
    private Coroutine _movement;
    
    public enum FolderState
    {
        Down,
        Up,
        MovingDown,
        MovingUp
    }

    public void SetBase()
    {
        _basePosition = transform.position;
    }

    public void ToggleFolder(bool isUp)
    {
        if (isUp)
        {
            if (_movement != null)
            {
                StopCoroutine(_movement);
            }
            _movement = StartCoroutine(MoveFolder(isUp));
        }
        else
        {
            if (_movement != null)
            {
                StopCoroutine(_movement);
            }
            _movement = StartCoroutine(MoveFolder(isUp));
        }
    }

    private IEnumerator MoveFolder(bool isUp)
    {
        if (!isUp)
        {
            transform.LeanMove(_basePosition, .2f);
            currentState = FolderState.MovingDown;
            yield return new WaitForSeconds(.2f);
            currentState = FolderState.Down;
        }
        else
        {
            transform.LeanMove(_basePosition + new Vector3(0, .04f, 0), .2f);
            currentState = FolderState.MovingUp;
            yield return new WaitForSeconds(.2f);
            currentState = FolderState.Up;
        }
    }
}
