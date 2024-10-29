using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePiece : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Logic to handle the beginning of a drag (e.g., change color)
        GetComponent<Renderer>().material.color = Color.yellow; // Example: change color on drag start
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the piece with the mouse
        transform.position = eventData.position; // Move to the current mouse position
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Logic to handle when dragging ends (e.g., snap to grid)
        // Example: snapping the piece to the nearest grid position
        SnapToGrid();
        GetComponent<Renderer>().material.color = Color.white; // Reset color on drag end
    }

    private void SnapToGrid()
    {
        // Implement snapping logic here
        // For example, you can round the position to the nearest grid point
        float gridSize = 1.0f; // Adjust this to your grid size
        Vector3 newPosition = new Vector3(
            Mathf.Round(transform.position.x / gridSize) * gridSize,
            Mathf.Round(transform.position.y / gridSize) * gridSize,
            transform.position.z
        );
        transform.position = newPosition; // Snap to calculated position
    }
}
