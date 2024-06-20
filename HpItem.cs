using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpItem : MonoBehaviour
{
    public GameObject Item;
    public Terrain terrain; 

    float currTime = 0f;

    void Update()
    {
        currTime += Time.deltaTime;

        if (currTime > 5)
        {
            Vector3 position = Vector3.zero;
            float newY;

            //터레인 고도가 0인 부분찾는 루프
            do
            {
                float newX = Random.Range(terrain.transform.position.x, terrain.transform.position.x + terrain.terrainData.size.x);
                float newZ = Random.Range(terrain.transform.position.z, terrain.transform.position.z + terrain.terrainData.size.z);
                newY = terrain.SampleHeight(new Vector3(newX, 0, newZ)) + terrain.transform.position.y;

                position = new Vector3(newX, newY, newZ);
            } while (newY != terrain.transform.position.y); //y값이 터레인의 y값과 같을 때까지 반복

            GameObject item = Instantiate(Item, position, Quaternion.identity);

            Debug.Log($"position: {position}");

            //5초 후에 아이템 제거하는 코루틴
            StartCoroutine(RemoveItemAfterTime(item, 5f));

            currTime = 0;
        }
    }

    //5초 후에 제거 코루틴
    IEnumerator RemoveItemAfterTime(GameObject item, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(item);

        Debug.Log("아이템 사라짐");
    }

    //아이템 처리
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("아이템 획득");
            Destroy(gameObject);
        }
    }
}
