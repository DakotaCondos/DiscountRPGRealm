using Nova;
using System.Threading.Tasks;
using UnityEngine;

public class MonsterPieceSpawner : MonoBehaviour
{
    [SerializeField] UIBlock2D backPlate;
    [SerializeField] UIBlock2D picture;
    [SerializeField] AudioClip spawnSound;
    [SerializeField] Color backplateColor;
    [SerializeField] float time;
    private CameraController cameraController;
    public static MonsterPieceSpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        cameraController = FindObjectOfType<CameraController>();
        gameObject.SetActive(false);
    }

    public async Task SpawnMonster(Monster monster, Space space, TaskHelper taskHelper)
    {
        backPlate.Color = backplateColor;
        picture.SetImage(monster.monsterTexture);
        transform.position = space.spawnStartPoint.position;
        gameObject.SetActive(true);
        cameraController.SetFocusObject(gameObject);
        if (spawnSound != null)
        {
            AudioManager.Instance.PlaySound(spawnSound, AudioChannel.SFX);
        }
        await ObjectTransformUtility.TransitionObjectSmooth(gameObject, space.spawnStartPoint, space.pieceMovePoint, time);
        cameraController.ClearFocusObject();
        taskHelper.isComplete = true;
        gameObject.SetActive(false);
    }
}
