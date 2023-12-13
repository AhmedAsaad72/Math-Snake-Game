using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;
    private bool isRightAnswer = false;
    private Snake snake;

    private void Awake()
    {
        snake = FindObjectOfType<Snake>();
    }

    public void Respawn(bool isRight = false)
    {
        isRightAnswer = isRight;
        Bounds bounds = gridArea.bounds;

        // Pick a random position inside the bounds
        // Round the values to ensure it aligns with the grid
        int x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
        int y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));

        // Prevent the food from spawning on the snake or other food
        while (snake.Occupies(x, y))
        {
            x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
            y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));
        }


        transform.position = new Vector2(x, y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isRightAnswer)
            GameManager.instance.score += 1;
        else
            GameManager.instance.score -= 1;

        GameManager.instance.GetNewQuestion();
    }

}
