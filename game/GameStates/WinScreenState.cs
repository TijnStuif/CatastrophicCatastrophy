using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.UI;
using Microsoft.Xna.Framework;

namespace Blok3Game.GameStates
{
    public class WinScreenState : MenuItem
    {
        public WinScreenState() : base()
        {
            CreateButtons();
            CreateTitle();
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void CreateButtons()
        {
            CreateButton(new Vector2(GameEnvironment.Screen.X / 2 - ButtonOffSet.X, GameEnvironment.Screen.Y / 2), "MAIN MENU", OnButtonMainMenuClicked);
        }

        private void OnButtonMainMenuClicked(UIElement element)
        {
            GameEnvironment.AssetManager.AudioManager.PlaySoundEffect("button_agree");
            nextScreenName = "MAIN_MENU_STATE";
            ButtonClicked();
        }

        private void CreateTitle()
        {
            SpriteGameObject victoryText = new ("Images/UI/victoryText", 0, "victory")
            {
                 Scale = 1,
             };

             //use the width and height of the title to position it in the center of the screen
             victoryText.Position = new Vector2((GameEnvironment.Screen.X / 2) - (victoryText.Width / 2), GameEnvironment.Screen.Y / 7);

        Add(victoryText);
        }
    }
}
