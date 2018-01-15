using System;
using System.Collections.ObjectModel;
using System.Linq;
using BlackJackMVVM.Model;
using System.ComponentModel;
using System.Windows.Input;
using BlackJackMVVM.App.Utility;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;

namespace BlackJackMVVM.App.ViewModel
{
    class MainWindowVM : INotifyPropertyChanged
    {
        private Deck _deck;
        public ICommand HitCommand { get; set; }
        public ICommand HoldCommand { get; set; }
        public ICommand PlayAgainCommand { get; set; }

        private bool _hold = false;

        private bool _endVisible;

        public bool EndVisible
        {
            get { return _endVisible; }
            set
            {
                _endVisible = value;
                RaisePropertyChanged(nameof(EndVisible));
            }
        }

        private ObservableCollection<Card> _dealerHand;

        public ObservableCollection<Card> DealerHand
        {
            get { return _dealerHand; }
            set
            {
                _dealerHand = value;
                RaisePropertyChanged(nameof(DealerHand));
            }
        }

        private ObservableCollection<Card> _playerHand;

        public ObservableCollection<Card> PlayerHand
        {
            get { return _playerHand; }
            set
            {
                _playerHand = value;
                RaisePropertyChanged(nameof(PlayerHand));
            }
        }

        private int _playerScore;

        public int PlayerScore
        {
            get { return _playerScore; }
            set
            {
                _playerScore = value;
                RaisePropertyChanged(nameof(PlayerScore));
            }
        }

        private int _dealerScore;

        public int DealerScore
        {
            get { return _dealerScore; }
            set
            {
                _dealerScore = value;
                RaisePropertyChanged(nameof(DealerScore));
            }
        }

        private string _winnerText;

        public string WinnerText
        {
            get { return _winnerText; }
            set
            {
                _winnerText = value;
                RaisePropertyChanged(nameof(WinnerText));
            }
        }

        private int _playerPoints;

        public int PlayerPoints
        {
            get { return _playerPoints; }
            set
            {
                _playerPoints = value;
                RaisePropertyChanged(nameof(PlayerPoints));
            }
        }

        private int _dealerPoints;

        public int DealerPoints
        {
            get { return _dealerPoints; }
            set
            {
                _dealerPoints = value;
                RaisePropertyChanged(nameof(DealerPoints));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowVM()
        {
            _deck = new Deck();
            LoadCommands();
            DealerHand = new ObservableCollection<Card>();
            PlayerHand = new ObservableCollection<Card>();
            NewGame();
            PlayerPoints = 0;
            DealerPoints = 0;
        }

        private void LoadCommands()
        {
            HitCommand = new CustomCommand(HitCards, CanHitCards);
            HoldCommand = new CustomCommand(HoldHand, CanHoldHand);
            PlayAgainCommand = new CustomCommand(PlayAgain, CanPlayAgain);
        }

        private void PlayAgain(object obj)
        {
            NewGame();

        }

        private bool CanPlayAgain(object obj)
        {
            if(_deck.cards.Count > 10)
            {
                return true;
            }
            return false;
        }

        private void HoldHand(object obj)
        {
            _hold = true;
            DealerHit();
        }

        private bool CanHoldHand(object obj)
        {
            if (_hold == true || PlayerScore > 21)
                return false;
            return true;

        }

        private void HitCards(object obj)
        {
            PlayerHand.Add(PullCard());
            UpdateScores();
            if (PlayerScore > 21)
            {
                DeclareWinner();
            }
        }

        private bool CanHitCards(object obj)
        {
            if (PlayerScore < 21 && _hold == false)
            {
                return true;
            }
            return false;
        }

        //Returns a random card and removes it from the deck
        public Card PullCard()
        {
            Card tempCard;
            int randNum;
            Random random = new Random();

            randNum = random.Next(0, _deck.cards.Count());

            tempCard = _deck.cards[randNum];
            _deck.cards.Remove(_deck.cards[randNum]);

            return tempCard;
        }

        public void NewGame()
        {
            EndVisible = false;
            _hold = false;
            DealerHand.Clear();
            PlayerHand.Clear();
            DealerHand.Add(PullCard());
            PlayerHand.Add(PullCard());
            UpdateScores();
        }

        public void UpdateScores()
        {
            DealerScore = 0;
            PlayerScore = 0;

            foreach (Card card in DealerHand)
            {
                int value = 0;

                if (card.rank >= 10)
                    value = 10;
                else if (card.rank > 1 && card.rank < 10)
                    value = card.rank;
                else
                {
                    if (DealerScore < 21)
                        value = 11;
                    else
                        value = 1;
                }
                DealerScore += value;
            }

            foreach (Card card in PlayerHand)
            {
                int value = 0;

                if (card.rank >= 10)
                    value = 10;
                else if (card.rank > 1 && card.rank < 10)
                    value = card.rank;
                else
                {
                    if (PlayerScore <= 21)
                        value = 11;
                    else
                        value = 1;
                }
                PlayerScore += value;
            }
        }

        /// <summary>
        /// Dealer hits when player holds
        /// </summary>
        private void DealerHit()
        {
            while (DealerScore <= PlayerScore && DealerScore < 21)
            {
                var task = Task.Run(() =>
                {
                    Thread.Sleep(1000);
                });

                task.ConfigureAwait(true)
                    .GetAwaiter()
                    .OnCompleted(() =>
                    {
                        DealerHand.Add(PullCard());
                        UpdateScores();
                    });
            }
            DeclareWinner();
        }

        private void DeclareWinner()
        {
            if (PlayerScore <= 21 && PlayerScore > DealerScore || DealerScore > 21)
            {
                WinnerText = "Player Wins!";
                PlayerPoints++;
            }
            else if (PlayerScore == DealerScore)
            {
                WinnerText = "Tie Game";
            }
            else if (DealerScore <= 21 && DealerScore > PlayerScore || PlayerScore > 21)
            {
                WinnerText = "Dealer Wins!";
                DealerPoints++;
            }
            EndVisible = true;
        }
    }
}
