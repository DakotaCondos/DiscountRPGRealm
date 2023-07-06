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

    private CameraController cameraController;
    private GameObject _midpointObject;


    private void Awake()
    {
        GameBoard = FindObjectOfType<GameBoard>();
        cameraController = FindObjectOfType<CameraController>();
        gameObject.SetActive(false);
        _midpointObject = new GameObject("MidpointObject");
    }

    public void MoveActor(TurnActor turnActor, Space start, Space end, TaskHelper helper)
    {
        //Change PivotPiece to look like actor
        backPlate.Color = colorIDs[turnActor.player.TeamID];
        picture.SetImage(turnActor.player.playerTexture);
        Moving(start, end, helper);
    }
    public void MoveMonster(Monster monster, Space start, Space end, TaskHelper helper)
    {
        //Change PivotPiece to look like actor
        backPlate.Color = colorIDs[7];
        picture.SetImage(monster.monsterTexture);
        helper.flag = true;
        Moving(start, end, helper, false); // dont follow monster movement as to jaring 
    }

    private void Moving(Space start, Space end, TaskHelper helper, bool cameraFollowMovement = true)
    {
        //get movement point
        List<Space> path = GameBoard.FindPath(start, end);
        List<Transform> points = new();
        foreach (var space in path)
        {
            points.Add(space.pieceMovePoint.transform);
        }

        gameObject.SetActive(true);

        if (!cameraFollowMovement)
        {
            // Get the positions of the two objects
            Vector3 position1 = start.gameObject.transform.position;
            Vector3 position2 = end.gameObject.transform.position;

            // Calculate the midpoint between the two positions
            Vector3 midpoint = (position1 + position2) / 2f;
            _midpointObject.transform.position = midpoint;

            // Look at midpoint
            cameraController.SetFocusObject(_midpointObject);
        }
        else
        {
            // Follow moving piece
            transform.position = points[0].position;
            cameraController.SetFocusObject(gameObject);
        }

        // move piece
        StartCoroutine(MoveToPoints(points.ToArray(), helper));

    }

    private IEnumerator MoveToPoints(Transform[] points, TaskHelper helper)
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
        cameraController.ClearFocusObject();
        helper.isComplete = true;
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
}