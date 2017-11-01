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

        /*--------------------------------------------------------------------------------
            Main Method
        --------------------------------------------------------------------------------*/
        /**
            Plays a game of Connect 4.
        */
        static void Main(string[] args)
        {
            Init();

            while (!_isGameOver)
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
        static private void Init()
        {
            _gameBoard = new Board();
            _playerGoesFirst = true;
            _isGameOver = false;
        }

        /**
            Play a round of Connect 4.
        */
        static void PlayRound()
        {
            DecideTurnOrder();

            /* Player's First Turn */
            if (_playerGoesFirst)
            {
                PlayerTurn();
            }

            while (!_gameBoard._isRoundOver)
            {
                /* AI's Turn */

                
                /* Check if game over. */

                
                /* Player's Turn */

                
                /* Check if game over. */
            }
        }

        /**
            Decide who will go first.
        */
        static void DecideTurnOrder()
        {
            /* Keep asking until the player gives a valid answer. */
            while (true)
            {
                System.Console.Write(System.String.Format("Would you like to go first (play as {0}) this round? (Y/N)\n>> ", Board.PLAYER_1_PIECE));
                string response = System.Console.ReadLine();
                System.Console.Write("\n\n");

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
        static private void PlayerTurn()
        {
            System.Console.Write("<< Player's Turn >>\n\n");
            System.Console.Write(_gameBoard);

            /* Keep asking for a column number until a valid answer is given. */
            while (true)
            {
                System.Console.Write(System.String.Format("Enter a column number to place your next piece. (1-{0})\n>> ", Board.BOARD_WIDTH));
                string response = System.Console.ReadLine();
                System.Console.Write("\n\n");
                
                if (response.Length > 0)
                {
                    int columnToTry = ((int)char.GetNumericValue(response[0])) - 1;

                    if (_gameBoard.TryMove(_playerPiece, columnToTry))
                    {
                        System.Console.Write(_gameBoard);
                        break;
                    }
                }
            }
        }
    }
}

