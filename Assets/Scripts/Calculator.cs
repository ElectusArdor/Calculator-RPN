using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Calculator : MonoBehaviour
{
    [SerializeField] private InputField numInpField, sigInpField;
    [SerializeField] private Text resultText;

    private List<float> nums;
    private List<string> listOfSimbols;
    private string[] allSigns = { "+", "-", "*", "/", "^" };
    private bool error;

    private void Start()
    {
        Screen.SetResolution(900, 900, false);
    }

    Func<float, float, float> Plus = (a, b) => a + b;
    Func<float, float, float> Minus = (a, b) => a - b;
    Func<float, float, float> Multiply = (a, b) => a * b;
    Func<float, float, float> Division = (a, b) => a / b;
    Func<float, float, float> Pow = (a, b) => Mathf.Pow(a, b);

    public void OnCalcClick()
    {
        error = false;
        listOfSimbols = numInpField.text.Split(' ').ToList<string>();
        nums = new List<float>();
        resultText.text = "";

        for (int i = 0; i < listOfSimbols.Count; i++)
        {
            if (allSigns.Contains(listOfSimbols[i]))
            {
                if (nums.Count < 2)
                {
                    Output("Недостаточно чисел");
                    error = true;
                }
                else
                {
                    if (listOfSimbols[i] == "^")
                        Calculate(Pow);
                    else if (listOfSimbols[i] == "/")
                    {
                        if (nums[nums.Count - 1] == 0)
                        {
                            Output("Ошибка. Деление на 0");
                            error = true;
                            break;
                        }
                        else
                            Calculate(Division);
                    }
                    else if (listOfSimbols[i] == "*")
                        Calculate(Multiply);
                    else if (listOfSimbols[i] == "+")
                        Calculate(Plus);
                    else
                        Calculate(Minus);
                }
            }
            else
            {
                try
                {
                    nums.Add(float.Parse(listOfSimbols[i]));
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
