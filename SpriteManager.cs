using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonogameAndroidProject
{
    public enum PLAYER_SPRITES
    {
        TOWER,

        NUM_PLAYER_SPRITES
    }

    public enum GUI_SPRITES
    {
        HOTBAR_1,
        HOTBAR_2,
        HOTBAR_3,
        HOTBAR_4,
        HOTBAR_5,
        SPELL_BOOK,
        ARROW_UP,
        ARROW_DOWN,
        ARROW_LEFT,
        ARROW_RIGHT,
        GUI_TOWER,
        QUIT_BUTTON,
        SEARCH_BUTTON,
        MAIN_MENU_BG,
        END_GAME_BG,
        BUTTON_MENU_PLAY,
        BUTTON_MENU_QUIT,
        BUTTON_MENU_LEADERBOARD,
        OVERWHELMED_TEXT,

        NUM_GUI_SPRITES
    }

    

    class SpriteManager
    {
        public static int playerNumTextures = (int)PLAYER_SPRITES.NUM_PLAYER_SPRITES;
        public static int enemyNumTextures = (int)ENEMY_TYPE.NUM_ENEMY_TYPE;
        public static int spellNumTextures = (int)SPELL_TYPE.NUM_SPELL_TYPE;
        public static int GUINumTextures = (int)GUI_SPRITES.NUM_GUI_SPRITES;
        
        private string[] playerSpriteFileNames = new string[playerNumTextures];
        private string[] enemySpriteFileNames = new string[enemyNumTextures];
        private string[] spellSpriteFileNames = new string[spellNumTextures];
        //private string[] animatedSpellSpriteFileNames = new string[SPELL_SPRITE_ANIMATIONS];
        private string[] GUISpriteFileNames = new string[GUINumTextures];

        public static int SPELL_SPRITE_ANIMATIONS = 59;
        
        ///List Textures here
        public SpriteManager()
        {
            playerSpriteFileNames[(int)PLAYER_SPRITES.TOWER] = "tower128.png";

            enemySpriteFileNames[(int)ENEMY_TYPE.GHOUL] = "enemy1.png";
            enemySpriteFileNames[(int)ENEMY_TYPE.RUNNING_GHOUL] = "enemy2.png";
            enemySpriteFileNames[(int)ENEMY_TYPE.HEAVY_ZOMBIE] = "enemy3.png";
            enemySpriteFileNames[(int)ENEMY_TYPE.SKELETON_KNIGHT] = "enemy4.png";
            enemySpriteFileNames[(int)ENEMY_TYPE.OGRE] = "enemy5.png";
            enemySpriteFileNames[(int)ENEMY_TYPE.WEREWOLF] = "enemy6.png";
            enemySpriteFileNames[(int)ENEMY_TYPE.GREEN_DRAGON] = "enemy7.png";

            //TODO - ADD Different sprites for these spells. 
            spellSpriteFileNames[(int)SPELL_TYPE.FIREBALL] = "spell1.png";
            spellSpriteFileNames[(int)SPELL_TYPE.ICELANCE] = "spell3.png";
            spellSpriteFileNames[(int)SPELL_TYPE.DESPAIR] = "spell4.png";
            spellSpriteFileNames[(int)SPELL_TYPE.LIGHTNING] = "spell2.png";
            spellSpriteFileNames[(int)SPELL_TYPE.RAPTURE] = "spell5.png";

            GUISpriteFileNames[(int)GUI_SPRITES.HOTBAR_1] = "gui_hotbar1.png";
            GUISpriteFileNames[(int)GUI_SPRITES.HOTBAR_2] = "gui_hotbar2.png";
            GUISpriteFileNames[(int)GUI_SPRITES.HOTBAR_3] = "gui_hotbar3.png";
            GUISpriteFileNames[(int)GUI_SPRITES.HOTBAR_4] = "gui_hotbar4.png";
            GUISpriteFileNames[(int)GUI_SPRITES.HOTBAR_5] = "gui_hotbar5.png";
            GUISpriteFileNames[(int)GUI_SPRITES.SPELL_BOOK] = "spellbook.png";
            GUISpriteFileNames[(int)GUI_SPRITES.ARROW_UP] = "arrowUp.png";
            GUISpriteFileNames[(int)GUI_SPRITES.ARROW_DOWN] = "arrowDown.png";
            GUISpriteFileNames[(int)GUI_SPRITES.ARROW_LEFT] = "arrowLeft.png";
            GUISpriteFileNames[(int)GUI_SPRITES.ARROW_RIGHT] = "arrowRight.png";
            GUISpriteFileNames[(int)GUI_SPRITES.GUI_TOWER] = "GUItower.png";
            GUISpriteFileNames[(int)GUI_SPRITES.QUIT_BUTTON] = "quit.png";
            GUISpriteFileNames[(int)GUI_SPRITES.SEARCH_BUTTON] = "search.png";

            GUISpriteFileNames[(int)GUI_SPRITES.MAIN_MENU_BG] = "MainMenu.png";
            GUISpriteFileNames[(int)GUI_SPRITES.END_GAME_BG] = "MainMenu.png";
            GUISpriteFileNames[(int)GUI_SPRITES.BUTTON_MENU_PLAY] = "button_play.png";
            GUISpriteFileNames[(int)GUI_SPRITES.BUTTON_MENU_QUIT] = "button_menu_quit.png";
            GUISpriteFileNames[(int)GUI_SPRITES.BUTTON_MENU_LEADERBOARD] = "button_lb.png";
            GUISpriteFileNames[(int)GUI_SPRITES.OVERWHELMED_TEXT] = "overwhelmed_text.png";
        }

        public string GetPlayerSpriteFileName(int type)
        {
            return playerSpriteFileNames[type];
        }

        public string GetEnemySpriteFileName(int type)
        {
            return enemySpriteFileNames[type];
        }

        public string GetSpellSpriteFileName(int type)
        {
            return spellSpriteFileNames[type];
        }

        public string GetGUISpriteFileName(int type)
        {
            return GUISpriteFileNames[type];
        }
    }
}

