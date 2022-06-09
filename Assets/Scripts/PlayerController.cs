using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInventory))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed, gravityModifier, jumpPower, runSpeed = 12f;
    public CharacterController charCon;
    private Vector3 moveInput;
    public Transform cameraTransform;

    public float mouseSensitivity;
    public bool invertX;
    public bool invertY;

    public bool canJump, canDoubleJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    public Animator anim;

    public Transform firePoint;

    public Gun activeGun;
    public List<Gun> allGuns = new List<Gun>();
    public List<Gun> unlockableGuns = new List<Gun>();

    public int currentGun;

    private PlayerInventory _playerInventory;

    public GameObject muzzleFlash;

    void Start()
    {
        currentGun--;
        SwitchGun();
    }


    private void Awake()
    {
        instance = this;
        _playerInventory = GetComponent<PlayerInventory>();
    }

    private bool _canAutoJump = false;

    void Update()
    {
        if (!UIController.instance.pauseScreen.activeInHierarchy)

        {

            if (GameManager.instance.GetGameState() != GameState.WalkMode)
                return;

            //MOVING
            float yStore = moveInput.y;

            Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
            Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");

            moveInput = horiMove + vertMove;
            moveInput.Normalize();

            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveInput = moveInput * runSpeed;
            }
            else
            {
                moveInput = moveInput * moveSpeed;
            }

            moveInput.y = yStore;

            moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

            _canAutoJump = false;
            if (charCon.isGrounded)
            {
                moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
                CheckForAutoJump();
            }

            //check for the ground        
            canJump = charCon.velocity.y <= 0 && Physics.OverlapSphere(groundCheckPoint.position, 0.35f, whatIsGround).Length > 0;

            //Handle Jumping
            if (Input.GetKeyDown(KeyCode.Space) || _canAutoJump)
            {
                if (canJump)
                {
                    moveInput.y = jumpPower * (_canAutoJump ? 2f : 1f);
                    canDoubleJump = true;
                }
                else if (canDoubleJump)
                {
                    moveInput.y = jumpPower;
                    canDoubleJump = false;
                }

            }

            charCon.Move(moveInput * Time.deltaTime);


            //CAMERA ROTATION
            Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

            if (invertX)
            {
                mouseInput.x = -mouseInput.x;
            }

            if (invertY)
            {
                mouseInput.y = -mouseInput.y;
            }

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
            cameraTransform.rotation = Quaternion.Euler(cameraTransform.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));


            //SHOOTING
            //single shots
            if (Input.GetMouseButtonDown(0) && activeGun.fireCounter <= 0 && activeGun.gameObject.activeSelf)
            {
                RaycastHit hit;
                if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 50f))
                {
                    if (Vector3.Distance(cameraTransform.position, hit.point) > 2f)
                    {
                        firePoint.LookAt(hit.point);
                    }
                }

                else
                {
                    firePoint.LookAt(cameraTransform.position + (cameraTransform.forward * 30f));
                }

       
                FireShot();
            }

            //repeating shots
            if (Input.GetMouseButton(0) && activeGun.canAutoFire)
            {
                if (activeGun.fireCounter <= 0 && activeGun.gameObject.activeSelf)
                {
                    FireShot();
                }


            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwitchGun();
            }


            anim.SetFloat("moveSpeed", moveInput.magnitude);
            anim.SetBool("onGround", canJump);

        }
    }

    public void FireShot()

    {
        //if (activeGun.currentAmmo > 0)
        //{
            //activeGun.currentAmmo--;
            Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);
            activeGun.fireCounter = activeGun.fireRate;

            //UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;
        //}

    }

    public void SwitchGun()
    {
        activeGun.gameObject.SetActive(false);
        GameManager.instance.SetActiveSwitchGunsTip(false);

        if (allGuns.Count == 0)
            return;

        currentGun++;

        if (currentGun >= allGuns.Count)
        {
            currentGun = 0;
        }

        activeGun = allGuns[currentGun];
        activeGun.gameObject.SetActive(true);

        //UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

        firePoint.position = activeGun.firepoint.position;
    }

    public void AddGun(string gunToAdd)

    {
        bool gunUnlocked = false;

        if (unlockableGuns.Count > 0)
        {
            for (int i = 0; i < unlockableGuns.Count; i++)
            {
                if (unlockableGuns[i].gunName == gunToAdd)
                {
                    gunUnlocked = true;

                    allGuns.Add(unlockableGuns[i]);

                    unlockableGuns.RemoveAt(i);

                    i = unlockableGuns.Count;
                }
            }

        }
        if (gunUnlocked)
        {
            currentGun = allGuns.Count - 1;
            SwitchGun();
        }

        GameManager.instance.ShowGunCrosshair();
    }

    
    public void RespawnPlayer(Transform checkpoint)
    {
        transform.position = checkpoint.position;
        gameObject.SetActive(true);
        GetComponent<PlayerHealthController>().SetInitialHealth();
    }

public void PinToObject(Transform obj)
    {
        transform.SetParent(obj);
        if (obj == null)
            charCon.enabled = true;
        else
            charCon.enabled = false;
    }

    private void CheckForAutoJump()
    {
        Collider[] hitColliders = Physics.OverlapSphere(groundCheckPoint.position, 0.35f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("B"))
            {
                _canAutoJump = true;
                return;
            }
        }
    }

    public void CollectLetter(char let)
    {
        _playerInventory.AddLetter(let);
    }

    public void UseLetter(char letter)
    {
        _playerInventory.UseLetter(letter);
    }
}
