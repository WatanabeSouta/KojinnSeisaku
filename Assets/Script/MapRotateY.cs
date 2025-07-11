using UnityEngine;

public class MapRotateY : MonoBehaviour
{
    public float angle = 90f;

    void Update()
    {
        if (gameObject.tag != "Y") return;

        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            transform.Rotate(angle, 0, 0, Space.World);  // X²‰ñ“]ic‰ñ“]j
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            transform.Rotate(-angle, 0, 0, Space.World); // X²‰ñ“]ic‰ñ“]j
        }
    }
}
