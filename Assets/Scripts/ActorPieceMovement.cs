using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorPieceMovement : MonoBehaviour
{
    public float jumpHeight = 1f;
    public float jumpTime = 0.5f;
    public float pauseTime = 0.5f;
    public Color[] colorIDs;


    [SerializeField] UIBlock2D backPlate;
    [SerializeField] UIBlock2D picture;
    [SerializeField] AudioClip jumpSound;

    GameBoard GameBoard;
    private int currentPointIndex = 0;

    private void Awake()
    {
        GameBoard = FindObjectOfType<GameBoard>();
        gameObject.SetActive(false);
    }

    public void MoveActor(TurnActor turnActor, Space start, Space end, TaskHelper helper)
    {
        //Change PivotPiece to look like actor
        backPlate.Color = (turnActor.isPlayer) ? colorIDs[turnActor.player.TeamID] : colorIDs[7];
        picture.SetImage(turnActor.player.playerTexture);

        //get movement point
        List<Space> path = GameBoard.FindPath(start, end);
        List<Transform> points = new();
        foreach (var space in path)
        {
            points.Add(space.pieceMovePoint.transform);
        }

        gameObject.SetActive(true);
        // move piece
        StartCoroutine(MoveToPoints(points.ToArray(), turnActor, end, helper));
    }

    private IEnumerator MoveToPoints(Transform[] points, TurnActor actor, Space end, TaskHelper helper)
    {
        // Move to the first point
        if (points.Length > 0)
        {
            transform.position = points[0].position;
            currentPointIndex = 1;
        }

        while (currentPointIndex < points.Length)
        {
            Vector3 currentPoint = points[currentPointIndex].position;
            Vector3 jumpPoint = new Vector3(currentPoint.x, currentPoint.y, currentPoint.z + jumpHeight);

            // Jump to the next point
            yield return JumpToPosition(jumpPoint);

            // Play sound and pause
            PlaySound();
            yield return new WaitForSeconds(pauseTime);

            // Move to the next point
            currentPointIndex++;
        }

        // Coroutine finished
        SetPieces(actor, end, helper);
        gameObject.SetActive(false);
    }

    private IEnumerator JumpToPosition(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < jumpTime)
        {
            float t = elapsedTime / jumpTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is accurate
        transform.position = targetPosition;
    }

    private void PlaySound()
    {
        AudioManager.Instance.PlaySound(jumpSound, AudioChannel.SFX);
    }

    private void SetPieces(TurnActor actor, Space end, TaskHelper helper)
    {
        // Implement your logic to set the pieces after the movement is complete
        Debug.Log("Setting pieces!");

        helper.isComplete = true;
    }
}