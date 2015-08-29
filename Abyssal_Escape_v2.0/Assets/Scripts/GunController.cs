using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class GunController : MonoBehaviour 
{
    public enum GunType { Semi, Burst, Auto };
    public GunType gunType;
    public float rpm;
	public float gunID;
    public LayerMask collisionMask;
    public float damage = 1;
    public int totalAmmo = 40;
    public int ammoPerMag = 10;

    // Components
    public Transform spawn;
    public Transform shellEjectPoint;
    public Rigidbody shell;
    private LineRenderer tracer;

    [HideInInspector]
    public GameGUI gui;

    // System
    private float secondsBetweenShots;
    private float nextShootTime;
    private int currentMagAmmo;
    private bool reloading;

    void Start()
    {
        secondsBetweenShots = 60 / rpm;

        if (GetComponent<LineRenderer>())
            tracer = GetComponent<LineRenderer>();

        if (gui)
            gui.SetAmmoInfo(totalAmmo, currentMagAmmo);
    }

    
	public void Shoot()
    {
        // If able to shoot, create a bolt
        if (CanShoot())
        {
            Ray ray = new Ray(spawn.position, spawn.forward);
            RaycastHit hit;
            float shotDistance = 20;

            // Check collision
            if (Physics.Raycast(ray, out hit, shotDistance, collisionMask))
            {
                shotDistance = hit.distance;

                if (hit.collider.GetComponent<Entity>())
                {
                    hit.collider.GetComponent<Entity>().TakeDamage(this.damage);
                    Debug.Log("Damage Dealt: " + damage);
                }
                    
            }

            Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red, 1);

            // Reset next shoot time
            nextShootTime = Time.time + secondsBetweenShots;
            currentMagAmmo--;

            // Set ammo gui
            if (gui)
                gui.SetAmmoInfo(totalAmmo, currentMagAmmo);

            // Play the sound
            GetComponent<AudioSource>().Play();

            // Draw tracer
            if (tracer)
                StartCoroutine("RenderTracer", ray.direction * shotDistance);


            // Shell
            Rigidbody newShell = Instantiate(shell, shellEjectPoint.position, GetComponent<Transform>().rotation) as Rigidbody;
            newShell.AddForce(shellEjectPoint.forward * Random.Range(150f, 200f) + spawn.forward * Random.Range(-10f, 10f));
        }
    }

    public void ShootAuto()
    {
        if (gunType == GunType.Auto)
            Shoot();
    }

    private bool CanShoot()
    {
        bool canShoot = true;

        if (Time.time < nextShootTime)
            canShoot = false;
        if (currentMagAmmo == 0)
            canShoot = false;
        if (reloading)
            canShoot = false;

        return canShoot;
    }

    public bool Reload()
    {
        if (totalAmmo != 0 && currentMagAmmo != ammoPerMag)
        {
            reloading = true;
            return true;
        }  
        else
            return false;
    }

    public void FinishReload()
    {
        reloading = false;
        currentMagAmmo = ammoPerMag;
        totalAmmo -= ammoPerMag;
        if (totalAmmo < 0)
        {
            currentMagAmmo += totalAmmo;
            totalAmmo = 0;
        }

        // Set GUI
        if (gui)
            gui.SetAmmoInfo(totalAmmo, currentMagAmmo);
    }

    public int GetCurrentMagAmmo()
    {
        return currentMagAmmo;
    }

    public void SetCurrentMagAmmo(int value)
    {
        currentMagAmmo = value;
    }


    IEnumerator RenderTracer(Vector3 endPoint)
    {
        tracer.enabled = true;
        tracer.SetPosition(0, spawn.position);              // Start position
        tracer.SetPosition(1, spawn.position + endPoint);   // End position

        yield return null;
        tracer.enabled = false;
    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float value)
    {
        damage = value;
    }

    public int GetAmmoPerMag()
    {
        return ammoPerMag;
    }

    public void SetAmmoPerMag(int value)
    {
        ammoPerMag = value;
    }

    public int GetTotalAmmo()
    {
        return totalAmmo;
    }

    public void SetTotalAmmo(int value)
    {
        totalAmmo = value;
    }

}
