using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    // Skybox 회전 속도
    public float rotationSpeed = 1f;

    void Update()
    {
        // 현재 회전 각도 계산
        float rotation = Time.time * rotationSpeed;

        // Skybox의 rotation 설정
        RenderSettings.skybox.SetFloat("_Rotation", rotation);
    }
}
