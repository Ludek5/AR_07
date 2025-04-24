using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveDistance = 0.2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) TryMove(Vector3.forward);
        if (Input.GetKeyDown(KeyCode.S)) TryMove(Vector3.back);
        if (Input.GetKeyDown(KeyCode.A)) TryMove(Vector3.left);
        if (Input.GetKeyDown(KeyCode.D)) TryMove(Vector3.right);
    }

    public void TryMove(Vector3 dir)
    {
        Vector3 start = transform.position;
        Vector3 targetPos = start + dir * moveDistance;

        // Raycast al nivel del centro del objeto ignorando triggers (como Goal)
        if (Physics.Raycast(start, dir, out RaycastHit hit, moveDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            Debug.Log("Objeto detectado: " + hit.collider.name);

            if (hit.collider.CompareTag("Box"))
            {
                GameObject box = hit.collider.gameObject;
                Vector3 boxTarget = box.transform.position + dir * moveDistance;

                // Verificamos si hay espacio para empujar la caja
                bool espacioLibre = !Physics.CheckSphere(boxTarget, 0.05f, ~0, QueryTriggerInteraction.Ignore);

                if (espacioLibre)
                {
                    Debug.Log("Empujando caja hacia: " + boxTarget);
                    box.transform.position = boxTarget;
                    transform.position = targetPos;

                    // Llamamos a la validación de victoria
                    GameManager gm = FindObjectOfType<GameManager>();
                    if (gm != null) gm.CheckWinCondition();
                }
                else
                {
                    Debug.Log("Caja no se puede mover, hay algo adelante.");
                }

                return;
            }

            Debug.Log("Colisión con otro objeto, no se mueve.");
            return;
        }

        // Si no hay nada, mover al jugador
        transform.position = targetPos;
    }
}
