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

            //�ͷ��� ���� 0�� �κ�ã�� ����
            do
            {
                float newX = Random.Range(terrain.transform.position.x, terrain.transform.position.x + terrain.terrainData.size.x);
                float newZ = Random.Range(terrain.transform.position.z, terrain.transform.position.z + terrain.terrainData.size.z);
                newY = terrain.SampleHeight(new Vector3(newX, 0, newZ)) + terrain.transform.position.y;

                position = new Vector3(newX, newY, newZ);
            } while (newY != terrain.transform.position.y); //y���� �ͷ����� y���� ���� ������ �ݺ�

            GameObject item = Instantiate(Item, position, Quaternion.identity);

            Debug.Log($"position: {position}");

            //5�� �Ŀ� ������ �����ϴ� �ڷ�ƾ
            StartCoroutine(RemoveItemAfterTime(item, 5f));

            currTime = 0;
        }
    }

    //5�� �Ŀ� ���� �ڷ�ƾ
    IEnumerator RemoveItemAfterTime(GameObject item, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(item);

        Debug.Log("������ �����");
    }

    //������ ó��
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("������ ȹ��");
            Destroy(gameObject);
        }
    }
}
