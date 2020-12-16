using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public bool is_witness;
    public bool is_tube;
    public bool is_sliding;
    public GameObject carpet;
    public RoomData data;
    private void Awake()
    {
        data = new RoomData();
        data.is_sliding = false;
        data.is_tube = false;
        data.is_witness = false;
        DataSaver.saveData<RoomData>(data, "room_data");
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (data.is_sliding)
            Destroy(carpet);
        if (data.is_sliding && data.is_tube && data.is_witness)
            // добавить анимацию
            gameObject.GetComponent<ChangeScene>().load();
    }
}
