using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2f; // ���̵� ��/�ƿ� ���� �ð�

    public PlayerHealth playerHealth;

    void Update()
    {
        End();
    }

    // �ӽ÷� Ű ����
    // �÷��̾� Hp ���� �ڵ� �ۼ��ؾ� ��
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

    // ���̵� �ƿ� �� �� �ε� �ڷ�ƾ �Լ�
    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        // ���̵� �ƿ�
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = timer / fadeDuration;
            fadeImage.color = Color.Lerp(Color.clear, Color.black, alpha);
            yield return null;
        }

        // �� �ε�
        SceneManager.LoadScene(sceneName);

        // ���̵� ��
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
