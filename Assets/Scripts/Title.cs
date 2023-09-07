using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public delegate void dltVoid_Void();

public class Title : MonoBehaviour {

	[SerializeField] Slider loadingBar;
    [SerializeField] ModeSelectPopup modeSelectPopup;
	[SerializeField] float waitTime = 7f;

	// Use this for initialization
	IEnumerator Start () {

        _ColorProc();

        Debug.Log("BEGIN");

        Screen.SetResolution(438, 730, false);

        yield return new WaitForSeconds(1f);

        //Debug.Log("Ad.Instance == " + AdvertisementManager.Instance);
        if (AdvertisementManager.Instance != null)
        {
			float time = 0f;
            while (true)
            {
                yield return null;

				loadingBar.value = time / waitTime;

				if (AdvertisementManager.Instance.Check_InterstitialIsLoaded())
                {
					while (true)
					{
						yield return null;

						loadingBar.value += 0.1f;
						if (loadingBar.value >= 1f)
							break;
					}

					Debug.Log("Check_InterstitialIsLoaded() is success");
                    break;
                }
				else
				{
					time += Time.deltaTime;
					if (time > waitTime)
						break;
				}
			}
            
            AdvertisementManager.Instance.Show_Interstitial();
        }

        //Debug.Log("before LoadScene(Main)");
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        modeSelectPopup.Activate(OnSearchModeSelected);
    }

    void OnSearchModeSelected()
    {
        StartCoroutine(_SearchModeSelected());
    }

    AsyncOperation op;
    IEnumerator _SearchModeSelected()
    {
        yield return null;

        op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Main_New");
        //op.allowSceneActivation = false;
        while (true)
        {
            yield return null;

            Debug.Log(op.progress);
            loadingBar.value = op.progress;

            if (op.isDone == true)
                break;
        }

        //op.allowSceneActivation = true;
    }

    void _ColorProc()
    {
        float r = PlayerPrefs.GetFloat("R");
        float g = PlayerPrefs.GetFloat("G");
        float b = PlayerPrefs.GetFloat("B");
        
        Camera.main.backgroundColor = new Color(r, g, b);
    }

    public void OnDeveloper()
    {
        Debug.Log("developer page");

        Application.OpenURL("http://foodybug.egloos.com/4107979");
    }
}
