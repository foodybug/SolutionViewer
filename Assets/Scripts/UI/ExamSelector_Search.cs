using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ExamSelector_Search : MonoBehaviour {

    #region - serialized -
    [Header("솔루션")]
    [SerializeField] SearchDB searchDB;
    [SerializeField] Slider loadingBar;

    [Header("타입, 학기, 검색어")]
	[SerializeField] Text txtType;
    [SerializeField] Button typeL;
    [SerializeField] Button typeR;
    [SerializeField] Text txtSemester;
    [SerializeField] Button semesterL;
    [SerializeField] Button semesterR;

    [SerializeField] InputField inflClass;
    [SerializeField] Button btnSearch;

    [SerializeField] Button btnShowAll;
    [SerializeField] Button btnCloseAll;
    [SerializeField] Button btnChangeNumber;

    [Header("탐색 결과")]
    [SerializeField] VerticalLayoutGroup vLayout;
    [SerializeField] Text txtSelectedClass;
    [SerializeField] ResultContent resultContent;

    [Header("해답, 나가기")]
    [SerializeField] GameObject buttonUnitRoot;
    List<ButtonUnit> arButtonUnit = new List<ButtonUnit>();
    [SerializeField] QuitCtrl quitCtrl;
    #endregion
    #region - member -
    List<SearchedData> curSearchedData = null;
    //SearchedData curSelectedData = null;
    eExamType curType = eExamType.Sub;
    int curSemester = 1;

	int m_NumberPage = 1;
	const int MaxPage_Sub = 4;
	const int MaxPage_End = 2;
    const int MaxPage_Ssn = 1;
    #endregion
    #region - init & release & update -
    void Awake()
    {
        foreach(ButtonUnit node in buttonUnitRoot.GetComponentsInChildren<ButtonUnit>())
        {
            arButtonUnit.Add(node);
        }

        quitCtrl.InitColor();

        typeL.onClick.AddListener(OnType_Left);
        typeR.onClick.AddListener(OnType_Right);
        semesterL.onClick.AddListener(OnSemester_Left);
        semesterR.onClick.AddListener(OnSemester_Right);

        btnSearch.onClick.AddListener(OnSearch);

        btnShowAll.onClick.AddListener(OnClick_OpenAll);
        btnCloseAll.onClick.AddListener(OnClick_CloseAll);
        btnChangeNumber.onClick.AddListener(OnClickButton_ChangeNumber);
    }

    IEnumerator Start()
	{
        _SetSolution(null);

        loadingBar.transform.parent.gameObject.SetActive(true);

        float startTime = Time.realtimeSinceStartup;
        while (true)
        {
            //yield return new WaitForSeconds(0.5f);
            yield return null;

            Debug.Log("searchDB.loadingProgress = " + searchDB.loadingProgress);
            loadingBar.value = searchDB.loadingProgress;

            if (searchDB.loadingProgress >= 1f)
                break;
        }

        Debug.Log("initializing time = " + (Time.realtimeSinceStartup - startTime).ToString("f2"));
        loadingBar.transform.parent.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            quitCtrl.gameObject.SetActive(true);
    }
	#endregion
	#region - event -
    void OnType_Left()
    {
        if (curType == eExamType.Sub)
            return;
        
        --curType;

        txtType.text = _GetCurExamTypeString();

        foreach (Transform node in vLayout.transform)
        {
            Destroy(node.gameObject);
        }

        _SetSolution(null);

        txtSelectedClass.text = "-";
    }

    void OnType_Right()
    {
        if (curType == eExamType.Ssn)
            return;

        ++curType;

        txtType.text = _GetCurExamTypeString();

        foreach (Transform node in vLayout.transform)
        {
            Destroy(node.gameObject);
        }

        _SetSolution(null);

        txtSelectedClass.text = "-";
    }

    void OnSemester_Left()
    {
        curSemester = 1;
        txtSemester.text = "1";

        foreach (Transform node in vLayout.transform)
        {
            Destroy(node.gameObject);
        }

        _SetSolution(null);

        txtSelectedClass.text = "-";
    }

    void OnSemester_Right()
    {
        curSemester = 2;
        txtSemester.text = "2";

        txtType.text = _GetCurExamTypeString();

        foreach (Transform node in vLayout.transform)
        {
            Destroy(node.gameObject);
        }

        _SetSolution(null);

        txtSelectedClass.text = "-";
    }

    string _GetCurExamTypeString()
    {
        switch (curType)
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

    void OnSearch()
    {
        curSearchedData = searchDB.GetSearchedData(curType, curSemester, inflClass.text);
        //curSelectedData = null;

        if (curSearchedData == null)
            return;

        foreach(Transform node in vLayout.transform)
        {
            Destroy(node.gameObject);
        }

        foreach (SearchedData node in curSearchedData)
        {
            GameObject obj = Instantiate(resultContent.gameObject, vLayout.transform);
            ResultContent content = obj.GetComponent<ResultContent>();
            content.Init(node, OnSelect);
        }

        m_NumberPage = -1;
        OnClickButton_ChangeNumber();
    }

    void OnSelect(SearchedData data)
    {
        string str = data.strResult;
        txtSelectedClass.text = str.Replace(":", "\n");

        _SetSolution(data.solution);
    }
	#endregion
	#region - public -
	#endregion
	#region - method -
	void _SetSolution(SortedDictionary<int, string> _solution)
	{
		foreach(ButtonUnit node in arButtonUnit)
		{
			node.Init(curType, _solution);
		}
	}
	#endregion
	#region - delegate -
	public void OnClickButton_ChangeNumber()
	{
		m_NumberPage++;
        switch(curType)
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
		//if(curSearchedData == null || curSelectedData == null)
  //          return;
        
		foreach(ButtonUnit node in arButtonUnit)
		{
			node.ChangeNumber(curType, m_NumberPage);
		}
	}

    public void OnClick_OpenAll()
    {
        foreach (ButtonUnit node in arButtonUnit)
        {
            node.OnClickOpenAll();
        }
    }

    public void OnClick_CloseAll()
    {
        foreach (ButtonUnit node in arButtonUnit)
        {
            node.OnClickCloseAll();
        }
    }
	#endregion
}
