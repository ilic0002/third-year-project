using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector2 originalPosition;          
    private RectTransform rectTransform;      
    private Canvas canvas;                    
    public RectTransform targetArea;          
    public bool isCorrectlyPlaced = false;    
    public PuzzleManager puzzleManager;       

    public Sprite puzzlePieceSprite;

    void Start()
{
    rectTransform = GetComponent<RectTransform>(); 
    if (rectTransform == null)
    {
        Debug.LogError("Missing RectTransform component on PuzzlePiece!");
        return;
    }

    canvas = GetComponentInParent<Canvas>(); 
    if (canvas == null)
    {
        Debug.LogError("Missing Canvas component in parent of PuzzlePiece!");
        return;
    }

    // Assign the sprite to the Image component
    Image imageComponent = GetComponent<Image>();
    if (imageComponent != null && puzzlePieceSprite != null)
    {
        imageComponent.sprite = puzzlePieceSprite;
    }
    else
    {
        Debug.LogWarning("No Image component or Sprite found on PuzzlePiece.");
    }
}

    public void Initialize(RectTransform target, PuzzleManager manager)
    {
        targetArea = target;          // Assign the target area
        puzzleManager = manager;      // Assign the PuzzleManager reference
        isCorrectlyPlaced = false;    // Reset placement status
        originalPosition = rectTransform.position; // Store initial position
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rectTransform.SetAsLastSibling();  // Bring this piece to the front while dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas.renderMode == RenderMode.WorldSpace)
        {
            rectTransform.position = eventData.position; // Use world position for world space canvas
        }
        else
        {
            rectTransform.position = eventData.position / canvas.scaleFactor; // For screen space canvas
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isCorrectlyPlaced)
        {
            return; // If the piece is already placed correctly, do nothing
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(targetArea, rectTransform.position, null))
        {
            rectTransform.position = targetArea.position;
            isCorrectlyPlaced = true;

            puzzleManager.CheckPuzzleCompletion(); // Notify manager
        }
        else
        {
            rectTransform.position = originalPosition; // Return to original position
        }
    }

    public bool IsPlacedCorrectly()
    {
        return isCorrectlyPlaced;
    }
}
