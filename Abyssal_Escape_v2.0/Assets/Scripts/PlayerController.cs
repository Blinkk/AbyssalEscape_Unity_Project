using UnityEngine;
using System.Collections;

[RequireComponent(typeof (CharacterController))]
public class PlayerController : MonoBehaviour 
{
    // Components
	public Transform handHold;
    private CharacterController controller;
	public GunController[] guns;
    private GunController currentGun;
	private Animator animator;
    private GameGUI gui;
    private AnimatorTransitionInfo armsTransitionInfo;

    // System
    private Quaternion targetRotation;
    private Camera camera;
    private Vector3 currentVelMod;
    private bool reloading;
    private WaveControl wc;

    // Handling
    public float rotationSpeed = 450;
    public float walkSpeed = 5.0f;
    public float runSpeed = 8.0f;
    private float acceleration = 5.0f;

    // Gun information variables
    private float ar_damage = -1;
    private float shotgun_damage = -1;
    private float pistol_damage = -1;
    private int ar_clip = -1;
    private int shotgun_clip = -1;
    private int pistol_clip = -1;
    private int ar_ammo = -1;
    private int shotgun_ammo = -1;
    private int pistol_ammo = -1;
    

	// Use this for initialization
	void Start () 
    {
		animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        camera = Camera.main;
        gui = GameObject.FindGameObjectWithTag("GUI").gameObject.GetComponent<GameGUI>();
        wc = camera.GetComponent<WaveControl>();

        // Set guns to default values
        GetGunByID(1).SetDamage(1.0f);
        GetGunByID(1).SetAmmoPerMag(32);
        GetGunByID(1).SetCurrentMagAmmo(32);
        GetGunByID(2).SetDamage(3.0f);
        GetGunByID(2).SetAmmoPerMag(2);
        GetGunByID(2).SetCurrentMagAmmo(2);
        GetGunByID(3).SetDamage(2.0f);
        GetGunByID(3).SetAmmoPerMag(10);
        GetGunByID(3).SetCurrentMagAmmo(10);

        // Equip default gun (AR)
		EquipGun(0);
        currentGun.SetCurrentMagAmmo(32);   
	}
	
	// Update is called once per frame
	void Update () 
    {
        // If new wave GUI is showing, don't update player
        if (!wc.IsNewWave())
        {
            ControlMouse();
            //ControlWASD();

            // Get animation info
            armsTransitionInfo = GetComponent<Animator>().GetAnimatorTransitionInfo(1);

            if (currentGun)
            {
                if (Input.GetButtonDown("Shoot"))
                    currentGun.Shoot();
                else if (Input.GetButton("Shoot"))
                    currentGun.ShootAuto();

                if (Input.GetButtonDown("Reload"))
                {
                    if (currentGun.Reload())
                    {
                        GetComponent<Animator>().SetTrigger("Reload");
                        reloading = true;
                    }
                }

                // Finish reloading
                if (reloading)
                {
                    if (armsTransitionInfo.nameHash == Animator.StringToHash("Reload -> Weapon Hold"))
                    {
                        currentGun.FinishReload();
                        reloading = false;
                    }
                }
            }

            for (int i = 0; i < guns.Length; i++)
            {
                if (Input.GetKeyDown((i + 1) + "") || Input.GetKeyDown("[" + (i + 1) + "]"))
                {
                    EquipGun(i);
                    break;
                }
            }
        }
    }

	void EquipGun(int index)
	{
        
		if (currentGun)
        {
            // Store the current gun's information
            if (currentGun.gunID == 1)
            {
                ar_ammo = currentGun.GetTotalAmmo();
                ar_clip = currentGun.GetCurrentMagAmmo();
                ar_damage = currentGun.GetDamage();
            }
            else if (currentGun.gunID == 2)
            {
                shotgun_ammo = currentGun.GetTotalAmmo();
                shotgun_clip = currentGun.GetCurrentMagAmmo();
                shotgun_damage = currentGun.GetDamage();
            }
            else if (currentGun.gunID == 3)
            {
                pistol_ammo = currentGun.GetTotalAmmo();
                pistol_clip = currentGun.GetCurrentMagAmmo();
                pistol_damage = currentGun.GetDamage();
            }
            Destroy(currentGun.gameObject);     // Destroy the current gun
        }
			
        // Instatiate a new gun
		currentGun = Instantiate (guns[index], handHold.position, handHold.rotation) as GunController;
		currentGun.transform.parent = handHold;
        currentGun.gui = gui;
		animator.SetFloat ("Weapon ID", currentGun.gunID);

        GetStoredGunInfo();     // Check if gun has stored info
	}

    public void GetStoredGunInfo()
    {
        // Check if new gun has stored data
        if (currentGun.gunID == 1)
        {
            if (ar_ammo >= 0 && ar_ammo <= 128) { currentGun.SetTotalAmmo(ar_ammo); }
            if (ar_clip >= 0 && ar_clip <= currentGun.GetAmmoPerMag()) currentGun.SetCurrentMagAmmo(ar_clip);
            if (ar_damage >= 1 && ar_damage <= 6) currentGun.SetDamage(ar_damage);

            if (ar_clip < 0) currentGun.SetCurrentMagAmmo(currentGun.GetAmmoPerMag());
        }
        if (currentGun.gunID == 2)
        {
            if (shotgun_ammo >= 0 && shotgun_ammo <= 20) currentGun.SetTotalAmmo(shotgun_ammo);
            if (shotgun_clip >= 0 && shotgun_clip <= currentGun.GetAmmoPerMag()) currentGun.SetCurrentMagAmmo(shotgun_clip);
            if (shotgun_damage >= 1 && shotgun_damage <= 15) currentGun.SetDamage(shotgun_damage);

            if (shotgun_clip < 0) currentGun.SetCurrentMagAmmo(currentGun.GetAmmoPerMag());
        }
        if (currentGun.gunID == 3)
        {
            if (pistol_ammo >= 0 && pistol_ammo <= 40) currentGun.SetTotalAmmo(pistol_ammo);
            if (pistol_clip >= 0 && pistol_clip <= currentGun.GetAmmoPerMag()) currentGun.SetCurrentMagAmmo(pistol_clip);
            if (pistol_damage >= 1 && pistol_damage <= 12) currentGun.SetDamage(pistol_damage);

            if (pistol_clip < 0) currentGun.SetCurrentMagAmmo(currentGun.GetAmmoPerMag());
        }
    }

    void ControlMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camera.transform.position.y - transform.position.y));

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        // Rotate player
        targetRotation = Quaternion.LookRotation(mousePos - new Vector3(transform.position.x, 0, transform.position.z));
        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y,
            targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);

        // Movement
        currentVelMod = Vector3.MoveTowards(currentVelMod, input, acceleration * Time.deltaTime);
        Vector3 motion = currentVelMod;
        motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? 0.7f : 1;
        motion *= (Input.GetButton("Run")) ? runSpeed : walkSpeed;
        motion += Vector3.up * -8;

        controller.Move(motion * Time.deltaTime);

		// Set up animation speed
		animator.SetFloat ("speed", Mathf.Sqrt (motion.x * motion.x + motion.z * motion.z));
    }
        

    void ControlWASD()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (input != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(input);
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y,
                targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
        }

        // Movement
        currentVelMod = Vector3.MoveTowards(currentVelMod, input, acceleration * Time.deltaTime);
        Vector3 motion = currentVelMod;
        motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? 0.7f : 1;
        motion *= (Input.GetButton("Run")) ? runSpeed : walkSpeed;
        motion += Vector3.up * -8;

        controller.Move(motion * Time.deltaTime);

		// Set up animation speed
		animator.SetFloat ("speed", Mathf.Sqrt (motion.x * motion.x + motion.z * motion.z));
    }

    public float GetGunID()
    {
        return currentGun.gunID;
    }

    public GunController GetGunByID(int id)
    {
        return guns[id - 1];
    }
}
