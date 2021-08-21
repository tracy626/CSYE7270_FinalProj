using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ThirdPersonController : MonoBehaviour
{
    public Camera MyCamera;
    public float Speed = 2f;
    public float SprintSpeed = 5f;
    public float RotationSpeed = 10f;
    public float AnimationBlendSpeed = 2f;
    public float JumpSpeed = 6f;

    public GameObject Firework;
    public GameObject WinText;
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
    public GameObject GameEnd;

    public Text TimeCounter;

    // public float JumpFromHeight = 0f;

    private CharacterController MyController;
    private Animator MyAnimator;

    private float mDesiredRotation = 0f;
    private float mDisiredAnimationSpeed = 0f;
    private bool mSpriting;
    private float mSpeedY = 0f;
    private float mGravity = -9.81f;
    private bool mJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        MyController = GetComponent<CharacterController>();
        MyAnimator = GetComponent<Animator>();

        Firework.SetActive(false);  
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") && !mJumping)
        {
            mJumping = true;
            MyAnimator.SetTrigger("Jump");

            // JumpFromHeight = this.transform.position.y;
            mSpeedY += JumpSpeed;
        }

        if (!MyController.isGrounded)
        {
            mSpeedY += mGravity * Time.deltaTime;
        } else if (mSpeedY < 0)
        {
            mSpeedY = 0;
        }

        MyAnimator.SetFloat("SpeedY", mSpeedY / JumpSpeed);

        if (mJumping && mSpeedY < 0)
        {
            // RaycastHit hit;
            // if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f, LayerMask.GetMask("Default")))
            // {
                mJumping = false;
                MyAnimator.SetTrigger("Land");
            // }
        }

        mSpriting = Input.GetKey(KeyCode.LeftShift);
        Vector3 movement = new Vector3(x, 0, z).normalized;

        Vector3 rotatedMovement = Quaternion.Euler(0, MyCamera.transform.rotation.eulerAngles.y, 0) * movement;
        Vector3 verticalMovement = Vector3.up * mSpeedY;

        MyController.Move((verticalMovement + (rotatedMovement * (mSpriting ? SprintSpeed : Speed))) * Time.deltaTime);

        if(rotatedMovement.magnitude > 0) {
            mDesiredRotation = Mathf.Atan2(rotatedMovement.x, rotatedMovement.z) * Mathf.Rad2Deg;
            mDisiredAnimationSpeed = (mSpriting ? 1f : 0.5f);
        } else {
            mDisiredAnimationSpeed = 0;
        }
        
        MyAnimator.SetFloat("MoveSpeed", Mathf.Lerp(MyAnimator.GetFloat("MoveSpeed"), mDisiredAnimationSpeed, AnimationBlendSpeed * Time.deltaTime));

        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, mDesiredRotation, 0);
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, RotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Teleporter")
        {
            Firework.SetActive(true);

            Timer.instance.EndTimer();
            StartCoroutine(CalculateScore());

            if (SceneManager.GetActiveScene().name != "Rooftop")
            {
                StartCoroutine(TransferScence());
            } else
            {
                StartCoroutine(GameEndShowUp());
            }
        }
    }

    IEnumerator CalculateScore()
    {
        if (TimeCounter.text.CompareTo("01:30.00") < 0)
        {
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(true);
        } else if (TimeCounter.text.CompareTo("03:00.00") < 0)
        {
            star1.SetActive(true);
            star2.SetActive(true);
        } else
        {
            star1.SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        WinText.SetActive(true);
    }

    IEnumerator TransferScence()
    {
        yield return new WaitForSeconds(6f);

        SceneManager.LoadScene("Rooftop");
    }

    IEnumerator GameEndShowUp()
    {
        yield return new WaitForSeconds(5f);

        GameEnd.SetActive(true);
    }
}
