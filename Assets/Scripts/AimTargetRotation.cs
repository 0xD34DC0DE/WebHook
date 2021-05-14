using UnityEngine;

public class AimTargetRotation : MonoBehaviour
{
    void Update()
    {
        transform.Rotate (0,90 * Time.deltaTime,0);
    }
}
