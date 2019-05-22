﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RapidGUI
{
    public class WindowLauncher : IDisposable
    {
        #region Type Define

        public class FuncData
        {
            public Func<bool> checkEnableFunc;
            public Action drawFunc;

            public void OnGUI()
            {
                if (checkEnableFunc())
                {
                    drawFunc();
                    GUI.DragWindow();
                }
            }
        }

        #endregion

        public string name;
        public bool isOpen;
        public List<FuncData> funcDatas = new List<FuncData>();

        public bool isEnable => funcDatas.Any(data => data.checkEnableFunc());

        Rect rect;

        public WindowLauncher(string name)
        {
            this.name = name;
        }

        public void Dispose()
        {
            WindowLauncherManager.Instance.Remove(this);
        }



        public void Add(Action drawFunc) => Add(() => true, drawFunc);


        public void Add(Func<bool> checkEnableFunc, Action drawFunc)
        {
            funcDatas.Add(new FuncData()
            {
                checkEnableFunc = checkEnableFunc,
                drawFunc = drawFunc
            });
        }


        public void OnGUI()
        {
            if ( isEnable )
            {
                isOpen = GUILayout.Toggle(isOpen, "❏ " + name, Style.toggle);
                WindowLauncherManager.Instance.Add(this);
            }
        }

        public void OnGUIWindow()
        {
            if (isOpen && isEnable)
            {
                rect = RGUI.ResizableWindow(GetHashCode(), rect,
                    (id) =>
                    {
                        funcDatas.ForEach(data => data.OnGUI());
                        GUI.DragWindow();
                    }
                    , name);
            }
        }


        #region Style

        public static class Style
        {
            public static readonly GUIStyle toggle;
            const int underLine = 3;


            static Style()
            {
                Color onColor = new Color(0.25f, 0.4f, 0.98f, 0.9f);

                toggle = CreateToggle(onColor);
                toggle.name = "launcher_unit_toggle";
            }

            static GUIStyle CreateToggle(Color onColor)
            {
                var style = new GUIStyle(GUI.skin.button);
                style.alignment = TextAnchor.MiddleLeft;

                //style.padding.left = 15;
                style.border = new RectOffset(0, 0, 1, underLine+1);

                var bgColorHover = Vector4.one * 0.5f;
                var bgColorActive = Vector4.one * 0.7f;

                style.onNormal.background = CreateToggleOnTex(onColor, Color.clear);
                style.onHover.background = CreateToggleOnTex(onColor, bgColorHover);
                style.onActive.background = CreateToggleOnTex(onColor * 1.5f, bgColorActive);

                style.normal.background = CreateTex(Color.clear);
                style.hover.background = CreateTex(bgColorHover);
                style.active.background = CreateTex(bgColorActive);

                return style;
            }

            static Texture2D CreateToggleOnTex(Color col, Color bg)
            {
                var tex = new Texture2D(1, underLine + 3);

                for (var x = 0; x < tex.width; ++x)
                {
                    for (var y = 0; y < tex.height; ++y)
                    {
                        var c = (y < underLine) ? col : bg;
                        tex.SetPixel(x, y, c);
                    }
                }

                tex.Apply();

                return tex;
            }

            static Texture2D CreateTex(Color col)
            {
                var tex = new Texture2D(1, 1);
                tex.SetPixel(0, 0, col);
                tex.Apply();

                return tex;
            }
        }

        #endregion
    }
}