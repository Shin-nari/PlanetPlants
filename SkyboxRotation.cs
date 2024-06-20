using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    // Skybox ȸ�� �ӵ�
    public float rotationSpeed = 1f;

    void Update()
    {
        // ���� ȸ�� ���� ���
        float rotation = Time.time * rotationSpeed;

        // Skybox�� rotation ����
        RenderSettings.skybox.SetFloat("_Rotation", rotation);
    }
}
