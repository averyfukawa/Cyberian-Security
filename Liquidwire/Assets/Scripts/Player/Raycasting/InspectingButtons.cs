using System.Collections;
using System.Collections.Generic;
using Player.Raycasting;
using UnityEngine;
using UnityEngine.UI;

public class InspectingButtons : MonoBehaviour
{
    private Button[] _childs;
    private RotatableObject[] _rotatables;
    // Start is called before the first frame update
    void Start()
    {
        _childs = gameObject.GetComponentsInChildren<Button>();
        _rotatables = FindObjectsOfType<RotatableObject>();
        foreach (var item in _rotatables)
        {
            item.SetButtons(_childs);
        }
        
        foreach (var child in _childs)
        {
            child.gameObject.SetActive(false);
        }
    }

    private RotatableObject Check()
    {
        foreach (var item in _rotatables)
        {
            if (item.GetActive())
            {
                return item;
            }
        }

        return null;
    }

    public void InspectUp()
    {
        var current = Check();
        current.ClickUp();
    }
    public void InspectDown()
    {
        var current = Check();
        current.ClickDown();
    }
}
