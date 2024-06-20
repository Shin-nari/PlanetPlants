using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.XR;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public Transform characterBody;
    [SerializeField]
    public Transform cameraArm;

    private Animator animator;

    //추후에 아이콘 등으로 대체
    public Image water; //물주는 중 
    public Image seed; //씨앗뿌리는 중 
    public Image plant; //괭이, 수확 중

    public GameObject SprayerPrefab; // 분무기 오브젝트
    public bool holdingSprayer = false; // 분무기를 들고 있는지 여부
    private GameObject heldSprayer;   // 들고 있는 분무기 오브젝트


    public GameObject ShovelPrefab;  // 괭이 오브젝트
    public bool holdingShoveler = false; // 괭이를 들고 있는지 여부
    private GameObject heldShovel;   // 들고 있는 괭이 오브젝트


    public GameObject SeedPrefab; // seed_a 프리팹
    private GameObject heldSeed; // 들고 있는 씨앗 오브젝트

    public GameObject SeedPrefab2; // 두 번째 식물의 씨앗 프리팹
    private GameObject heldSeed2; // 들고 있는 두 번째 식물의 씨앗 오브젝트
    public bool holdingSeed2 = false; // 두 번째 식물의 씨앗을 들고 있는지 여부

    public GameObject SeedPrefab3; // 세 번째 식물의 씨앗 프리팹
    private GameObject heldSeed3; // 들고 있는 세 번째 식물의 씨앗 오브젝트
    public bool holdingSeed3 = false; // 세 번째 식물의 씨앗을 들고 있는지 여부

    private GameObject heldItem; // 현재 들고 있는 아이템
    public Transform hand; // 플레이어의 손 위치

    public bool holdingSeed = false; // 씨앗을 들고 있는지 여부

    public delegate void SprayAction(bool holdingSprayer); // 분무기를 들거나 내려놓을 때 호출될 델리게이트
    public static event SprayAction OnSprayerToggled;

    public delegate void ShovelAction(bool holdingSprayer); //괭이를 들거나 내려놓을 때 호출될 델리게이트
    public static event ShovelAction OnShovelerToggled;

    private GameObject currentSprayer;
    // 정적으로 접근 가능한 PlayerController 인스턴스
    public static PlayerController Instance { get; private set; }

    public KeyCode interactKey = KeyCode.T; // 상호작용 키 설정


    public string animationTriggerName = "Action";

    private bool isAnimating = false; //애니메이션 중인지 여부를 나타내는 변수

    void Start()
    {

        // 'Hand_R' 오브젝트를 찾아서 hand 변수에 할당
        hand = transform.Find("Hand");

        //// 캐릭터에 붙어있는 애니메이터 컴포넌트를 가져와서 저장. 
        animator = GetComponentInChildren<Animator>();

        // hand 오브젝트의 부모를 캐릭터의 Transform으로 설정
        hand.SetParent(characterBody);

    }

    void Update()
    {
        LookAround();
        Move();

        HandleActions();

        // 키 입력을 감지하여 분무기를 들거나 내려놓기
       /* if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleSprayer();

            // 플레이어가 움직일 때마다 분무기의 위치를 조정
            if (holdingSprayer)
            {
                SprayerPrefab.transform.position = hand.position;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleShoveler();

            // 플레이어가 움직일 때마다 괭이의 위치를 조정
            if (holdingShoveler)
            {
                ShovelPrefab.transform.position = hand.position;
            }
        }

        // 이 부분에 식물 프리팹을 들 수 있는 기능 추가
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TogglePlantSeed();

            if (holdingSeed)
            {
                SeedPrefab.transform.position = hand.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            TogglePlantSeed2();

            if (holdingSeed2)
            {
                SeedPrefab2.transform.position = hand.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            TogglePlantSeed3();

            if (holdingSeed3)
            {
                SeedPrefab3.transform.position = hand.position;
            }
        }
        // 마우스 우클릭 시 씨앗 심기
        if (Input.GetMouseButtonDown(1))
        {
            PlantSeed();
        }*/

    }
    void PlantSeed()
    {
        // 씨앗을 들고 있고, 마우스가 밭을 클릭한 경우에만 실행
        if (holdingSeed || holdingSeed2 || holdingSeed3)
        {
            // 마우스로 클릭된 지점에서 ray를 발사하여 충돌한 객체 정보를 가져옴
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("FarmField"))
                {
                    GameObject plantImage;
                    if (holdingSeed)
                        plantImage = Instantiate(SeedPrefab, hit.point, Quaternion.identity);
                    if (holdingSeed2)
                        plantImage = Instantiate(SeedPrefab2, hit.point, Quaternion.identity);
                    if (holdingSeed3)
                        plantImage = Instantiate(SeedPrefab3, hit.point, Quaternion.identity);
                    Debug.Log("씨앗을 밭에 심음");

                    //animator.SetBool("isAction", true);
                }
            }
            else
            {
                //animator.SetBool("isAction", false); // 추가: 애니메이션을 비활성화하는 부분
            }
        }

    }

    public void TogglePlantSeed3()
    {

        holdingSeed3 = !holdingSeed3; // 씨앗을 들거나 내려놓음

        if (holdingSeed3)
        {
            // 씨앗을 플레이어의 손 위치로 이동시킴
            SeedPrefab3.transform.position = hand.position;
            heldSeed3 = Instantiate(SeedPrefab3, hand.position, Quaternion.identity);
            heldSeed3.transform.parent = hand; // 씨앗을 손의 자식으로 설정

            holdingSeed3 = true;
            Debug.Log("씨앗 들기");

            //seed.gameObject.SetActive(true); //씨앗 활성화
        }
        else
        {
            // 씨앗을 플레이어 앞에 떠 있도록 위치 조정
            SeedPrefab3.transform.position = transform.position + transform.forward * 2f; // 예시로 캐릭터 앞으로 2m 이동
            SeedPrefab3.transform.parent = null; // 씨앗의 부모를 해제하여 씬의 루트에 배치
            holdingSeed3 = false;

            // 씨앗 비활성화
            // Destroy(heldSeed);
            heldSeed3.SetActive(false);
            Debug.Log("씨앗 내려놓기");

            //seed.gameObject.SetActive(false); //씨앗 비활성화
        }
    }
    public void TogglePlantSeed2()
    {

        holdingSeed2 = !holdingSeed2; // 씨앗을 들거나 내려놓음

        if (holdingSeed2)
        {
            // 씨앗을 플레이어의 손 위치로 이동시킴
            SeedPrefab2.transform.position = hand.position;
            heldSeed2 = Instantiate(SeedPrefab2, hand.position, Quaternion.identity);
            heldSeed2.transform.parent = hand; // 씨앗을 손의 자식으로 설정

            holdingSeed2 = true;
            Debug.Log("씨앗 들기");

            //seed.gameObject.SetActive(true); //씨앗 활성화
        }
        else
        {
            // 씨앗을 플레이어 앞에 떠 있도록 위치 조정
            SeedPrefab2.transform.position = transform.position + transform.forward * 2f; // 예시로 캐릭터 앞으로 2m 이동
            SeedPrefab2.transform.parent = null; // 씨앗의 부모를 해제하여 씬의 루트에 배치
            holdingSeed2 = false;

            // 씨앗 비활성화
            // Destroy(heldSeed);
            heldSeed2.SetActive(false);
            Debug.Log("씨앗 내려놓기");

            //seed.gameObject.SetActive(false); //씨앗 비활성화
        }
    }
    public void TogglePlantSeed()
    {

        holdingSeed = !holdingSeed; // 씨앗을 들거나 내려놓음

        if (holdingSeed)
        {
            // 씨앗을 플레이어의 손 위치로 이동시킴
            SeedPrefab.transform.position = hand.position;
            heldSeed = Instantiate(SeedPrefab, hand.position, Quaternion.identity);
            heldSeed.transform.parent = hand; // 씨앗을 손의 자식으로 설정

            holdingSeed = true;
            Debug.Log("씨앗 들기");

            //seed.gameObject.SetActive(true); //씨앗 활성화
        }
        else
        {
            // 씨앗을 플레이어 앞에 떠 있도록 위치 조정
            SeedPrefab.transform.position = transform.position + transform.forward * 2f; // 예시로 캐릭터 앞으로 2m 이동
            SeedPrefab.transform.parent = null; // 씨앗의 부모를 해제하여 씬의 루트에 배치
            holdingSeed = false;

            // 씨앗 비활성화
            // Destroy(heldSeed);
            heldSeed.SetActive(false);
            Debug.Log("씨앗 내려놓기");

            //seed.gameObject.SetActive(false); //씨앗 비활성화
        }
    }

    public void ToggleShoveler()
    {
        holdingShoveler = !holdingShoveler;

        if (holdingShoveler)
        {

            // 괭이를 플레이어의 손 위치로 이동시킴
            heldShovel = Instantiate(ShovelPrefab, hand.position, Quaternion.identity);
            heldShovel.transform.parent = hand; // 괭이를 손에 부모로 설정
            // 괭이의 로테이션을 조정하여 원하는 방향으로 회전시킴
            heldShovel.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            holdingShoveler = true;
            Debug.Log("괭이 장착");

            //plant.gameObject.SetActive(true); //괭이 활성화

            //// 괭이를 플레이어의 손 위치로 이동시킴
            //ShovelPrefab.transform.position = hand.position;
            //ShovelPrefab.transform.parent = hand; // 괭이를 손에 부모로 설정
            //holdingShoveler = true;
            //Debug.Log("괭이 장착");

            //plant.gameObject.SetActive(true); //괭이 활성화
        }
        else
        {
            // 괭이를 플레이어 앞에 떠 있도록 위치 조정
            Destroy(heldShovel); // 괭이 오브젝트를 제거
            holdingShoveler = false;
            Debug.Log("괭이 해제");

           /// plant.gameObject.SetActive(false); //괭이 비활성화

            //// 괭이를 플레이어 앞에 떠 있도록 위치 조정
            //ShovelPrefab.transform.position = transform.position + transform.forward * 2f; // 예시로 캐릭터 앞으로 2m 이동
            //ShovelPrefab.transform.parent = null; // 괭이의 부모를 해제하여 씬의 루트에 배치
            //holdingShoveler = false;
            //Debug.Log("괭이 해제");

            //plant.gameObject.SetActive(false); //괭이 비활성화
        }
        // 이벤트 발생
        OnShovelerToggled?.Invoke(holdingShoveler);
    }

    public void ToggleSprayer()
    {
        holdingSprayer = !holdingSprayer; // 분무기를 들거나 내려놓음

        if (holdingSprayer)
        {

            // 괭이를 플레이어의 손 위치로 이동시킴
            heldSprayer = Instantiate(SprayerPrefab, hand.position, Quaternion.identity);
            heldSprayer.transform.parent = hand; // 괭이를 손에 부모로 설정
            // 괭이의 로테이션을 조정하여 원하는 방향으로 회전시킴
            heldSprayer.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            holdingSprayer = true;
            Debug.Log("분무기 장착");
            //// 분무기를 플레이어의 손 위치로 이동시킴
            //SprayerPrefab.transform.position = hand.position;
            //SprayerPrefab.transform.parent = hand; // 분무기를 손에 부모로 설정
            //holdingSprayer = true;
            //Debug.Log("분무기 장착");

           // water.gameObject.SetActive(true); //물뿌리는 활성화
        }

        else
        {
            // 분무기를 플레이어 앞에 떠 있도록 위치 조정
            Destroy(heldSprayer); // 괭이 오브젝트를 제거
            holdingSprayer = false;
            Debug.Log("분무기 해제");
            //water.gameObject.SetActive(false); //물뿌리는 비활성화
        }
        // 이벤트 발생
        OnSprayerToggled?.Invoke(holdingSprayer);
    }

    private void Move()
    {
        // Input.GetAxis로 가로 세로 이동 입력값을 가져옴.
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // moveInput이 0이면 이동이 발생하지 않는 것이도 0이 아니면 이동이 발생하는 것.
        bool isMove = moveInput.magnitude != 0;

        // ismove가 발생하면 걷는 애니메이션 작동 ismove가 발생하지 않는 다면 대기 애니메이션 작동
        //animator.SetBool("isWalk", isMove);

        if (isMove)
        {
            // 평면화 시켜서 저장
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            // 바라보고 있는 방향으로 이동
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            // 캐릭터가 이동할 때 카메라가 바라보는 방향으로 바라보게 함.
            characterBody.forward = lookForward;
            // 캐릭터 이동
            transform.position += moveDir * Time.deltaTime * 5f;

            animator.SetBool("isWalk", true);
            animator.SetBool("isRun", false);

            // 캐릭터 달리기
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.position += moveDir * Time.deltaTime * 20f;
                animator.SetBool("isWalk", false);
                animator.SetBool("isRun", true);
            }
            else
            {
                animator.SetBool("isRun", false);
            }

        }
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            animator.Play("Idle");
            animator.SetBool("isWalk", false);
        }
    }
    void HandleActions()
    {
        if (Input.GetKeyDown(KeyCode.T) && !isAnimating)
        {
            StartCoroutine(PlayActionAnimation("Action", plant));

            TogglePlantSeed3();

            if (holdingSeed3)
            {
                SeedPrefab3.transform.position = hand.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && !isAnimating)
        {
            StartCoroutine(PlayActionAnimation("Action", water));

            ToggleSprayer();

            // 플레이어가 움직일 때마다 분무기의 위치를 조정
            if (holdingSprayer)
            {
                SprayerPrefab.transform.position = hand.position;
            }
        }

        if ((Input.GetKeyDown(KeyCode.R) || Input.GetMouseButton(1)) && !isAnimating)
        {
            StartCoroutine(PlayActionAnimation("Action", plant));
            if (Input.GetKeyDown(KeyCode.R))
            {
                ToggleShoveler();

                // 플레이어가 움직일 때마다 괭이의 위치를 조정
                if (holdingShoveler)
                {
                    ShovelPrefab.transform.position = hand.position;
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                PlantSeed();
            }
        }

        if ((Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.U)) && !isAnimating)
        {
            StartCoroutine(PlayActionAnimation("Action", seed));
            if (Input.GetKeyDown(KeyCode.Y))
            {
                TogglePlantSeed();

                if (holdingSeed)
                {
                    SeedPrefab.transform.position = hand.position;
                }
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                TogglePlantSeed2();

                if (holdingSeed2)
                {
                    SeedPrefab2.transform.position = hand.position;
                }
            }
        }

        // 애니메이션이 끝난 후 isAnimating을 false로 설정
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Action") && stateInfo.normalizedTime >= 1.0f) 
        {
            animator.SetBool("isAction", false);
            isAnimating = false; // 애니메이션 상태를 false로 설정
        }
    }

    IEnumerator PlayActionAnimation(string animationName, Image actionImage)
    {
        isAnimating = true;
        animator.SetBool("isAction", true); // 애니메이션 시작을 알리는 상태 설정
        actionImage.gameObject.SetActive(true); // 액션 이미지 활성화

        // 애니메이션이 끝날 때까지 대기
        yield return new WaitForSeconds(1f);

        // 애니메이션 종료 처리
        actionImage.gameObject.SetActive(false); // 액션 이미지 비활성화
        animator.SetBool("isAction", false); // 애니메이션 종료 상태 설정
        isAnimating = false;

        // Idle 상태로 돌아가기
        animator.Play("Idle");
    }

    // 마우스 움직임에 따라서 카메라 회전.
    private void LookAround()
    {
        // 이전 위치에서 얼마나 움직였는지 알게 해주고 mouseDelta 변수에 저장
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        // 카메라 암의 회전 값을 오일러 값으로 변환.
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        // 카메라가 뒤집어 지는 것을 방지.
        // 카메라 상하 움직임 제한.
        // x가 180도 보다 작은 경우는 위로 회전 하는 경우.
        // 0도가 아닌 -1도를 최저점 으로 하는 이유는 0도로 하면 카메라가 수평면 이하로 내려가지 않는 문제가 발생.
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }

        else
        {
            // x값이 180보다 큰 경우는 아래로 회전 하는 경우.
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);

    }
}