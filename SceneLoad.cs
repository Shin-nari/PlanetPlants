using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoad : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2f; // 페이드 인/아웃 지속 시간
    public CanvasGroup[] buttonCanvasGroups; // 버튼 UI들의 CanvasGroup 배열

    // 게임 시작 여부
    private bool gameStarted = false;

    // 게임 시작 버튼 클릭 시 호출될 함수
    public void StartGame()
    {
        // 게임 시작 버튼 클릭 시 게임이 시작되었음을 설정합니다.
        gameStarted = true;
        StartCoroutine(FadeOutAndLoadScene());
    }

    // 페이드 아웃 및 씬 로드 코루틴 함수
    public IEnumerator FadeOutAndLoadScene()
    {
        // 페이드 아웃
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = timer / fadeDuration;
            fadeImage.color = Color.Lerp(Color.clear, Color.black, alpha);

            // 각 버튼 UI 투명도 조절
            foreach (CanvasGroup canvasGroup in buttonCanvasGroups)
            {
                canvasGroup.alpha = 1f - alpha;
            }

            yield return null;
        }

        // 버튼 UI 비활성화
        foreach (CanvasGroup canvasGroup in buttonCanvasGroups)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        // 씬 로드
        SceneManager.LoadScene("SampleScene");

        // 씬 로드 후 페이드 인
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
