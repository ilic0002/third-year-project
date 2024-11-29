using UnityEngine;

public class PuzzleGenerator : MonoBehaviour
{
    public Transform pieceStartArea; 
    public PuzzleManager puzzleManager; 
    public GameObject puzzlePiecePrefab; 
    public RectTransform[] targetAreas; 

    public void GeneratePuzzle()
    {
        if (pieceStartArea == null)
        {
            Debug.LogError("Piece Start Area is not assigned in PuzzleGenerator.");
            return;
        }

        if (targetAreas.Length == 0)
        {
            Debug.LogError("Target areas are not assigned in PuzzleGenerator.");
            return;
        }

        // Clear any existing puzzle pieces in the start area
        foreach (Transform child in pieceStartArea)
        {
            Destroy(child.gameObject);
        }

        // Instantiate puzzle pieces
        for (int i = 0; i < targetAreas.Length; i++) 
        {
            GameObject newPiece = Instantiate(puzzlePiecePrefab, pieceStartArea);
            PuzzlePiece puzzlePiece = newPiece.GetComponent<PuzzlePiece>();
            if (puzzlePiece != null)
            {
                puzzlePiece.Initialize(targetAreas[i], puzzleManager);
            }
            else
            {
                Debug.LogError("PuzzlePiece component missing on the puzzle piece prefab.");
            }
        }
    }
}
