using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSearch
{
    #region - member -
    protected const int SubExamCount = 15;
    protected const int EndExamCount = 35;
    protected const int SsnExamCount = 50;

    protected const int UnitCount = 5;

    public eExamType type { get; protected set; }
    public int year { get; protected set; }
    public int semester { get; protected set; }
    #endregion
    protected void SetBasicInfo(eExamType type, int year, int semester)
    {
        this.type = type;
        this.year = year;
        this.semester = semester;
    }

    protected void LoadFile(TextAsset _asset)
    {
        string[] lines = _asset.text.Split('\n');

        int lineCount = 0;
        //string inputData = null;
        foreach (string node in lines)
        {
//			Debug.Log( "Parsing : " + node );
            string[] stringList = node.Split(',');
            if (stringList.Length == 0)//첫라인은 패스
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

    //public abstract List<Dictionary<int, SortedDictionary<int, string>>> GetSearchedSolution(string className);
    public abstract List<SearchedData> GetSearchedData(string className);
	//public abstract Sdic_ClassData GetSerializedSolution();

}

public class SearchedData
{
    public eExamType type { get; protected set; }
    public int year { get; protected set; }
    public int semester { get; protected set; }

    public string strResult { get; private set; }
    public SortedDictionary<int, string> solution;

    public SearchedData(eExamType type, int year, int semester,
        string name, int grade, SortedDictionary<int, string> solution)
    {
        this.type = type;
        this.year = year;
        this.semester = semester;

        strResult = year + "년" + name + "(" + grade + "학년)";
        this.solution = solution;
    }

	//public SearchedData(eExamType type, int year, int semester,
	//	string name, int grade, Sdic_NumberSolution solution)
	//{
	//	this.type = type;
	//	this.year = year;
	//	this.semester = semester;

	//	strResult = year + "년" + name + "(" + grade + "학년)";

	//	this.solution.

	//	this.solution = solution.cop;
	//}

	public SearchedData(eExamType type, int year, int semester,
        string name, SortedDictionary<int, string> solution)
    {
        this.type = type;
        this.year = year;
        this.semester = semester;

        strResult = year + "년" + name;
        this.solution = solution;
    }

	//public SearchedData(eExamType type, int year, int semester,
	//	string name, Sdic_NumberSolution solution)
	//{
	//	this.type = type;
	//	this.year = year;
	//	this.semester = semester;

	//	strResult = year + "년" + name;
	//	this.solution = solution;
	//}
}