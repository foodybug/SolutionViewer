using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void dltVoid_SearchedData(SearchedData data);

public class ResultContent : MonoBehaviour {

    Button btn;
    Text txt;
    dltVoid_SearchedData dlt;
    SearchedData data;

    void Awake()
    {
        btn = GetComponent<Button>();
        txt = btn.GetComponentInChildren<Text>();
        btn.onClick.AddListener(OnSelect);
    }
    
    public void Init(SearchedData data, dltVoid_SearchedData dlt)
    {
        this.data = data;
        this.dlt = dlt;

        txt.text = data.strResult;
    }
    
	void OnSelect()
    {
        dlt(data);
    }
}
