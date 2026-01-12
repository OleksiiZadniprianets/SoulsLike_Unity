using UnityEngine;

public class PlayerRotateWithMouseDirect : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 3f;

    private float currentYRotation = 0f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");

        currentYRotation += mouseX * mouseSensitivity;

        transform.rotation = Quaternion.Euler(0f, currentYRotation, 0f);
    }
}