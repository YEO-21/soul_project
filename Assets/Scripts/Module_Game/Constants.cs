using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameModule
{ 
    public static class Constants
    {
        public const string INPUTMODE_GAME = "GameMode";
        public const string INPUTMODE_UI = "UIMode";

        /// <summary>
        /// Tags
        /// </summary>
        public const string TAG_PLAYER = "Player";




        /// <summary>
        /// �÷��̾� �⺻ ���� �ڵ�
        /// </summary>
        public const string PLAYER_ATTACKCODE_NORMAL        = "0001";
        public const string PLAYER_ATTACKCODE_NORMAL1ST     = "0001";
        public const string PLAYER_ATTACKCODE_NORMAL2ND     = "0002";
        public const string PLAYER_ATTACKCODE_NORMAL3RD     = "0003";

        /// <summary>
        /// ��ȣ�ۿ� ī�޶� Priorty
        /// </summary>
        public const int INTERACT_CAMERA_ENABLE_PRIORITY = 20;
        public const int INTERACT_CAMERA_DISABLE_PRIORITY = 0;
    }
}
