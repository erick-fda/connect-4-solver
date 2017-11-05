/*========================================================================================
    ConnectFourAI                                                                    *//**
	
    An AI that uses the negamax algorithm with alpha and beta pruning to determine the 
    best next move in a game of Connect 4.
	
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
	ConnectFourAI
========================================================================================*/
/**
    An AI that uses the negamax algorithm with alpha and beta pruning to determine the 
    best next move in a game of Connect 4.
*/
public static class ConnectFourAI
{
    /*------------------------------------------------------------------------------------
		Class Fields
	------------------------------------------------------------------------------------*/
    private const int MAX_DEPTH_TO_SEARCH = 15; // 7.5 s on first turn
    //private const int MAX_DEPTH_TO_SEARCH = 14; // 2.9 s on first turn

    /*------------------------------------------------------------------------------------
		Class Properties
	------------------------------------------------------------------------------------*/
    private static int _cellsConsidered { get; set; }
    private static SysGeneric.List<int> _columnPriorities { get; set; }
    private static int _alpha { get; set; }
    private static int _beta { get; set; }
    private static SysGeneric.List<int> _validMoves { get; set; }
    private static int _cellsPerColumn { get; set; }

    /*------------------------------------------------------------------------------------
		Class Methods
	------------------------------------------------------------------------------------*/
    /**
        Returns the best next move for the current player in a game of Connect 4 using a 
        negamax algorithm.
    */
    public static int SolveWithNegamax(Board board)
    {
        /* Make sure we check middle columns first. */
        if (_columnPriorities == null)
        {
            System.Console.Write("Initializing the column priorities list...\n\n");
            InitializeColumnPriorities();
        }

        /* Make sure we check only valid columns. */
        _validMoves = new SysGeneric.List<int>();
        for (int eachMove = 0; eachMove < Board.BOARD_WIDTH; eachMove++)
        {
            if (board.IsMoveValid(eachMove) == Board.MOVE_VALID)
            {
                _validMoves.Add(eachMove);
            }
        }

        /* The starting alpha and beta values are those for losing and winning immediately. */
        int _alpha = -22 + board._moveCount;
        int _beta = 22 - board._moveCount;

        /* For each column, get the value of moving to that column. */
        int bestMoveColumn = 0;
        int bestMoveValue = -100;

        foreach (int eachMoveColumn in _columnPriorities)
        {
            _cellsConsidered = 0;

            /* If the move is valid... */
            if (_validMoves.Contains(eachMoveColumn))
            {
                //System.Console.Write(System.String.Format("Considering column {0}...\n\n", eachMoveColumn + 1));

                /* Create a new board that represents the move. */
                Board eachMoveBoard = new Board(board);
                eachMoveBoard.TryMove(COMP8901_Asg04.Program._opponentPiece, eachMoveColumn);

                /* Get the value of the board after the move. */
                int eachMoveValue = -Negamax(eachMoveBoard, -_beta, -_alpha, 1, false);

                //System.Console.Write(System.String.Format("\tValue of column {0} is {1}.\n\n", eachMoveColumn + 1, eachMoveValue));

                /* If this is the first option or better than all previous options, it's the best one so far. 

                    Note that this means that if there are multiple options with the same value, it will take the 
                    first (a.k.a., middlemost) one. */
                if (eachMoveValue > bestMoveValue)
                {
                    bestMoveValue = eachMoveValue;
                    bestMoveColumn = eachMoveColumn;
                    //System.Console.Write(System.String.Format("\tColumn {0} is the best move so far.\n\n", eachMoveColumn + 1));
                }

                /* No need to check for moves worse than this one. */
                if (_alpha < bestMoveValue)
                {
                    _alpha = bestMoveValue;
                    //PrintMoveInfo(eachMoveColumn, eachMoveValue);
                }

            }
        }

        return bestMoveColumn;
    }

    /**
        Initializes the column priorities list.
        This ordering ensures that middle columns are tested first, 
        as they are preferable, since they have more opoortunities to lead to 
        winning moves.
    */
    private static void InitializeColumnPriorities()
    {
        _columnPriorities = new SysGeneric.List<int>();
        _columnPriorities.Add(3);
        _columnPriorities.Add(2);
        _columnPriorities.Add(4);
        _columnPriorities.Add(1);
        _columnPriorities.Add(5);
        _columnPriorities.Add(0);
        _columnPriorities.Add(6);
    }

    /**
        The negamax algorithm for determining the value of a move.
        This method calls itself recursively.

        Alpha is the min cap on the range of move values to consider.
        Beta is the max cap in the range of move values to consider.

        The value returned by this method is as follows:
        
            - If this move might lead to a victory for the given player, 
                this method returns a positive value. The higher the 
                value, the fewer moves until the projected victory (and 
                thus the better the move is considered to be).

            - If this move will definitely lead to a loss for the given 
                player, this method returns a negative value. The lower 
                the value, the fewer moves until the projected loss (and 
                thus the worse the move is considered to be.

            - If this move will definitely lead to a tie, 
                this method returns 0.

        To use this method to determine the best move at a given point in a game, 
        create a Board object for each available move that represents the state of 
        the board after playing that move. Then call this method once for each of 
        these board objects, keeping track of the value each returns. The move that 
        returned the highest value is the best move for the given player to play. 
        
        The best move in any case will be the move that wins the game the soonest, 
        if winning is possible. Else, it will be a move that forces a tie, if tying 
        the game is possible. Else, it will be the move that loses as late in the game 
        as possible.
    */
    private static int Negamax(Board board, int alpha, int beta, int depth, bool isAiTurn)
    {
        _cellsConsidered++;
        //System.Console.Write(System.String.Format("Cells considered: {0}\n", _cellsConsidered));

        /* If the game is already a tie, return 0. */
        if (board._isRoundOver && board._winner == Board.TIE_GAME)
        {
            //System.Console.Write("Found a possibility for a tie game!\n\n");

            /* We can tie, so don't bother checking anything worse than a tie. */
            if (isAiTurn)
            {
                _alpha = 0;
            }
            else
            {
                _beta = 0;
            }

            return 0;
        }

        /* If the player can win with this move, return the appropriate value. */
        for (int eachColumn = 0; eachColumn < Board.BOARD_WIDTH; eachColumn++)
        {
            if (board.IsMoveValid(eachColumn) == Board.MOVE_VALID &&
                board.IsWinningMove(board._nextTurn, eachColumn))
            {
                //System.Console.Write("Found a possibility for a winning game!\n\n");

                /* Winning move was found, so don't bother checking anything worse (if the player) 
                    or better (if the opponent). */
                if (isAiTurn)
                {
                    _alpha = (Board.MAX_MOVES_IN_GAME + 1 - board._moveCount) / 2;
                }
                else
                {
                    _beta = -((Board.MAX_MOVES_IN_GAME + 1 - board._moveCount) / 2);
                }

                return (Board.MAX_MOVES_IN_GAME + 1 - board._moveCount) / 2;
            }
        }

        /* If the player can't win on this turn, then the value of this position 
            must be less than an immediate win. */
        int maxValue = (Board.MAX_MOVES_IN_GAME - 1 - board._moveCount) / 2;

        /* If the maximum possible value is less than beta, 
            update beta, as we're not going to find anythign 
            better than this for the current move being considered.

            If alpha and beta converge, return the new beta. */
        if (maxValue < beta)
        {
            beta = maxValue;

            if (alpha >= beta)
            {
                /* Set alpha and beta appropriately before returning. */
                _alpha = isAiTurn ? alpha : -beta;
                _beta = isAiTurn ? beta : -alpha;

                return beta;
            }
        }

        /* If we're at the maximum search depth, just return the current alpha. */
        if (depth >= MAX_DEPTH_TO_SEARCH)
        {
            //System.Console.Write(System.String.Format("Reached maximum depth of {0}!\n", depth));

            /* Set alpha and beta appropriately before returning. */
            _alpha = isAiTurn ? alpha : -beta;
            _beta = isAiTurn ? beta : -alpha;

            return alpha;
        }

        /* Look at each of the possible moves after this one (in column priority order) 
            and pick the best one. */
        for (int eachMove = 0; eachMove < Board.BOARD_WIDTH; eachMove++)
        {
            /* If the move is valid... */
            if (board.IsMoveValid(_columnPriorities[eachMove]) == Board.MOVE_VALID)
            {
                /* Create a new board that represents the move. */
                Board eachMoveBoard = new Board(board);
                eachMoveBoard.TryMove(eachMoveBoard._nextTurn, _columnPriorities[eachMove]);

                /* Get the value of the board after the next move. 
                    
                    Note that the board is evaluated for the OTHER player 
                    (since it would be their turn next), which is why the 
                    alpha and beta are reversed and negated. The overall 
                    score is then negated again to get the value of the board 
                    for the CURRENT player. */
                int eachBoardValue = -Negamax(eachMoveBoard, -beta, -alpha, depth + 1, !isAiTurn);

                /* If the value for this move can match the current beta, 
                    keep this value, as it's the best we're going to get searching 
                    this move. */
                if (eachBoardValue >= beta)
                {
                    /* Set alpha and beta appropriately before returning. */
                    _alpha = isAiTurn ? alpha : -beta;
                    _beta = isAiTurn ? beta : -alpha;

                    return eachBoardValue;
                }

                /* If the value for this move is greater than the current alpha, 
                    update alpha, as we don't need to consider moves with a lower value 
                    than this one. */
                if (eachBoardValue > alpha)
                {
                    alpha = eachBoardValue;
                }
            }
        }

        /* After considering all of our options, return alpha, 
            which is essentially our worst-case projection for this move. */
        _alpha = isAiTurn ? alpha : -beta;
        _beta = isAiTurn ? beta : -alpha;

        return alpha;
    }

    /** @deprecated Prints out incorrect information. */
    //private static void PrintMoveInfo(int moveColumn, int moveValue)
    //{
    //    /* Print a message on the value of this move. */
    //    System.Console.Write(System.String.Format("\tAlpha: {0} Beta: {1}\n\n", _alpha, _beta));

    //    /* For winning moves */
    //    if (moveValue > 0)
    //    {
    //        int turnsUntilVictory = (22 - moveValue);

    //        if (turnsUntilVictory > 0)
    //        {
    //            System.Console.Write(
    //                System.String.Format("\tPlaying column {0} will put the AI {1} turns away from victory.\n\n",
    //                moveColumn + 1, turnsUntilVictory));
    //        }
    //        else
    //        {
    //            System.Console.Write(
    //                System.String.Format("\tPlaying column {0} will win the game for the AI.\n\n",
    //                moveColumn + 1));
    //        }
    //    }
    //    /* For losing moves */
    //    else if (moveValue < 0)
    //    {
    //        int turnsUntilDefeat = (22 + moveValue);

    //        if (turnsUntilDefeat > 0)
    //        {
    //            System.Console.Write(
    //                System.String.Format("\tPlaying column {0} will put the AI {1} turns away from defeat.\n\n",
    //                moveColumn + 1, turnsUntilDefeat));
    //        }
    //        else
    //        {
    //            System.Console.Write(
    //                System.String.Format("\tPlaying column {0} will lose the game for the AI.\n\n",
    //                moveColumn + 1));
    //        }
    //    }
    //    /* For tying moves */
    //    else
    //    {
    //        System.Console.Write(
    //            System.String.Format("\tPlaying column {0} will tie the game.\n\n",
    //            moveColumn + 1));
    //    }
    //}

    /** @deprecated Old version of the negamax function. */
    //private static int Negamax(Board board, int alpha, int beta, bool _isTop)
    //{
    //    _cellsConsidered++;
    //    //System.Console.Write(System.String.Format("Cells considered: {0}\n", _cellsConsidered));

    //    /* Difficulty */
    //    if (_cellsConsidered >= _cellsPerColumn)
    //    {
    //        if (_isTop)
    //        {
    //            _alpha = alpha;
    //            _beta = beta;
    //        }

    //        return alpha;
    //    }

    //    /* If the game is already a tie, return 0. */
    //    if (board._isRoundOver && board._winner == Board.TIE_GAME)
    //    {
    //        if (_isTop)
    //        {
    //            _alpha = alpha;
    //            _beta = beta;
    //        }

    //        return 0;
    //    }

    //    /* If the player can win with this move, return the appropriate value. */
    //    for (int eachColumn = 0; eachColumn < Board.BOARD_WIDTH; eachColumn++)
    //    {
    //        if (board.IsMoveValid(eachColumn) == Board.MOVE_VALID && 
    //            board.IsWinningMove(board._nextTurn, eachColumn))
    //        {
    //            return (Board.MAX_MOVES_IN_GAME + 1 - board._moveCount) / 2;
    //        }
    //    }

    //    /* If the player can't win on this turn, then the value of this position 
    //        must be less than an immediate win. */
    //    int maxValue = (Board.MAX_MOVES_IN_GAME - 1 - board._moveCount) / 2;

    //    /* If the maximum possible value is less than beta, 
    //        update beta, as we're not going to find anythign 
    //        better than this for the current move being considered.

    //        If alpha and beta converge, return the new beta. */
    //    if (maxValue < beta)
    //    {
    //        beta = maxValue;

    //        if (alpha >= beta)
    //        {
    //            if (_isTop)
    //            {
    //                _alpha = alpha;
    //                _beta = beta;
    //            }

    //            return beta;
    //        }
    //    }

    //    /* Look at each of the possible moves after this one (in column priority order) 
    //        and pick the best one. */
    //    for (int eachMove = 0; eachMove < Board.BOARD_WIDTH; eachMove++)
    //    {
    //        /* If the move is valid... */
    //        if (board.IsMoveValid(_columnPriorities[eachMove]) == Board.MOVE_VALID)
    //        {
    //            /* Create a new board that represents the move. */
    //            Board eachMoveBoard = new Board(board);
    //            eachMoveBoard.TryMove(_columnPriorities[eachMove]);

    //            /* Get the value of the board after the next move. 
                    
    //                Note that the board is evaluated for the OTHER player 
    //                (since it would be their turn next), which is why the 
    //                alpha and beta are reversed and negated. The overall 
    //                score is then negated again to get the value of the board 
    //                for the CURRENT player. */
    //            int eachBoardValue = -Negamax(eachMoveBoard, -beta, -alpha, false);

    //            /* If the value for this move can match the current beta, 
    //                keep this value, as it's the best we're going to get searching 
    //                this move. */
    //            if (eachBoardValue >= beta)
    //            {
    //                if (_isTop)
    //                {
    //                    _alpha = alpha;
    //                    _beta = beta;
    //                }

    //                return eachBoardValue;
    //            }

    //            /* If the value for this move is greater than the current alpha, 
    //                update alpha, as we don't need to consider moves with a lower value 
    //                than this one. */
    //            if (eachBoardValue > alpha)
    //            {
    //                alpha = eachBoardValue;
    //            }
    //        }
    //    }

    //    /* After considering all of our options, return alpha, 
    //        which is essentially our worst-case projection for this move. */
    //    if (_isTop)
    //    {
    //        _alpha = alpha;
    //        _beta = beta;
    //    }

    //    return alpha;
    //}
}
