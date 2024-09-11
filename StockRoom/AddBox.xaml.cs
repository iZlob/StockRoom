using StockRoom.Objects;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StockRoom
{

    public partial class AddBox : Window
    {
        private MainWindow _mainWindow;

        public AddBox()
        {
            InitializeComponent();

            this.Loaded += AddBox_Loaded;
        }

        private void AddBox_Loaded(object sender, RoutedEventArgs e)
        {
            _mainWindow = this.Owner as MainWindow;//Получаем родительское окно

            foreach(var item in _mainWindow.pallets)//Добавляем имена паллет на КомбоБокс
            {
                PalletCombo.Items.Add(item.Name);
            }
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)//подтверждаем ввод данных
        {
            Box newBox = new Box();
            float Lenght = Convert.ToSingle(LBox.Text.Replace('.', ','));
            float Width = Convert.ToSingle(WBox.Text.Replace('.', ','));
            float Height = Convert.ToSingle(HBox.Text.Replace('.', ','));
            float Weight = Convert.ToSingle(MBox.Text.Replace('.', ','));
            DateTime date = Convert.ToDateTime(Date.Text);            
            string palletName = PalletCombo.Text;

            int count = 0;

            if (_mainWindow.boxes != null)//выясняем коллическво коробок на складе всего
            {
                count = _mainWindow.boxes.Count;
            }

            newBox.Name += count;//присваиваем новой коробке условное имя

            foreach (var item in _mainWindow.pallets)//привязываем коробку к паллету
            {
                if (item.Name == palletName)
                {
                    newBox.Pallet_id = item.Id;
                    break;
                }
            }

            if (Lenght > 0 && Lenght < _mainWindow.pallets[newBox.Pallet_id - 1].Lenght)//проверяем корректность ввода данных
            {
                newBox.Lenght = Lenght;
            }
            else
            {
                LBox.Background = new SolidColorBrush(Colors.LightCoral);
                LBox.Text = "";
                return;
            }

            if (Width > 0 && Width < _mainWindow.pallets[newBox.Pallet_id - 1].Width)//проверяем корректность ввода данных
            {
                newBox.Width = Width;
            }
            else
            {
                WBox.Background = new SolidColorBrush(Colors.LightCoral);
                WBox.Text = "";
                return;
            }

            if (Height > 0)//проверяем корректность ввода данных
            {
                newBox.Height = Height;
            }
            else
            {
                HBox.Background = new SolidColorBrush(Colors.LightCoral);
                HBox.Text = "";
                return;
            }

            if (Weight > 0)//проверяем корректность ввода данных
            {
                newBox.Weight = Weight;
            }
            else
            {
                MBox.Background = new SolidColorBrush(Colors.LightCoral);
                MBox.Text = "";
                return;
            }

            newBox.Volume = Width * Lenght * Height;//вычисляем объем коробки

            foreach (var radio in Radios.Children)//вычисляем и устанавливаем даты изготовления и годности
            {
                var _radio = radio as RadioButton;

                if (_radio.IsChecked == true)
                {
                    if (_radio.Content.ToString() == "дата изг.")
                    {
                        newBox.DateStart = date;
                        newBox.DateEnd = date.AddDays(100);
                    }
                    else
                    {
                        newBox.DateEnd = date;
                        newBox.DateStart = DateTime.MinValue;
                    }
                }
            }         

            _mainWindow.db_operations.AddBox(newBox);//добавляем в БД коробку
            _mainWindow.db_operations.UpdatePalette(newBox.Pallet_id, Weight, newBox.Volume, newBox.DateEnd);//обновляем данные палетта

            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
