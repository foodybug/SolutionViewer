using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuitCtrl : MonoBehaviour {

    [SerializeField] Slider sliderR;
    [SerializeField] Slider sliderG;
    [SerializeField] Slider sliderB;
    
    public void InitColor()
    {
        sliderR.value = PlayerPrefs.GetFloat("R", 0.1f);
        sliderG.value = PlayerPrefs.GetFloat("G", 0.2f);
        sliderB.value = PlayerPrefs.GetFloat("B", 0.4f);

        _ColorChange(0f);

        Slider.SliderEvent e = new Slider.SliderEvent();
        e.AddListener(_ColorChange);
        sliderR.onValueChanged = e;
        sliderG.onValueChanged = e;
        sliderB.onValueChanged = e;
    }

    void _ColorChange(float v)
    {
        Camera.main.backgroundColor = new Color(sliderR.value, sliderG.value, sliderB.value);

        PlayerPrefs.SetFloat("R", sliderR.value);
        PlayerPrefs.SetFloat("G", sliderG.value);
        PlayerPrefs.SetFloat("B", sliderB.value);
    }

    void OnEnable()
    {
        if (AdvertisementManager.Instance != null)
            AdvertisementManager.Instance.Show_Banner();
    }

    void Update()
    {
        _PressBtn_Back();
    }

    void _PressBtn_Back()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            OnClick_No();
    }

    public void OnClick_Open()
    {
        gameObject.SetActive(true);
    }

    public void OnClick_Yes()
    {
        //if (AdvertisementManager.Instance != null)
        //{
        //    //AdvertisementManager.Instance.Show_Banner();
        //    AdvertisementManager.Instance.Show_Interstitial();
        //}
        //
        //Invoke("_Quit", 0.5f);
        _Quit();
    }

    void _Quit()
    {
        Application.Quit();
    }

    public void OnClick_No()
    {
        gameObject.SetActive(false);

        if (AdvertisementManager.Instance != null)
            AdvertisementManager.Instance.Close_Banner();
    }

    public void OnClick_DeveloperBlog()
    {
        Application.OpenURL("http://foodybug.egloos.com/4107979");
    }
}
