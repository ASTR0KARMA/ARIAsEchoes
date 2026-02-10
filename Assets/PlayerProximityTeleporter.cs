using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerProximityTeleporter : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float triggerDistance = 100f;

    private void Start()
    {
        NarrationSystem.Instance.StartNarration(Narration.Act1_Reveil); // LE BARBAR
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < triggerDistance)
        {
            SceneManager.LoadScene("Echo1");
            return;
        }
    }
}