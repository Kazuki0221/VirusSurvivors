using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExpLevel
{
    [SerializeField] int _exp;
    [SerializeField] int _remainExp;
    [SerializeField] int _level = 1;
    [SerializeField] int _minLevel;

    public int Exp => _exp;
    public int Level => _level;

    int nowIndex = 0;//現在のレベルの添え字
    int nextIndex = 0;//次のレベルの添え字


    public void AddExp(int exp, int[] expArray)
    {
        _exp = Mathf.Clamp(_exp + exp, 0, expArray[expArray.Length - 1]);
        UpdateLevel(expArray);
        UpdateRemainExp(expArray);
    }

    void UpdateLevel(int[]expArray)
    {
        for(int i = 0; i < expArray.Length; i++)
        {
            if (expArray[i] <= _exp)
            {
                nowIndex = i;
            }
        }
        _level = nowIndex + 1;
    }

    void UpdateRemainExp(int[] expArray)
    {

        for (int i = 0; i < expArray.Length; i++)
        {
            if (expArray[i] > _exp)
            {
                nextIndex = i;
                break;
            }
        }
        _remainExp = expArray[nextIndex] - _exp;
    }
}
