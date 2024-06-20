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

    //���Ŀ� ������ ������ ��ü
    public Image water; //���ִ� �� 
    public Image seed; //���ѻѸ��� �� 
    public Image plant; //����, ��Ȯ ��

    public GameObject SprayerPrefab; // �й��� ������Ʈ
    public bool holdingSprayer = false; // �й��⸦ ��� �ִ��� ����
    private GameObject heldSprayer;   // ��� �ִ� �й��� ������Ʈ


    public GameObject ShovelPrefab;  // ���� ������Ʈ
    public bool holdingShoveler = false; // ���̸� ��� �ִ��� ����
    private GameObject heldShovel;   // ��� �ִ� ���� ������Ʈ


    public GameObject SeedPrefab; // seed_a ������
    private GameObject heldSeed; // ��� �ִ� ���� ������Ʈ

    public GameObject SeedPrefab2; // �� ��° �Ĺ��� ���� ������
    private GameObject heldSeed2; // ��� �ִ� �� ��° �Ĺ��� ���� ������Ʈ
    public bool holdingSeed2 = false; // �� ��° �Ĺ��� ������ ��� �ִ��� ����

    public GameObject SeedPrefab3; // �� ��° �Ĺ��� ���� ������
    private GameObject heldSeed3; // ��� �ִ� �� ��° �Ĺ��� ���� ������Ʈ
    public bool holdingSeed3 = false; // �� ��° �Ĺ��� ������ ��� �ִ��� ����

    private GameObject heldItem; // ���� ��� �ִ� ������
    public Transform hand; // �÷��̾��� �� ��ġ

    public bool holdingSeed = false; // ������ ��� �ִ��� ����

    public delegate void SprayAction(bool holdingSprayer); // �й��⸦ ��ų� �������� �� ȣ��� ��������Ʈ
    public static event SprayAction OnSprayerToggled;

    public delegate void ShovelAction(bool holdingSprayer); //���̸� ��ų� �������� �� ȣ��� ��������Ʈ
    public static event ShovelAction OnShovelerToggled;

    private GameObject currentSprayer;
    // �������� ���� ������ PlayerController �ν��Ͻ�
    public static PlayerController Instance { get; private set; }

    public KeyCode interactKey = KeyCode.T; // ��ȣ�ۿ� Ű ����


    public string animationTriggerName = "Action";

    private bool isAnimating = false; //�ִϸ��̼� ������ ���θ� ��Ÿ���� ����

    void Start()
    {

        // 'Hand_R' ������Ʈ�� ã�Ƽ� hand ������ �Ҵ�
        hand = transform.Find("Hand");

        //// ĳ���Ϳ� �پ��ִ� �ִϸ����� ������Ʈ�� �����ͼ� ����. 
        animator = GetComponentInChildren<Animator>();

        // hand ������Ʈ�� �θ� ĳ������ Transform���� ����
        hand.SetParent(characterBody);

    }

    void Update()
    {
        LookAround();
        Move();

        HandleActions();

        // Ű �Է��� �����Ͽ� �й��⸦ ��ų� ��������
       /* if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleSprayer();

            // �÷��̾ ������ ������ �й����� ��ġ�� ����
            if (holdingSprayer)
            {
                SprayerPrefab.transform.position = hand.position;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleShoveler();

            // �÷��̾ ������ ������ ������ ��ġ�� ����
            if (holdingShoveler)
            {
                ShovelPrefab.transform.position = hand.position;
            }
        }

        // �� �κп� �Ĺ� �������� �� �� �ִ� ��� �߰�
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
        // ���콺 ��Ŭ�� �� ���� �ɱ�
        if (Input.GetMouseButtonDown(1))
        {
            PlantSeed();
        }*/

    }
    void PlantSeed()
    {
        // ������ ��� �ְ�, ���콺�� ���� Ŭ���� ��쿡�� ����
        if (holdingSeed || holdingSeed2 || holdingSeed3)
        {
            // ���콺�� Ŭ���� �������� ray�� �߻��Ͽ� �浹�� ��ü ������ ������
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
                    Debug.Log("������ �翡 ����");

                    //animator.SetBool("isAction", true);
                }
            }
            else
            {
                //animator.SetBool("isAction", false); // �߰�: �ִϸ��̼��� ��Ȱ��ȭ�ϴ� �κ�
            }
        }

    }

    public void TogglePlantSeed3()
    {

        holdingSeed3 = !holdingSeed3; // ������ ��ų� ��������

        if (holdingSeed3)
        {
            // ������ �÷��̾��� �� ��ġ�� �̵���Ŵ
            SeedPrefab3.transform.position = hand.position;
            heldSeed3 = Instantiate(SeedPrefab3, hand.position, Quaternion.identity);
            heldSeed3.transform.parent = hand; // ������ ���� �ڽ����� ����

            holdingSeed3 = true;
            Debug.Log("���� ���");

            //seed.gameObject.SetActive(true); //���� Ȱ��ȭ
        }
        else
        {
            // ������ �÷��̾� �տ� �� �ֵ��� ��ġ ����
            SeedPrefab3.transform.position = transform.position + transform.forward * 2f; // ���÷� ĳ���� ������ 2m �̵�
            SeedPrefab3.transform.parent = null; // ������ �θ� �����Ͽ� ���� ��Ʈ�� ��ġ
            holdingSeed3 = false;

            // ���� ��Ȱ��ȭ
            // Destroy(heldSeed);
            heldSeed3.SetActive(false);
            Debug.Log("���� ��������");

            //seed.gameObject.SetActive(false); //���� ��Ȱ��ȭ
        }
    }
    public void TogglePlantSeed2()
    {

        holdingSeed2 = !holdingSeed2; // ������ ��ų� ��������

        if (holdingSeed2)
        {
            // ������ �÷��̾��� �� ��ġ�� �̵���Ŵ
            SeedPrefab2.transform.position = hand.position;
            heldSeed2 = Instantiate(SeedPrefab2, hand.position, Quaternion.identity);
            heldSeed2.transform.parent = hand; // ������ ���� �ڽ����� ����

            holdingSeed2 = true;
            Debug.Log("���� ���");

            //seed.gameObject.SetActive(true); //���� Ȱ��ȭ
        }
        else
        {
            // ������ �÷��̾� �տ� �� �ֵ��� ��ġ ����
            SeedPrefab2.transform.position = transform.position + transform.forward * 2f; // ���÷� ĳ���� ������ 2m �̵�
            SeedPrefab2.transform.parent = null; // ������ �θ� �����Ͽ� ���� ��Ʈ�� ��ġ
            holdingSeed2 = false;

            // ���� ��Ȱ��ȭ
            // Destroy(heldSeed);
            heldSeed2.SetActive(false);
            Debug.Log("���� ��������");

            //seed.gameObject.SetActive(false); //���� ��Ȱ��ȭ
        }
    }
    public void TogglePlantSeed()
    {

        holdingSeed = !holdingSeed; // ������ ��ų� ��������

        if (holdingSeed)
        {
            // ������ �÷��̾��� �� ��ġ�� �̵���Ŵ
            SeedPrefab.transform.position = hand.position;
            heldSeed = Instantiate(SeedPrefab, hand.position, Quaternion.identity);
            heldSeed.transform.parent = hand; // ������ ���� �ڽ����� ����

            holdingSeed = true;
            Debug.Log("���� ���");

            //seed.gameObject.SetActive(true); //���� Ȱ��ȭ
        }
        else
        {
            // ������ �÷��̾� �տ� �� �ֵ��� ��ġ ����
            SeedPrefab.transform.position = transform.position + transform.forward * 2f; // ���÷� ĳ���� ������ 2m �̵�
            SeedPrefab.transform.parent = null; // ������ �θ� �����Ͽ� ���� ��Ʈ�� ��ġ
            holdingSeed = false;

            // ���� ��Ȱ��ȭ
            // Destroy(heldSeed);
            heldSeed.SetActive(false);
            Debug.Log("���� ��������");

            //seed.gameObject.SetActive(false); //���� ��Ȱ��ȭ
        }
    }

    public void ToggleShoveler()
    {
        holdingShoveler = !holdingShoveler;

        if (holdingShoveler)
        {

            // ���̸� �÷��̾��� �� ��ġ�� �̵���Ŵ
            heldShovel = Instantiate(ShovelPrefab, hand.position, Quaternion.identity);
            heldShovel.transform.parent = hand; // ���̸� �տ� �θ�� ����
            // ������ �����̼��� �����Ͽ� ���ϴ� �������� ȸ����Ŵ
            heldShovel.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            holdingShoveler = true;
            Debug.Log("���� ����");

            //plant.gameObject.SetActive(true); //���� Ȱ��ȭ

            //// ���̸� �÷��̾��� �� ��ġ�� �̵���Ŵ
            //ShovelPrefab.transform.position = hand.position;
            //ShovelPrefab.transform.parent = hand; // ���̸� �տ� �θ�� ����
            //holdingShoveler = true;
            //Debug.Log("���� ����");

            //plant.gameObject.SetActive(true); //���� Ȱ��ȭ
        }
        else
        {
            // ���̸� �÷��̾� �տ� �� �ֵ��� ��ġ ����
            Destroy(heldShovel); // ���� ������Ʈ�� ����
            holdingShoveler = false;
            Debug.Log("���� ����");

           /// plant.gameObject.SetActive(false); //���� ��Ȱ��ȭ

            //// ���̸� �÷��̾� �տ� �� �ֵ��� ��ġ ����
            //ShovelPrefab.transform.position = transform.position + transform.forward * 2f; // ���÷� ĳ���� ������ 2m �̵�
            //ShovelPrefab.transform.parent = null; // ������ �θ� �����Ͽ� ���� ��Ʈ�� ��ġ
            //holdingShoveler = false;
            //Debug.Log("���� ����");

            //plant.gameObject.SetActive(false); //���� ��Ȱ��ȭ
        }
        // �̺�Ʈ �߻�
        OnShovelerToggled?.Invoke(holdingShoveler);
    }

    public void ToggleSprayer()
    {
        holdingSprayer = !holdingSprayer; // �й��⸦ ��ų� ��������

        if (holdingSprayer)
        {

            // ���̸� �÷��̾��� �� ��ġ�� �̵���Ŵ
            heldSprayer = Instantiate(SprayerPrefab, hand.position, Quaternion.identity);
            heldSprayer.transform.parent = hand; // ���̸� �տ� �θ�� ����
            // ������ �����̼��� �����Ͽ� ���ϴ� �������� ȸ����Ŵ
            heldSprayer.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            holdingSprayer = true;
            Debug.Log("�й��� ����");
            //// �й��⸦ �÷��̾��� �� ��ġ�� �̵���Ŵ
            //SprayerPrefab.transform.position = hand.position;
            //SprayerPrefab.transform.parent = hand; // �й��⸦ �տ� �θ�� ����
            //holdingSprayer = true;
            //Debug.Log("�й��� ����");

           // water.gameObject.SetActive(true); //���Ѹ��� Ȱ��ȭ
        }

        else
        {
            // �й��⸦ �÷��̾� �տ� �� �ֵ��� ��ġ ����
            Destroy(heldSprayer); // ���� ������Ʈ�� ����
            holdingSprayer = false;
            Debug.Log("�й��� ����");
            //water.gameObject.SetActive(false); //���Ѹ��� ��Ȱ��ȭ
        }
        // �̺�Ʈ �߻�
        OnSprayerToggled?.Invoke(holdingSprayer);
    }

    private void Move()
    {
        // Input.GetAxis�� ���� ���� �̵� �Է°��� ������.
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // moveInput�� 0�̸� �̵��� �߻����� �ʴ� ���̵� 0�� �ƴϸ� �̵��� �߻��ϴ� ��.
        bool isMove = moveInput.magnitude != 0;

        // ismove�� �߻��ϸ� �ȴ� �ִϸ��̼� �۵� ismove�� �߻����� �ʴ� �ٸ� ��� �ִϸ��̼� �۵�
        //animator.SetBool("isWalk", isMove);

        if (isMove)
        {
            // ���ȭ ���Ѽ� ����
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            // �ٶ󺸰� �ִ� �������� �̵�
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            // ĳ���Ͱ� �̵��� �� ī�޶� �ٶ󺸴� �������� �ٶ󺸰� ��.
            characterBody.forward = lookForward;
            // ĳ���� �̵�
            transform.position += moveDir * Time.deltaTime * 5f;

            animator.SetBool("isWalk", true);
            animator.SetBool("isRun", false);

            // ĳ���� �޸���
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

            // �÷��̾ ������ ������ �й����� ��ġ�� ����
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

                // �÷��̾ ������ ������ ������ ��ġ�� ����
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

        // �ִϸ��̼��� ���� �� isAnimating�� false�� ����
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Action") && stateInfo.normalizedTime >= 1.0f) 
        {
            animator.SetBool("isAction", false);
            isAnimating = false; // �ִϸ��̼� ���¸� false�� ����
        }
    }

    IEnumerator PlayActionAnimation(string animationName, Image actionImage)
    {
        isAnimating = true;
        animator.SetBool("isAction", true); // �ִϸ��̼� ������ �˸��� ���� ����
        actionImage.gameObject.SetActive(true); // �׼� �̹��� Ȱ��ȭ

        // �ִϸ��̼��� ���� ������ ���
        yield return new WaitForSeconds(1f);

        // �ִϸ��̼� ���� ó��
        actionImage.gameObject.SetActive(false); // �׼� �̹��� ��Ȱ��ȭ
        animator.SetBool("isAction", false); // �ִϸ��̼� ���� ���� ����
        isAnimating = false;

        // Idle ���·� ���ư���
        animator.Play("Idle");
    }

    // ���콺 �����ӿ� ���� ī�޶� ȸ��.
    private void LookAround()
    {
        // ���� ��ġ���� �󸶳� ���������� �˰� ���ְ� mouseDelta ������ ����
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        // ī�޶� ���� ȸ�� ���� ���Ϸ� ������ ��ȯ.
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        // ī�޶� ������ ���� ���� ����.
        // ī�޶� ���� ������ ����.
        // x�� 180�� ���� ���� ���� ���� ȸ�� �ϴ� ���.
        // 0���� �ƴ� -1���� ������ ���� �ϴ� ������ 0���� �ϸ� ī�޶� ����� ���Ϸ� �������� �ʴ� ������ �߻�.
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }

        else
        {
            // x���� 180���� ū ���� �Ʒ��� ȸ�� �ϴ� ���.
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);

    }
}