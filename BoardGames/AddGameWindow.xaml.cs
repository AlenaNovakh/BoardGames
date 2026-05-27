using System;
using System.Windows;
using System.Windows.Controls;
using Model.Core;

namespace BoardGames.WPF
{
    public partial class AddGameWindow : Window
    {
        // Публичное свойство — CatalogWindow читает созданную игру отсюда после закрытия.
        // Помечаем как nullable (BoardGame?) — оно действительно null до нажатия кнопки.
        public BoardGame? CreatedGame { get; private set; }

        public AddGameWindow()
        {
            InitializeComponent();
        }

        // Вызывается при смене типа в ComboBox.
        // Скрываем/показываем нужное поле через Visibility.
        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // SelectedItem у ComboBox — это ComboBoxItem, у него Content — строка.
            string type = (cmbType.SelectedItem as ComboBoxItem)?.Content?.ToString();

            // В WPF Visibility.Visible = видимо, Visibility.Collapsed = скрыто И не занимает место.
            // Visibility.Hidden = скрыто, но занимает место (как opacity:0 в CSS).
            switch (type)
            {
                case "CardGame":
                    if (lblExtra != null)
                    {
                        lblExtra.Content = "Размер колоды:";
                        txtDeckSize.Visibility = Visibility.Visible;
                        cmbComplexity.Visibility = Visibility.Collapsed;
                        chkNoisy.Visibility = Visibility.Collapsed;
                    }
                    break;

                case "EuroGame":
                    if (lblExtra != null)
                    {
                        lblExtra.Content = "Сложность:";
                        txtDeckSize.Visibility = Visibility.Collapsed;
                        cmbComplexity.Visibility = Visibility.Visible;
                        chkNoisy.Visibility = Visibility.Collapsed;
                    }
                    break;

                case "PartyGame":
                    if (lblExtra != null)
                    {
                        lblExtra.Content = "Особенности:";
                        txtDeckSize.Visibility = Visibility.Collapsed;
                        cmbComplexity.Visibility = Visibility.Collapsed;
                        chkNoisy.Visibility = Visibility.Visible;
                    }
                    break;
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название игры.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtMinP.Text, out int minP) || minP < 1)
            {
                MessageBox.Show("Введите корректное минимальное число игроков (≥1).",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtMaxP.Text, out int maxP) || maxP < minP)
            {
                MessageBox.Show($"Максимум игроков должен быть ≥ {minP}.",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtAge.Text, out int age) || age < 0)
            {
                MessageBox.Show("Введите корректный возраст (≥0).",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string type = (cmbType.SelectedItem as ComboBoxItem)?.Content?.ToString();

            try
            {
                CreatedGame = type switch
                {
                    "CardGame" => new CardGame(
                        txtName.Text.Trim(), minP, maxP, age,
                        txtDesc.Text.Trim(), "",
                        int.TryParse(txtDeckSize.Text, out int deck) ? deck : 52),

                    "EuroGame" => new EuroGame(
                        txtName.Text.Trim(), minP, maxP, age,
                        txtDesc.Text.Trim(), "",
                        // Индекс выбранного элемента соответствует значению enum:
                        // 0=Low, 1=Medium, 2=High
                        (Complexity)(cmbComplexity.SelectedIndex)),

                    "PartyGame" => new PartyGame(
                        txtName.Text.Trim(), minP, maxP, age,
                        txtDesc.Text.Trim(), "",
                        chkNoisy.IsChecked == true), // IsChecked — bool? поэтому == true

                    _ => throw new InvalidOperationException("Неизвестный тип")
                };

                // DialogResult = true — сигнал для CatalogWindow: "пользователь нажал OK".
                // В WPF ShowDialog() возвращает bool? — именно это значение.
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}