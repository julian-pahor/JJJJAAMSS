using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LoadingScreen : MonoBehaviour
{

    public GameObject loadScreenImage;

    bool loading;

    private void Start()
    {
        if (loadScreenImage != null)
            loadScreenImage.SetActive(false);
    }

    public void BeginLoad(string scene)
    {
        if(loading)
            return;

        loading = true;

        if(loadScreenImage != null)
            loadScreenImage.SetActive(true);

        StartCoroutine(AsyncLoad(scene));

    }

    IEnumerator AsyncLoad(string scene)
    {
        yield return new WaitForSeconds(1);

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        while(!operation.isDone)
        {
            yield return null;
        }
        
    }

}
