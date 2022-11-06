using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Lab6Starter
{
    public class GamesViewModel
    {
        // Class is used to dispaly the games in the ListView of the MainPage.xaml
        public ObservableCollection<Game> Games { get; private set; }

        /// <summary>
        /// Constructor for a GamesViewModel object that creates the ObservableCollection of Games
        /// </summary>
        public GamesViewModel()
        {
            Games = new ObservableCollection<Game>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameToBeAdded"></param>
        public bool AddGame(Game gameToBeAdded)
        {
            try
            {
                Games.Add(gameToBeAdded);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public ObservableCollection<Game> GetGames()
        {
            return Games;
        }
    }
}
