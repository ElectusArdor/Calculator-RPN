using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Calculator : MonoBehaviour
{
    [SerializeField] private InputField numInpField;
    [SerializeField] private Text resultText;

    private Dictionary<string, Func<float, float, float>> functions;    //  Dictionary of available functions
    private List<string> listOfSimbols; //  List-converted user-supplied string
    private List<float> nums;   //  The current list of values when the input string is processed by the loop
    private bool error;

    Func<float, float, float> Plus = (a, b) => a + b;
    Func<float, float, float> Minus = (a, b) => a - b;
    Func<float, float, float> Multiply = (a, b) => a * b;
    Func<float, float, float> Division = (a, b) => a / b;
    Func<float, float, float> Pow = (a, b) => Mathf.Pow(a, b);
    Func<float, float, float> Root = (a, b) => Mathf.Pow(b, 1 / a);
    Func<float, float, float> Percents = (a, b) => a / b * 100;

    void Start()
    {
        functions = new Dictionary<string, Func<float, float, float>> {
            { "+", Plus },
            { "-", Minus },
            { "*", Multiply },
            { "/", Division },
            { "^", Pow },
            { "r", Root },
            { "%", Percents } };
    }

    public void OnCalcClick()
    {
        error = false;  //  Reset the error
        listOfSimbols = numInpField.text.Split(' ').ToList<string>();
        nums = new List<float>();
        resultText.text = "";

        foreach (string str in listOfSimbols)
        {
            if (functions.Keys.Contains(str))
            {
                if (nums.Count < 2)
                {
                    Output("Недостаточно чисел");
                    error = true;
                }
                else
                {
                    if (str == "/" & nums[nums.Count - 1] == 0)
                    {
                        Output("Ошибка. Деление на 0");
                        error = true;
                        break;
                    }
                    else
                        Calculate(functions[str]);
                }
            }
            else if (str == "") { } //  User tremor protection
            else
            {
                try
                {
                    nums.Add(float.Parse(str));
                }
                catch
                {
                    Output("Некорректно введены данные");
                    error = true;
                    break;
                }
            }
        }

        if (nums.Count != 1)
        {
            Output("Недостаточно действий");
            error = true;
        }    

        if (!error)
            Output(nums[0].ToString());
    }

    ///<summary>
    ///     Calls the passed function and updates the list of values
    ///</summary>
    private void Calculate(Func<float, float, float> func)
    {
        nums[nums.Count - 2] = func(nums[nums.Count - 2], nums[nums.Count - 1]);
        nums.RemoveAt(nums.Count - 1);
    }

    private void Output(string text)
    {
        resultText.text = text;
    }
}
