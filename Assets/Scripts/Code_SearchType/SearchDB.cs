using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;

public class SearchDB : MonoBehaviour {

    // 형태, 학기, 과목명, 연도, 데이터
    SortedDictionary<eExamType, MultiSortedDictionary<int, BaseSearch>> mdicSearch =
        new SortedDictionary<eExamType, MultiSortedDictionary<int, BaseSearch>>();

    public float loadingProgress { get { return curClassCount * totalClassCount; } }
    float totalClassCount = 0f;
    int curClassCount = 0;

    IEnumerator Start()
	{
		totalClassCount = 1f;

		//yield break;

		UnityEngine.Object[] list = Resources.LoadAll("Table") as UnityEngine.Object[];
        totalClassCount = (float)list.Length;
        totalClassCount = 1f / totalClassCount;

        // 각 파일 처리
        foreach (UnityEngine.Object node in list)
        {
            TextAsset sol = node as TextAsset;
            if (sol == null)
            {
                Debug.LogError("SolutionDB:: Awake: Invalid Asset include in [Table] folder");
                continue;
            }

            // 과목명, 연도, 학기
            string[] path = sol.name.Split('/');
            if (path.Length == 0)
            {
                Debug.LogError("Invalid path = " + sol.name + ". skip this file");
                yield break;
            }

            string[] strs = path[path.Length - 1].Split('_');
            if (strs.Length != 3)
            {
                Debug.LogError("Invalid file name = " + path[path.Length - 1] + ". skip this file");
                yield break;
            }

            int year = int.Parse(strs[0]);
            int semester = int.Parse(strs[1]);
            eExamType type = (eExamType)Enum.Parse(typeof(eExamType), strs[2], true);

            //Debug.Log("Year = " + m_Year + ", Semester = " + m_Semester + ", Type = " + m_Type);

            BaseSearch search = null;
            switch (type)
            {
                case eExamType.Sub:
                    search = new Search_Sub(type, year, semester, sol);
                    break;
                case eExamType.End:
                    search = new Search_End(type, year, semester, sol);
                    break;
                case eExamType.Ssn:
                    search = new Search_Ssn(type, year, semester, sol);
                    break;
                default:
                    Debug.LogError("Solution:: LoadSolution: invalid type = " + type);
                    continue;
            }

            if (mdicSearch.ContainsKey(type) == false)
                mdicSearch.Add(type, new MultiSortedDictionary<int, BaseSearch>());

            mdicSearch[type].Add(semester, search);
            curClassCount += 1;

            if(curClassCount % 5 == 0)
                yield return null;
        }
    }

	//  public List<SearchedData> GetSearchedData(eExamType type, int semester, string className)
	//  {
	//GeneratedSearchedSolution serializedSolution = Resources.Load("SerializedSolution") as GeneratedSearchedSolution;
	//return serializedSolution.GetSearchedData(type, semester, className);
	//  }

	public List<SearchedData> GetSearchedData(eExamType type, int semester, string className)
	{
		List<SearchedData> list = new List<SearchedData>();

		if (mdicSearch.ContainsKey(type) == true && mdicSearch[type].ContainsKey(semester) == true)
		{
			foreach (BaseSearch node in mdicSearch[type][semester])
			{
				list.AddRange(node.GetSearchedData(className));
			}
		}

		return list;
	}

#if UNITY_EDITOR
	//[UnityEditor.MenuItem("YDE/Generate Solution Dictionary")]
	//static void GenerateSolutionDictionary()
	//{
	//	GeneratedSearchedSolution serializedSolution = Resources.Load("SerializedSolution") as GeneratedSearchedSolution;
	//	if (serializedSolution == null)
	//	{
	//		serializedSolution = ScriptableObject.CreateInstance<GeneratedSearchedSolution>();
	//		UnityEditor.AssetDatabase.CreateAsset(serializedSolution, "Assets/Resources/SerializedSolution.asset");
	//		UnityEditor.AssetDatabase.Refresh();
	//	}
	//	serializedSolution.Clear();

	//	UnityEngine.Object[] list = Resources.LoadAll("Table") as UnityEngine.Object[];

	//	// 각 파일 처리
	//	foreach (UnityEngine.Object node in list)
	//	{
	//		TextAsset sol = node as TextAsset;
	//		if (sol == null)
	//		{
	//			Debug.LogError("SolutionDB:: Awake: Invalid Asset include in [Table] folder");
	//			continue;
	//		}

	//		// 과목명, 연도, 학기
	//		string[] path = sol.name.Split('/');
	//		if (path.Length == 0)
	//		{
	//			Debug.LogError("Invalid path = " + sol.name + ". skip this file");
	//			break;
	//		}

	//		string[] strs = path[path.Length - 1].Split('_');
	//		if (strs.Length != 3)
	//		{
	//			Debug.LogError("Invalid file name = " + path[path.Length - 1] + ". skip this file");
	//			break;
	//		}

	//		int year = int.Parse(strs[0]);
	//		int semester = int.Parse(strs[1]);
	//		eExamType type = (eExamType)Enum.Parse(typeof(eExamType), strs[2], true);

	//		//Debug.Log("Year = " + year + ", Semester = " + semester + ", Type = " + type);

	//		BaseSearch search = null;
	//		switch (type)
	//		{
	//			case eExamType.Sub:
	//				search = new Search_Sub(type, year, semester, sol);
	//				break;
	//			case eExamType.End:
	//				search = new Search_End(type, year, semester, sol);
	//				break;
	//			case eExamType.Ssn:
	//				search = new Search_Ssn(type, year, semester, sol);
	//				break;
	//			default:
	//				Debug.LogError("Solution:: LoadSolution: invalid type = " + type);
	//				continue;
	//		}

	//		serializedSolution.Add(search);
	//	}

	//	UnityEditor.EditorUtility.SetDirty(serializedSolution);
	//	UnityEditor.AssetDatabase.SaveAssets();
	//	UnityEditor.AssetDatabase.Refresh();
	//}
#endif
}