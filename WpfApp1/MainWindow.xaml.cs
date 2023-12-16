using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System;
using System.Linq;
using System.Windows;


namespace WordleGame
{
    public partial class MainWindow : Window
    {
        private string secretWord; // Change this to a word list or generate randomly

        private int attemptsLeft = 5;

        public MainWindow()
        {
            InitializeComponent();
        }

        private SQLiteConnection ConnectToDatabase()
        {
            string connectionString = "Data Source=words.db;Version=3;";
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            return connection;
        }
        
        private void SelectRandomWord()
        {
            SQLiteConnection connection = ConnectToDatabase();
            string query = "SELECT * FROM words ORDER BY RANDOM() LIMIT 1";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    // Replace "YourColumnName" with the actual name of the column you want to retrieve
                    secretWord = reader["word"].ToString();
                }
            }
            else
            {
                MessageBox.Show("No data available.");
            }

            reader.Close();
            connection.Close();
        }

        

        private void GuessButton_Click(object sender, RoutedEventArgs e)
        {
           
            string guess = TextBox.Text.ToLower();
            if (guess.Length != secretWord.Length)
            {
                MessageBox.Show("Guess should be of the same length as the secret word.");
                return;
            }

            attemptsLeft--;
            if (attemptsLeft == 0)
            {
                MessageBox.Show($"Out of attempts! The word was '{secretWord}'.");
                ResetGame();
                return;
            }

            string feedback = GenerateFeedback(guess);
            FeedbackTextBlock.Text = feedback;

            if (feedback == "Correct!")
            {
                MessageBox.Show($"Congratulations! You guessed the word '{secretWord}'!");
                ResetGame();
            }
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            SelectRandomWord();
            ResetGame();
        }

        private string GenerateFeedback(string guess)
        {
            if (guess == secretWord)
            {
                return "Correct!";
            }

            string feedback = string.Concat(guess.Select((c, i) => secretWord[i] == c ? secretWord[i].ToString() : (secretWord.Contains(c) ? "+" : "-")));
            return feedback;
        }

        private void ResetGame()
        {
            attemptsLeft = 5;
            TextBox.Text = "";
            FeedbackTextBlock.Text = "";
        }
    }
}






