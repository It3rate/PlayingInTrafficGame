#define LOCALGAME

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Smuck.Screens;
using Microsoft.Xna.Framework.GamerServices;
using V2DRuntime.Network;
using Microsoft.Xna.Framework.Net;
using DDW.Display;
using Smuck.Components;
using Smuck.Enums;
using System.Reflection;

namespace Smuck
{
    public class SmuckGame : V2DGame
    {
        public static List<HighScoreElement> highScores;
        public static List<Level> Levels;

		public SmuckGame():base()
		{
			Components.Add(new GamerServicesComponent(this));
		}

        private static GameOverlay gameOverlay;
        public static GameOverlay GameOverlay
        {
            get
            {
                if (gameOverlay == null)
                {
                    SymbolImport si = new SymbolImport("titleScreen", "gameOverlay");
                    gameOverlay = new GameOverlay(si);
                    gameOverlay.Visible = false;
                    stage.AddScreen(gameOverlay);
                }
                return gameOverlay;
            }
        }
		protected override void LoadContent()
		{
			base.LoadContent();
			FontManager.Instance.AddFont("Arial Black", V2DGame.contentManager.Load<SpriteFont>(@"ArialBlack"));
            stage.InitializeAudio(@"Content\audio\smuck.xgs", @"Content\audio\Wave Bank.xwb", @"Content\audio\Sound Bank.xsb");
		}

        Screen titleScreen;
        protected override void CreateScreens()
        {
            SymbolImport si = new SymbolImport("titleScreen", "entryScreen");
            titleScreen = new StartScreen(si);
            stage.AddScreen(titleScreen);

            AddLevel("twoLaneScreen", typeof(TwoLaneScreen));
            AddLevel("wideBoulevardScreen", typeof(WideBoulevardScreen));
            AddLevel("twoBoulevardScreen", typeof(TwoBoulevardScreen));
            AddLevel("crosswalkScreen", typeof(CrosswalkScreen));
            AddLevel("housesScreen", typeof(HousesScreen));
            AddLevel("twoTrainTwoRestScreen", typeof(TwoTrainTwoRestScreen));
            AddLevel("twoTrainScreen", typeof(TwoTrainScreen));
            AddLevel("allCarsScreen", typeof(AllCarsScreen));
            AddLevel("twoCanaltwoBoulScreen", typeof(TwoCanalTwoBoulevardScreen));
            AddLevel("twoCanalScreen", typeof(TwoCanalScreen));
            AddLevel("twoCanalTwoTrainScreen", typeof(TwoCanalTwoTrainScreen));
            AddLevel("allTrainScreen", typeof(AllTrainScreen));
            AddLevel("laneChangeScreen", typeof(LaneChangeScreen));
            AddLevel("allWaterScreen", typeof(AllWaterScreen));
            AddLevel("spaceMediumScreen", typeof(SpaceMediumScreen));
            AddLevel("steamRollerScreen", typeof(SteamRollerScreen));
        }
        private void AddLevel(string levelName, Type levelType)
        {
            SymbolImport si = new SymbolImport("screens", levelName);
            ConstructorInfo ci = levelType.GetConstructor(new Type[]{si.GetType()});
            object o = ci.Invoke(new object[] { si });
            stage.AddScreen((BaseScreen)o);
        }
        protected override void Initialize()
        {
			base.Initialize();
			this.isFullScreen = false;

			NetworkManager.Instance.OnNewGamer += new NetworkManager.NewGamerDelegate(NewGamerHandler);
			NetworkManager.Instance.OnGamerLeft += new NetworkManager.GamerLeftDelegate(GamerLeftHandler);
        }

        public override void AddingScreen(Screen screen)
        {
            base.AddingScreen(screen);
            GameOverlay.Visible = false;
        }
        public override void RemovingScreen(Screen screen)
        {
            base.RemovingScreen(screen);
            if (screen is BaseScreen && screen.Contains(gameOverlay))
            {
                gameOverlay.Visible = false;
                ((BaseScreen)screen).gameOverlay = null;
                
            }
        }

		void NewGamerHandler(Microsoft.Xna.Framework.Net.NetworkGamer gamer, int gamerIndex)
		{
			AddGamer(gamer, gamerIndex);
		}
		void GamerLeftHandler(Microsoft.Xna.Framework.Net.NetworkGamer gamer, int gamerIndex)
		{
			RemoveGamer(gamer);
		}

		public override void AddGamer(NetworkGamer gamer, int gamerIndex)
		{
			base.AddGamer(gamer, gamerIndex);
			if (gamer.IsLocal)
			{
				gamer.IsReady = true;
			}
			// todo: possible on the fly additions
			//CreatePlayer(gamer, gamerIndex);
		}
		public override void RemoveGamer(NetworkGamer gamer)
		{
			base.RemoveGamer(gamer);
		}

        public List<HighScoreElement> GetHighScores()
        {
            LoadHighScores();
            highScores.Sort();
            return highScores;
        }
        public void SetHighScores(List<HighScoreElement> hs)
        {
            highScores = hs;
            SaveHighScores();
        }
        protected void LoadHighScores()
        {
            if (highScores == null)
            {
                highScores = new List<HighScoreElement> { 
                    new HighScoreElement("aaa", 67, true),
                    new HighScoreElement( "bbb", 37, true),
                    new HighScoreElement( "avvvaa", 38, true),
                    new HighScoreElement( "ddssa", 127, true),
                    new HighScoreElement( "erwwer", 142, true),
                    new HighScoreElement( "uukjlkh", 97, true),
                    new HighScoreElement( "yuiyu", 34, true),
                    new HighScoreElement( "nmzxczx", 55, true), };
            }
        }
        protected void SaveHighScores()
        {
            //highScores = new int[]{67,34,23,98,125,25};
        }

		public override void ExitToMainMenu()
		{
			base.ExitToMainMenu();
			stage.SetScreen("entryScreen");
		}
        protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
        protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
		{
			//GraphicsDevice.RenderState.DepthBufferEnable = true;
            base.Draw(gameTime);
        }
    }
}