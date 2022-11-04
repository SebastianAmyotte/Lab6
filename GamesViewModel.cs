using System.Collections.Generic;

namespace Lab6Starter
{
    public class GamesViewModel
    {
        // Class is used to dispaly the games in the ListView of the MainPage.xaml
        public IList<Game> Games { get; private set; }

        public GamesViewModel()
        {
            Games = new List<Game>();

            // TODO: Delete
            Games.Add(new Game { Winner = "X", Time = "00:23" });
            Games.Add(new Game { Winner = "Y", Time = "00:23" });
            Games.Add(new Game { Winner = "X", Time = "00:23" });

        }
    }
}
