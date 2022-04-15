using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public GameObject FpsModel;
    public GameObject NonFpsModel;

    public GameObject PlayerUIPrefab;

    public PlayMovementController playMovementController;

    public Camera fpsCamera;

    private Animator animator;
    public Avatar FPSAvatar;
    public Avatar NonFPSAvatar;

    public TextMeshProUGUI PlayerName;

    private Shooting shooting;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isLocalPlayer", photonView.IsMine);
        playMovementController = GetComponent<PlayMovementController>();

        shooting = GetComponent<Shooting>();        

        FpsModel.SetActive(photonView.IsMine);
        NonFpsModel.SetActive(!photonView.IsMine);

        animator.avatar = photonView.IsMine ? FPSAvatar : NonFPSAvatar;

        PlayerName.text = photonView.Owner.NickName;

        if (photonView.IsMine)
        {

            GameObject playerUI = Instantiate(PlayerUIPrefab);
            playMovementController.fixedTouchField = playerUI.transform.Find("RotationTouchField").GetComponent<FixedTouchField>();
            playMovementController.joystick = playerUI.transform.Find("FixedJoystick").GetComponent<Joystick>();

            fpsCamera.enabled = true;

            playerUI.transform.Find("FireButton").GetComponent<Button>().onClick.AddListener(() => shooting.Fire());

        }
        else
        {         

            playMovementController.enabled = false;
            GetComponent<RigidbodyFirstPersonController>().enabled = false;
            fpsCamera.enabled = false;

        }

    }
}
