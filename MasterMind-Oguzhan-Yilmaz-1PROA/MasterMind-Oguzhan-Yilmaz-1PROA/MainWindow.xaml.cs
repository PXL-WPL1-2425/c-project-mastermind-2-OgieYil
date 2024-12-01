using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mastermind_PE_Oguzhan_Yilmaz_1PROA
{
    public partial class MainWindow : Window
    {
        private string[] generatedCode; // The randomly generated code
        private int attemptsLeft = 10;  // Number of attempts the player has

        public MainWindow()
        {
            InitializeComponent();
            GenerateRandomCode();
            OpvullenComboBoxes();
            UpdateTitle();
        }

        private void GenerateRandomCode()
        {
            Random random = new Random();
            string[] Colors = { "Rood", "Geel", "Oranje", "Wit", "Groen", "Blauw" };
            generatedCode = Enumerable.Range(0, 4).Select(_ => Colors[random.Next(Colors.Length)]).ToArray();
        }

        private void OpvullenComboBoxes()
        {
            string[] Colors = { "Rood", "Geel", "Oranje", "Wit", "Groen", "Blauw" };
            ComboBox1.ItemsSource = Colors;
            ComboBox2.ItemsSource = Colors;
            ComboBox3.ItemsSource = Colors;
            ComboBox4.ItemsSource = Colors;
        }

        private void UpdateTitle()
        {
            this.Title = $"MasterMind - Pogingen over: {attemptsLeft}";
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Label1.Background = GetBrushFromColorName(ComboBox1.SelectedItem as string ?? "default");
            Label2.Background = GetBrushFromColorName(ComboBox2.SelectedItem as string ?? "default");
            Label3.Background = GetBrushFromColorName(ComboBox3.SelectedItem as string ?? "default");
            Label4.Background = GetBrushFromColorName(ComboBox4.SelectedItem as string ?? "default");
        }

        private SolidColorBrush GetBrushFromColorName(string colorName)
        {
            return colorName switch
            {
                "Rood" => Brushes.Red,
                "Geel" => Brushes.Yellow,
                "Oranje" => Brushes.Orange,
                "Wit" => Brushes.White,
                "Groen" => Brushes.Green,
                "Blauw" => Brushes.Blue,
                _ => Brushes.Transparent
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (attemptsLeft <= 0)
            {
                MessageBox.Show("Game Over! Je hebt geen pogingen meer.", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetGame();
                return;
            }

            string[] userCode = {
                ComboBox1.SelectedItem != null ? ComboBox1.SelectedItem.ToString() : "default",
                ComboBox2.SelectedItem != null ? ComboBox2.SelectedItem.ToString() : "default",
                ComboBox3.SelectedItem != null ? ComboBox3.SelectedItem.ToString() : "default",
                ComboBox4.SelectedItem != null ? ComboBox4.SelectedItem.ToString() : "default"
            };

            string feedback = GenerateFeedback(userCode);
            LogAttempt(userCode, feedback);

            CheckColor(Label1, userCode[0], 0);
            CheckColor(Label2, userCode[1], 1);
            CheckColor(Label3, userCode[2], 2);
            CheckColor(Label4, userCode[3], 3);

            if (userCode.SequenceEqual(generatedCode))
            {
                MessageBox.Show("Gefeliciteerd! Je hebt de code gekraakt!", "Winnaar", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetGame();
                return;
            }

            attemptsLeft--; // Decrease attempts by 1
            UpdateTitle(); // Update the title with the remaining attempts

            if (attemptsLeft == 0)
            {
                MessageBox.Show($"Game Over! De code was: {string.Join(", ", generatedCode)}", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetGame();
            }
        }

        private string GenerateFeedback(string[] userCode)
        {
            int correctPosition = 0;
            int correctColorWrongPosition = 0;

            for (int i = 0; i < 4; i++)
            {
                if (userCode[i] == generatedCode[i])
                {
                    correctPosition++;
                }
                else if (generatedCode.Contains(userCode[i]))
                {
                    correctColorWrongPosition++;
                }
            }

            return $"Rood: {correctPosition}, Wit: {correctColorWrongPosition}";
        }

        private void LogAttempt(string[] userCode, string feedback)
        {
            string attempt = $"Poging: {string.Join(", ", userCode)} | Feedback: {feedback}";
            AttemptsListBox.Items.Add(attempt);
        }

        private void CheckColor(Label label, string selectedColor, int position)
        {
            if (selectedColor == generatedCode[position])
            {
                label.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                label.BorderThickness = new Thickness(3);
            }
            else if (generatedCode.Contains(selectedColor))
            {
                label.BorderBrush = new SolidColorBrush(Colors.Wheat);
                label.BorderThickness = new Thickness(3);
            }
            else
            {
                label.BorderBrush = Brushes.Transparent;
                label.BorderThickness = new Thickness(0);
            }
        }

        private void ResetGame()
        {
            // Reset the game to play again
            GenerateRandomCode();
            attemptsLeft = 10;
            UpdateTitle();

            // Clear ComboBoxes, Labels, and Attempts ListBox
            ComboBox1.SelectedItem = null;
            ComboBox2.SelectedItem = null;
            ComboBox3.SelectedItem = null;
            ComboBox4.SelectedItem = null;

            Label1.BorderBrush = Brushes.Transparent;
            Label2.BorderBrush = Brushes.Transparent;
            Label3.BorderBrush = Brushes.Transparent;
            Label4.BorderBrush = Brushes.Transparent;

            Label1.Background = Brushes.LightGray;
            Label2.Background = Brushes.LightGray;
            Label3.Background = Brushes.LightGray;
            Label4.Background = Brushes.LightGray;

            AttemptsListBox.Items.Clear();
        }
    }
}
