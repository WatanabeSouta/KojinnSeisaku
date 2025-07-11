using UnityEngine;

public class RotateX : MonoBehaviour
{
    public float angle = 90f;

    void Update()
    {
        if (gameObject.tag != "X") return;

        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            transform.Rotate(0, angle, 0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            transform.Rotate(0, -angle, 0);
        }
    }
}
