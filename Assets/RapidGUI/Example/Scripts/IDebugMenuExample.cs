﻿using UnityEngine;

namespace RapidGUI.Example
{
    public class IDebugMenuExample : MonoBehaviour, IDebugMenu
    {
        public void DebugMenu()
        {
            GUILayout.Label("IDebugMenu");
        }
    }
}