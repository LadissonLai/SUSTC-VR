using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAOrderObserver
{
    // 飞轮螺栓有自己的拆卸顺序，非固定递增
    private List<int> feilunOrder = new List<int>{1,5,8,3,6,2,4,7};
    private Dictionary<string, int> orderList = new Dictionary<string, int>();
    public bool Observe(string objectName, int order, out bool needSendMsg, out string errorID)
    {
        needSendMsg = false;
        errorID = null;

        bool isSpecial = objectName.Equals("飞轮螺栓");

        // 如果已经判断过该零件组是错误的顺序
        if(orderList.ContainsKey(objectName) && orderList[objectName] < 0)
        {
            return false;
        }
            
        // 如果还没有判断过该零件组的拆卸顺序，先初始化
        if(!orderList.ContainsKey(objectName))
        {
            if(isSpecial)
            {
                orderList[objectName] = order;
            }
            else
            {
                orderList[objectName] = 1;
            }
        }

        // 判断是否是期待的顺序
        if(orderList[objectName] != order)
        {
            orderList[objectName] = -1;

            needSendMsg = true;

            switch (objectName)
            {
                case "凸轮轴盖螺栓":
                    errorID = "20102";
                    break;
                case "凸轮轴轴承盖螺栓-进气侧": 
                case "凸轮轴轴承盖螺栓-排气侧":
                case "气缸盖10号螺栓":
                    errorID = "30102";
                    break;
                case "曲轴箱螺栓1长":
                    errorID = "70202";
                    break;
                case "飞轮螺栓":
                    errorID = "70101";
                    break;
            }
            return false;
        }
        else
        {
            if(isSpecial)
            {
                orderList[objectName] = feilunOrder[(feilunOrder.IndexOf(orderList[objectName]) + 1) % feilunOrder.Count];
            }
            else
            {
                orderList[objectName]++;
            }
        }

        return true;
    }
}
