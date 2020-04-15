package com.server.protocol;

public class ProtocolObject {
	//비동기서버이기 때문에 이방식은 테스트 필요
    public static ResLogin _resLogin = new ResLogin();
    public static ResRoomAreaJoin _resRoomAreaJoin = new ResRoomAreaJoin();
    public static ResMessage _resMessage = new ResMessage();

    public static ResRoomAreaList _resRoomAreaList = new ResRoomAreaList();

    public void SetData()
    {
        _resRoomAreaList.recordRoomAreas = new RecordRoomArea[3];
        for (int i = 0; i < _resRoomAreaList.recordRoomAreas.length; i++)
        {
            _resRoomAreaList.recordRoomAreas[i] = new RecordRoomArea();
            _resRoomAreaList.recordRoomAreas[i].idRoom = "-" + (i + 1);
        }

        _resRoomAreaList.recordRoomAreas[0].name = "서울";
        _resRoomAreaList.recordRoomAreas[1].name = "경기";
        _resRoomAreaList.recordRoomAreas[2].name = "부산";

        _resLogin.recordRoomAreas = _resRoomAreaList.recordRoomAreas;
    }
}
