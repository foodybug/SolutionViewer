using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;

public enum eExamType { Sub, End, Ssn, MAX = 3 }

public class SolutionDB : MonoBehaviour {
    
	// 연도, 학기, 형태, 데이터
	SortedDictionary<int, SortedDictionary<int, Dictionary<eExamType, Solution>>> m_dicSolution =
		new SortedDictionary<int, SortedDictionary<int, Dictionary<eExamType, Solution>>>();

	void Awake()
	{
        UnityEngine.Object[] list = Resources.LoadAll("Table") as UnityEngine.Object[];
        foreach(UnityEngine.Object node in list)
        {
            TextAsset sol = node as TextAsset;
            if (sol == null)
            {
                Debug.LogError("SolutionDB:: Awake: Invalid Asset include in [Table] folder");
                continue;
            }
            ReadSolution(sol);//파일 1개
        }
    }

	void ReadSolution(TextAsset _asset)
	{
		Solution solution = new Solution();
        solution.LoadSolution(_asset);

		if(m_dicSolution.ContainsKey(solution.Year) == false)
			m_dicSolution.Add(solution.Year, new SortedDictionary<int, Dictionary<eExamType, Solution>>());
		
		if(m_dicSolution[solution.Year].ContainsKey(solution.Semester) == false)
			m_dicSolution[solution.Year].Add(solution.Semester, new Dictionary<eExamType, Solution>());

		m_dicSolution[solution.Year][solution.Semester].Add(solution.Type, solution);
	}

	#region - select solution -
	public int DecreaseYear(int _curYear)
	{
		int cur = _curYear - 1;
		if(m_dicSolution.ContainsKey(cur) == true)
			return cur;
		else
			return _curYear;
	}
	public int IncreaseYear(int _curYear)
	{
		int cur = _curYear + 1;
		if(m_dicSolution.ContainsKey(cur) == true)
			return cur;
		else
			return _curYear;
	}

	public int DecreaseSemester(int _curYear, int _curSemester)
	{
		int cur = _curSemester - 1;
		if(m_dicSolution.ContainsKey(_curYear) == true)
		{
			if(m_dicSolution[_curYear].ContainsKey(cur) == true)
				return cur;
		}

		return _curSemester;
	}

	public int IncreaseSemester(int _curYear, int _curSemester)
	{
		int cur = _curSemester + 1;
		if(m_dicSolution.ContainsKey(_curYear) == true)
		{
			if(m_dicSolution[_curYear].ContainsKey(cur) == true)
				return cur;
		}
		
		return _curSemester;
	}

	public eExamType ChangeExamType(int _curYear, int _curSemester, eExamType _type, bool left)
	{
        int cur = (int)_type;
        if (left == true)
        {
            --cur;
            if (cur < 0)
                cur = (int)eExamType.MAX - 1;
        }
        else
        {
            ++cur;
            if (cur >= (int)eExamType.MAX)
                cur = 0;
        }
        
		if(m_dicSolution.ContainsKey(_curYear) == true)
		{
			if(m_dicSolution[_curYear].ContainsKey(_curSemester) == true)
			{
				if(m_dicSolution[_curYear][_curSemester].ContainsKey((eExamType)cur) == true)
					return (eExamType)cur;
			}
		}
		
		return _type;
	}

	public Solution GetSolution(int _curYear, int _curSemester, eExamType _type)
	{
		if(m_dicSolution.ContainsKey(_curYear) == true)
		{
			if(m_dicSolution[_curYear].ContainsKey(_curSemester) == true)
			{
				if(m_dicSolution[_curYear][_curSemester].ContainsKey(_type) == true)
					return m_dicSolution[_curYear][_curSemester][_type];
			}
		}

		Debug.LogError("SolutionDB:: GetSolution: solution is not found." +
			"year = " + _curYear + ", semester = " + _curSemester + ", exam type = " + _type);
		return null;
	}
#endregion
}

public class Solution
{
	#region - member -
	int m_Year; public int Year{get{return m_Year;}}
	int m_Semester; public int Semester{get{return m_Semester;}}
	eExamType m_Type; public eExamType Type{get{return m_Type;}}

    BaseSolution solution = null;
	#endregion
	#region - init -
    public void LoadSolution(TextAsset _asset)
    {
        string[] path = _asset.name.Split('/');
        if (path.Length == 0)
        {
            Debug.LogError("Invalid path = " + _asset.name + ". skip this file");
            return;
        }

        string[] strs = path[path.Length - 1].Split('_');
        if (strs.Length != 3)
        {
            Debug.LogError("Invalid file name = " + path[path.Length - 1] + ". skip this file");
            return;
        }

        m_Year = int.Parse(strs[0]);
        m_Semester = int.Parse(strs[1]);
        m_Type = (eExamType)Enum.Parse(typeof(eExamType), strs[2], true);

        switch(m_Type)
        {
            case eExamType.Sub:
                solution = new Solution_Sub(_asset);
                break;
            case eExamType.End:
                solution = new Solution_End(_asset);
                break;
            case eExamType.Ssn:
                solution = new Solution_Ssn(_asset);
                break;
            default:
                Debug.LogError("Solution:: LoadSolution: invalid type = " + m_Type);
                break;
        }
        
        //Debug.Log("Year = " + m_Year + ", Semester = " + m_Semester + ", Type = " + m_Type);
    }
	#endregion
	#region - public -
	public int DecreaseGrade(int _curGrade)
	{
        return solution.DecreaseGrade(_curGrade);
	}
	public int IncreaseGrade(int _curGrade)
	{
        return solution.IncreaseGrade(_curGrade);
    }

	public Dictionary<string, SortedDictionary<int, string>> GetSearchedSolution(int _grade, string _txt)
	{
        return solution.GetSearchedSolution(_grade, _txt);
	}
	#endregion
}