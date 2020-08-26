using Opdracht7MaakEenYahtzeespel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Opdracht8Boter_Kaas_Eieren
{
    public partial class Form1 : Form
    {
        private const int percentageTotal = 100;

        private const string exceptionGameBoardButtonGivenPositionsImpossible = "the given position to a game board button doesn't exist";
        private const string exceptionBoardFillingNotSetUpCorrectedly = "the board filling is not set up correctly wtih the correct width and/or height";
        private const string exceptionGameBoardButtonsNotSetUpCorrectedly = "the game board buttons are not set up correctly with the correct width and/or height";

        private const string messageBoxStringPositionAlreadyTaken = "that position is already taken and can't be taken again";
        private const string messageBoxWinsMessageString = " wins";
        private const string messageBoxStatemateMessageString = "nether player wins";
        private const string currentPlayerText = "current player: ";
        private const string playerOne = "X";
        private const string playerTwo = "O";

        private const int gameBoardWdithSpaces = 3;
        private const int gameBoardHeightSpaces = 3;
        private const int labelHeightPercentage = 10;
        private const float widthFontCorrectorCurrentPlayerIndicator = 0.08F;
        private const float heightFontCorrector = 0.5F;
        private const float fontCorrectorGameBoardButton = 0.5F;

        private Label currentPlayerIndicator;
        private Button[,] gameBoardButtons;

        private string[,] boardFilling;
        private bool playerOneTurn;

        private int labelHeight;
        private int widthStartPosition;
        private int heightStartPosition;
        private int gameBoardSpaceSize;
        private float currentPlayerIndicatorFontSize;
        private float gameBoardButtonFontSize;

        public Form1()
        {
            InitializeComponent();

            playerOneTurn = true;
            
            SetupLocationVariables();
            SetupCurrentPlayerIndicator();
            SetupBoardFilling();
            SetupGameBoardButtons();
        }
        private void SetupLocationVariables()
        {
            labelHeight = (int)(1.0 * ClientRectangle.Height / percentageTotal * labelHeightPercentage);

            int widthSpace = ClientRectangle.Width / gameBoardWdithSpaces;
            int heightSpace = (ClientRectangle.Height-labelHeight) / gameBoardHeightSpaces;
            gameBoardSpaceSize = Math.Min(widthSpace, heightSpace);

            int boardCenterWidth = ClientRectangle.Width / 2;
            int boardCenterHeight = (ClientRectangle.Height - labelHeight) / 2 + labelHeight;

            widthStartPosition = (int)(boardCenterWidth - gameBoardSpaceSize * (gameBoardWdithSpaces / 2.0));
            heightStartPosition = (int)(boardCenterHeight - gameBoardSpaceSize * (gameBoardHeightSpaces / 2.0));

            float fontSizeWidthDetermained = ClientRectangle.Width * widthFontCorrectorCurrentPlayerIndicator;
            float fontSizeHeightDetermained = labelHeight * heightFontCorrector;
            currentPlayerIndicatorFontSize = Math.Min(fontSizeWidthDetermained, fontSizeHeightDetermained);
            gameBoardButtonFontSize = gameBoardSpaceSize * fontCorrectorGameBoardButton;
        }
        private void SetupCurrentPlayerIndicator()
        {currentPlayerIndicator = new Label();
            currentPlayerIndicator.Width = ClientRectangle.Width;
            currentPlayerIndicator.Height = labelHeight;
            currentPlayerIndicator.Location = new Point(0, 0);
            currentPlayerIndicator.Font = new Font(currentPlayerIndicator.Font.FontFamily, currentPlayerIndicatorFontSize, currentPlayerIndicator.Font.Style);
            Controls.Add(currentPlayerIndicator);
            UpdateCurrentPlayerIndicator();
        }
        private void SetupBoardFilling()
        {
            boardFilling = new string[gameBoardWdithSpaces, gameBoardHeightSpaces];
        }
        private void SetupGameBoardButtons()
        {
            gameBoardButtons = new Button[gameBoardWdithSpaces, gameBoardHeightSpaces];
            for(int i = 0 ; i < gameBoardWdithSpaces; i++)
            {
                for (int o = 0; o < gameBoardHeightSpaces; o++)
                {
                    CreateGameBoardButton(i, o);
                }
            }
        }
        private void CreateGameBoardButton(int buttonPositionX, int buttonPositionY)
        {
            if(gameBoardButtons.GetLength(0) == gameBoardWdithSpaces && gameBoardButtons.GetLength(1)== gameBoardHeightSpaces)
            {
                if(buttonPositionX < gameBoardWdithSpaces && buttonPositionY < gameBoardButtons.GetLength(1))
                {
                    gameBoardButtons[buttonPositionX, buttonPositionY] = new Button();
                    gameBoardButtons[buttonPositionX, buttonPositionY].Width = gameBoardSpaceSize;
                    gameBoardButtons[buttonPositionX, buttonPositionY].Height = gameBoardSpaceSize;
                    gameBoardButtons[buttonPositionX, buttonPositionY].Location = new Point(widthStartPosition + buttonPositionX * gameBoardSpaceSize, heightStartPosition + buttonPositionY * gameBoardSpaceSize);
                    gameBoardButtons[buttonPositionX, buttonPositionY].Font = new Font(gameBoardButtons[buttonPositionX, buttonPositionY].Font.FontFamily, gameBoardButtonFontSize, gameBoardButtons[buttonPositionX, buttonPositionY].Font.Style);
                    Action< object, EventArgs> buttonAction = (object sender, EventArgs e) =>
                    {
                        if (gameBoardButtons.GetLength(0) == gameBoardWdithSpaces && gameBoardButtons.GetLength(1) == gameBoardHeightSpaces)
                        {
                            if (boardFilling.GetLength(0) == gameBoardWdithSpaces && boardFilling.GetLength(1) == gameBoardHeightSpaces)
                            {
                                if (buttonPositionX < gameBoardWdithSpaces && buttonPositionY < gameBoardButtons.GetLength(1))
                                    {
                                    if(boardFilling[buttonPositionX, buttonPositionY] == null)
                                    {
                                        string activePlayer;
                                        if (playerOneTurn)
                                        {
                                            activePlayer = playerOne;
                                        }
                                        else
                                        {
                                            activePlayer = playerTwo;
                                        }
                                        gameBoardButtons[buttonPositionX, buttonPositionY].Text = activePlayer;
                                        boardFilling[buttonPositionX, buttonPositionY] = activePlayer;
                                        playerOneTurn = !playerOneTurn;
                                        CheckIfEndGame();
                                        UpdateCurrentPlayerIndicator();
                                    }
                                    else
                                    {
                                        MessageBox.Show(messageBoxStringPositionAlreadyTaken);
                                    }
                                }
                                else
                                {
                                    throw new Exception(exceptionGameBoardButtonGivenPositionsImpossible);
                                }
                            }
                            else
                            {
                                throw new Exception(exceptionGameBoardButtonsNotSetUpCorrectedly);
                            }
                        }
                        else
                        {
                            throw new Exception(exceptionBoardFillingNotSetUpCorrectedly);
                        }
                    };
                    gameBoardButtons[buttonPositionX, buttonPositionY].Click += new EventHandler(buttonAction);
                    Controls.Add(gameBoardButtons[buttonPositionX, buttonPositionY]);
                }
                else
                {
                    throw new Exception(exceptionGameBoardButtonGivenPositionsImpossible);
                }
            }
            else
            {
                throw new Exception(exceptionGameBoardButtonsNotSetUpCorrectedly);
            }
        }

        private void UpdateCurrentPlayerIndicator()
        {
            string player;
            if (playerOneTurn)
            {
                player = playerOne;
            }
            else
            {
                player = playerTwo;
            }
            currentPlayerIndicator.Text = currentPlayerText + player;
        }

        private void CheckIfEndGame()
        {
            if (boardFilling.GetLength(0) == gameBoardWdithSpaces && boardFilling.GetLength(1) == gameBoardHeightSpaces)
            {
                bool someoneCanWin = false;
                for(int x = 0; x < gameBoardWdithSpaces; x++)
                {
                    for (int y = 0; y < gameBoardHeightSpaces; y++)
                    {
                        string toTestAgainst = boardFilling[x, y];

                        bool startingWithPlayer = toTestAgainst != null;
                        string ifNullContainsCanWinByIncreaseX = null;
                        string ifNullContainsCanWinByIncreaseY = null;
                        string ifNullContainsCanWinByIncreaseXY = null;
                        string ifNullContainsCanWinByDecreaseXIncreaseY = null;

                        bool canIncreaseX = x + (gameBoardWdithSpaces - 1) < gameBoardWdithSpaces;
                        bool canDecreaseX = x - (gameBoardWdithSpaces - 1) >= 0;
                        bool canIncreaseY = y + (gameBoardHeightSpaces - 1) < gameBoardHeightSpaces;
                        //bool canDecreaseY = y - (gameBoardHeightSpaces - 1) >= 0;
                            

                        bool winsByIncreaseX = canIncreaseX && startingWithPlayer;
                        bool canWinByIncreaseX = canIncreaseX;
                        bool winsByIncreaseY = canIncreaseY && startingWithPlayer;
                        bool canWinByIncreaseY = canIncreaseY;
                        bool winsByIncreaseXY = canIncreaseX && canIncreaseY && startingWithPlayer;
                        bool canWinByIncreaseXY = canIncreaseX && canIncreaseY;
                        bool winsByDecreaseXIncreaseY = canDecreaseX && canIncreaseY && startingWithPlayer;
                        bool canWinByDecreaseXIncreaseY = canDecreaseX && canIncreaseY;

                        for (int i = 1; i < gameBoardWdithSpaces; i++)
                        {
                            if (winsByIncreaseX)
                            {
                                if (boardFilling[x + i, y] != null)
                                {
                                    winsByIncreaseX = toTestAgainst.Equals(boardFilling[x + i, y]);
                                }
                                else
                                {
                                    winsByIncreaseX = false;
                                }
                            }
                            if (canWinByIncreaseX)
                            {
                                if (boardFilling[x + i, y] != null)
                                {
                                    if (startingWithPlayer)
                                    {
                                        canWinByIncreaseX = toTestAgainst.Equals(boardFilling[x + i, y]);
                                    }
                                    else
                                    {
                                        if (ifNullContainsCanWinByIncreaseX == null)
                                        {
                                            ifNullContainsCanWinByIncreaseX = boardFilling[x + i, y];
                                        }
                                        else
                                        {
                                            canWinByIncreaseX = ifNullContainsCanWinByIncreaseX.Equals(boardFilling[x + i, y]);
                                        }
                                    }
                                }
                            }
                            if (winsByIncreaseY)
                            {
                                if (boardFilling[x, y + i] != null)
                                {
                                    winsByIncreaseY = toTestAgainst.Equals(boardFilling[x, y + i]);
                                }
                                else
                                {
                                    winsByIncreaseY = false;
                                }
                            }
                            if (canWinByIncreaseY)
                            {
                                if (boardFilling[x, y + i] != null)
                                {
                                    if (startingWithPlayer)
                                    {
                                        canWinByIncreaseY = toTestAgainst.Equals(boardFilling[x, y + i]) || boardFilling[x, y + i] == null;
                                    }
                                    else
                                    {
                                        if (ifNullContainsCanWinByIncreaseY == null)
                                        {
                                            ifNullContainsCanWinByIncreaseY = boardFilling[x, y + i];
                                        }
                                        else
                                        {
                                            canWinByIncreaseY = ifNullContainsCanWinByIncreaseY.Equals(boardFilling[x, y + i]);
                                        }
                                    }
                                }
                            }
                            if (winsByIncreaseXY)
                            {
                                if (boardFilling[x + i, y + i] != null)
                                {
                                    winsByIncreaseXY = toTestAgainst.Equals(boardFilling[x + i, y + i]);
                                }
                                else
                                {
                                    winsByIncreaseXY = false;
                                }
                            }
                            if (canWinByIncreaseXY)
                            {
                                if (boardFilling[x + i, y + i] != null)
                                {
                                    if (startingWithPlayer)
                                    {
                                    canWinByIncreaseXY = toTestAgainst.Equals(boardFilling[x + i, y + i]) || boardFilling[x + i, y + i] == null;
                                    }
                                    else
                                    {
                                        if (ifNullContainsCanWinByIncreaseXY == null)
                                        {
                                            ifNullContainsCanWinByIncreaseXY = boardFilling[x + i, y + i];
                                        }
                                        else
                                        {
                                            canWinByIncreaseXY = ifNullContainsCanWinByIncreaseXY.Equals(boardFilling[x + i, y + i]);
                                        }
                                    }
                                }
                            }
                            if (winsByDecreaseXIncreaseY)
                            {
                                if (boardFilling[x - i, y + i] != null)
                                {
                                    winsByDecreaseXIncreaseY = toTestAgainst.Equals(boardFilling[x - i, y + i]);
                                }
                                else
                                {
                                    winsByDecreaseXIncreaseY = false;
                                }
                            }
                            if (canWinByDecreaseXIncreaseY)
                            {
                                if (boardFilling[x - i, y + i] != null)
                                {
                                    if (startingWithPlayer)
                                    {
                                        canWinByDecreaseXIncreaseY = toTestAgainst.Equals(boardFilling[x - i, y + i]) || boardFilling[x - i, y + i] == null;
                                    }
                                    else
                                    {
                                        if (ifNullContainsCanWinByDecreaseXIncreaseY == null)
                                        {
                                            ifNullContainsCanWinByDecreaseXIncreaseY = boardFilling[x - i, y + i];
                                        }
                                        else
                                        {
                                            canWinByDecreaseXIncreaseY = ifNullContainsCanWinByDecreaseXIncreaseY.Equals(boardFilling[x - i, y + i]);
                                        }
                                    }
                                }
                            }
                        }

                        if (winsByIncreaseX || winsByIncreaseY || winsByIncreaseXY || winsByDecreaseXIncreaseY)
                        {
                            MessageBox.Show(toTestAgainst + messageBoxWinsMessageString);
                            ResetGame();
                            return;
                        }

                        someoneCanWin = someoneCanWin || canWinByIncreaseX || canWinByIncreaseY || canWinByIncreaseXY || canWinByDecreaseXIncreaseY;
                    }
                }
                if(someoneCanWin == false)
                {
                    MessageBox.Show(messageBoxStatemateMessageString);
                    ResetGame();
                    return;
                }
            }
            else
            {
                throw new Exception(exceptionGameBoardButtonsNotSetUpCorrectedly);
            }
        }
        private void ResetGame()
        {
            if (boardFilling.GetLength(0) == gameBoardWdithSpaces && boardFilling.GetLength(1) == gameBoardHeightSpaces)
            {
                if (gameBoardButtons.GetLength(0) == gameBoardWdithSpaces && gameBoardButtons.GetLength(1) == gameBoardHeightSpaces)
                {
                    playerOneTurn = true;

                    for (int x = 0; x < gameBoardWdithSpaces; x++)
                    {
                        for(int y = 0; y < gameBoardHeightSpaces; y++)
                        {
                            boardFilling[x, y] = null;
                            gameBoardButtons[x, y].Text = null;
                        }
                    }
                }
                else
                {
                    throw new Exception(exceptionGameBoardButtonsNotSetUpCorrectedly);
                }
            }
            else
            {
                throw new Exception(exceptionBoardFillingNotSetUpCorrectedly);
            }
        }
    }
}
