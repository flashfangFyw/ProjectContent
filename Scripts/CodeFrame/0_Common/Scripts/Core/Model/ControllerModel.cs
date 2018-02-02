using UnityEngine;
using System.Collections;
//using VRTK;
namespace ffDevelopmentSpace
{
    public class ControllerModel : ModelBase
    {

        //#region rightController
        //public enum RightControllerToggle
        //{
        //    OpenPoint = 1,//开启射线
        //    ClosePoint = 2,//关闭射线
        //    Telepot = 3//导航
        //}
        //public RightControllerToggle rightControllerState= RightControllerToggle.OpenPoint;
        //public bool isOpenPointState()
        //{
        //    return rightControllerState == RightControllerToggle.OpenPoint;
        //}
        //public void ROpenPoint()
        //{
        //    rightControllerState = RightControllerToggle.OpenPoint;
        //    dispatchEvent(RightControllerMsg.OPEN_POINT);
        //}
        //public void RClosePoint()
        //{
        //    rightControllerState = RightControllerToggle.ClosePoint;
        //    dispatchEvent(RightControllerMsg.CLOSE_POINT);
        //}


        //public void ROpenTeleport()
        //{
        //    rightControllerState = RightControllerToggle.Telepot;
        //    dispatchEvent(RightControllerMsg.OPEN_TELEPORT);
        //}
        //#endregion
        //#region leftController
        //public void showSandboxArround()
        //{
        //    dispatchEvent(LeftControllerMsg.SANDBOX_ARROUND);
        //}

        //public void changeTime()
        //{
        //    dispatchEvent(LeftControllerMsg.TIME_CHANGE);
        //}
        //public void openSetting()
        //{
        //    dispatchEvent(LeftControllerMsg.OPEN_SETTING);
        //}

        //public void backSandBox()
        //{
        //    dispatchEvent(LeftControllerMsg.BACK_SANDBOX);
        //}
        //public void changeStyle()
        //{
        //    dispatchEvent(LeftControllerMsg.CHANGE_STYLE);
        //}
        //#endregion
        //#region roomStyle
        //public int buildingNo = 1;//楼号
        //public int floor = 6;//楼层
        //public int room = 1;//房间号
        //public bool isSelectRoomStyle = false;
        //public  int selectType = 0;
        //public void SelectItem(int index)
        //{
        //    isSelectRoomStyle = true;
        //    selectType = index;
        //    if (index==0)
        //    {
        //        Debug.Log("选择  楼号");
        //        //dispatchEvent(RoomStyleSelectMsg.BUILDING_NUMBER);
        //    }
        //    else if(index==1)
        //    {
        //        Debug.Log("选择 楼层");
        //        //dispatchEvent(RoomStyleSelectMsg.FLOOR);
        //    }
        //    else
        //    {
        //        Debug.Log("选择 房型");
        //        //dispatchEvent(RoomStyleSelectMsg.ROOM_NUMBER);
        //    }
        //    dispatchEvent(RoomStyleSelectMsg.SELECT_ROOM_STYLE);
        //    //dispatchEvent(RightControllerMsg.CLOSE_RIGHT_CONTROLLER);
        //}
        //public void SelectBuildingNumber(int number)
        //{
        //    buildingNo = number;
        //    dispatchEvent(RoomStyleSelectMsg.BUILDING_NUMBER);
        //}
        //public void SelectFloorNumber(int number)
        //{
        //    floor = number;
        //    dispatchEvent(RoomStyleSelectMsg.FLOOR);
        //    dispatchEvent(RoomStyleSelectMsg.CLOSEMODEL);
        //}
        //public  void SelectRoomNumber(int number)
        //{
        //    room = number;
        //    dispatchEvent(RoomStyleSelectMsg.ROOM_NUMBER);
        //}
        //public void SelectNumber(float index)
        //{
        //    if (selectType == 0)
        //    {
        //        //Debug.Log("选择  楼号");
        //        buildingNo = (int)index;
        //        dispatchEvent(RoomStyleSelectMsg.BUILDING_NUMBER);
        //    }
        //    else if (selectType == 1)
        //    {

        //        floor = (int)index;
        //        Debug.Log("选择 楼层="+ floor);
        //        dispatchEvent(RoomStyleSelectMsg.FLOOR);
        //    }
        //    else
        //    {
        //        //Debug.Log("选择 房型");
        //        room = (int)index;
        //        dispatchEvent(RoomStyleSelectMsg.ROOM_NUMBER);
        //    }
        //    //dispatchEvent(RightControllerMsg.CLOSE_RIGHT_CONTROLLER);
        //}
        //public  void SandboxModelSelectRoom()
        //{
        //    dispatchEvent(RoomStyleSelectMsg.ROOM_NUMBER);
        //}
        //public void SandboxModelSelectBuilding()
        //{
        //    dispatchEvent(RoomStyleSelectMsg.SHOWBUILDING);
        //}
        //public void SandboxModelSelectArea()
        //{
        //    dispatchEvent(RoomStyleSelectMsg.ARROUND);
        //}
        //public string getNumberByIndex(int index)
        //{
        //    if(index==0)
        //    {
        //        return buildingNo + "";
        //    }
        //    else if (index == 1)
        //    {
        //        return floor + "";
        //    }
        //    else
        //    {
        //        return room + "";
        //    }
        //}
        //public float currentAngle=0;
        //public void SetAngleChange(float angle)
        //{
        //    currentAngle =360-angle;
        //    dispatchEvent(RoomStyleSelectMsg.ANGLE_CHANGE);
        //}
        //public void confirmNumber()
        //{
        //    dispatchEvent(RoomStyleSelectMsg.NUMBER_CONFIRM);
        //    dispatchEvent(RoomStyleSelectMsg.END_SELECT_ROOM_STYLE);
        //}

        //public void refreshUI()
        //{
        //    dispatchEvent(RoomStyleSelectMsg.REFRESH_UI);
        //}
        //private float _selectNumber = 1;
        //public float selectNumber
        //{
        //    set
        //    {
        //        _selectNumber = value;
        //        SelectNumber(_selectNumber);
        //        //dispatchEvent(RoomStyleSelectMsg.END_SELECT_ROOM_STYLE);
        //    }
        //    get
        //    {
        //        return _selectNumber;
        //    }
        //}
        //#endregion

    }
}
