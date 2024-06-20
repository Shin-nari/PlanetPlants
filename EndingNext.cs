using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingNext : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2f; // ���̵� ��/�ƿ� ���� �ð�
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
