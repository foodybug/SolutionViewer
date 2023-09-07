using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelectPopup : MonoBehaviour {

    [SerializeField] Button btnClassic;
    [SerializeField] Button btnSearch;

    dltVoid_Void dlt;

    private void Awake()
    {
        btnClassic.onClick.AddListener(OnClassic);
        btnSearch.onClick.AddListener(OnSearch);

        gameObject.SetActive(false);
    }

    public void Activate(dltVoid_Void dlt)
    {
        gameObject.SetActive(true);
        this.dlt = dlt;
    }

    void OnClassic()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    void OnSearch()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main_New");

        //gameObject.SetActive(false);

        //dlt();
    }
}
