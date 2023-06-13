using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class RoomList : MonoBehaviourPunCallbacks
{

    [SerializeField] private RoomItem  roomItemPrefab;
    private List<RoomItem> roomItemsList = new List<RoomItem>();
    [SerializeField] Transform contentObject;

    [SerializeField] private float timeBetweenUpdates = 1.5f;
    private float nextUpdateTime;


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
        nextUpdateTime = Time.time + timeBetweenUpdates;
    }


}

    public void UpdateRoomList(List<RoomInfo> list)
    {

        //updating the current list
        foreach (RoomItem item in roomItemsList)
        {
            Debug.Log(item);
            Destroy(item);
        }
        roomItemsList.Clear();

        foreach (RoomInfo room in list)
        {
            if (room.RemovedFromList) return;
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject); // instantiate inb the content
            newRoom.UpdateRoom(room.Name, room.MaxPlayers, room.PlayerCount);
            roomItemsList.Add(newRoom);

        }

    }
}
