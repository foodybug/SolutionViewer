using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSolution
{
    protected const int SubExamCount = 15;
    protected const int EndExamCount = 35;
    protected const int SsnExamCount = 50;

    protected const int UnitCount = 5;

    protected void LoadFile(TextAsset _asset)
    {
        string[] lines = _asset.text.Split('\n');

        int lineCount = 0;
        //string inputData = null;
        foreach (string node in lines)
        {
            //			Debug.Log( "Parsing : " + node );
            string[] stringList = node.Split(',');
            if (stringList.Length == 0)
            {
                continue;
            }
            //string keyValue = stringList[0];
            if (ParseLine(stringList, lineCount) == false)
            {
                Debug.LogError("Parsing fail : " + lineCount + "'th line. " + stringList.ToString());
            }

            lineCount++;
        }
    }

    protected abstract bool ParseLine(string[] line, int lineCount);

    public virtual int DecreaseGrade(int _curGrade)
    {
        Debug.Log("BaseSolution:: DecreaseGrade: ignore command.");
        return 0;
    }

    public virtual int IncreaseGrade(int _curGrade)
    {
        Debug.Log("BaseSolution:: IncreaseGrade: ignore command.");
        return 0;
    }

    public abstract Dictionary<string, SortedDictionary<int, string>> GetSearchedSolution(int _grade, string _txt);
}
