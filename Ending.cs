using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2f; // 페이드 인/아웃 지속 시간

    public PlayerHealth playerHealth;

    void Update()
    {
        End();
    }

    // 임시로 키 설정
    // 플레이어 Hp 관련 코드 작성해야 함
    void End()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(FadeOutAndLoadScene("GoodEnding"));
        }
        else if (Input.GetKeyDown(KeyCode.Space)) //playerHealth.currentHealth <= 0
        {
            StartCoroutine(FadeOutAndLoadScene("BadEnding"));
        }
    }

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
