//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;

//[System.Serializable]
//public class GeneratedSearchedSolution : ScriptableObject
//{
//	// 학기, [과목명, 연도, 데이터(번호, 정답)]
//	[SerializeField] List<Sdic_ClassData> _sdicSub_1; public List<Sdic_ClassData> sdicSub_1 { get { return _sdicSub_1;} }
//	[SerializeField] List<Sdic_ClassData> _sdicSub_2; public List<Sdic_ClassData> sdicSub_2 { get { return _sdicSub_2; } }
//	[SerializeField] List<Sdic_ClassData> _sdicEnd_1; public List<Sdic_ClassData> sdicEnd_1 { get { return _sdicEnd_1; } }
//	[SerializeField] List<Sdic_ClassData> _sdicEnd_2; public List<Sdic_ClassData> sdicEnd_2 { get { return _sdicEnd_2; } }
//	[SerializeField] List<Sdic_ClassData_WithoutGrade> _sdicSsn_1; public List<Sdic_ClassData_WithoutGrade> sdicSsn_1 { get { return _sdicSsn_1; } }
//	[SerializeField] List<Sdic_ClassData_WithoutGrade> _sdicSsn_2; public List<Sdic_ClassData_WithoutGrade> sdicSsn_2 { get { return _sdicSsn_2; } }

//	public void Add(BaseSearch search)
//	{
//		switch(search.type)
//		{
//			case eExamType.Sub:
//				Search_Sub sub = search as Search_Sub;
//				switch (sub.semester)
//				{
//					case 1:
//						_sdicSub_1.Add(sub.GetSerializedSolution());
//						break;
//					case 2:
//						_sdicSub_2.Add(sub.GetSerializedSolution());
//						break;
//					default:
//						Debug.LogError("GeneratedSearchedSolution:: Add: [eExamType.Sub] invalid semester = " + sub.semester);
//						break;
//				}
//				break;
//			case eExamType.End:
//				Search_End end = search as Search_End;
//				switch (end.semester)
//				{
//					case 1:
//						_sdicEnd_1.Add(end.GetSerializedSolution());
//						break;
//					case 2:
//						_sdicEnd_2.Add(end.GetSerializedSolution());
//						break;
//					default:
//						Debug.LogError("GeneratedSearchedSolution:: Add: [eExamType.End] invalid semester = " + end.semester);
//						break;
//				}
//				break;
//			case eExamType.Ssn:
//				Search_Ssn ssn = search as Search_Ssn;
//				switch (ssn.semester)
//				{
//					case 1:
//						_sdicSsn_1.Add(ssn.GetSerializedSolution());
//						break;
//					case 2:
//						_sdicSsn_2.Add(ssn.GetSerializedSolution());
//						break;
//					default:
//						Debug.LogError("GeneratedSearchedSolution:: Add: [eExamType.Ssn] invalid semester = " + ssn.semester);
//						break;
//				}
//				break;
//			default:
//				Debug.LogError("Solution:: LoadSolution: invalid type = " + search.type);
//				break;
//		}
//	}

//	public void Clear()
//	{
//		_sdicSub_1 = new List<Sdic_ClassData>();
//		_sdicSub_2 = new List<Sdic_ClassData>();
//		_sdicEnd_1 = new List<Sdic_ClassData>();
//		_sdicEnd_2 = new List<Sdic_ClassData>();
//		_sdicSsn_1 = new List<Sdic_ClassData_WithoutGrade>();
//		_sdicSsn_2 = new List<Sdic_ClassData_WithoutGrade>();
//	}

//	public List<SearchedData> GetSearchedData(eExamType type, int semester, string className)
//	{
//		List<SearchedData> searched = new List<SearchedData>();
//		List<Sdic_ClassData> list = null;

//		switch (type)
//		{
//			case eExamType.Sub:
//				switch (semester)
//				{
//					case 1: list = _sdicSub_1; break;
//					case 2: list = _sdicSub_2; break;
//					default: Debug.LogError("GeneratedSearchedSolution:: GetSearchedData: [eExamType.Sub] invalid semester = " + semester); break;
//				}
				
//				foreach (Sdic_ClassData node1 in list)
//				{
//					foreach(KeyValuePair<string, Sdic_GradeData> node2 in node1)
//					{
//						if (node2.Key.Contains(className) == true)
//						{
//							foreach(KeyValuePair<int, Sdic_NumberSolution> node3 in node2.Value)
//							{
//								searched.Add(new SearchedData(type, node1.year, semester, node2.Key, node3.Key, new SortedDictionary<int, string>(node3.Value)));
//							}
//						}
//					}
//				}

//				break;
//			case eExamType.End:
//				switch (semester)
//				{
//					case 1: list = _sdicSub_1; break;
//					case 2: list = _sdicSub_2; break;
//					default: Debug.LogError("GeneratedSearchedSolution:: GetSearchedData: [eExamType.Sub] invalid semester = " + semester); break;
//				}

//				foreach (Sdic_ClassData node1 in list)
//				{
//					foreach (KeyValuePair<string, Sdic_GradeData> node2 in node1)
//					{
//						if (node2.Key.Contains(className) == true)
//						{
//							foreach (KeyValuePair<int, Sdic_NumberSolution> node3 in node2.Value)
//							{
//								searched.Add(new SearchedData(type, node1.year, semester, node2.Key, node3.Key, new SortedDictionary<int, string>(node3.Value)));
//							}
//						}
//					}
//				}
//				break;
//			case eExamType.Ssn:
//				List<Sdic_ClassData_WithoutGrade> listSsn = null;
//				switch (semester)
//				{
//					case 1: listSsn = _sdicSsn_1; break;
//					case 2: listSsn = _sdicSsn_2; break;
//					default: Debug.LogError("GeneratedSearchedSolution:: GetSearchedData: [eExamType.Sub] invalid semester = " + semester); break;
//				}

//				foreach (Sdic_ClassData_WithoutGrade node1 in listSsn)
//				{
//					foreach (KeyValuePair<string, Sdic_NumberSolution> node2 in node1)
//					{
//						if (node2.Key.Contains(className) == true)
//						{
//							searched.Add(new SearchedData(type, node1.year, semester, node2.Key, new SortedDictionary<int, string>(node2.Value)));
//						}
//					}
//				}
//				break;
//			default:
//				Debug.LogError("Solution:: LoadSolution: invalid type = " + type);
//				break;
//		}

//		return searched;
//	}
//}

////[Serializable]
////public class Sdic_SolutionEnd : SerializableDictionary<int, SerializableDictionary<string, SerializableDictionary<int, SerializableDictionary<int, string>>>>
////{

////}

//#region - inner class -
////[Serializable]
////public class Sdic_SemesterData : SerializableDictionary<int, Sdic_ClassData>
////{
////}

//[Serializable]
//public class Sdic_ClassData : SerializableDictionary<string, Sdic_GradeData>
//{
//	public int year = -1;
//	public Sdic_ClassData(int year)
//	{
//		this.year = year;
//	}
//}

//[Serializable]
//public class Sdic_GradeData : SerializableDictionary<int, Sdic_NumberSolution>
//{
//}

//[Serializable]
//public class Sdic_ClassData_WithoutGrade : SerializableDictionary<string, Sdic_NumberSolution>
//{
//	public int year = -1;
//	public Sdic_ClassData_WithoutGrade(int year)
//	{
//		this.year = year;
//	}
//}

//[Serializable]
//public class Sdic_NumberSolution : SerializableDictionary<int, string>
//{
//	//Dictionary<int, string> dic { get { return (SortedDictionary < int, string>) this; } }
//}
//#endregion