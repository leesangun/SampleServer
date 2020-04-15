package com.server.protocol;

public class ProtocolObject {
	//�񵿱⼭���̱� ������ �̹���� �׽�Ʈ �ʿ�
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

        _resRoomAreaList.recordRoomAreas[0].name = "����";
        _resRoomAreaList.recordRoomAreas[1].name = "���";
        _resRoomAreaList.recordRoomAreas[2].name = "�λ�";

        _resLogin.recordRoomAreas = _resRoomAreaList.recordRoomAreas;
    }
}
