using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6Starter;

/**
 * 
 * Name: Sebastian Amyotte, Paul Hwang, Michael Rogers
 * Date: 11/7/2022
 * Description: Game logic for a good ol' game of Tic Tac Toe
 * Bugs: None, we believe
 * Reflection: Refactoring the IsThereAWinner method into smaller
 * methods felt like the best thing to do. There is definitely
 * a better algorithm I could have written, but for the sake
 * of readability over (extremely) small performance losses,
 * I will keep the code like this instead - S.A.
 * We implemented all bonus point features except for the database
 */

/// <summary>
/// The model class for TicTacToe
/// </summary>
internal class TicTacToeGame
{
    internal const int GRID_SIZE = 3;
    Player[,] grid = new Player[GRID_SIZE, GRID_SIZE];
    int[] scores = { 0, 0 };

    /// <summary>
    /// The player about to make a move
    /// </summary>
    public Player CurrentPlayer
    {
        get;
        set;
    }

    public void DumpBoard()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                Console.WriteLine("Grid[i,j]: "+ grid[i, j]);
            }
        }
    }

    // can access TicTacToeGame instance using [ , ]
    public Player this[int row, int col]
    {
        get => grid[row, col];
        set
        {
            grid[row, col] = value;

        }
    }

    /// <summary>
    /// Access to X's score
    /// </summary>
    public int XScore
    {
        get
        {
            return scores[(int)Player.X];
        }
    }

    /// <summary>
    /// Access to Y's score
    /// </summary>
    public int OScore
    {
        get
        {
            return scores[(int)Player.O];
        }
    }

    /// <summary>
    /// Increments the score of the respective victor if they win
    /// </summary>
    /// <param name="victor">Player that has won</param>
    public void IncrementScore(Player victor)
    {
        if(victor == Player.X || victor == Player.Both)
        {
            scores[1]++;
        }
        if (victor == Player.O || victor == Player.Both)
        {
            scores[0]++;
        }
    }

    /// <summary>
    /// Resets the game
    /// </summary>
    public TicTacToeGame()
    {
        ResetGame();
    }


    /// <summary>
    /// Processes the current turn - returns true if there is a victor, false otherwise
    /// </summary>
    /// <param name="row">clicked row</param>
    /// <param name="col">clicked column</param>
    /// <param name="victor">whoever won or Player.Nobody if nobody's won</param>
    /// <returns>true if there is a victor</returns>
    public Boolean ProcessTurn(int row, int col, out Player victor)
    {
        if (grid[row, col] == Player.X || grid[row, col] == Player.O) // already occupied, so ignore
        {
            victor = Player.Nobody;
            return false;
        }

        grid[row, col] = CurrentPlayer; // record the entry

        victor = IsThereAWinner();
        if (victor == Player.Nobody)
        {
            ToggleCurrentPlayer();
            return false;
        }
        return true;
    }

    /// <summary>
    /// Returns Player.X or Player.O if there is a winner, Player.Nobody if nobody's won, Player.Both if there's a tie
    /// </summary>
    /// <returns>The player that won</returns>
    public Player IsThereAWinner()
    {
        Player potentialWinner;
        //Check all straight axis
        potentialWinner = WinnerOnStraightAxis();
        if (potentialWinner != Player.Nobody)
        {
            return potentialWinner;
        }
        //Check diagonals
        potentialWinner = WinnerOnDiagonalAxis();
        if (potentialWinner != Player.Nobody)
        {
            return potentialWinner;
        }
        //No winner found, check for a filled grid condition
        return IsGridFilled() ? Player.Both : potentialWinner;
    }

    /// <summary>
    ///  Checks for a winner on the diagonal axis
    /// </summary>
    /// <returns>
    /// Returns the winning player, or Player.none if no victor
    /// </returns>
    public Player WinnerOnDiagonalAxis()
    {
        int sumTopLeftBottomRight = 0;
        int sumBottomLeftTopRight = 0;
        for (int i = 0; i < GRID_SIZE; i++)
        {
            sumTopLeftBottomRight += (int)grid[i, i];
            sumBottomLeftTopRight += (int)grid[GRID_SIZE - i - 1, i];
        }
        //Check for a winner
        if (sumTopLeftBottomRight == 0 || sumBottomLeftTopRight == 0)
        {
            return Player.O;
        }
        else if (sumTopLeftBottomRight == GRID_SIZE || sumBottomLeftTopRight == GRID_SIZE)
        {
            return Player.X;
        }
        //If no winner return no one
        return Player.Nobody;
    }

    /// <summary>
    ///  Checks every straight axis (horizontal and vertical) for winners
    /// </summary>
    /// <returns>
    /// Returns the winning player, or Player.none if no victor
    /// </returns>
    public Player WinnerOnStraightAxis()
    {
        Player verticalPotentialWinner = Player.Nobody;
        Player horizontalPotentialWinner = Player.Nobody;
        for (int i = 0; i < GRID_SIZE; i++)
        {
            //Check for a vertical winner
            verticalPotentialWinner = grid[i, 0];
            for (int j = 1; j < GRID_SIZE && verticalPotentialWinner != Player.Nobody; j++)
            {
                if (verticalPotentialWinner != grid[i, j])
                {
                    verticalPotentialWinner = Player.Nobody;
                }
            }
            //Check for a horizontal winner
            horizontalPotentialWinner = grid[0, i];
            for (int j = 1; j < GRID_SIZE && horizontalPotentialWinner != Player.Nobody; j++)
            {
                if (horizontalPotentialWinner != grid[j, i])
                {
                    horizontalPotentialWinner = Player.Nobody;
                }
            }
            //Check for a victor so far
            if (verticalPotentialWinner != Player.Nobody)
            {
                return verticalPotentialWinner;
            }
            else if (horizontalPotentialWinner != Player.Nobody)
            {
                return horizontalPotentialWinner;
            }
        }
        return Player.Nobody;
    }

    /// <summary>
    /// Checks to see if the grid has been completely filled and no possible moves exist
    /// </summary>
    /// <returns>
    /// A boolean, TRUE if the grid is full, FALSE otherwise
    /// </returns>
    public bool IsGridFilled()
    {
        bool gridFilled = true;
        for (int i = 0; i < GRID_SIZE && gridFilled; i++)
        {
            for (int j = 0; j < GRID_SIZE && gridFilled; j++)
            {
                //Check every square to see if it's filled
                //If there is a square yet to fill, quit the loop
                gridFilled = grid[i, j] != Player.Nobody;
            }
        }
        return gridFilled;
    }
    
    /// <summary>
    /// Resets the grid and sets the current player to X
    /// </summary>
    public void ResetGame()
    {
        for (int r = 0; r < GRID_SIZE; r++)
        {
            for (int c = 0; c < GRID_SIZE; c++)
            {
                grid[r, c] = Player.Nobody;
            }
        }
        CurrentPlayer = Player.X; // X always goes first
    }

    /// <summary>
    /// Toggles the current Player: X => O, O => X
    /// If Empty, ignore it
    /// </summary>
    private void ToggleCurrentPlayer()
    {
        CurrentPlayer = (CurrentPlayer == Player.X) ? Player.O : Player.X;
    }
}

