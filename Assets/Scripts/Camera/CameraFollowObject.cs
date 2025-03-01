using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    public float BiasSpeed = 0.25f;

    [SerializeField]    
    private Transform playerTransform;

    private bool isFacingRight;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        transform.position = playerTransform.position;
    }

    public void CallTurn()
    {
        LeanTween.rotateY(gameObject, DetermineEndRotation(), BiasSpeed).setEaseInOutSine();
    }

    private float DetermineEndRotation() 
    {
        isFacingRight = !isFacingRight;

        if (isFacingRight) 
        {
            return 180f;
        }
        else
        {
            return 0f;
        }
    }
}
