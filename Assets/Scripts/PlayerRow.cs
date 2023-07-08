using Nova;
using UnityEngine;

public class PlayerRow : MonoBehaviour
{
    public TextBlock playerNameBlock;
    public TextBlock teamBlock;
    public Player player;
    public UIBlock2D gradiantBlock;
    public UIBlock2D imageBlock;

    public void Delete()
    {
        GameManager.Instance.RemovePlayer(player);
        Destroy(gameObject);
    }
}
