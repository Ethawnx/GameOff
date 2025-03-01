using UnityEngine;
using UnityEngine.SceneManagement;

public class LaserDevice : MonoBehaviour
{
    Player player;
    public GameObject ShatteredFX;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !player.IsInvulnerable) 
        {
            GameObject.Instantiate(ShatteredFX, player.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Invoke(nameof(ResetGame), 1f);
        }
    }
    public void ResetGame() 
    {
        SceneManager.LoadScene("Corp");
    }
}
