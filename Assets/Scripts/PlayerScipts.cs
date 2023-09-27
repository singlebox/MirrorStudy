using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerScipts : NetworkBehaviour
{
    private SceneScript sceneScript; 
    public GameObject[] playerWeapon;
    private int currentWeapon=0;
    public GameObject floatingInfo;
    public TextMesh nameText;       
    [SyncVar(hook = nameof(OnPlayerNameChanged))]       //���ϱ�ǩ[����]�ڸı�ʱ���������Ѹı��˵���Ϣ���͸����пͻ��ˣ��ͻ���ִ�ж�Ӧ�ķ���
    private string playerName;                          //����˲���ִ�иú���
    private Material playerMateialClone;
    [SyncVar(hook =nameof(OnPlayerColorChanged))]
    private Color playerColor;

    [SyncVar(hook =nameof(OnWeaponChanged))]
    private int currentWeaponSynced=0;

    private Weapon activeWeapon;
    private float coolDownTime;

    private void OnWeaponChanged(int oldIndex,int newIndex)
    {
        if (newIndex==0) {
            playerWeapon[2].SetActive(false);
            activeWeapon = null;
            sceneScript.canvasBUlletText.text = "No Weapon!";
        }
        else
        {
            if(oldIndex==1)
            {
                playerWeapon[1].SetActive(false);
                playerWeapon[2].SetActive(true);
            }
            else
            {
                playerWeapon[1].SetActive(true);
            }

            activeWeapon = playerWeapon[newIndex].GetComponent<Weapon>();
            sceneScript.canvasBUlletText.text = activeWeapon.bulletCount.ToString();
        }
    }

    private void OnPlayerNameChanged(string oldStr,string newStr)
    {
        nameText.text = newStr;     //�������Ŀͻ��˸ı�
    }
    private void OnPlayerColorChanged(Color oldCol,Color newCol)
    {
        nameText.color = newCol;
        playerMateialClone.SetColor("_EmissionColor", newCol);

        GetComponent<Renderer>().material = playerMateialClone;
    }

    [Command]           //�ڿͻ��˵��ã��ڷ����ִ��     
    private void CmdSetupPlayer(string nameValue,Color colorValue)
    {
        playerName = nameValue;
        playerColor = colorValue;
        sceneScript.statusText = $"{playerName} joined";        
    }

    [Command]
    public void CmdChangeActiveWeapon(int newIndex)
    {
        currentWeaponSynced = newIndex;

    }
    [Command]
    private void CmdShoot()         //�ͻ��˵��ã������ִ��
    {
        RpcWeaponFire();
    }


    [ClientRpc]
    public void RpcWeaponFire()     //����˵��ã��ͻ���ִ��   
    {
        var bullet = Instantiate(activeWeapon.bullet, activeWeapon.firePos.position, activeWeapon.firePos.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * activeWeapon.bulletSpeed;
        Destroy(bullet, activeWeapon.bulletLife);
    }


    [Command]
    public void CmdSendPlayerMessage()
    {
        if(sceneScript)
        {
            sceneScript.statusText = $"{playerName} say hello {Random.Range(0, 99)}";
        }
    }

    public override void OnStartLocalPlayer()
    {
        sceneScript.playerScript = this;
        

        Camera.main.transform.SetParent(transform); //�������
        Camera.main.transform.localPosition = new Vector3(0,0f,0.15f);

        floatingInfo.transform.localPosition = new Vector3(0f,-3f, 6f);      //ʹ�������λ���ӽ����·�
        floatingInfo.transform.localScale = new Vector3(1f, 1f, 1f);
        floatingInfo.transform.localRotation = Quaternion.Euler(0,180,0); //new Vector3(0, 180, 0);
        Change();
    }

    private void Awake()
    {
        playerMateialClone = GetComponent<Renderer>().material;
        sceneScript = FindObjectOfType<SceneScript>();
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            floatingInfo.transform.LookAt(Camera.main.transform);
            return;
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            Change();
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            currentWeapon = (currentWeapon + 1) % 3;
            CmdChangeActiveWeapon(currentWeapon);
        }
        if(Input.GetButtonDown("Fire1"))
        {
            if(activeWeapon&&Time.time>coolDownTime&&activeWeapon.bulletCount>0)
            {
                coolDownTime = Time.time + activeWeapon.coolDown;
                activeWeapon.bulletCount--;
                sceneScript.canvasBUlletText.text = activeWeapon.bulletCount.ToString();
                CmdShoot();
            }
        }
        var moveX= Input.GetAxis("Horizontal") * Time.deltaTime * 110f;
        var moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

        transform.Rotate(0,moveX,0);
        transform.Translate(0, 0, moveZ);
    }
    /// <summary>
    /// �ı���ɫ�����֡������˷�����Ϣ
    /// </summary>
    private void Change()
    {
        var tempName = $"Player {Random.Range(1, 1000)}";
        var tempColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        CmdSetupPlayer(tempName, tempColor);
        playerMateialClone.SetColor("_EmissionColor", tempColor);
        GetComponent<Renderer>().material = playerMateialClone;
    }

}
