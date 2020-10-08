using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{

    //Animator component attached to weapon
    Animator anim;

    [Header("Gun Camera")]
    //Main gun camera
    public Camera gunCamera;

    [Header("Gun Camera Options")]
    //How fast the camera field of view changes when aiming 
    [Tooltip("How fast the camera field of view changes when aiming.")]
    public float fovSpeed = 15.0f;
    //Default camera field of view
    [Tooltip("Default value for camera field of view (40 is recommended).")]
    public float defaultFov = 40.0f;

    public float aimFov = 25.0f;

    [Header("Weapon Sway")]
    //Enables weapon sway
    [Tooltip("Toggle weapon sway.")]
    public bool weaponSway;

    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmoothValue = 4.0f;

    private Vector3 initialSwayPosition;

    //Used for fire rate
    private float lastFired;
    [Header("Weapon Settings")]
    //How fast the weapon fires, higher value means faster rate of fire
    [Tooltip("How fast the weapon fires, higher value means faster rate of fire.")]
    public float fireRate;

    //Check if walking
    private bool isWalking;

    [Header("Bullet Settings")]
    //Bullet
    [Tooltip("How much force is applied to the bullet when shooting.")]
    public float bulletForce = 100;
    [Tooltip("How long after reloading that the bullet model becomes visible " +
	    "again, only used for out of ammo reload animations.")]
    public float showBulletInMagDelay = 0.6f;
    [Tooltip("The bullet model inside the mag, not used for all weapons.")]
    public SkinnedMeshRenderer bulletInMagRenderer;

    [Header("Muzzleflash Settings")]
    public bool randomMuzzleflash = false;
    //min should always bee 1
    private int minRandomValue = 1;

    [Range(2, 25)]
    public int maxRandomValue = 5;

    private int randomMuzzleflashValue;

    public bool enableMuzzleflash = true;
    public ParticleSystem muzzleParticles;
    public bool enableSparks = true;
    public ParticleSystem sparkParticles;
    public int minSparkEmission = 1;
    public int maxSparkEmission = 7;

    [Header("Muzzleflash Light Settings")]
    public Light muzzleflashLight;
    public float lightDuration = 0.02f;

    [Header("Audio Source")]
    //Main audio source
    public AudioSource mainAudioSource;
    //Audio source used for shoot sound
    public AudioSource shootAudioSource;

    [System.Serializable]
    public class prefabs
    {
	   [Header("Prefabs")]
	   public Transform bulletPrefab;
	   public Transform casingPrefab;
    }
    public prefabs Prefabs;

    [System.Serializable]
    public class spawnpoints
    {
	   [Header("Spawnpoints")]
	   //Array holding casing spawn points 
	   //(some weapons use more than one casing spawn)
	   //Casing spawn point array
	   public Transform casingSpawnPoint;
	   //Bullet prefab spawn from this point
	   public Transform bulletSpawnPoint;
    }
    public spawnpoints Spawnpoints;

    public AudioClip shootSound;

    private void Awake()
    {
	   //Set the animator component
	   anim = GetComponent<Animator>();

	   muzzleflashLight.enabled = false;
    }

    private void Start()
    {
	   // Weapon sway
	   initialSwayPosition = transform.localPosition;

	   // Set the shoot sound to audio source
	   shootAudioSource.clip = shootSound;
    }

    private void LateUpdate()
    {
	   //Weapon sway
	   if (weaponSway == true)
	   {
		  float movementX = -Input.GetAxis("Mouse X") * swayAmount;
		  float movementY = -Input.GetAxis("Mouse Y") * swayAmount;

		  //Clamp movement to min and max values
		  movementX = Mathf.Clamp
			  (movementX, -maxSwayAmount, maxSwayAmount);
		  movementY = Mathf.Clamp
			  (movementY, -maxSwayAmount, maxSwayAmount);

		  //Lerp local pos
		  Vector3 finalSwayPosition = new Vector3
			  (movementX, movementY, 0);
		  transform.localPosition = Vector3.Lerp
			  (transform.localPosition, finalSwayPosition +
				  initialSwayPosition, Time.deltaTime * swaySmoothValue);
	   }
    }

    private void Update()
    {
	   //If randomize muzzleflash is true, genereate random int values
	   if (randomMuzzleflash == true)
	   {
		  randomMuzzleflashValue = Random.Range(minRandomValue, maxRandomValue);
	   }

	   //Continosuly check which animation is currently playing
	   //AnimationCheck();

	   // Automatic fire
	   if (Input.GetMouseButton(0))
	   {
		  //Shoot automatic
		  if (Time.time - lastFired > 1 / fireRate)
		  {
			 lastFired = Time.time;
			 shootAudioSource.clip = shootSound;
			 shootAudioSource.Play();

			 // Spawn bullet from bullet spawnpoint
			 var bullet = (Transform)Instantiate(
				 Prefabs.bulletPrefab,
				 Spawnpoints.bulletSpawnPoint.transform.position,
				 Spawnpoints.bulletSpawnPoint.transform.rotation);

			 Debug.Log("bullet position: " + Spawnpoints.bulletSpawnPoint.transform.position);

			 // Add velocity to the bullet
			 bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletForce;

			 // Spawn casing prefab at spawnpoint
			 Instantiate(Prefabs.casingPrefab,
				 Spawnpoints.casingSpawnPoint.transform.position,
				 Spawnpoints.casingSpawnPoint.transform.rotation);
		  }
	   }
    }

    //Enable bullet in mag renderer after set amount of time
    private IEnumerator ShowBulletInMag()
    {

	   //Wait set amount of time before showing bullet in mag
	   yield return new WaitForSeconds(showBulletInMagDelay);
	   bulletInMagRenderer.GetComponent<SkinnedMeshRenderer>().enabled = true;
    }

    //Show light when shooting, then disable after set amount of time
    private IEnumerator MuzzleFlashLight()
    {

	   muzzleflashLight.enabled = true;
	   yield return new WaitForSeconds(lightDuration);
	   muzzleflashLight.enabled = false;
    }
}
