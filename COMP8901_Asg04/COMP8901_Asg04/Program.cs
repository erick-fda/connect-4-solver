/*========================================================================================
    COMP8901 Assignment 04                                                           *//**
	
    A Connect 4 game with an AI opponent that uses the negamax algorithm with alpha and 
    beta pruning to choose moves.
	
    Copyright 2017 Erick Fernandez de Arteaga. All rights reserved.
        https://www.linkedin.com/in/erick-fda
        https://bitbucket.org/erick-fda
        
	@author Erick Fernandez de Arteaga
    @version 0.0.0
	@file
	
*//*====================================================================================*/

/*========================================================================================
	Dependencies
========================================================================================*/


/*========================================================================================
	MyProgram
========================================================================================*/
/**
    A Connect 4 game with an AI opponent that uses the negamax algorithm with alpha and 
    beta pruning to choose moves.
*/
namespace COMP8901_Asg04
{
    class Program
    {
        /*--------------------------------------------------------------------------------
            Class Fields
        --------------------------------------------------------------------------------*/


        /*--------------------------------------------------------------------------------
            Class Properties
        --------------------------------------------------------------------------------*/
        static private Board _gameBoard { get; set; }
        static private bool _playerGoesFirst { get; set; }
        static private char _playerPiece { get; set; }
        static private char _opponentPiece { get; set; }
        static private bool _isGameOver { get; set; }
        static private bool _isMultiplayer { get; set; }
        static private bool _playAgain { get; set; }

        /*--------------------------------------------------------------------------------
            Main Method
        --------------------------------------------------------------------------------*/
        /**
            Plays a game of Connect 4.
        */
        static void Main(string[] args)
        {
            Init();
            
            System.Console.Write("<< WELCOME TO CONNECT 4 >>\n");
            System.Console.Write("*Rated the #1 two-player turn-based family board game about aligning 4 pieces!*\n\n");

            while (_playAgain)
            {
                PlayRound();
            }

            System.Console.Write("Thanks for playing! Press ENTER to exit...\n\n");
            System.Console.ReadLine();
            return;
        }

        /*--------------------------------------------------------------------------------
            Class Methods
        --------------------------------------------------------------------------------*/
        /**
            Initializes fields.
        */
        private static void Init()
        {
            _playerGoesFirst = true;
            _isGameOver = false;
            _isMultiplayer = false;
            _playAgain = true;
        }

        /**
            Play a round of Connect 4.
        */
        private static void PlayRound()
        {
            _gameBoard = new Board();

            AskMultiplayer();

            /* Decide who plays first. */
            if (_isMultiplayer)
            {
                _playerGoesFirst = true;
                _playerPiece = Board.PLAYER_1_PIECE;
                _opponentPiece = Board.PLAYER_2_PIECE;
            }
            else
            {
                AskTurnOrder();
            }

            /* Player's First Turn */
            if (_playerGoesFirst)
            {
                PlayerTurn(_playerPiece);
            }

            while (!_gameBoard._isRoundOver)
            {
                /* Opponent's Turn */
                if (_isMultiplayer)
                {
                    /* Player 2's Turn */
                    PlayerTurn(_opponentPiece);
                }
                else
                {
                    /* AI's Turn */

                }

                /* Check if round over. */
                if (_gameBoard._isRoundOver)
                {
                    break;
                }

                /* Player's Turn */
                PlayerTurn(_playerPiece);
            }

            PrintRoundResults();

            AskPlayAgain();
        }

        /**
            Asks the player if this round will be multiplayer.
        */
        private static void AskMultiplayer()
        {
            /* Keep asking until the player gives a valid answer. */
            while (true)
            {
                string response = AskPlayer("Would you like to play multiplayer this round? (Y/N)\n>> ");

                if (response.Length > 0)
                {
                    if (char.ToUpper(response[0]) == 'Y')
                    {
                        _isMultiplayer = true;
                    }
                    else
                    {
                        _isMultiplayer = false;
                    }

                    break;
                }
            }
        }

        /**
            Asks the player a question and returns their response.
        */
        private static string AskPlayer(string question)
        {
            System.Console.Write(question);
            string response = System.Console.ReadLine();
            System.Console.Write("\n\n");
            return response;
        }

        /**
            Asks the player who will go first.
        */
        private static void AskTurnOrder()
        {
            /* Keep asking until the player gives a valid answer. */
            while (true)
            {
                string response = AskPlayer(System.String.Format("Would you like to go first (play as {0}) this round? (Y/N)\n>> ", Board.PLAYER_1_PIECE));

                if (response.Length > 0)
                {
                    if (char.ToUpper(response[0]) == 'Y')
                    {
                        _playerGoesFirst = true;
                        _playerPiece = Board.PLAYER_1_PIECE;
                        _opponentPiece = Board.PLAYER_2_PIECE;
                    }
                    else
                    {
                        _playerGoesFirst = false;
                        _playerPiece = Board.PLAYER_2_PIECE;
                        _opponentPiece = Board.PLAYER_1_PIECE;
                    }

                    break;
                }
            }
        }

        /**
            Gets a move from the player.
        */
        private static void PlayerTurn(char piece)
        {
            /* Print out who's turn it is. */
            if (_isMultiplayer)
            {
                if (piece == _playerPiece)
                {
                    System.Console.Write("<< Player 1's Turn >>\n\n");
                }
                else
                {
                    System.Console.Write("<< Player 2's Turn >>\n\n");
                }
            }
            else
            {
                System.Console.Write("<< Player's Turn >>\n\n");
            }
            System.Console.Write(_gameBoard);

            /* Keep asking for a column number until a valid answer is given. */
            while (true)
            {
                string response = AskPlayer(System.String.Format("Enter a column number to place your next piece. (1-{0})\n>> ", Board.BOARD_WIDTH));
                
                if (response.Length > 0)
                {
                    int columnToTry = ((int)char.GetNumericValue(response[0])) - 1;

                    if (_gameBoard.TryMove(piece, columnToTry))
                    {
                        System.Console.Write(_gameBoard);
                        break;
                    }
                }
            }
        }

        /**
            Prints the results of the last round played.
        */
        private static void PrintRoundResults()
        {
            /* Print round results. */
            System.Console.Write("<< ROUND RESULTS >>\n");

            if (_gameBoard._winner == _playerPiece)
            {
                if (_isMultiplayer)
                {
                    System.Console.Write("Congratulations, player 1 (X)! You win!\n\n");
                }
                else
                {
                    System.Console.Write("Congratulations, player! You win!\n\n");
                }
            }
            else if (_gameBoard._winner == _opponentPiece)
            {
                if (_isMultiplayer)
                {
                    System.Console.Write("Congratulations, player 2 (O)! You win!\n\n");
                }
                else
                {
                    System.Console.Write("The AI wins!\n\n");
                }
            }
            else
            {
                System.Console.Write("The result is a tie.\n\n");
            }
        }

        /**
            Asks the player if they want to play another round.
        */
        private static void AskPlayAgain()
        {
            while (true)
            {
                string response = AskPlayer("Would you like to play another round? (Y/N)\n>> ");

                if (response.Length > 0)
                {
                    if (char.ToUpper(response[0]) == 'Y')
                    {
                        _playAgain = true;
                    }
                    else
                    {
                        _playAgain = false;
                    }

                    break;
                }
            }
        }
    }
}

