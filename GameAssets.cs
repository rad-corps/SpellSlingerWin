using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SpellSlingerWindowsPort
{
    class GameAssets
    {
        private readonly Object threadSafeLock = new Object();
        private List<Entity> DrawList;                  //Used to track ALL objects
        public List<Texture2D> GUITextureList;
        public List<Texture2D> TextureList;            //tracks ALL textures from DrawList
        public List<Texture2D> EnemyTextureList;       //tracks ALL textures from DrawList
        public List<Texture2D> SpellTextureList;        //tracks spell textures
        public Texture2D spellTexture;
        private List<Enemy> EnemyList;                  //tracking enemies
        private List<Tower> TowerList;                  //Who knows we might want multi player one day?
        private List<Spell> SpellList;                   //tracks active/current spells
        public List<GUI_Component> GUIList;
        public List<GUI_Component> MenuList;
        public Texture2D leaderboardBG;

        public GameAssets()
        {
            DrawList = new List<Entity>();                                                          //All objects added to DrawList - use this to draw to screen.
            TextureList = new List<Texture2D>();
            EnemyTextureList = new List<Texture2D>();
            SpellTextureList = new List<Texture2D>();
            GUITextureList = new List<Texture2D>();
            EnemyList = new List<Enemy>();
            TowerList = new List<Tower>();
            SpellList = new List<Spell>();
            GUIList = new List<GUI_Component>();
            MenuList = new List<GUI_Component>();            
        }

        public int DrawListCount
        {
            get { return DrawList.Count; }
        }

        public Entity DrawListItem(int index_)
        {
            return DrawList[index_];
        }

        public int EnemyListCount
        {
            get { return EnemyList.Count; }
        }

        public Enemy EnemyListItem(int index_)
        {
            return EnemyList[index_];
        }

        public void EnemyListAdd(Enemy e_)
        {
            lock (threadSafeLock)
            {
                EnemyList.Add(e_);
                DrawList.Add(e_);
            }
        }

        public int TowerListCount
        {
            get { return TowerList.Count; }
        }

        public Tower TowerListItem(int index_)
        {
            return TowerList[index_];
        }

        public void TowerListAdd(Tower e_)
        {
            lock (threadSafeLock)
            {
                TowerList.Add(e_);
                DrawList.Add(e_);
            }
        }

        public int SpellListCount
        {
            get { return SpellList.Count; }
        }

        public Spell SpellListItem(int index_)
        {
            return SpellList[index_];
        }

        public void SpellListAdd(Spell e_)
        {
            lock (threadSafeLock)
            {
                SpellList.Add(e_);
                DrawList.Add(e_);
            }
        }


        //GUI
        public int GUIListCount
        {
            get { return GUIList.Count; }
        }

        public GUI_Component GUIListItem(int index_)
        {
            return GUIList[index_];
        }

        public void GUIListAdd(GUI_Component e_)
        {
            lock (threadSafeLock)
            {
                GUIList.Add(e_);
            }
        }
        //GUI

        //MENU
        public int MenuListCount
        {
            get { return MenuList.Count; }
        }

        public GUI_Component MenuListItem(int index_)
        {
            return MenuList[index_];
        }

        public void MenuListAdd(GUI_Component e_)
        {
            lock (threadSafeLock)
            {
                MenuList.Add(e_);
            }
        }
        //MENU

        //flush draw/enemy/spell/gui
        public void FlushEntities()
        {
            //Flush Spells
            if (SpellList.Count > 0)
            {
                for (int i = 0; i < SpellList.Count; i++)
                {
                    SpellListItem(i).Active = false;
                }
            }
            //Flush enemies
            if (EnemyList.Count > 0)
            {
                for (int i = 0; i < EnemyList.Count; i++)
                {
                    EnemyListItem(i).Active = false;
                }
            }

            //We do not need to flush drawlist here as we'll keep the player & the rest will sync against the spell/enemy inactive flags

            RemoveEntitiesMarkedForDelete();

            //Flush GUIList & MenuList & remove appropriate
            if (GUIList.Count > 0)
            {
                for (int i = 0; i < GUIList.Count; i++)
                {
                    GUIListItem(i).Active = false;
                }
            }
            if (GUIList.Count > 0)
            {
                for (int i = GUIList.Count - 1; i >= 0; i--)
                {
                    if (!GUIList[i].Active)
                    {
                        GUIList.RemoveAt(i);
                    }
                }
            }

            if (MenuList.Count > 0)
            {
                for (int i = 0; i < MenuList.Count; i++)
                {
                    MenuListItem(i).Active = false;
                }
            }
            if (MenuList.Count > 0)
            {
                for (int i = MenuList.Count - 1; i >= 0; i--)
                {
                    if (!MenuList[i].Active)
                    {
                        MenuList.RemoveAt(i);
                    }
                }
            }

        }


        //This method can definitely be tidied up.. 
        //need to look into how we can write the delete loop just once. 
        public void RemoveEntitiesMarkedForDelete()
        {
            lock (threadSafeLock)
            {
                //Remove inactive spells from SpellList
                if (SpellList.Count > 0)
                {
                    for (int i = SpellList.Count - 1; i >= 0; i--)
                    {
                        if (!SpellList[i].Active)
                        {
                            SpellList.RemoveAt(i);
                        }
                    }
                }

                //Remove inactive enemies from EnemyList
                if (EnemyList.Count > 0)
                {
                    for (int i = EnemyList.Count - 1; i >= 0; i--)
                    {
                        if (!EnemyList[i].Active)
                        {
                            EnemyList.RemoveAt(i);
                        }
                    }
                }

                //Remove any inactive items from draw call - iterate in reverse
                if (DrawList.Count > 0)
                {
                    for (int i = DrawList.Count - 1; i >= 0; i--)
                    {
                        if (!DrawList[i].Active)
                        {
                            DrawList.RemoveAt(i);
                        }
                    }
                }


            }
        }
    }
}
