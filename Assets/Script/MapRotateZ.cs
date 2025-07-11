using UnityEngine;

public class MapRotateZ : MonoBehaviour
{
    public float angle = 90f;

    void Update()
    {
        if (gameObject.tag != "Z") return;

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            transform.Rotate(0, angle, 0, Space.World);  // YŽ²‰ñ“]
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            transform.Rotate(0, -angle, 0, Space.World); // YŽ²‰ñ“]
        }
    }
}
