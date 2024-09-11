using StockRoom.Objects;
using System;
using System.Windows;
using System.Windows.Media;

namespace StockRoom
{
    public partial class AddPallet : Window
    {
        private MainWindow _mainWindow;

        public AddPallet()
        {
            InitializeComponent();

            this.Loaded += AddPallet_Loaded;
        }

        private void AddPallet_Loaded(object sender, RoutedEventArgs e)
        {
            _mainWindow = this.Owner as MainWindow;//получаем родительское окно
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)//подтверждение ввода
        {
            Pallet newPallet = new Pallet();
            float height = Convert.ToSingle(HPallet.Text.Replace('.', ','));
            float lenght = Convert.ToSingle(LPallet.Text.Replace('.', ','));
            float width = Convert.ToSingle(WPallet.Text.Replace('.', ','));

            int count = 0;

            if (_mainWindow.pallets != null)//вычисляем количество паллетов на складе
            {
                count = _mainWindow.pallets.Count;
            }

            newPallet.Name += count;//присваиваем имя новому паллету

            if (lenght > 0)//проверем корректность ввода и присваеваем значение
            {
                newPallet.Lenght = lenght;
            }
            else
            {
                LPallet.Background = new SolidColorBrush(Colors.LightCoral);
                LPallet.Text = "";
                return;
            }

            if (width > 0)//проверем корректность ввода и присваеваем значение
            {
                newPallet.Width = width;
            }
            else
            {
                WPallet.Background = new SolidColorBrush(Colors.LightCoral);
                WPallet.Text = "";
                return;
            }

            if (height > 0)//проверем корректность ввода и присваеваем значение
            {
                newPallet.Height = height;
            }
            else
            {
                HPallet.Background = new SolidColorBrush(Colors.LightCoral);
                HPallet.Text = "";
                return;
            }

            newPallet.Weight = 30;//начальный вес паллета

            _mainWindow.db_operations.AddPallet(newPallet);//добавляем паллет на склад

            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
