using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FraudMessage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using File = UnityEngine.Windows.File;

public class ScenarioManager : MonoBehaviour
{
    public List<Scenario> scenarios;
    private int _currentScenario = 0;

    public TextAsset[] scenarioFiles;

    public Button answer1Btn;
    public Button answer2Btn;
    private int _currentButtonId = 0;

    private List<Answer> _answers = new List<Answer>();

    [SerializeField] private TextMeshProUGUI _answerButton1;
    [SerializeField] private TextMeshProUGUI _answerButton2;
    [SerializeField] private TextMeshProUGUI _scenarioText;

    // Start is called before the first frame update
    void Start()
    {
        scenarios = new List<Scenario>();
        
        SaveScenario sc = new SaveScenario();
        scenarios.Add(sc.Load());
        
        answer1Btn.onClick.AddListener(delegate { CurrentButton(1); });
        answer2Btn.onClick.AddListener(delegate { CurrentButton(2); });

        ShowAnswers();
    }

    private string TOJSON()
    {
        return JsonUtility.ToJson(this);
    }

    private void CurrentButton(int number)
    {
        _currentButtonId = number;
        ClickOn();
    }

    // this is a cleaner and presumably more usable implementation of the original approach.
    // it is not necessary for the current code to run, but you can reuse it if you ever need something similar
    public TextMeshProUGUI FindTextChildOfObject(GameObject target)
    {
        return target.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowAnswers()
    {
        // LoopThroughScenarios();
        SubScenario currentSub = scenarios[_currentScenario].GetSub();
        
        _scenarioText.text = currentSub.GetText();

        // check if the end of the scenario has been reached
        if (currentSub.GetIsEnd())
        {
            answer1Btn.gameObject.SetActive(false);

            // button you won / failed. Return to home
            answer2Btn.onClick.RemoveAllListeners();

            answer2Btn.onClick.AddListener(delegate { FinishGame(); });
            
            _answerButton2.text = "finish game";
        }
        else
        {
            _answers = currentSub.GetAnswers();

            _answerButton1.text = _answers[0].Get_answerText();
            
            _answerButton2.text = _answers[1].Get_answerText();
        }   
    }

    public void ClickOn()
    {
        SubScenario currentSub = scenarios[_currentScenario].GetSub();
        _answers = currentSub.GetAnswers();

        if (_currentButtonId == 1)
        {
            scenarios[_currentScenario].SetCurrentSub(_answers[0].Get_nextScenario());
        }
        else
        {
            scenarios[_currentScenario].SetCurrentSub(_answers[1].Get_nextScenario());
        }

        ShowAnswers();
    }

    public void FinishGame()
    {
        Debug.Log("You won. Scene change NYI");
    }
    
}