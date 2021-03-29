using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class Snake : MonoBehaviour
{
    public int touchSensitivity = 15;
    public bool singleTouchMovement = false;
    private Touch touch;
    private Vector2 touchStartPosition, touchEndPosition;
    private int dirIndex = 2;

    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<Vector2Int> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;

    public void Setup (LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }

    private void Awake()
    {
        gridPosition = new Vector2Int(10, 10);
        gridMoveTimerMax = .9f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1, 0);

        snakeMovePositionList = new List<Vector2Int>();
        snakeBodySize = 3;

        snakeBodyPartList = new List<SnakeBodyPart>();
    }

    private void Update()
    {
        HandleKeyboardInput();
        if (singleTouchMovement == false)
        {
            HandleTouchInput();
        }
        else if (singleTouchMovement == true)
        {
            HandleSingleTouchInput();
        }
        HandleGridMovement();
    }

    private void HandleSingleTouchInput()
    {
        Vector2Int[] dirs = { new Vector2Int(0, +1), new Vector2Int(+1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                if (dirIndex > 3 )
                {
                    dirIndex = 0;
                }
                gridMoveDirection.x = dirs[dirIndex].x;
                gridMoveDirection.y = dirs[dirIndex].y;
                dirIndex++;
            }
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPosition = touch.position;
            }

            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended)
            {
                touchEndPosition = touch.position;

                // Calculate Vector of movement
                float x = touchEndPosition.x - touchStartPosition.x;
                float y = touchEndPosition.y - touchStartPosition.y;

                // Round Movement
                float absX = Mathf.Abs(x);
                float absY = Mathf.Abs(y);

                if (absX <= 2 && absY <= 2)
                {
                    CMDebug.TextPopupMouse("Swipe to Move!");
                }

                else if (absX > absY)
                {
                    if (x >= touchSensitivity)
                    {
                        if (gridMoveDirection.x != -1)
                        {
                            gridMoveDirection.x = +1;
                            gridMoveDirection.y = 0;
                        }
                    }
                    else if (x <= -touchSensitivity)
                    {
                        if (gridMoveDirection.x != +1)
                        {
                            gridMoveDirection.x = -1;
                            gridMoveDirection.y = 0;
                        }
                    }
                }

                else
                {
                    if (y >= touchSensitivity)
                    {
                        if (gridMoveDirection.y != -1)
                        {
                            gridMoveDirection.x = 0;
                            gridMoveDirection.y = +1;
                        }
                    }
                    else if (y <= -touchSensitivity)
                    {
                        if (gridMoveDirection.y != +1)
                        {
                            gridMoveDirection.x = 0;
                            gridMoveDirection.y = -1;
                        }
                    }
                }
            }
        }
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection.y != -1)
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = +1;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection.y != +1)
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection.x != +1)
            {
                gridMoveDirection.x = -1;
                gridMoveDirection.y = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection.x != -1)
            {
                gridMoveDirection.x = +1;
                gridMoveDirection.y = 0;
            }
        }
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;

            snakeMovePositionList.Insert(0, gridPosition);

            gridPosition += gridMoveDirection;

            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            if (snakeAteFood)
            {
                float speedChange = .05f;
                gridMoveTimerMax -= speedChange;
                speedChange /= 5;
                // Snake Ate food, grow body
                snakeBodySize++;
                CreateSnakeBodyPart();
            }
            
            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            /*for (int i = 0; i < snakeMovePositionList.Count; i++)
            {
                Vector2Int snakeMovePosition = snakeMovePositionList[i];
                World_Sprite worldSprite = World_Sprite.Create(new Vector3(snakeMovePosition.x, snakeMovePosition.y), Vector3.one * .5f, Color.white);
                FunctionTimer.Create(worldSprite.DestroySelf, gridMoveTimerMax);
            }*/

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) - 90); // - 90 as unity thinks of up as 90deg.

            UpdateSnakeBodyParts();
        }
    }

    private void CreateSnakeBodyPart()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetGridPosition(snakeMovePositionList[i]);
        }
    }   

    // Function to calculate angle from origin intersecting a given point.
    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    // Return the full list of positions occupied by the snake: Head & Body
    public List<Vector2Int> GetFullSnakeGridPositionList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        gridPositionList.AddRange(snakeMovePositionList);
        return gridPositionList;
    }




    private class SnakeBodyPart
    {

        private Vector2Int gridPosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -1 - bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetGridPosition(Vector2Int gridPosition)
        {
            this.gridPosition = gridPosition;
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
        }

    }
}
