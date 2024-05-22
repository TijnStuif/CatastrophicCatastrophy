using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.Helpers;
using Blok3Game.GameObjects;
using Blok3Game.SpriteGameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace Blok3Game.GameStates
{
    public class GameState : GameObjectList
    {
        //all lists, objects and variables at the start of the game for the gamestate are created here
        private List<PlayerBullet> playerBulletList;
        private List<EnemyBullet> enemyBulletList;
        private List<ShootingEnemy> shootingEnemyList;
        private List<FastEnemy> fastEnemyList;
        private List<Currency> currencyList;
        public List<GameObject> toRemoveList;
        private List<Box> boxlist;
        public Player player;
        public Crosshair crosshair;
        public CatGun catGun;
        public Box box;
        public YellowBox yellowBox;
        public PurpleBox purpleBox;
        public ShootingEnemy shootingEnemy;
        public FastEnemy fastEnemy;
        public DashIndicator dashIndicator;
        public WaveIndicator waveIndicator;
        public TextGameObject playerHealth;
        public int WaveCounter = 1;
        public int ChosenEnemy = 0;
        public int FramesPerSecond = 60;
        public int WaveIndicatorShowTime = -20;
        private bool NewWave = true;
        private SpriteGameObject background;
        public TextGameObject playerCurrency;
        public int EnemyShoot = 0;
        public int PlayerShootCooldown = 0;
        public int PlayerAttackTimes = 0;
        private bool pickedUpPurple = false;
        private bool pickedUpYellow = false;

        public GameState() : base()
        {
            CreateBackground();
            //Aanmaken van een nieuwe lijst
            shootingEnemyList = new List<ShootingEnemy>();
            playerBulletList = new List<PlayerBullet>();
            enemyBulletList = new List<EnemyBullet>();
            currencyList = new List<Currency>();
            fastEnemyList = new List<FastEnemy>();
            boxlist = new List<Box>();
            toRemoveList = new List<GameObject>();

            //Spawn Enemies for the first wave
            SpawnStandardEnemies();

            player = new Player(3, 5, new Vector2((GameEnvironment.Screen.X / 2) - (90 / 2), (GameEnvironment.Screen.Y / 2) - (90 / 2)))
            {
                Gamestate = this
            };
            Add(player);

            yellowBox = new YellowBox(new Vector2(10, 10));
            boxlist.Add(yellowBox);

            Add(yellowBox);

            purpleBox = new PurpleBox(new Vector2(200, 200));
            boxlist.Add(purpleBox);

            Add(purpleBox);

            crosshair = new Crosshair(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
            Add(crosshair);

            catGun = new CatGun(player, crosshair, new Vector2(10, 10));
            Add(catGun);

            playerHealth = new TextGameObject("Fonts/SpriteFont@20px", 1);
            Add(playerHealth);
            playerHealth.Text = $"{player.HitPoints}";
            playerHealth.Color = new(255, 255, 255);
            playerHealth.Parent = player;

            playerCurrency = new TextGameObject("Fonts/SpriteFont@20px", 1);
            Add(playerCurrency);
            playerCurrency.Text = $"you collected {player.currencyCounter} currency";
            playerHealth.Color = new(255, 255, 255);
            playerCurrency.Position = new Vector2(0, 20);

            dashIndicator = new DashIndicator(Vector2.Zero);
            Add(dashIndicator);
            dashIndicator.Parent = player;

            ShowWaveIndicator();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

<<<<<<< game/GameStates/GameState.cs
            ShowWaveIndicator();

            switch (WaveCounter)
            {
                case 1: //Wave 1
                    if (shootingEnemyList.Count == 0)
                    {
                        WaveCounter++;
                        NewWave = true;
                        WaveIndicatorShowTime = 0;
                        ResetBullets();
                        SpawnStandardEnemies();
                    }
                    break;
                case 2: //Wave 2
                    if (shootingEnemyList.Count == 0)
                    {
                        WaveCounter++;
                        NewWave = true;
                        WaveIndicatorShowTime = 0;
                        ResetBullets();
                        SpawnStandardEnemies();
                    }
                    break;
                case 3: //Wave 3
                    if (shootingEnemyList.Count == 0)
                    {
                        ResetBullets();
                        GameEnvironment.GameStateManager.SwitchToState("WIN_SCREEN_STATE");
                    }
                    break;
            }

            //switches to lose screen if player's HP falls below 0
            if (player.HitPoints <= 0)
            {
                Retry();
                GameEnvironment.GameStateManager.SwitchToState("LOSE_SCREEN_STATE");
            }

            
            if (PlayerShootCooldown != 0)
            {
                PlayerShootCooldown--;
            }

            //Loop door de lijst met enemies
            foreach (var enemy in shootingEnemyList)
            {
                enemy.EnemySeeking(player.Position);

                if (enemy.EnemyShootCooldown >= 120)
                {
                    EnemyShoots(enemy);
                    enemy.EnemyShootCooldown = 0;
                }
                enemy.EnemyShootCooldown++;

                player.HandleCollision(enemy);
                foreach (var playerBullet in playerBulletList)
                {
                    if (playerBullet.CheckForEnemyCollision(enemy))
                    {
                        Currency currency = new(enemy.Position + new Vector2(enemy.Width / 2, enemy.Height / 2))
                        {
                            Scale = 2
                        };
                        currencyList.Add(currency);
                        Add(currency);
                        toRemoveList.Add(enemy);
                        toRemoveList.Add(playerBullet);
                    }

                }
            }

            foreach (Box box in boxlist)
            {
                if (player.CheckForPlayerCollision(box))
                {
                    if (box is YellowBox yellowBox)
                    {
                        pickedUpYellow = true;
                    }
                    if (box is PurpleBox purpleBox)
                    {
                        pickedUpPurple = true;
                    }
                    toRemoveList.Add(box);
                }
            }

            foreach (var enemyBullet in enemyBulletList)
            {
                player.HandleCollision(enemyBullet);
            }

            foreach (var currency in currencyList)
            {
                player.HandleCollision(currency);
            }

            //if-statement that flashes red colouring over the player to indicate that they have been hit, and are currently invulnerable
            if (player.InvulnerabilityCooldown >= 0)
            {
                if (player.InvulnerabilityCooldown % (FramesPerSecond / 2) > (FramesPerSecond / 4))
                {
                    player.Shade = new Color(255, 0, 0);
                }
                if (player.InvulnerabilityCooldown % (FramesPerSecond / 2) < (FramesPerSecond / 4))
                {
                    player.Shade = new Color(255, 255, 255);
                }
            }
            //removes all objects that are put in the toRemoveList. We use this because we can't remove items from a list while using a foreach-loop on it
            foreach (var gameObject in toRemoveList)
            {
                if (gameObject is PlayerBullet playerBullet)
                {
                    playerBulletList.Remove(playerBullet);
                }
                if (gameObject is ShootingEnemy shootingEnemy)
                {
                    shootingEnemyList.Remove(shootingEnemy);
                }
                if (gameObject is EnemyBullet enemyBullet)
                {
                    enemyBulletList.Remove(enemyBullet);
                }
                Remove(gameObject);
                }
                if (gameObject is FastEnemy fastEnemy)
                {
                    fastEnemyList.Remove(fastEnemy);
                }
                if (gameObject is Currency currency)
                {
                    currencyList.Remove(currency);
                }
                if (gameObject is Box box)
                {
                    boxlist.Remove(box);
                }
                Remove(gameObject);
            }
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            if (inputHelper.MouseLeftButtonPressed && PlayerShootCooldown == 0)
            {
                PlayerShoot(inputHelper.MousePosition.X, inputHelper.MousePosition.Y);
            }
        }

        private void SpawnStandardEnemies()
        {
            Random random = new Random();

            int swap = 0;
            //For-loop om meerdere enemies aan te maken
            for (int i = 0; i < 10 * WaveCounter; i++)
            {
                int XPosition, YPosition;

                //Willekeurige posities waar de enemies spawnen
                XPosition = random.Next(0 - 250, GameEnvironment.Screen.X + 650);
                YPosition = random.Next(0 - 250, GameEnvironment.Screen.Y + 650);

                //Do-While loop die ervoor zorgt dat de enemies aan de buiten randen spawnen 
                //De swap variabele zorgt ervoor dat de enemies evenredig worden verdeel aan alle kanten
                do
                {
                    if (swap % 2 == 0)
                    {
                        XPosition = random.Next(0 - 250, GameEnvironment.Screen.X + 650);
                        swap++;
                    }
                    else
                    {
                        YPosition = random.Next(0 - 250, GameEnvironment.Screen.Y + 650);
                        swap++;
                    }
                } while (XPosition >= 0 - 150 && XPosition <= GameEnvironment.Screen.X + 150 &&
                         YPosition >= 0 - 150 && YPosition <= GameEnvironment.Screen.Y + 150);

                //Aanmaken van de enemies
                shootingEnemy = new ShootingEnemy(1, 1, new Vector2(XPosition, YPosition));
                shootingEnemyList.Add(shootingEnemy);
                Add(shootingEnemy);

            }
        }

        private void SpawnFastEnemies()
        {
            Random random = new Random();

            int swap = 0;
            //For-loop om meerdere enemies aan te maken
            for (int i = 0; i < 10 * WaveCounter; i++)
            {
                int XPosition, YPosition;

                //Willekeurige posities waar de enemies spawnen
                XPosition = random.Next(0 - 100, GameEnvironment.Screen.X + 500);
                YPosition = random.Next(0 - 100, GameEnvironment.Screen.Y + 500);


                //Do-While loop die ervoor zorgt dat de enemies aan de buiten randen spawnen 
                //De swap variabele zorgt ervoor dat de enemies evenredig worden verdeel aan alle kanten
                do
                {
                    if (swap % 2 == 0)
                    {
                        XPosition = random.Next(0 - 100, GameEnvironment.Screen.X + 500);
                        swap++;
                    }
                    else
                    {
                        YPosition = random.Next(0 - 100, GameEnvironment.Screen.Y + 500);
                        swap++;
                    }

                } while (XPosition >= 0 && XPosition <= GameEnvironment.Screen.X &&
                         YPosition >= 0 && YPosition <= GameEnvironment.Screen.Y);

                //Aanmaken van de enemies
                fastEnemy = new FastEnemy(1, 4, new Vector2(XPosition, YPosition));
                fastEnemyList.Add(fastEnemy);
                Add(fastEnemy);
            }
        }

        private void PlayerShoot(float MousePositionX, float MousePositionY)
        {
            float ShootPositionX = player.Position.X + player.Width / 2;
            float ShootPositionY = player.Position.Y + player.Height / 2;
            double bulletAngle = Math.Atan2(MousePositionY - ShootPositionY, MousePositionX - ShootPositionX);

            if (pickedUpPurple)
            {
                for (int i = -1; i < 2; i++)
                {
                    PlayerBullet playerBullet = new(new Vector2(ShootPositionX, ShootPositionY), bulletAngle - 0.5f * i, 18);
                    playerBulletList.Add(playerBullet);
                    Add(playerBullet);
                }
            }
            else
            {
                PlayerBullet playerBullet = new(new Vector2(ShootPositionX, ShootPositionY), bulletAngle, 18);
                playerBulletList.Add(playerBullet);
                Add(playerBullet);
            }

            if (pickedUpYellow)
            {
                PlayerShootCooldown = 5;
            }
            else
            {
                PlayerShootCooldown = 8;
            }
        }

        private void EnemyShoots(ShootingEnemy shootingEnemy)
        {
            float ShootPositionX = shootingEnemy.Position.X + shootingEnemy.Width / 2;
            float ShootPositionY = shootingEnemy.Position.Y + shootingEnemy.Height / 2;
            double bulletAngle = Math.Atan2((player.Position.Y + player.Height / 2) - ShootPositionY, (player.Position.X + player.Width / 2) - ShootPositionX);

            EnemyBullet enemyBullet = new(new Vector2(ShootPositionX, ShootPositionY), bulletAngle, 15);

            enemyBulletList.Add(enemyBullet);
            Add(enemyBullet);
        }

        private void ShowWaveIndicator()
        {
            if (WaveIndicatorShowTime == 0)
            {
                waveIndicator = new WaveIndicator(new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 2), WaveCounter - 1);
                Add(waveIndicator);

                WaveIndicatorShowTime++;
                waveIndicator.Sprite.SheetIndex = WaveCounter - 1;
            }

            if (NewWave && WaveIndicatorShowTime <= 120)
            {
                WaveIndicatorShowTime++;
            }
            else
            {
                NewWave = false;
                toRemoveList.Add(waveIndicator);
            }
        }

        private void CreateBackground()
        {
            background = new SpriteGameObject("Images/UI/Background/woodFloorBackground", -1, "background")
            {
                Scale = 2.1f,
            };

            //use the width and height of the background to position it in the center of the screen
            background.Position = new Vector2((GameEnvironment.Screen.X / 2) - (background.Width / 2), 0);

            Add(background);
        }


        public void Retry()
        {
            ResetBullets();
            ResetEnemies();
            WaveCounter = 1;
            player.InvulnerabilityCooldown = 0;
            player.HitPoints = player.BaseHitPoints;
            playerHealth.Text = $"{player.HitPoints}";
            waveIndicator.Sprite.SheetIndex = 0;
        }

        private void ResetBullets()
        {
            foreach (PlayerBullet playerBullet in playerBulletList)
            {
                toRemoveList.Add(playerBullet);
            }
            foreach (EnemyBullet enemyBullet in enemyBulletList)
            {
                toRemoveList.Add(enemyBullet);
            }
        }
        private void ResetEnemies()
        {
            foreach (ShootingEnemy shootingEnemy in shootingEnemyList)
            {
                toRemoveList.Add(shootingEnemy);
            }
        }
    }
}


