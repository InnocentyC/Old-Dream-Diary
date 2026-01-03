using System.Collections.Generic;
using UnityEngine;
using System;

    [Serializable] // 必须有这个，否则 InteractableItem 的列表里显示不出来
    public class RoomStateEvent
    {
        public RoomState triggerState;

        [Header("进入该状态时执行的行为")]
        public List<StateAction> actions;
    }
