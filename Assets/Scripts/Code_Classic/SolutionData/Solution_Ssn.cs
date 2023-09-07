using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution_Ssn : BaseSolution {

    // 과목명, 번호, 정답
    SortedDictionary<string, SortedDictionary<int, string>> m_dicSolutionData =
        new SortedDictionary<string, SortedDictionary<int, string>>();

    public Solution_Ssn(TextAsset asset)
    {
        LoadFile(asset);
    }

    protected override bool ParseLine(string[] inputData, int lineCount)
    {
        try
        {
            if (inputData.Length <= 2)
                return true;

            if (inputData[0] == "" || inputData[0] == "?" || inputData[1] == "?")
                return true;

            string className = "undefined";

            string[] solution = new string[inputData.Length - 2];
            for (int i = 2; i < inputData.Length; ++i)
            {
                solution[i - 2] = inputData[i];
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

            bool single = false;
            if (solution[0].Length == 1)
                single = true;

            for (int i = 0; i < SsnExamCount; ++i)
            {
                if (m_dicSolutionData.ContainsKey(className) == false)
                    m_dicSolutionData.Add(className, new SortedDictionary<int, string>());

                if (solution[i / UnitCount].Length == 0 ||
                    solution[i / UnitCount] == "" ||
                    solution[i / UnitCount] == "\r" ||
                    solution[i / UnitCount].Contains("?") ||
                    solution[i / UnitCount].Contains("*"))
                {
    //				Debug.LogWarning("Solution:: Parse: no more solution. class = " + className + ", count = " + i);
                    break;
                }

                try
                {
                    if(single == false)
                        m_dicSolutionData[className].Add(i + 1, solution[i / UnitCount][i % UnitCount].ToString());
                    else
                        m_dicSolutionData[className].Add(i + 1, solution[i].ToString());
                }
                catch (Exception e)
                {
                    string ex = "Solution_Ssn:: ParseLine: single = " + single +
                        ", className = " + className + ", i = " + i + ", UnitCount = " + UnitCount;
                    Debug.LogError(e);
                    Debug.LogError(ex);
                }

                //			Debug.Log((i + 1) + "'th solution is " + solution[i / UnitCount][i % UnitCount]);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        return true;
    }

    public override Dictionary<string, SortedDictionary<int, string>> GetSearchedSolution(int _grade, string _txt)
    {
        if (_txt == null)
            return null;

        Dictionary<string, SortedDictionary<int, string>> list = new Dictionary<string, SortedDictionary<int, string>>();
        
        foreach (KeyValuePair<string, SortedDictionary<int, string>> node in m_dicSolutionData)
        {
//			Debug.Log(node.Key);

            if (node.Key.Contains(_txt) == true)
                list.Add(node.Key, node.Value);
        }

        return list;
    }

    public override int DecreaseGrade(int _curGrade)
    {
        Debug.Log("BaseSolution:: DecreaseGrade: ignore command.");
        return 0;
    }

    public override int IncreaseGrade(int _curGrade)
    {
        Debug.Log("BaseSolution:: IncreaseGrade: ignore command.");
        return 0;
    }
}
