using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    

    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _fallYPanTime = 0.35f;
    public float _fallSpeedYDampingChangeThreshold = -15f;

    public bool IsLerpingYDamping {  get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }

    private Coroutine _lerpYPanCoroutine;

    public CinemachinePositionComposer cinPosCom;

    private float _normPanValue;
    
    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
        _normPanValue = cinPosCom.Damping.y;
    }
    public void LerpYDamping(bool isPlayerFalling)
    {
        _lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }
    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        float startDampAmount = cinPosCom.Damping.y;
        float endDampAmount = 0f;

        if (isPlayerFalling)
        {
            endDampAmount = _fallPanAmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = _normPanValue;
        }

        float elapsedTime = 0f;
        while (elapsedTime < _fallYPanTime) 
        {
            elapsedTime += Time.deltaTime;
            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / _fallYPanTime));
            cinPosCom.Damping = new Vector3(cinPosCom.Damping.x, lerpedPanAmount, cinPosCom.Damping.z);
            yield return null;
        }
        IsLerpingYDamping = false;
    }
}
