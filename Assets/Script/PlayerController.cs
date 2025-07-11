using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float rayDistance = 10f;
    public float doubleTapTime = 0.3f;

    public float maxHorizontalAngle = 70f; // ���E �}70�x�i�v140�x�j
    public float maxVerticalAngle = 90f;   // �㉺ �}90�x

    private Rigidbody rb;
    private bool isGrounded = false;
    private float lastSpaceTime = -1f;
    private Vector3 currentGravity = Vector3.down;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = currentGravity * 9.81f;
        Cursor.lockState = CursorLockMode.Locked;  // �v���C���̓}�E�X���b�N
    }

    void Update()
    {
        // ���݂̏d�͂Ɋ�Â��Ēn�ʂƐ������v�Z
        Vector3 gravityDir = Physics.gravity.normalized;
        Vector3 right = Vector3.Cross(gravityDir, Vector3.forward).normalized;
        if (right == Vector3.zero)
            right = Vector3.Cross(gravityDir, Vector3.right).normalized;
        Vector3 forward = Vector3.Cross(right, gravityDir).normalized;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = (right * h + forward * v) * moveSpeed;
        rb.velocity = move + Vector3.Project(rb.velocity, gravityDir);

        // �W�����v�^���[�v
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            float now = Time.time;

            if (now - lastSpaceTime < doubleTapTime)
            {
                TryWarpForward();
            }
            else
            {
                rb.AddForce(-gravityDir * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }

            lastSpaceTime = now;
        }

        // Shift�ŏd�͂�؂�ւ��i����p�`�F�b�N���j
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TryChangeGravityWithViewLimit();
        }

        // �f�o�b�O�\���F�V�[���r���[��Ray
        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.green);
    }

    void TryWarpForward()
    {
        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        if (forward.magnitude < 0.1f) return;

        if (Physics.Raycast(transform.position, forward, out RaycastHit hit, rayDistance))
        {
            if (IsGroundTag(hit.collider.tag))
            {
                Vector3 warpPosition = hit.point + -Physics.gravity.normalized;
                transform.position = warpPosition;
                rb.velocity = Vector3.zero;
            }
        }
    }

    void TryChangeGravityWithViewLimit()
    {
        Vector3 forward = transform.forward;

        if (Physics.Raycast(transform.position, forward, out RaycastHit hit, rayDistance))
        {
            string tag = hit.collider.tag;
            if (tag == "X" || tag == "Y" || tag == "Z")
            {
                // �p�x�����`�F�b�N
                Vector3 toTarget = (hit.point - transform.position).normalized;

                float horizontalAngle = Vector3.Angle(Vector3.ProjectOnPlane(transform.forward, Vector3.up), Vector3.ProjectOnPlane(toTarget, Vector3.up));
                float verticalAngle = Vector3.Angle(transform.forward, toTarget);

                if (horizontalAngle <= maxHorizontalAngle && verticalAngle <= maxVerticalAngle)
                {
                    Vector3 newUp = -hit.normal;
                    Physics.gravity = -newUp * 9.81f;
                    currentGravity = -newUp;

                    // �v���C���[�̉�]�����킹��
                    Quaternion targetRotation = Quaternion.FromToRotation(transform.up, newUp) * transform.rotation;
                    transform.rotation = targetRotation;

                    Debug.Log("�d�͕ύX�F���� = " + Physics.gravity + ", �^�O = " + tag);
                }
                else
                {
                    Debug.Log("�������p�x�����O�i�����F" + horizontalAngle + "��, �����F" + verticalAngle + "���j");
                }
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (IsGroundTag(contact.otherCollider.tag))
            {
                if (Vector3.Angle(contact.normal, -Physics.gravity.normalized) < 10f)
                {
                    isGrounded = true;
                    break;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsGroundTag(collision.gameObject.tag))
        {
            isGrounded = false;
        }
    }

    private bool IsGroundTag(string tag)
    {
        return tag == "Ground" || tag == "X" || tag == "Y" || tag == "Z";
    }
}
