using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    enum QuestionType { Add, Minus, Mul, Div }
    [SerializeField] private GameObject answerPrefab;
    [SerializeField] private int numberOfAnswers;
    [SerializeField] private TextMeshProUGUI scoreText, question;
    [SerializeField] private BoxCollider2D gridArea;
    [SerializeField] private QuestionType questionType;

    private string questionSymbol;
    private int questionX, questionY;
    private List<GameObject> answersList;
    public static GameManager instance;
    public int score = 0;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        answersList = new List<GameObject>();
        GetNewQuestion();
    }

    void Update()
    {
        scoreText.text = "Score: " + score;
    }
    public void GetNewQuestion()
    {
        switch (questionType)
        {
            case QuestionType.Add:
                questionSymbol = " + ";
                questionX = Random.Range(0, 50); questionY = Random.Range(0, 50);
                break;
            case QuestionType.Minus:
                questionSymbol = " - ";
                questionX = Random.Range(0, 50); questionY = Random.Range(0, questionX);
                break;
            case QuestionType.Mul:
                questionSymbol = " * ";
                questionX = Random.Range(0, 12); questionY = Random.Range(0, 12);
                break;
            case QuestionType.Div:
                questionSymbol = " / ";
                questionX = Random.Range(1, 100); questionY = 1;

                List<int> divisors = new List<int>();
                for (int i = 2; i <= questionX; i++)
                    if (questionX % i == 0) divisors.Add(i);

                questionY = divisors[Random.Range(0, divisors.Count - 1)];
                break;
            default:
                questionSymbol = " + ";
                questionX = Random.Range(0, 50); questionY = Random.Range(0, 50);
                break;
        }
        question.text = questionX.ToString() + questionSymbol + questionY.ToString() + " =";
        Respawn();
    }
    public void Respawn()
    {
        for (int i = 0; i < answersList.Count; i++) { Destroy(answersList[i]); }
        answersList.Clear();

        for (int i = 0; i < numberOfAnswers; i++)
        {
            GameObject answerInstance = Instantiate(answerPrefab);
            answersList.Add(answerInstance);

            TextMeshPro answerText = answerInstance.GetComponentInChildren<TextMeshPro>();

            int questionAnswer = Random.Range(0, 100);

            if (i == 0)
                questionAnswer = GetCorrectAnswer();
            answerText.text = questionAnswer.ToString();

            Food newAnswer = answerInstance.GetComponent<Food>();
            newAnswer.gridArea = gridArea;
            newAnswer.Respawn(ValidateAnswer(questionX, questionY, questionAnswer));
        }
    }
    private bool ValidateAnswer(int x, int y, int ans)
    {
        switch (questionType)
        {
            case QuestionType.Add:
                return x + y == ans;
            case QuestionType.Minus:
                return x - y == ans;
            case QuestionType.Mul:
                return x * y == ans;
            case QuestionType.Div:
                return x / y == ans;
            default:
                return false;
        }
    }
    private int GetCorrectAnswer()
    {
        switch (questionType)
        {
            case QuestionType.Add:
                return questionX + questionY;
            case QuestionType.Minus:
                return questionX - questionY;
            case QuestionType.Mul:
                return questionX * questionY;
            case QuestionType.Div:
                return questionX / questionY;
            default:
                return 0;
        }
    }
}

