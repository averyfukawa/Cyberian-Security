using System;
using System.Collections;
using System.Collections.Generic;
using FraudMessage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class ScenarioManager : MonoBehaviour
{
    private List<Scenario> _scenarios = new List<Scenario>();
    private int _currentScenario = 0;

    public Button answer1Btn;
    public Button answer2Btn;
    private int _currentButtonId = 0;

    private List<Answer> _answers = new List<Answer>();

    // Start is called before the first frame update
    void Start()
    {
        _scenarios.Add(new Scenario());
        answer1Btn.onClick.AddListener(delegate { CurrentButton(1); });
        answer2Btn.onClick.AddListener(delegate { CurrentButton(2); });
        ShowAnswers();
    }

    private void CurrentButton(int number)
    {
        _currentButtonId = number;
        ClickOn();
    }

    public TextMeshProUGUI FindTextChild(string name)
    {
        return transform.Find(name).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    public void ShowAnswers()
    {
        SubScenario currentSub = _scenarios[_currentScenario].GetSub();

        transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = currentSub.GetText();

        // check if the end of the scenario has been reached
        if (currentSub.GetIsEnd())
        {
            answer1Btn.gameObject.SetActive(false);

            // button you won / failed. Return to home
            answer2Btn.onClick.RemoveAllListeners();

            answer2Btn.onClick.AddListener(delegate { FinishGame(); });

            TextMeshProUGUI child2 = FindTextChild("Answer2btn");
            child2.text = "finish game";
        }
        else
        {
            _answers = currentSub.GetAnswers();

            TextMeshProUGUI child1 = FindTextChild("Answer1btn");
            child1.text = _answers[0].Get_answerText();

            TextMeshProUGUI child2 = FindTextChild("Answer2btn");
            child2.text = _answers[1].Get_answerText();
        }
    }

    public void ClickOn()
    {
        SubScenario currentSub = _scenarios[_currentScenario].GetSub();
        _answers = currentSub.GetAnswers();

        if (_currentButtonId == 1)
        {
            _scenarios[_currentScenario].SetCurrentSub(_answers[0].Get_nextScenario());
        }
        else
        {
            _scenarios[_currentScenario].SetCurrentSub(_answers[1].Get_nextScenario());
        }

        ShowAnswers();
    }

    public void FinishGame()
    {
        Debug.Log("You won. Scene change NYI");
    }


    // Update is called once per frame
    void Update()
    {
    }
}