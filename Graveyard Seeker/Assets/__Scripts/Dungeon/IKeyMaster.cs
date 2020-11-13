using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKeyMaster
{
    int keyCount { get; set; } // количество ключей
    int GetFacing(); // реализован в классе Player
}
