using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using static Lab6Starter.GamesViewModel;

namespace Lab6Starter;
/**
 * 
 * Name: Sebastian Amyotte + Paul 
 * Date: 11/7/2022
 * Description: Lab 6 Software Engineering
 * Bugs: None, we believe
 * Reflection: A lot of good practice for XAML, as well as practicing binding properties
 * 
 */



/// <summary>
/// The MainPage, this is a 1-screen app
/// </summary>
/// 
public partial class MainPage : ContentPage
{
    TicTacToeGame ticTacToe; // model class
    Button[,] grid;          // stores the buttons
    bool isPlaying = false;  // bool var that determines if the game is being played
    TimeOnly time = new();   // can represent time for the time
    GamesViewModel games;   // games object to allow adding to the ListView ItemSource
    


    /// <summary>
    /// initializes the component
    /// </summary>
    public MainPage()
    {
        InitializeComponent();
        RandomizeGameFieldColor(null, null);
        ticTacToe = new TicTacToeGame();
        grid = new Button[TicTacToeGame.GRID_SIZE, TicTacToeGame.GRID_SIZE] { { Tile00, Tile01, Tile02 }, { Tile10, Tile11, Tile12 }, { Tile20, Tile21, Tile22 } };
        games = new GamesViewModel();           // creates a games object so we can add to the ListView
        GamesLV.ItemsSource = games.GetGames(); // Sets the ItemSource of the ListView
    }

    public void DumpBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Console.WriteLine("Grid[i,j]: " + grid[i, j]);
            }
        }
    }


    /// <summary>
    /// Handles button clicks - changes the button to an X or O (depending on whose turn it is)
    /// Checks to see if there is a victory - if so, invoke 
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleButtonClick(object sender, EventArgs e)
    {
        Console.WriteLine("UI: ");
        DumpBoard();
        Console.WriteLine("Game logic: ");
        ticTacToe.DumpBoard();
        Player victor;
        Player currentPlayer = ticTacToe.CurrentPlayer;

        // if the game just started, we will change isPlaying and start running the timer
        if (!isPlaying)
        {
            isPlaying = true;
            RunTimer();
        }

        Button button = (Button)sender;
        int row;
        int col;

        FindTappedButtonRowCol(button, out row, out col);
        if (button.Text.ToString() != "")
        { // if the button has an X or O, bail
            DisplayAlert("Illegal move!", "That spot is already taken!", "Oh, sorry");
            return;
        }
        button.Text = currentPlayer.ToString();
        Boolean gameOver = ticTacToe.ProcessTurn(row, col, out victor);

        if (gameOver)
        {
            ticTacToe.IncrementScore(victor);
            CelebrateVictory(victor);
        }
    }

    Random rng = new Random();
    private void RandomizeGameFieldColor(object sender, EventArgs e)
    {
        Resources["randomColor"] = Color.FromRgb(rng.Next(100, 256), rng.Next(100, 256), rng.Next(100, 256));
    }

    /// <summary>
    /// Returns the row and col of the clicked row
    /// There used to be an easier way to do this ...
    /// </summary>
    /// <param name="button"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    private void FindTappedButtonRowCol(Button button, out int row, out int col)
    {
        row = -1;
        col = -1;

        for (int r = 0; r < TicTacToeGame.GRID_SIZE; r++)
        {
            for (int c = 0; c < TicTacToeGame.GRID_SIZE; c++)
            {
                if(button == grid[r, c])
                {
                    row = r;
                    col = c;
                    return;
                }
            }
        }
        
    }


    /// <summary>
    /// Celebrates victory, displaying a message box and resetting the game
    /// </summary>
    private async void CelebrateVictory(Player victor)
    {
        //MessageBox.Show(Application.Current.MainWindow, String.Format("Congratulations, {0}, you're the big winner today", victor.ToString()));
        XScoreLBL.Text = String.Format("X's Score: {0}", ticTacToe.XScore);
        OScoreLBL.Text = String.Format("O's Score: {0}", ticTacToe.OScore);

        PauseTimer();
        await DisplayAlert($"Congratulations, {victor}!", "You're a big winner today!", "OK");
        games.AddGame(new Game { Winner = $"{victor}", Time = $"{timeLabel.Text}" });
        ResetGame();
    }
    /// <summary>
    /// Button handler for reset button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ResetButton(object sender, EventArgs e)
    {
        // Reset game logic
        ResetGame();
    }

    /// <summary>
    /// Resets the grid buttons so their content is all ""
    /// </summary>
    private void ResetGame()
    {
        ResetTimer();

        //Reset lavels in all buttons
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                grid[i, j].Text = "";
            }
        }
        ticTacToe.ResetGame();
    }

    /// <summary>
    /// Runs the timer
    /// </summary>
    private async void RunTimer()
    {
        while (isPlaying)
        {
            time = time.Add(TimeSpan.FromSeconds(1));
            SetTime();
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }

    /// <summary>
    /// Stops the timer after game is over so that it displays
    /// at the end of the game rather than continuing
    /// </summary>
    private void PauseTimer()
    {
        isPlaying = false;
        RunTimer();
    }

    /// <summary>
    /// Sets the text of the timer button
    /// </summary>
    private void SetTime()
    {
        timeLabel.Text = $"{time.Minute:00}:{time.Second:00}";
    }

    /// <summary>
    /// Resets the timer and isPlaying variable
    /// </summary>
    private void ResetTimer()
    {
        isPlaying = false;
        time = new TimeOnly();
        SetTime();
    }
}



