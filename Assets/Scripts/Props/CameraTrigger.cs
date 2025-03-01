using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraTrigger : MonoBehaviour
{
    public float radius = 1f;
    [Range(0, 1)]
    public float angThreshold = 0.5f;

    private Transform laserGun;
    private Transform target;
    private new Light2D light;
    private Animator anim;
    private Quaternion defualtRotation;
    private bool isOut;
    private SecurityCamera SecurityCamera;
    bool isDetected;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        laserGun = GetComponentInParent<Light2D>().transform;
        light = GetComponentInParent<Light2D>();
        anim = GetComponentInParent<Animator>();
        defualtRotation = laserGun.rotation;
        SecurityCamera = GetComponentInParent<SecurityCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDetected)
        {
            AimAtTarget();
            SecurityCamera.Shoot();
        }
        if (isOut)
        {
            laserGun.rotation = Quaternion.Slerp(laserGun.rotation, defualtRotation, 2f * Time.deltaTime);
            if (Quaternion.Angle(laserGun.rotation, defualtRotation) <= 1f) 
            {
                laserGun.rotation = defualtRotation;
                isOut = false;
                anim.SetTrigger("Exited");
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("IsInside", true);
            isDetected = true;
            isOut = false;
            light.color = Color.red;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("IsInside", false);
            //anim.SetBool("IsInside", false);
            //anim.SetTrigger("Exited");
            light.color = Color.white;
            isDetected = false;
            //laserGun.rotation = defualtRotation;
            isOut = true;
        }
    }
    private IEnumerator ResetRotation() 
    {
        
        yield return new WaitForSeconds(5f);
    }
    public void AimAtTarget()
    {
        Vector2 direction = target.position - transform.position;
        direction.Normalize();
        // Rotate the gun to point at the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        laserGun.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
        SecurityCamera.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
    }
}
