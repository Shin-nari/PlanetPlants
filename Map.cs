using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform 컴포넌트
    public Transform spaceShip; // 우주선의 Transform 컴포넌트
    public float activationDistance = 30f; // 활성화 거리

    public GameObject mapUI; // UI 창

    private bool isPlayerLocked = false; // 플레이어의 움직임을 제한하는 플래그
    private PlayerController playerController;

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        // 플레이어와 우주선 사이의 거리 계산
        float distanceToSpaceShip = Vector3.Distance(player.position, spaceShip.position);

        // 거리가 활성화 거리를 초과하는지 확인
        if (distanceToSpaceShip > activationDistance)
        {
            // UI 창을 활성화
            mapUI.SetActive(true);

            // 플레이어의 움직임을 제한
            isPlayerLocked = true;
        }
        else
        {
            // UI 창을 비활성화
            mapUI.SetActive(false);

            // 플레이어의 움직임 제한 해제
            isPlayerLocked = false;
        }

        // 플레이어의 움직임 제한
        if (isPlayerLocked)
        {
            //hp가 닳는 것으로..
        }
    }
}