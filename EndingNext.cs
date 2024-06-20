using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingNext : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2f; // 페이드 인/아웃 지속 시간
    //public Text spaceText;

    void Start()
    {
        //StartCoroutine(BlinkText());
    }

    void Update()
    {
        NextStart();
    }

    void NextStart()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FadeOutAndLoadScene("Start"));
        }
    }

    /*IEnumerator BlinkText()
    {
        while (true)
        {
            spaceText.text = "";
            yield return new WaitForSeconds(.3f);
            spaceText.text = "Spacebar to Start";
            yield return new WaitForSeconds(.3f);
        }
    }*/

    // 페이드 아웃 및 씬 로드 코루틴 함수
    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        // 페이드 아웃
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = timer / fadeDuration;
            fadeImage.color = Color.Lerp(Color.clear, Color.black, alpha);
            yield return null;
        }

        // 씬 로드
        SceneManager.LoadScene(sceneName);

        // 페이드 인
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = timer / fadeDuration;
            fadeImage.color = Color.Lerp(Color.black, Color.clear, alpha);
            yield return null;
        }
    }
}
