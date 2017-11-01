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
    private SysGeneric.List<SysGeneric.List<char>> _board { get; set; }
    private int _moveCount { get; set; }
    private string _moves { get; set; }
    private SysGeneric.List<int> _piecesInColumn { get; set; }
    public bool _isRoundOver { get; set; }


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
        for (int eachColumn = 0; eachColumn < BOARD_WIDTH; eachColumn++)
        {
            _piecesInColumn.Add(0);
        }
        _isRoundOver = false;
    }

    /*------------------------------------------------------------------------------------
		Instance Methods
	------------------------------------------------------------------------------------*/
    /*
        Tries to make a particular move on the game board.
        Returns whether or not the move was made.
    */
    public bool TryMove(char piece, int column)
    {
        /* Fail if the round is over. */
        if (_isRoundOver)
        {
            System.Console.Write("Can't play a move because the round is already over.\n");
            return false;
        }

        /* Fail if the column is already full. */
        if (!IsMoveValid(column))
        {
            return false;
        }

        /* Play the move. */
        PlayMove(piece, column);

        return true;
    }

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

    /**
        Plays the given move.
        Only moves that return true when passed to IsMoveValid() should be passed to this method.
    */
    private void PlayMove(char piece, int column)
    {
        /* Check if this move will end the game. */
        if (_moveCount == MAX_MOVES_IN_GAME - 1 || 
            IsWinningMove(piece, column))
        {
            _isRoundOver = true;
        }

        /* Play the move. */
        _board[column][_piecesInColumn[column]] = piece;
        _moveCount++;
        _moves += column;
        _piecesInColumn[column]++;
    }

    /**
        Checks if the given move is a winning move.
        Only moves that return true when passed to IsMoveValid() should be passed to this method.
    */
    public bool IsWinningMove(char piece, int column)
    {
        /* If the three pieces below the one to be placed are of the same type, 
            then this move is a vertical win. */
        if (_piecesInColumn[column] >= 3 && 
            _board[column][_piecesInColumn[column] - 1] == piece &&
            _board[column][_piecesInColumn[column] - 2] == piece &&
            _board[column][_piecesInColumn[column] - 3] == piece)
        {
            return true;
        }

        /* Check horizontal and diagonal wins. */
        for (int verticalOffset = -1; verticalOffset <= 1; verticalOffset++)                // For spaces above and below the move...
        {
            int adjacentCount = 0;

            for (int horizontalOffset = -1; horizontalOffset <= 1; horizontalOffset += 2)   // For spaces to the left and right of the move...
            {
                for (int cellX = column + horizontalOffset,                                 // For each cell to check...
                     cellY = _piecesInColumn[column] + (horizontalOffset * verticalOffset);
                     cellX >= 0 &&                                                          // If we're inside the bounds of the board...
                     cellX < BOARD_WIDTH &&
                     cellY >= 0 &&
                     cellY < BOARD_HEIGHT &&
                     _board[cellX][cellY] == piece;                                         // ...and if this cell is of the same type as the one to be placed...
                     adjacentCount++)                                                       // ...increment our count of adjacent pieces.
                {
                    /* Move to the next cell to check. */
                    cellX += horizontalOffset;
                    cellY += horizontalOffset * verticalOffset;
                }
            }

            /* If we got three or more adjacent pieces, this is a winning move. */
            if (adjacentCount >= 3)
            {
                return true;
            }
        }

        /* If we reach this point, it means no winning moves were found. */
        return false;
    }

    public override string ToString()
    {
        string stringVal = "";

        /* Print cells. */
        for (int rowNum = BOARD_HEIGHT - 1; rowNum >= 0; rowNum--)
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
