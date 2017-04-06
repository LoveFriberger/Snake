using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject tileObject;
    public static Dictionary<Vector2Int, Tile> tileDict = new Dictionary<Vector2Int, Tile>();
    private void Awake()
    {
        BuildBoard();
        GameManager.Score += PlaceNewFruit;
        GameManager.ClearBoard += EmptyAllBoardTiles;
        GameManager.StartGame += PlaceNewFruit;
    }

    //This creates all the tiles in  the right place.
    private void BuildBoard()
    {
        var tileOffset = new Vector2Int(-GameManager.boardSize.x / 2, -GameManager.boardSize.y / 2);
        for (var x = 0; x < GameManager.boardSize.x; x++)
        {
            for (var y = 0; y < GameManager.boardSize.y; y++)
            {
                var position = new Vector2Int(x + tileOffset.x, y + tileOffset.y);
                var tileClone = (GameObject)Instantiate(tileObject, (Vector2)position, Quaternion.identity, transform);
                tileClone.name = string.Format("Tile {0}x{1}", position.x, position.y);
                tileClone.GetComponent<Tile>().position = position;

                //This sees if the tile is on the edge of the board, and if so makes it a wall.
                if (x == 0 || 
                    y == 0 ||
                    x == GameManager.boardSize.x -1 ||
                    y == GameManager.boardSize.y -1)
                {
                    tileClone.GetComponent<Tile>().type = TileType.Wall;
                    tileClone.GetComponent<SpriteRenderer>().color = Color.grey;
                    tileClone.name = tileClone.name + "(Wall)";
                }
                tileDict.Add(position, tileClone.GetComponent<Tile>());
            }
        }
        //Set the camera to our board.
        Camera.main.GetComponent<CameraManager>().SetupCamera();
    }

    //This finds  an empty spot on our board and places a fruit there
    public static void PlaceNewFruit()
    {
        var emptyTiles = tileDict.Values.Where(tile => tile.type == TileType.Empty).ToList();
        if (emptyTiles.Count == 0)
            GameManager.Win();
        else
        {
            var randomTile = emptyTiles[Random.Range(0, emptyTiles.Count)];
            randomTile.SetFruit();
        }
    }

    private void EmptyAllBoardTiles()
    {
        foreach (var tile in tileDict.Values)
        {
            if(tile.type != TileType.Wall)
                tile.SetEmpty();
        }
    }
}
