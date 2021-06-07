using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _optionPrefab;
    [SerializeField]
    private GameObject _continuePrefab;

    private List<GameObject> _buttons = new List<GameObject>();
    private Action<int> _callback;

    public void CreateOptions(List<string> allOptions, Action<int> callBack)
    {
        _callback = callBack;
        for(int x = 0; x < allOptions.Count; x++)
        {
            GameObject button = Instantiate(_optionPrefab);
            OptionsButtonHandler buttonHandler = button.GetComponent<OptionsButtonHandler>();
            buttonHandler.transform.SetParent(transform,false);
            buttonHandler.SetText(allOptions[x]);
            buttonHandler.SetValueAndButtonCallBack(x, ButtonCallBack);
            _buttons.Add(button);
        }
    }
    
    public void CreateContinue(MessageBoxHud caller)
    {
        GameObject button = Instantiate(_continuePrefab);
        button.GetComponent<Button>().onClick.AddListener(caller.OkayPressed);
        _buttons.Add(button);
    }

    void ButtonCallBack(int value)
    {
        _callback(value);
    }

    public float CellHeight()
    {
        return GetComponent<GridLayoutGroup>().cellSize.y;
    }

    public void ClearList()
    {
        foreach(var but in _buttons)
        {
            Destroy(but.gameObject);
        }
        _buttons.Clear();
    }
}
