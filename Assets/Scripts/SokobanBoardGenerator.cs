using UnityEngine;

public class SokobanBoardGenerator : MonoBehaviour
{
    public Vector3 boardOrigin = Vector3.zero; // Se actualiza luego con AR
    public float tileSize = 0.2f;

    private string[] map = new string[]
    {
        "WWWWW",
        "WP  W",
        "W B W",
        "W  GW",
        "WWWWW"
    };

    public void GenerateBoard()
    {
        for (int y = 0; y < map.Length; y++)
        {
            string row = map[y];
            for (int x = 0; x < row.Length; x++)
            {
                char c = row[x];
                Vector3 pos = boardOrigin + new Vector3(x * tileSize, 0f, -y * tileSize);

                switch (c)
                {
                    case 'W': CreateWall(pos); break;
                    case 'P': CreatePlayer(pos); break;
                    case 'B': CreateBox(pos); break;
                    case 'G': CreateGoal(pos); break;
                    default: CreateFloor(pos); break;
                }
            }
        }
    }

    void CreateFloor(Vector3 pos)
    {
        var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.position = pos + Vector3.down * 0.05f;
        floor.transform.localScale = Vector3.one * tileSize * 0.1f;
        floor.GetComponent<Renderer>().material.color = Color.gray;
    }

    void CreateWall(Vector3 pos)
    {
        var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.position = pos;
        wall.transform.localScale = Vector3.one * tileSize;
        wall.name = "Wall";
        wall.GetComponent<Renderer>().material.color = Color.black;
    }

    void CreateBox(Vector3 pos)
    {
        var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.transform.position = pos + Vector3.up * (tileSize * 0.5f); // Eleva para que esté encima del piso
        box.transform.localScale = Vector3.one * tileSize * 0.9f;

        var rb = box.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        rb.isKinematic = true;

        box.tag = "Box";
        box.name = "Box";
        box.GetComponent<Renderer>().material.color = Color.yellow;
    }

    void CreateGoal(Vector3 pos)
    {
        var goal = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        goal.transform.position = pos + Vector3.up * 0.01f;
        goal.transform.localScale = new Vector3(tileSize, 0.02f, tileSize);
        goal.name = "Goal";
        goal.tag = "Goal";
        goal.GetComponent<Renderer>().material.color = Color.green;

        Collider col = goal.GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true; // ✅ Lo hacemos trigger
        }
    }

    void CreatePlayer(Vector3 pos)
    {
        var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.transform.position = pos + Vector3.up * 0.1f;
        player.transform.localScale = new Vector3(tileSize * 0.6f, tileSize * 0.6f, tileSize * 0.6f);
        player.name = "Player";
        player.tag = "Player";
        player.AddComponent<PlayerController>();
        player.AddComponent<PlayerTouchController>();
        player.GetComponent<Renderer>().material.color = Color.blue;
    }
}
