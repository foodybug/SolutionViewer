using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ExamSelector : MonoBehaviour {

	#region - serialized -
    [Header("연도, 학기, 타입, 학년")]
	[SerializeField] Text txtYear;
	[SerializeField] Text txtSemester;
	[SerializeField] Text txtType;
	[SerializeField] Text txtGrade;

	[SerializeField] InputField inputFieldClass;
	[SerializeField] Button btnClass_L;
	[SerializeField] Button btnClass_R;
	[SerializeField] Text txtClass;

	[SerializeField] SolutionDB m_SolutionDB;

	[SerializeField] ButtonUnit[] m_arButtonUnit;
    [SerializeField] QuitCtrl quitCtrl;
	#endregion
	#region - member -
	int m_CurYear = 2014;
	int m_CurSemester = 1;
	eExamType m_CurType = eExamType.Sub;

	Solution m_CurSolution;
	Dictionary<string, SortedDictionary<int, string>> m_sdicSolution;

	int m_CurGrade = 4;
	string m_CurClass;
	int m_IndexInSelectedClasses = 0;

	int m_NumberPage = 1;
	const int MaxPage_Sub = 4;
	const int MaxPage_End = 2;
    const int MaxPage_Ssn = 1;
    #endregion
    #region - init & release & update -
    void Awake()
    {
        m_CurYear = int.Parse(txtYear.text);

        quitCtrl.InitColor();
    }

    void Start()
	{
		m_CurSolution = m_SolutionDB.GetSolution(m_CurYear, m_CurSemester, m_CurType);
		_SetSolutionBySelectedIndex(true);
	}

    void Update()
    {
        _PressBtn_Back();
    }
	#endregion
	#region - call back -
	#endregion
	#region - public -
	#endregion
	#region - method -
	void SetSolution(SortedDictionary<int, string> _solution)
	{
		foreach(ButtonUnit node in m_arButtonUnit)
		{
			node.Init(m_CurType, _solution);
		}
	}

    void _PressBtn_Back()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            quitCtrl.gameObject.SetActive(true);
    }
	#endregion
	#region - delegate -
	public void OnClickButton_Year_L()
	{
		m_CurYear = m_SolutionDB.DecreaseYear(m_CurYear);
		txtYear.text = m_CurYear.ToString();

		m_CurSolution = m_SolutionDB.GetSolution(m_CurYear, m_CurSemester, m_CurType);
		m_sdicSolution = m_CurSolution.GetSearchedSolution(m_CurGrade, m_CurClass);
		_SetSolutionBySelectedIndex(true);
	}
	public void OnClickButton_Year_R()
	{
		m_CurYear = m_SolutionDB.IncreaseYear(m_CurYear);
		txtYear.text = m_CurYear.ToString();

		m_CurSolution = m_SolutionDB.GetSolution(m_CurYear, m_CurSemester, m_CurType);
		m_sdicSolution = m_CurSolution.GetSearchedSolution(m_CurGrade, m_CurClass);
		_SetSolutionBySelectedIndex(true);
	}

	public void OnClickButton_Semester_L()
	{
		m_CurSemester = m_SolutionDB.DecreaseSemester(m_CurYear, m_CurSemester);
		txtSemester.text = m_CurSemester.ToString();

		m_CurSolution = m_SolutionDB.GetSolution(m_CurYear, m_CurSemester, m_CurType);
		m_sdicSolution = m_CurSolution.GetSearchedSolution(m_CurGrade, m_CurClass);
		_SetSolutionBySelectedIndex(true);
	}
	public void OnClickButton_Semester_R()
	{
		m_CurSemester = m_SolutionDB.IncreaseSemester(m_CurYear, m_CurSemester);
		txtSemester.text = m_CurSemester.ToString();

		m_CurSolution = m_SolutionDB.GetSolution(m_CurYear, m_CurSemester, m_CurType);
		m_sdicSolution = m_CurSolution.GetSearchedSolution(m_CurGrade, m_CurClass);
		_SetSolutionBySelectedIndex(true);
	}

	public void OnClickButton_Type_L()
	{
		m_CurType = m_SolutionDB.ChangeExamType(m_CurYear, m_CurSemester, m_CurType, true);
        txtType.text = _GetCurExamType();
        if (m_CurType == eExamType.Ssn)
            txtGrade.text = "-";
        else
            txtGrade.text = m_CurGrade.ToString();

        m_CurSolution = m_SolutionDB.GetSolution(m_CurYear, m_CurSemester, m_CurType);
		m_sdicSolution = m_CurSolution.GetSearchedSolution(m_CurGrade, m_CurClass);
		_SetSolutionBySelectedIndex(true);
	}
	public void OnClickButton_Type_R()
	{
		m_CurType = m_SolutionDB.ChangeExamType(m_CurYear, m_CurSemester, m_CurType, false);
        txtType.text = _GetCurExamType();
        if (m_CurType == eExamType.Ssn)
            txtGrade.text = "-";
        else
            txtGrade.text = m_CurGrade.ToString();

        m_CurSolution = m_SolutionDB.GetSolution(m_CurYear, m_CurSemester, m_CurType);
		m_sdicSolution = m_CurSolution.GetSearchedSolution(m_CurGrade, m_CurClass);
		_SetSolutionBySelectedIndex(true);
	}
	string _GetCurExamType()
	{
        switch(m_CurType)
        {
            case eExamType.Sub:
                return "대체시험";
            case eExamType.End:
                return "기말시험";
            case eExamType.Ssn:
                return "계절시험";
            default:
                return "Unknown";
        }
	}

	public void OnClickButton_Grade_L()
	{
		if(m_CurSolution == null)
			return;

        if (m_CurType == eExamType.Ssn)
        {
            txtGrade.text = "-";
            return;
        }

        m_CurGrade = m_CurSolution.DecreaseGrade(m_CurGrade);
		txtGrade.text = m_CurGrade.ToString();

		m_sdicSolution = m_CurSolution.GetSearchedSolution(m_CurGrade, m_CurClass);
		_SetSolutionBySelectedIndex(true);
	}
	public void OnClickButton_Grade_R()
	{
		if(m_CurSolution == null)
			return;

        if (m_CurType == eExamType.Ssn)
        {
            txtGrade.text = "-";
            return;
        }

        m_CurGrade = m_CurSolution.IncreaseGrade(m_CurGrade);
		txtGrade.text = m_CurGrade.ToString();

		m_sdicSolution = m_CurSolution.GetSearchedSolution(m_CurGrade, m_CurClass);
		_SetSolutionBySelectedIndex(true);
	}

	public void OnClickButton_Class_L()
	{
		if(m_CurSolution == null ||
		   m_sdicSolution == null)
			return;

		int cur = m_IndexInSelectedClasses - 1;
		if(cur > 0)
		{
			--m_IndexInSelectedClasses;
			_SetSolutionBySelectedIndex();
		}
	}
	public void OnClickButton_Class_R()
	{
		if(m_CurSolution == null ||
		   m_sdicSolution == null)
			return;
		
		int cur = m_IndexInSelectedClasses;
		if(cur < m_sdicSolution.Count)
		{
			++m_IndexInSelectedClasses;
			_SetSolutionBySelectedIndex();
		}
	}
	public void OnInputTextEnd(InputField _field)
	{
		if(m_CurSolution == null)
			return;

		m_CurClass = _field.text;
		m_sdicSolution = m_CurSolution.GetSearchedSolution(m_CurGrade, m_CurClass);
		m_IndexInSelectedClasses = 1;
		_SetSolutionBySelectedIndex();
	}
	void _SetSolutionBySelectedIndex(bool _refresh = false)
	{
		if(m_CurSolution == null ||
		   m_sdicSolution == null)
			return;

		if(_refresh == true)
		{
			m_IndexInSelectedClasses = 1;
			m_NumberPage = 0;
			_ChangeNumber();
		}

		bool treat = false;
		int count = 0;
		foreach(KeyValuePair<string, SortedDictionary<int, string>> node in m_sdicSolution)
		{
			++count;
			if(m_IndexInSelectedClasses == count)
			{
				txtClass.text = node.Key;
				SetSolution(node.Value);

				treat = true;
				break;
			}
		}

		if(treat == false)
		{
			txtClass.text = "";
		}
	}

	public void OnClickButton_ChangeNumber()
	{
		m_NumberPage++;
        switch(m_CurType)
        {
            case eExamType.Sub:
                m_NumberPage = m_NumberPage % MaxPage_Sub;
                break;
            case eExamType.End:
                m_NumberPage = m_NumberPage % MaxPage_End;
                break;
            case eExamType.Ssn:
                m_NumberPage = m_NumberPage % MaxPage_Ssn;
                break;
        }
        
		_ChangeNumber();
	}

	void _ChangeNumber()
	{
		if(m_CurSolution == null ||
			m_sdicSolution == null)
			return;
        
		foreach(ButtonUnit node in m_arButtonUnit)
		{
			node.ChangeNumber(m_CurType, m_NumberPage);
		}
	}

    public void OnClick_OpenAll()
    {
        foreach (ButtonUnit node in m_arButtonUnit)
        {
            node.OnClickOpenAll();
        }
    }

    public void OnClick_CloseAll()
    {
        foreach (ButtonUnit node in m_arButtonUnit)
        {
            node.OnClickCloseAll();
        }
    }
	#endregion
}
