using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkEntity : MonoBehaviourPunCallbacks, IPunObservable
{
    private Vector2 networkPosition;
    private float networkXScale;

    private float smoothing = 10f;

    private void Start()
    {
        networkPosition = transform.position;
    }

    protected void updatePosition()
    {
        transform.position = Vector2.Lerp(transform.position, networkPosition, Time.deltaTime * smoothing);
        transform.localScale = 
            Vector3.Lerp(
                transform.localScale, 
                new Vector3(networkXScale, transform.localScale.y, transform.localScale.z), 
                Time.deltaTime * smoothing
                );
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.localScale.x);
        } else
        {
            networkPosition = (Vector2)stream.ReceiveNext();
            networkXScale = (float)stream.ReceiveNext();
        }
    }
}
