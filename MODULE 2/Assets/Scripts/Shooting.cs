using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class Shooting : MonoBehaviourPunCallbacks
{
    public Camera camera;
    public GameObject HitEffectPrefab;

    public GameObject KillFeedPrefab;
    
 
    private Animator animator;

    private int kills = 0;
    

    [Header("HP stuff")]
    public float maxHealth = 100;
    public float currentHealth;
    public Image healthbar;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthbar.fillAmount = currentHealth / maxHealth;
    }



    public void Fire()
    {
        RaycastHit hit;
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out hit,200))
        {
            Debug.Log(hit.collider.gameObject.name);

            photonView.RPC("CreateHitEffect", RpcTarget.All, hit.point);

            if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
            {
                if(hit.collider.GetComponent<Shooting>().currentHealth > 0)
                {
                    hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 25);
                    if (enemyDied(hit.collider.gameObject))
                    {
                        UpdateKillCount();
                    }
                }
                
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damage, PhotonMessageInfo info)
    {
        currentHealth -= damage;
        healthbar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();

            Debug.Log(info.Sender.NickName + " Killed " + info.photonView.Owner.NickName);

            GameObject kill = Instantiate(KillFeedPrefab);
            GameObject killParent = GameObject.Find("KillFeed");

            kill.transform.SetParent(killParent.GetComponent<Transform>());

            kill.transform.Find("Killer").GetComponent<Text>().text = info.Sender.NickName;
            kill.transform.Find("Victim").GetComponent<Text>().text = info.photonView.Owner.NickName;

            Destroy(kill, 20.0f);

            //photonView.RPC("UpdateKillFeed", RpcTarget.All);
            if (photonView.IsMine)
            {
                StartCoroutine(RespawnCountDown());
            }            
        }
    }

    [PunRPC]
    public void CreateHitEffect(Vector3 pos)
    {
        GameObject hitEffectGameObject = Instantiate(HitEffectPrefab, pos, Quaternion.identity);
        Destroy(hitEffectGameObject, 0.2f);

    }

    private IEnumerator RespawnCountDown()
    {
        GameObject respawnText = GameObject.Find("RespawnText");
        float respawnTime = 5.0f;

        while(respawnTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime--;

            transform.GetComponent<PlayMovementController>().enabled = false;
            respawnText.GetComponent<Text>().text = "You are dead. Respawning in : " + respawnTime.ToString(".00");
        }

        
        respawnText.GetComponent<Text>().text = "";
        animator.SetBool("isDead", false);

        Vector3 randomPoint = RespawnPoints.Instance.SpawnPoints[Random.Range(0, RespawnPoints.Instance.SpawnPoints.Count)];

        this.transform.position = randomPoint;

        transform.GetComponent<PlayMovementController>().enabled = true;

        photonView.RPC("RegainHealth", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void RegainHealth()
    {
        currentHealth = maxHealth;
        healthbar.fillAmount = currentHealth / maxHealth;
    }

    public void Die()
    {
        if (photonView.IsMine)
        {
            animator.SetBool("isDead", true);
            
        }
    }

    [PunRPC]
    public void UpdateKillFeed(PhotonMessageInfo info)
    {
        GameObject kill = Instantiate(KillFeedPrefab);
        GameObject killParent = GameObject.Find("KillFeed");

        kill.transform.SetParent(killParent.GetComponent<Transform>());

        kill.transform.Find("Killer").GetComponent<Text>().text = info.Sender.NickName;
        kill.transform.Find("Victim").GetComponent<Text>().text = info.photonView.Owner.NickName;

        Destroy(kill, 20.0f);
    }

    public void UpdateKillCount()
    {
        if (kills < 9) {
            kills++;
            Debug.Log(photonView.Owner.NickName + " has " + kills + " kills");
            GameObject killCount = GameObject.Find("Kills");

            killCount.GetComponent<TextMeshProUGUI>().text = "Kills: " + kills.ToString();

        }
        else
        {
            kills++;

            Debug.Log(photonView.Owner.NickName + " has " + kills + " kills");
            GameObject killCount = GameObject.Find("Kills");

            killCount.GetComponent<TextMeshProUGUI>().text = "Kills: " + kills.ToString();

            photonView.RPC("EndGame", RpcTarget.All);
        }
    }

     [PunRPC]
     public IEnumerator EndGame(PhotonMessageInfo info)
     {
        GameObject VictoryPanel = GameObject.Find("VictoryPanel");

        transform.GetComponent<PlayMovementController>().enabled = false;

        VictoryPanel.GetComponent<Canvas>().enabled = true;

        SoundManager.Instance.playMusic();

        GameObject.Find("WinnerName").GetComponent<Text>().text = info.Sender.NickName;
        
        yield return new WaitForSeconds(10.0f);

        Debug.Log("Leaving");
        LeaveRoom();
     }
  
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public bool enemyDied(GameObject target)
    {
        if (target.GetComponent<Shooting>().currentHealth <= 0) return true;
        else return false;
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }
}
