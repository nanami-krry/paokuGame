using System.Collections;
using System.Collections.Generic;

public interface IReusable
{
    //取出
    void OnSpawn();
    //回收
    void OnUnSpawn();
    
}
