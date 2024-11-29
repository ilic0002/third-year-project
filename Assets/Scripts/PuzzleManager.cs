using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public PuzzleGenerator puzzleGenerator; 
    public Transform pieceStartArea; 
    public RectTransform[] targetAreas; 
    private List<PuzzlePiece> puzzlePieces; 
    private bool isPuzzleComplete = false; 

    void Start()
    {
        // Ensure that the puzzle generator is set up and generate the first puzzle
        if (puzzleGenerator != null)
        {
            puzzleGenerator.GeneratePuzzle();
        }

        // Ensure pieceStartArea is assigned and check the puzzle pieces
        if (pieceStartArea == null)
        {
            Debug.LogError("Piece Start Area is not assigned in PuzzleManager.");
            return;
        }

        // Initialize the list of puzzle pieces
        puzzlePieces = new List<PuzzlePiece>();

        // Check if there are any puzzle pieces in the pieceStartArea
        foreach (Transform piece in pieceStartArea)
        {
            PuzzlePiece puzzlePiece = piece.GetComponent<PuzzlePiece>();
            if (puzzlePiece != null)
            {
                puzzlePieces.Add(puzzlePiece);
            }
            else
            {
                Debug.LogWarning("Piece without PuzzlePiece component found in PieceStartArea.");
            }
        }

        // Ensure puzzle pieces list is populated
        if (puzzlePieces.Count == 0)
        {
            Debug.LogError("No Puzzle Pieces found in PieceStartArea.");
        }

        // Initialize the puzzle pieces (e.g., set the target areas for each piece)
        foreach (PuzzlePiece piece in puzzlePieces)
        {
            piece.Initialize(targetAreas[puzzlePieces.IndexOf(piece)], this);
        }
    }

    void Update()
    {
        // Continuously check if the puzzle is complete during gameplay
        CheckPuzzleCompletion();
    }

    public void CheckPuzzleCompletion()
{
    if (IsPuzzleComplete())
    {
        if (!isPuzzleComplete) // Only take action if it's the first time the puzzle is completed
        {
            Debug.Log("Puzzle Complete!");
            // Optionally load the next puzzle or show a completion UI
            Invoke("NextPuzzle", 2f); 
            isPuzzleComplete = true; 
        }
    }
    else
    {
        // Reset the isPuzzleComplete flag if the puzzle is incomplete
        isPuzzleComplete = false;
    }
}


    private bool IsPuzzleComplete()
    {
        // Ensure puzzlePieces is initialized and not empty
        if (puzzlePieces == null || puzzlePieces.Count == 0)
        {
            Debug.LogError("Puzzle pieces are not initialized or are empty.");
            return false;
        }

        // Check if all pieces are placed correctly
        foreach (PuzzlePiece piece in puzzlePieces)
        {
            if (piece == null) 
            {
                Debug.LogWarning("Null PuzzlePiece in puzzlePieces list.");
                continue;
            }

            if (!piece.IsPlacedCorrectly()) 
            {
                return false;
            }
        }
        return true;
    }

    private void NextPuzzle()
    {
        puzzleGenerator.GeneratePuzzle(); // Generate a new puzzle

        // Update the puzzlePieces list with new pieces
        puzzlePieces.Clear();
        foreach (Transform piece in pieceStartArea)
        {
            PuzzlePiece puzzlePiece = piece.GetComponent<PuzzlePiece>();
            if (puzzlePiece != null)
            {
                puzzlePieces.Add(puzzlePiece);
            }
        }

        // Reinitialize puzzle pieces with the new target areas
        foreach (PuzzlePiece piece in puzzlePieces)
        {
            piece.Initialize(targetAreas[puzzlePieces.IndexOf(piece)], this);
        }
    }

    public void ResetPuzzle()
    {
        // Reset the puzzle by returning all pieces to their original positions
        foreach (PuzzlePiece piece in puzzlePieces)
        {
            piece.transform.position = piece.originalPosition; // Reset puzzle piece positions
            piece.isCorrectlyPlaced = false; // Reset placement status
        }

        isPuzzleComplete = false; 
    }
}
