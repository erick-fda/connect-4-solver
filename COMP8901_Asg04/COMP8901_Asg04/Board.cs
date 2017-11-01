/*========================================================================================
    Board                                                                            *//**
	
    A Connect 4 game board.
	
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
using SysGeneric = System.Collections.Generic;

/*========================================================================================
	Board
========================================================================================*/
/**
    A Connect 4 game board.
*/
public class Board
{
    /*------------------------------------------------------------------------------------
		Instance Fields
	------------------------------------------------------------------------------------*/
    public const int BOARD_WIDTH = 7;
    public const int BOARD_HEIGHT = 6;

    private const char EMPTY = ' ';
    public const char PLAYER_1_PIECE = 'X';
    public const char PLAYER_2_PIECE = 'O';

    //private const char ROW_SEPARATOR_CHAR = '-';
    //private readonly string ROW_SEPARATOR;
    //private const string CELL_SEPARATOR = "|";

    private const string CELL_SEPARATOR_BEFORE = "[";
    private const string CELL_SEPARATOR_AFTER = "]";

    private const int MAX_MOVES_IN_GAME = BOARD_WIDTH * BOARD_HEIGHT;

    /*------------------------------------------------------------------------------------
		Instance Properties
	------------------------------------------------------------------------------------*/
    SysGeneric.List<SysGeneric.List<char>> _board { get; set; }
    int _moveCount { get; set; }
    string _moves { get; set; }
    SysGeneric.List<int> _piecesInColumn { get; set; }

    /*------------------------------------------------------------------------------------
		Constructors & Destructors
	------------------------------------------------------------------------------------*/
    public Board()
    {
        /* Initialize the board to all spaces empty. */
        _board = new SysGeneric.List<SysGeneric.List<char>>();
        for (int columnNum = 0; columnNum < BOARD_WIDTH; columnNum++)
        {
            SysGeneric.List<char> column = new SysGeneric.List<char>();

            for (int cellNum = 0; cellNum < BOARD_HEIGHT; cellNum++)
            {
                column.Add(EMPTY);
            }

            _board.Add(column);
        }

        ///* Initialize the row separator string. */
        //ROW_SEPARATOR = new System.String(ROW_SEPARATOR_CHAR, BOARD_WIDTH * 2 + 1);

        _moveCount = 0;
        _moves = "";
        _piecesInColumn = new SysGeneric.List<int>(BOARD_WIDTH);
    }

    /*------------------------------------------------------------------------------------
		Instance Methods
	------------------------------------------------------------------------------------*/
    /*
        Tries to make a particular move on the game board.
        Returns whether or not the move was made.
    */
    //public bool TryMove(char piece, int column)
    //{
    //    if (IsMoveValid(column))
    //    {
    //        _board[]

    //        return true;
    //    }

    //    return false;
    //}

    public bool IsMoveValid(int column)
    {
        /* Fail if the column number is invalid. */
        if (column < 0 ||
            column > BOARD_WIDTH - 1)
        {
            System.Console.Write("That's not a valid column number! Please enter a valid column number.\n");
            return false;
        }

        /* Fail if the column is full. */
        if (_piecesInColumn[column] >= BOARD_HEIGHT)
        {
            System.Console.Write("That column is full! Please choose another column.\n");
            return false;
        }

        return true;
    }

    public override string ToString()
    {
        string stringVal = "";

        /* Print cells. */
        for (int rowNum = 0; rowNum < BOARD_HEIGHT; rowNum++)
        {
            for (int columnNum = 0; columnNum < BOARD_WIDTH; columnNum++)
            {
                stringVal += CELL_SEPARATOR_BEFORE + _board[columnNum][rowNum] + CELL_SEPARATOR_AFTER;
            }

            stringVal += "\n";
        }

        /* Print column numbers. */
        stringVal += " ";
        for (int i = 0; i < BOARD_WIDTH; i++)
        {
            stringVal += (i + 1) + "  ";
        }

        stringVal += "\n\n";

        return stringVal;
    }
}
