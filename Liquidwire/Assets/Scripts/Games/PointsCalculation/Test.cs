using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject mailPrefab;

    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        TestCalc(9, 1, 123);
    }

    public void TestCalc(int errors, int difficulty, int time)
    {
        CaseGrading caseGrading = new CaseGrading();
        bool evaluation = caseGrading.Evaluation(errors, difficulty);
        float points = caseGrading.Points(time, errors, difficulty, evaluation);
        
        GameObject newEmail = Instantiate(mailPrefab, canvas.transform);
        newEmail.GetComponent<SendingMail>().SetMail(evaluation);
        
        Debug.Log("Evaluation:" + evaluation + " Points: " + points);
    }
}