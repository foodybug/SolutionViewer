using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search_End : BaseSearch
{
	// 과목명, 학년, 번호, 정답
	List<Data> _solution = new List<Data>();

	//List<Dictionary<int, SortedDictionary<int, string>>> _dicSolution =
	//	new List< Dictionary<int, SortedDictionary<int, string>>>();

	//public SortedDictionary<string, Dictionary<int, SortedDictionary<int, string>>> dicSolution { get { return _dicSolution; } }

	public Search_End(eExamType type, int year, int semester, TextAsset asset)
    {
        SetBasicInfo(type, year, semester);
        LoadFile(asset);
    }

	public Search_End(BaseSearch sub, TextAsset asset)
	{
		if (sub.type != eExamType.End)
		{
			Debug.LogError("Search_End:: constructor: invalid type = " + sub.type);
			return;
		}

		SetBasicInfo(sub.type, sub.year, sub.semester);
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

        //Debug.Log("grade = " + grade + ", className = " + className);
		SortedDictionary<int, string> dic = new SortedDictionary<int, string>();
		for (int i = 0; i < EndExamCount; ++i)
        {
            if (solution[i / UnitCount].Length == 0 || solution[i / UnitCount].Contains("?"))
            {
//				Debug.LogWarning("Solution:: Parse: no more solution. class = " + className + ", count = " + i);
                break;
            }

            try
            {
				dic.Add(i + 1, solution[i / UnitCount][i % UnitCount].ToString());
				//_dicSolution[className][grade].Add(i + 1, solution[i / UnitCount][i % UnitCount].ToString());
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

        Data data = new Data();
        data.className = className;
        data.grade = grade;
        data.solution = dic;
        _solution.Add(data);

        return true;
    }

    public override List<SearchedData> GetSearchedData(string name)
    {
        List<SearchedData> list = new List<SearchedData>();
        foreach (Data node in _solution)
        {
            if (node.className.Contains(name) == true)
            {
				list.Add(new SearchedData(type, year, semester, node.className, node.grade, node.solution));
            }
        }

		//Debug.Log("Search_Sub:: GetSearchedSolution: solution = " + name + "'s size is " + list.Count);

		return list;
    }

	//public SerializableDictionary<string, SerializableDictionary<int, SerializableDictionary<int, string>>> GetSerializedSolution()
	//{
	//	SerializableDictionary<string, SerializableDictionary<int, SerializableDictionary<int, string>>> sdic =
	//		new SerializableDictionary<string, SerializableDictionary<int, SerializableDictionary<int, string>>>();

	//	foreach (KeyValuePair<string, Dictionary<int, SortedDictionary<int, string>>> node1 in _dicSolution)
	//	{
	//		if (sdic.ContainsKey(node1.Key) == false)
	//			sdic.Add(node1.Key, new SerializableDictionary<int, SerializableDictionary<int, string>>());

	//		foreach (KeyValuePair<int, SortedDictionary<int, string>> node2 in node1.Value)
	//		{
	//			if (sdic[node1.Key].ContainsKey(node2.Key) == false)
	//				sdic[node1.Key].Add(node2.Key, new SerializableDictionary<int, string>());

	//			foreach (KeyValuePair<int, string> node3 in node2.Value)
	//			{
	//				sdic[node1.Key][node2.Key].Add(node3.Key, node3.Value);
	//			}
	//		}
	//	}

	//	return sdic;
	//}

	//public Sdic_ClassData GetSerializedSolution()
	//{
	//	return null;

	//	Sdic_ClassData sdic = new Sdic_ClassData(year);

	//	foreach (KeyValuePair<string, Dictionary<int, SortedDictionary<int, string>>> node1 in _dicSolution)
	//	{
	//		if (sdic.ContainsKey(node1.Key) == false)
	//			sdic.Add(node1.Key, new Sdic_GradeData());

	//		foreach (KeyValuePair<int, SortedDictionary<int, string>> node2 in node1.Value)
	//		{
	//			if (sdic[node1.Key].ContainsKey(node2.Key) == false)
	//				sdic[node1.Key].Add(node2.Key, new Sdic_NumberSolution());

	//			foreach (KeyValuePair<int, string> node3 in node2.Value)
	//			{
	//				sdic[node1.Key][node2.Key].Add(node3.Key, node3.Value);
	//			}
	//		}
	//	}

	//	return sdic;
	//}

	class Data
	{
		public string className;
		public int grade;
		public SortedDictionary<int, string> solution = new SortedDictionary<int, string>();
	}
}
