using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{

    private static float speedTilesPerSecond = 12;
    private static int startLength = 5;
    private static List<Tile> snakeTiles = new List<Tile>();
    private static Direction currentDirection;
    private static Direction nextDirection;
    private static float nextTimeToMove;

    public void Awake()
    {
        GameManager.ClearBoard += () => { snakeTiles = new List<Tile>(); };
        GameManager.StartGame += StartSnake;
    }

    public static void StartSnake()
    {
        currentDirection = Direction.Right;
        nextDirection = currentDirection;

        //Loop through the start lenght of the snake and add the snake tiles. The list is reversed to have the front of the snake at the top of the list.
        for (var i = startLength-1; i >= 0; i--)
        {
            var tilePosition = new Vector2Int(-i, 0);
            var tile = BoardManager.tileDict[tilePosition];

            //The ends of the snake are different
            if (i == 0)
                tile.SetSnakeEnd(currentDirection);
            else if (i == startLength - 1)
                tile.SetSnakeEnd(OppositeDirection(currentDirection));
            else
                tile.SetStraightSnake(currentDirection);
            snakeTiles.Add(tile);
        }
        nextTimeToMove = Time.time + (1 / speedTilesPerSecond);
    }

    private void Update()
    {
        KeyInput();
        if (GameManager.running && Time.time > nextTimeToMove)
        {
            MoveSnake();
            nextTimeToMove = Time.time + (1 / speedTilesPerSecond);
        }
    }

    private void KeyInput()
    {
        //Sets the next direction we move in.
        if (Input.GetKeyDown(KeyCode.UpArrow))
            nextDirection = Direction.Up;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            nextDirection = Direction.Down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            nextDirection = Direction.Left;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            nextDirection = Direction.Right;
    }

    private void MoveSnake()
    {
        var nextTile = CalculateNextTile();
        var headTile = snakeTiles[snakeTiles.Count - 1];

        //If we hit a wall or our selves, we lose.
        if (nextTile.type == TileType.Wall || nextTile.type == TileType.Snake)
            GameManager.Lose();
        else
        {
            snakeTiles.Add(nextTile);
            //See if we hit a fruit.
            if (nextTile.type == TileType.Fruit)
            {
                GameManager.Score();
                nextTile.RunParticleSystem();
            }
            else
            {
                //Set the new end of the snake and remove the old one.
                var newLastTile = snakeTiles[1];
                newLastTile.SetSnakeEnd(OppositeDirection(newLastTile.direction));
                snakeTiles[0].SetEmpty();
                snakeTiles.RemoveAt(0);
            }
            
            //See if we're moving straight ahead
            if (currentDirection == nextDirection)
            {
                headTile.SetStraightSnake(currentDirection);
                nextTile.SetSnakeEnd(currentDirection);
            }
            else
            {
                headTile.SetCornerSnake(currentDirection, nextDirection);
                nextTile.SetSnakeEnd(nextDirection);
                currentDirection = nextDirection;
            }


        }
        
    }

    private Tile CalculateNextTile()
    {
        //Gets the new tile from our old head of the snake and the new direction.
        nextDirection = nextDirection == OppositeDirection(currentDirection) ? currentDirection : nextDirection;
        var movement = new Vector2Int(0, 0);
        switch (nextDirection)
        {
            case Direction.Up:
                movement = new Vector2Int(0, 1);
                break;
            case Direction.Down:
                movement = new Vector2Int(0, -1);
                break;
            case Direction.Left:
                movement = new Vector2Int(-1, 0);
                break;
            case Direction.Right:
                movement = new Vector2Int(1, 0);
                break;
        }
        return BoardManager.tileDict[snakeTiles[snakeTiles.Count-1].position + movement];
    }

    private static Direction OppositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            default:
                return Direction.Left;
        }
    }
}
