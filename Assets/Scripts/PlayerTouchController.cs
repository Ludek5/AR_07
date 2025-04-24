using UnityEngine;

public class PlayerTouchController : MonoBehaviour
{
    public FixedJoystick joystick;
    public float moveDistance = 0.2f;
    private float threshold = 0.7f;
    private Vector3 lastMove = Vector3.zero;

    void Start()
    {
        if (joystick == null)
        {
            joystick = GameObject.FindObjectOfType<FixedJoystick>();
            if (joystick == null)
                Debug.LogError("No se encontró un FixedJoystick en la escena.");
        }
    }

    void Update()
    {
        if (joystick == null) return; // Evitar errores si aún no se asignó

        Vector2 input = joystick.Direction;
        Vector3 dir = Vector3.zero;

        if (input.y > threshold) dir = Vector3.forward;
        else if (input.y < -threshold) dir = Vector3.back;
        else if (input.x < -threshold) dir = Vector3.left;
        else if (input.x > threshold) dir = Vector3.right;
        else lastMove = Vector3.zero;

        if (dir != Vector3.zero && dir != lastMove)
        {
            GetComponent<PlayerController>().TryMove(dir);
            lastMove = dir;
            Invoke("ResetMove", 0.25f);
        }
    }

    void ResetMove()
    {
        lastMove = Vector3.zero;
    }
}