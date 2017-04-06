using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject snakeStraightObject;
    public GameObject snakeCornerObject;
    public GameObject snakeStraightEndObject;
    public GameObject fruitObject;
    public GameObject particleSystemObject;
    public TileType type = TileType.Empty;
    public Direction direction;
    public Vector2Int position;

    //Rotates the ends of the snake to the right direction.
    public void SetSnakeEnd(Direction direction)
    {
        //The default is that  the end of the snake points upward.
        snakeStraightEndObject.transform.rotation = Quaternion.Euler(0, 0, 90 * (int)direction);
        ClearTile();
        snakeStraightEndObject.SetActive(true);
        type = TileType.Snake;
    }

    //Rotates the middle, straight part of the snake to the right direction.
    public void SetStraightSnake(Direction direction)
    {
        //The default is that  the end of the snake points upward.
        snakeStraightObject.transform.rotation = Quaternion.Euler(0, 0, 90 * ((int)direction % 2 ));
        ClearTile();
        snakeStraightObject.SetActive(true);
        type = TileType.Snake;
        this.direction = direction;
    }

    //Rotates the corner parts to the correct direction.
    public void SetCornerSnake(Direction directionIn, Direction directionOut)
    {
        //Depending on the relative placement of the in and out direction, we may have to flip it 270 degrees.
        var flipCorner = ((int)directionIn + 1) % 4 == (int)directionOut ? 1: 0 ;
        snakeCornerObject.transform.rotation = Quaternion.Euler(0, 0, 90 * (int)directionIn + 270 * flipCorner);
        ClearTile();
        snakeCornerObject.SetActive(true);
        type = TileType.Snake;
        direction = directionOut;
    }

    //Activates the tile's fruit.
    public void SetFruit()
    {
        fruitObject.SetActive(true);
        type = TileType.Fruit;
    }

    //Sets the tile as empty.
    public void SetEmpty()
    {
        ClearTile();
        type = TileType.Empty;
    }

    //This makes sure there is no snake part or fruit on the tile.
    private void ClearTile()
    {
        snakeStraightObject.SetActive(false);
        snakeCornerObject.SetActive(false);
        fruitObject.SetActive(false);
        snakeStraightEndObject.SetActive(false);
    }

    //Starts up the particle system
    public void RunParticleSystem()
    {
        particleSystemObject.SetActive(true);
        StartCoroutine(ParticleSystemTimer());
    }

    private IEnumerator ParticleSystemTimer()
    {
        yield return new WaitForSeconds(0.3f);
        particleSystemObject.SetActive(false);
        yield break;
    }
}
