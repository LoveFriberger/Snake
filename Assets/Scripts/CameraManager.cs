using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private float relativeSpaceAboveBoard = 0.1f;

    public void SetupCamera()
    {
        //The size of the orthographic camera is the distance from the middle of the sceen to the bottom, in world units.
        //This finds the size needed to fit in the board and text fields.
        float sizeToFitHorizontal = (GameManager.boardSize.x / Camera.main.aspect) / 2;
        float sizeToFitVertical = GameManager.boardSize.y / 2f;
        var sizeToFitVerticalWithSpaceAbove = sizeToFitVertical + relativeSpaceAboveBoard * sizeToFitVertical;
    
        //When we have the size of the camera, we need to move it upwards, since the text is above the centralized board.
        var boardSize = sizeToFitHorizontal > sizeToFitVerticalWithSpaceAbove ? sizeToFitHorizontal : sizeToFitVerticalWithSpaceAbove;
        var halfSpaceAboveBoard = relativeSpaceAboveBoard * boardSize;
        transform.position += new Vector3(0, halfSpaceAboveBoard, 0);
        Camera.main.orthographicSize = boardSize  + halfSpaceAboveBoard;
    }
}
