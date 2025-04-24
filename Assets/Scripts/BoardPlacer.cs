using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class BoardPlacer : MonoBehaviour
{
    public GameObject sokobanBoardManager;
    private ARRaycastManager arRaycastManager;
    private Camera arCamera;
    private bool placed = false;

    void Start()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        arCamera = Camera.main;
    }

    void Update()
    {
        if (placed) return;

#if UNITY_EDITOR
        // Para pruebas con clic del mouse en el editor
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = arCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                PlaceBoard(hit.point);
            }
        }
#else
        // Para dispositivos móviles (toques)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            List<ARRaycastHit> hits = new List<ARRaycastHit>();

            if (arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                PlaceBoard(hitPose.position);
            }
        }
#endif
    }

    void PlaceBoard(Vector3 position)
    {
        sokobanBoardManager.transform.position = position;
        sokobanBoardManager.GetComponent<SokobanBoardGenerator>().boardOrigin = position;
        sokobanBoardManager.GetComponent<SokobanBoardGenerator>().GenerateBoard();
        placed = true;
    }
}