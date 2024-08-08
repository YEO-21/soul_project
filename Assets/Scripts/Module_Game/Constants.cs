using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameModule
{ 
    public static class Constants
    {
        /// <summary>
        /// ȭ�� ũ�� ����
        /// </summary>
        public static Vector2Int SCREEN_SIZE => new Vector2Int(1920, 1080);

        public const float HUD_INVISIBLE_DISTANCE = 5.0f;

        /// <summary>
        /// Tags...
        /// </summary>
        public const string TAG_PLAYER = "Player";

        public const string INPUTMODE_GAME = "GameMode";
        public const string INPUTMODE_UI = "UIMode";

        /// <summary>
        /// �÷��̾� �⺻ ���� �ڵ�
        /// </summary>
        public const string PLAYER_ATTACKCODE_NORMAL        = "0001";
        public const string PLAYER_ATTACKCODE_NORMAL1ST     = "0001";
        public const string PLAYER_ATTACKCODE_NORMAL2ND     = "0002";
        public const string PLAYER_ATTACKCODE_NORMAL3RD     = "0003";

        /// <summary>
        /// ��ȣ�ۿ� ī�޶� Priority 
        /// </summary>
        public const int INTERACT_CAMEAR_ENABLE_PRIORITY = 20;
        public const int INTERACT_CAMEAR_DISABLE_PRIORITY = 0;
    }
}
