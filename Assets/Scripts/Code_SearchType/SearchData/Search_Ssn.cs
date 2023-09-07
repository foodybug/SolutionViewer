using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search_Ssn : BaseSearch
{
    // 과목명, 번호, 정답
	List<Data> _solution = new List<Data>();

 //   SortedDictionary<string, SortedDictionary<int, string>> _dicSolution =
 //       new SortedDictionary<string, SortedDictionary<int, string>>();

	//public SortedDictionary<string, SortedDictionary<int, string>> dicSolution { get { return _dicSolution; } }
	
	public Search_Ssn(eExamType type, int year, int semester, TextAsset asset)
    {
        SetBasicInfo(type, year, semester);
        LoadFile(asset);
    }

	public Search_Ssn(BaseSearch sub, TextAsset asset)
	{
		if (sub.type != eExamType.Ssn)
		{
			Debug.LogError("Search_Ssn:: constructor: invalid type = " + sub.type);
			return;
		}

		SetBasicInfo(sub.type, sub.year, sub.semester);
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

			SortedDictionary<int, string> dic = new SortedDictionary<int, string>();
			for (int i = 0; i < SsnExamCount; ++i)
            {
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
						dic.Add(i + 1, solution[i / UnitCount][i % UnitCount].ToString());
					else
						dic.Add(i + 1, solution[i].ToString());
				}
                catch (Exception e)
                {
                    string ex = "Solution_Ssn:: ParseLine: single = " + single +
                        ", type = " + type + ", year = " + year + ", semester = " + semester +
                        ", className = " + className + ", i = " + i + ", UnitCount = " + UnitCount;
                    Debug.LogError(e);
                    Debug.LogError(ex);
                }
                
				//			Debug.Log((i + 1) + "'th solution is " + solution[i / UnitCount][i % UnitCount]);
			}

            Data data = new Data();
            data.className = className;
            data.solution = dic;
            _solution.Add(data);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        return true;
    }

	public override List<SearchedData> GetSearchedData(string name)
	{
		List<SearchedData> list = new List<SearchedData>();
		foreach (Data node in _solution)
		{
			if (node.className.Contains(name) == true)
			{
				list.Add(new SearchedData(type, year, semester, node.className, node.solution));
			}
		}

		//Debug.Log("Search_Sub:: GetSearchedSolution: solution = " + node.Key + " is found");

		return list;
	}

	//public override List<SearchedData> GetSearchedData(string name)
 //   {
 //       List<SearchedData> list = new List<SearchedData>();
 //       foreach (KeyValuePair<string, SortedDictionary<int, string>> node in _dicSolution)
 //       {
 //           if (node.Key.Contains(name) == true)
 //           {
 //               list.Add(new SearchedData(type, year, semester, node.Key, node.Value));

 //               //Debug.Log("Search_Sub:: GetSearchedSolution: solution = " + node.Key + " is found");
 //           }
 //       }

 //       return list;
 //   }

	//public Sdic_ClassData_WithoutGrade GetSerializedSolution()
	//{
	//	return null;

	//	Sdic_ClassData_WithoutGrade sdic = new Sdic_ClassData_WithoutGrade(year);

	//	foreach (KeyValuePair<string, SortedDictionary<int, string>> node1 in _dicSolution)
	//	{
	//		if (sdic.ContainsKey(node1.Key) == false)
	//			sdic.Add(node1.Key, new Sdic_NumberSolution());

	//		foreach (KeyValuePair<int, string> node2 in node1.Value)
	//		{
	//			sdic[node1.Key].Add(node2.Key, node2.Value);
	//		}
	//	}

	//	return sdic;
	//}

	class Data
	{
		public string className;
		public SortedDictionary<int, string> solution = new SortedDictionary<int, string>();
	}
}
