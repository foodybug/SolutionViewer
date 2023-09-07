using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonUnit : MonoBehaviour {

	#region - static -
	const int UnitCount = 5;
	const int SubExamMaxPage = 3;
	const int MainExamMaxPage = 7;
	const string BlindText = "x";

	const int MaxNumber_Sub = 15;
	const int MaxNumber_End = 35;
    const int MaxNumber_Ssn = 50;
	#endregion
	#region - serialized -
	[SerializeField] int index_;

	[SerializeField] Button btn1_;
	[SerializeField] Button btn2_;
	[SerializeField] Button btn3_;
	[SerializeField] Button btn4_;
	[SerializeField] Button btn5_;
	[SerializeField] Button btnAll_;

	[SerializeField] Text txt1_;
	[SerializeField] Text txt2_;
	[SerializeField] Text txt3_;
	[SerializeField] Text txt4_;
	[SerializeField] Text txt5_;
	#endregion
	#region - member -
	string[] m_Solution = new string[5]{BlindText, BlindText, BlindText, BlindText, BlindText};
	int m_BeginIndex = 0;
	#endregion
	#region - init & release & update -
	void _InitNumber(int _offset)
	{
		if(gameObject.activeSelf == false)
			return;

		btn1_.GetComponentInChildren<Text>().text = (m_BeginIndex + _offset + 1).ToString();
		btn2_.GetComponentInChildren<Text>().text = (m_BeginIndex + _offset + 2).ToString();
		btn3_.GetComponentInChildren<Text>().text = (m_BeginIndex + _offset + 3).ToString();
		btn4_.GetComponentInChildren<Text>().text = (m_BeginIndex + _offset + 4).ToString();
		btn5_.GetComponentInChildren<Text>().text = (m_BeginIndex + _offset + 5).ToString();
	}

	void Start()
	{
		m_BeginIndex = UnitCount * (index_ - 1);

		_InitNumber(0);
	}

	public void Init(eExamType _type, SortedDictionary<int, string> _solution)
	{
		_BlindSolution();

		if(index_ >= 4 && _type == eExamType.Sub)
		{
			gameObject.SetActive(false);
			m_Solution = new string[5]{BlindText, BlindText, BlindText, BlindText, BlindText};
			return;
		}

		gameObject.SetActive(true);

        _InitSolution(_solution);
	}

	void _InitSolution(SortedDictionary<int, string> _solution)
	{
        if (_solution == null)
        {
            for (int i = 0; i < UnitCount; ++i)
            {
                m_Solution[i] = "x";
            }

            return;
        }
        
        for (int i=0; i<UnitCount; ++i)
		{
			if(_solution.ContainsKey(m_BeginIndex + i + 1) == true)
				m_Solution[i] = _solution[m_BeginIndex + i + 1];
			else
			{
				//Debug.Log("ButtonUnit:: _InitSolution: key not is found. panel index = " + index_ +
				//          ", index = " + (m_BeginIndex + i + 1));

				m_Solution[i] = "x";
			}
		}
	}
    void _OpenSolution()
    {
        txt1_.text = m_Solution[0];
        txt2_.text = m_Solution[1];
        txt3_.text = m_Solution[2];
        txt4_.text = m_Solution[3];
        txt5_.text = m_Solution[4];
    }
    void _BlindSolution()
	{
		txt1_.text = BlindText;
		txt2_.text = BlindText;
		txt3_.text = BlindText;
		txt4_.text = BlindText;
		txt5_.text = BlindText;
	}
	#endregion
	#region - public -
	public void ChangeNumber(eExamType _type, int _numberPage)
	{
		int offset = _type == eExamType.Sub ? MaxNumber_Sub : MaxNumber_End;
        switch(_type)
        {
            case eExamType.Sub:
                offset = MaxNumber_Sub;
                break;
            case eExamType.End:
                offset = MaxNumber_End;
                break;
            case eExamType.Ssn:
                offset = MaxNumber_Ssn;
                break;
        }

        offset *= _numberPage;
        _InitNumber(offset);
	}
	#endregion
	#region - delegate -
	public void OnClickButton1()
	{
		if(txt1_.text == BlindText)
			txt1_.text = m_Solution[0];
		else
			txt1_.text = BlindText;
	}
	public void OnClickButton2()
	{
		if(txt2_.text == BlindText)
			txt2_.text = m_Solution[1];
		else
			txt2_.text = BlindText;
	}
	public void OnClickButton3()
	{
		if(txt3_.text == BlindText)
			txt3_.text = m_Solution[2];
		else
			txt3_.text = BlindText;
	}
	public void OnClickButton4()
	{
		if(txt4_.text == BlindText)
			txt4_.text = m_Solution[3];
		else
			txt4_.text = BlindText;
	}
	public void OnClickButton5()
	{
		if(txt5_.text == BlindText)
			txt5_.text = m_Solution[4];
		else
			txt5_.text = BlindText;
	}
	public void OnClickOpenAll()
	{
		_OpenSolution();
	}
    public void OnClickCloseAll()
    {
        _BlindSolution();
    }
    #endregion
}
