using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonogameAndroidProject
{
    class GUI
    {

        Factory objectFactory;
        ViewPort viewPort;

        public GUI(Factory objectFactory_, ViewPort viewPort_)
        {
            objectFactory = objectFactory_;
            viewPort = viewPort_;
        }

        public void GUIIntro()
        {

        }

        public void GUIMenu()
        {
            objectFactory.CreateGUIComponent(GUI_SPRITES.MAIN_MENU_BG, viewPort.ViewPortWidth * .5f, viewPort.ViewPortHeight * .5f, viewPort.ViewPortWidth, viewPort.ViewPortHeight, true, true, 999);
            objectFactory.CreateGUIComponent(GUI_SPRITES.BUTTON_MENU_PLAY, viewPort.ViewPortWidth * .3f, viewPort.ViewPortHeight * .23f, 512, 128, true, true, 998);
            objectFactory.CreateGUIComponent(GUI_SPRITES.BUTTON_MENU_QUIT, viewPort.ViewPortWidth * .44f, viewPort.ViewPortHeight * .8f, 256, 128, true, true, 997);
            objectFactory.CreateGUIComponent(GUI_SPRITES.BUTTON_MENU_LEADERBOARD, viewPort.ViewPortWidth * .92f, viewPort.ViewPortHeight * .745f, 256, 128, true, true, 996);
        }

        public void GUIPlayGame()
        {
            //int padX = 25;
            //int padY = 25;
            //int fill = 10;
            //^Not in demo scope - to be implemented
            //Scaling, individual hotbar buttons, load from spellbook, 50% transparant image (if inactive mark inactive image as active) lulz 

            //For scope of demo this is fine. Spellbook is not currently active  (all objects will be interactive - mouse on arrow will scroll screen etc)
            //Hotbar - Dimensions of actual .png need to be reworked - not final implication of hotbar graphic
            int width = 96;
            int height = 96;
            int pad = 10;           //padding between spell buttons
            int xpad;
            int ypad;
            int distFromX = (height >> 1) + (height >> 2);

            objectFactory.CreateGUIComponent(GUI_SPRITES.HOTBAR_1, (0 * pad) + (0 * (viewPort.ViewPortWidth / 5)) + distFromX, viewPort.ViewPortHeight - distFromX, width, height, true, true, 1);
            objectFactory.CreateGUIComponent(GUI_SPRITES.HOTBAR_2, (1 * pad) + (1 * (viewPort.ViewPortWidth / 5)) + distFromX, viewPort.ViewPortHeight - distFromX, width, height, false, true, 2);
            objectFactory.CreateGUIComponent(GUI_SPRITES.HOTBAR_3, (2 * pad) + (2 * (viewPort.ViewPortWidth / 5)) + distFromX, viewPort.ViewPortHeight - distFromX, width, height, false, true, 3);
            objectFactory.CreateGUIComponent(GUI_SPRITES.HOTBAR_4, (3 * pad) + (3 * (viewPort.ViewPortWidth / 5)) + distFromX, viewPort.ViewPortHeight - distFromX, width, height, false, true, 4);
            objectFactory.CreateGUIComponent(GUI_SPRITES.HOTBAR_5, (4 * pad) + (4 * (viewPort.ViewPortWidth / 5)) + distFromX, viewPort.ViewPortHeight - distFromX, width, height, false, true, 5);
            
            //Spellbook - removed for quit button (double tap to exit game)
            width = 64;
            height = 64;
            pad = 10;
            //objectFactory.CreateGUIComponent(GUI_SPRITES.SPELL_BOOK, viewPort.ViewPortWidth - width * 0.5f - pad, viewPort.ViewPortHeight - height * 0.5f - pad, 64, 64, true, true, 0);
            objectFactory.CreateGUIComponent(GUI_SPRITES.QUIT_BUTTON, viewPort.ViewPortWidth - width * 0.5f - pad, viewPort.ViewPortHeight - height * 0.5f - pad, 64, 64, true, true, (int)GUI_SPRITES.QUIT_BUTTON);

            ////Arrows
            //width = 32;
            //height = 32;
            //objectFactory.CreateGUIComponent(GUI_SPRITES.ARROW_UP, viewPort.ViewPortWidth * 0.5f, height * 0.5f, width, height, true, true, 0);
            //objectFactory.CreateGUIComponent(GUI_SPRITES.ARROW_DOWN, viewPort.ViewPortWidth * 0.5f, viewPort.ViewPortHeight - height * 0.5f, width, height, true, true, 0);
            //objectFactory.CreateGUIComponent(GUI_SPRITES.ARROW_LEFT, width * 0.5f, viewPort.ViewPortHeight * 0.5f, width, height, true, true, 0);
            //objectFactory.CreateGUIComponent(GUI_SPRITES.ARROW_RIGHT, viewPort.ViewPortWidth - width * 0.5f, viewPort.ViewPortHeight * 0.5f, width, height, true, true, 0);

            //Tower - thank you captain obvious
            width = 64;
            height = 128;
            xpad = 40;
            ypad = 80;
            objectFactory.CreateGUIComponent(GUI_SPRITES.GUI_TOWER, width * 0.5f + xpad, height * 0.5f + ypad, width, height, true, true, 0);

            //Overwhelmed Text
            width = 1024;
            height = 64;
            objectFactory.CreateGUIComponent(GUI_SPRITES.OVERWHELMED_TEXT, viewPort.ViewPortWidth * 0.5f, viewPort.ViewPortHeight * 0.3f, width, height, true, false, (int)GUI_SPRITES.OVERWHELMED_TEXT);

        }

        public void GUILeaderboard()
        {
            int width = 64;
            int height = 64;
            int pad = 10;
            objectFactory.CreateGUIComponent(GUI_SPRITES.QUIT_BUTTON, viewPort.ViewPortWidth - width * 0.5f - pad, viewPort.ViewPortHeight - height * 0.5f - pad, 64, 64, true, true, 995);
            objectFactory.CreateGUIComponent(GUI_SPRITES.SEARCH_BUTTON, 300 * 0.5f + pad, viewPort.ViewPortHeight - 90 * 0.5f - pad, 300, 90, true, true, 990);
        }

        public void GUIEnd()
        {

        }


    }
}
