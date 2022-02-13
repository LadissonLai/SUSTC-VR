using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAOrderObserver
{
    public Dictionary<string, int> orderList = new Dictionary<string, int>();
    public bool Observe(string objectName, int order)
    {
        if(orderList.ContainsKey(objectName) && orderList[objectName] < 0)
            return false;
            
        if(!orderList.ContainsKey(objectName))
            orderList[objectName] = 1;

        if(orderList[objectName] != order)
        {
            orderList[objectName] = -1;
            return false;
        }
        else
        {
            orderList[objectName]++;
        }
        return true;
    }
}
