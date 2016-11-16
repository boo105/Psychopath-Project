using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour {

    private GameObject player;
    private bool followPlayer = true;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void UpdateLate () {
        if (followPlayer == true)
        {
            CamFollowPlayer();
        }
    }

    public void setFollowPlayer(bool val)
    {
        followPlayer = val;
    }

    void CamFollowPlayer()
    {
        Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        this.transform.position = newPos;
    }
}
