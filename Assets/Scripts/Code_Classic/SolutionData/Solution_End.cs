using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution_End : BaseSolution
{
    // 학년, 과목명, 번호, 정답
    SortedDictionary<int, Dictionary<string, SortedDictionary<int, string>>> m_dicSolutionData =
        new SortedDictionary<int, Dictionary<string, SortedDictionary<int, string>>>();

    public Solution_End(TextAsset asset)
    {
        LoadFile(asset);
    }

    protected override bool ParseLine(string[] inputData, int lineCount)
    {
        if (inputData.Length <= 2)
            return true;

        if (inputData[0] == "")
            return true;
        
        int grade = 0;
        string className = "undefined";

        string[] solution = new string[inputData.Length - 2];
        for (int i = 2; i < inputData.Length; ++i)
        {
            solution[i - 2] = inputData[i];
        }

        inputData[0] = inputData[0].Replace("학년", "");
        if (int.TryParse(inputData[0], out grade) == false)
        {
            Debug.LogError("Invalid grade = " + inputData[0] + ". skip rest progress");
            return false;
        }

        className = inputData[1];
        if (className.Contains("\"") == true)
        {
            int endIdx = 2;
            foreach (string node in solution)
            {
                className += "," + node;
                ++endIdx;

                if (node.Contains("\"") == true)
                    break;
            }

            //Debug.Log("Solution:: Parse: invalid class name = " + className);
            string logSolution = "";

            solution = new string[inputData.Length - endIdx];
            for (int i = endIdx; i < inputData.Length; ++i)
            {
                solution[i - endIdx] = inputData[i];

                logSolution += "," + inputData[i];
            }

            //Debug.Log(logSolution);
        }

        //		Debug.Log("grade = " + grade + ", className = " + className);
        for (int i = 0; i < EndExamCount; ++i)
        {
            if (m_dicSolutionData.ContainsKey(grade) == false)
                m_dicSolutionData.Add(grade, new Dictionary<string, SortedDictionary<int, string>>());

            if (m_dicSolutionData[grade].ContainsKey(className) == false)
                m_dicSolutionData[grade].Add(className, new SortedDictionary<int, string>());

            if (solution[i / UnitCount].Length == 0 || solution[i / UnitCount].Contains("?"))
            {
                //				Debug.LogWarning("Solution:: Parse: no more solution. class = " + className + ", count = " + i);
                break;
            }

            try
            {
                m_dicSolutionData[grade][className].Add(i + 1, solution[i / UnitCount][i % UnitCount].ToString());
            }
            catch (Exception e)
            {
                string ex = "Solution_End:: ParseLine: grade = " + grade +
                    ", className = " + className +
                    ", i = " + i + ", UnitCount = " + UnitCount;
                Debug.LogError(e);
                Debug.LogError(ex);
            }

            //			Debug.Log((i + 1) + "'th solution is " + solution[i / UnitCount][i % UnitCount]);
        }

        return true;
    }

    public override Dictionary<string, SortedDictionary<int, string>> GetSearchedSolution(int _grade, string _txt)
    {
        if (_txt == null)
            return null;

        Dictionary<string, SortedDictionary<int, string>> list = new Dictionary<string, SortedDictionary<int, string>>();

        if (m_dicSolutionData.ContainsKey(_grade) == true)
        {
            foreach (KeyValuePair<string, SortedDictionary<int, string>> node in m_dicSolutionData[_grade])
            {
                //				Debug.Log(node.Key);

                if (node.Key.Contains(_txt) == true)
                    list.Add(node.Key, node.Value);
            }
        }

        return list;
    }

    public override int DecreaseGrade(int _curGrade)
    {
        int cur = _curGrade - 1;
        if (m_dicSolutionData.ContainsKey(cur) == true)
            return cur;
        else if (m_dicSolutionData.ContainsKey(_curGrade) == true)
            return _curGrade;
        else
            return 1;
    }

    public override int IncreaseGrade(int _curGrade)
    {
        int cur = _curGrade + 1;
        if (m_dicSolutionData.ContainsKey(cur) == true)
            return cur;
        else if (m_dicSolutionData.ContainsKey(_curGrade) == true)
            return _curGrade;
        else
            return 4;
    }
}
