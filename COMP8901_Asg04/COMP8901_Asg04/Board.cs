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
using System.Linq;

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
    private const int NUM_PLAYERS = 2;

    private const char EMPTY_CELL = ' ';
    public const char PLAYER_1_PIECE = 'X';
    public const char PLAYER_2_PIECE = 'O';

    public const int MOVE_VALID = 0;
    public const int MOVE_INVALID_COLUMN_INVALID = 1;
    public const int MOVE_INVALID_COLUMN_FULL = 2;
    public const int MOVE_INVALID_ROUND_OVER = 3;

    private const string CELL_SEPARATOR_BEFORE = "[";
    private const string CELL_SEPARATOR_AFTER = "]";

    public const int MAX_MOVES_IN_GAME = BOARD_WIDTH * BOARD_HEIGHT;
    public const char TIE_GAME = EMPTY_CELL;

    /*------------------------------------------------------------------------------------
		Instance Properties
	------------------------------------------------------------------------------------*/
    public SysGeneric.List<SysGeneric.List<char>> _board { get; set; }
    private SysGeneric.List<char> _playerPieces { get; set; }
    public int _moveCount { get; private set; }
    private string _moves { get; set; }
    private SysGeneric.List<int> _piecesInColumn { get; set; }
    public bool _isRoundOver { get; private set; }
    public char _winner { get; private set; }

    /* Returns what the next piece to play should be, 
        based on the number of turns so far.*/
    public char _nextTurn
    {
        get
        {
            return _playerPieces[_moveCount % NUM_PLAYERS];
        }
    }

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
                column.Add(EMPTY_CELL);
            }

            _board.Add(column);
        }

        ///* Initialize the row separator string. */
        //ROW_SEPARATOR = new System.String(ROW_SEPARATOR_CHAR, BOARD_WIDTH * 2 + 1);
        _playerPieces = new SysGeneric.List<char>();
        _playerPieces.Add(PLAYER_1_PIECE);
        _playerPieces.Add(PLAYER_2_PIECE);
        _moveCount = 0;
        _moves = "";
        _piecesInColumn = new SysGeneric.List<int>(BOARD_WIDTH);
        for (int eachColumn = 0; eachColumn < BOARD_WIDTH; eachColumn++)
        {
            _piecesInColumn.Add(0);
        }
        _isRoundOver = false;
        _winner = TIE_GAME;
    }

    /**
        Copy constructor.
    */
    public Board(Board otherBoard)
    {
        _board = otherBoard._board.Select(x => x.ToList()).ToList();
        _playerPieces = new SysGeneric.List<char>(otherBoard._playerPieces);
        _moveCount = otherBoard._moveCount;
        _moves = System.String.Copy(otherBoard._moves);
        _piecesInColumn = new SysGeneric.List<int>(otherBoard._piecesInColumn);
        _isRoundOver = otherBoard._isRoundOver;
        _winner = otherBoard._winner;
    }

    /*------------------------------------------------------------------------------------
		Instance Methods
	------------------------------------------------------------------------------------*/
    /*
        Tries to make a particular move on the game board.
        Returns an int code indicating whether the move was valid (in which case it was 
        also executed), and the error code if the move was not valid.
    */
    public int TryMove(char piece, int column)
    {
        /* Play the move if it's valid.
            Return the result code in any case. */
        int resultCode = IsMoveValid(column);

        if (resultCode == MOVE_VALID)
        {
            PlayMove(piece, column);
        }

        return resultCode;
    }

    /**
        Returns an int code indicating whether the given move is valid and the error code 
        if the move is not valid.
    */
    public int IsMoveValid(int column)
    {
        /* Fail if the round is over. */
        if (_isRoundOver)
        {
            return MOVE_INVALID_ROUND_OVER;
        }

        /* Fail if the column number is invalid. */
        if (column < 0 ||
            column > BOARD_WIDTH - 1)
        {
            return MOVE_INVALID_COLUMN_INVALID;
        }

        /* Fail if the column is full. */
        if (_piecesInColumn[column] >= BOARD_HEIGHT)
        {
            return MOVE_INVALID_COLUMN_FULL;
        }

        return MOVE_VALID;
    }

    /**
        Plays the given move.
        Only moves that return true when passed to IsMoveValid() should be passed to this method.
    */
    private void PlayMove(char piece, int column)
    {
        /* Check if this move will end the game. */
        if (IsWinningMove(piece, column))
        {
            _winner = piece;
            _isRoundOver = true;
        }
        else if (_moveCount == MAX_MOVES_IN_GAME - 1)
        {
            _winner = TIE_GAME;
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
        /* For cells above and below the move... */
        for (int verticalOffset = -1; verticalOffset <= 1; verticalOffset++)
        {
            int adjacentCount = 0;

            /* For cells to the left and right of the move... */
            for (int horizontalOffset = -1; horizontalOffset <= 1; horizontalOffset += 2)
            {
                /* For each cell to check... */
                for (int cellX = column + horizontalOffset, 
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

    public bool IsAlmostWinningMove(char piece, int column)
    {
        /* If the two pieces below the one to be placed are of the same type, 
            then this move is almost a vertical win. */
        if (_piecesInColumn[column] >= 3 &&
            _board[column][_piecesInColumn[column] - 1] == piece &&
            _board[column][_piecesInColumn[column] - 2] == piece)
        {
            return true;
        }

        /* Check horizontal and diagonal wins. */
        /* For cells above and below the move... */
        for (int verticalOffset = -1; verticalOffset <= 1; verticalOffset++)
        {
            int adjacentCount = 0;

            /* For cells to the left and right of the move... */
            for (int horizontalOffset = -1; horizontalOffset <= 1; horizontalOffset += 2)
            {
                /* For each cell to check... */
                for (int cellX = column + horizontalOffset,
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

            /* If we got two or more adjacent pieces, this is almost a winning move. */
            if (adjacentCount >= 2)
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
