using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using StockRoom.DataBase;
using StockRoom.Objects;

namespace StockRoom
{
    public partial class MainWindow : Window
    {
        public DBOperations db_operations;
        public ObservableCollection<Pallet> pallets;
        public ObservableCollection<Pallet> pallets3;
        public ObservableCollection<Box> boxes;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            this.Activated += MainWindow_Activated;
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            ExtractData();//извлекаем данные из БД
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ExtractData();
            AllPalletsBtn_Click(sender, e);//загружаем таблицу на страницу
        }

        private void ExtractData()//метод извлечения данных из БД и запись их в коллекции
        {
            db_operations = new DBOperations();
            pallets = new ObservableCollection<Pallet>();
            pallets3 = new ObservableCollection<Pallet>();
            boxes = new ObservableCollection<Box>();

            var tempPallets = db_operations.GetAllPallets();
            var tempPallets3 = db_operations.Get3Pallets();
            var tembBoxes = db_operations.GetAllBoxes();

            if (tempPallets.Count != 0)
            {
                foreach (var p in tempPallets)
                {
                    foreach (var i in p)
                    {
                        pallets.Add(i);
                    }
                }
            }

            if (tempPallets3.Count != 0)
            {
                foreach (var p in tempPallets3)
                {
                    foreach (var i in p)
                    {
                        pallets3.Add(i);
                    }
                }
            }

            if (tembBoxes.Count != 0)
            {
                foreach (var box in tembBoxes)
                {
                    boxes.Add(box);
                }
            }
        }

        private void AddPalletBtn_Click(object sender, RoutedEventArgs e)//открываем окно для добавления паллета
        {
            AddPallet pallet = new AddPallet();

            pallet.Owner = this;
            pallet.ShowDialog();
        }

        private void AddBoxBtn_Click(object sender, RoutedEventArgs e)//открываем окно для добавления коробки 
        {
            AddBox box = new AddBox();

            box.Owner = this;
            box.ShowDialog();
        }

        private void AllBoxesBtn_Click(object sender, RoutedEventArgs e)//добавляем на страницу таблицу с коробками
        {
            DataGrid table = new DataGrid();
            table.AutoGenerateColumns = false;
            table.RowBackground = new SolidColorBrush(Colors.LightBlue);
            table.IsReadOnly = true;
            table.Margin = new Thickness(20);
            table.ItemsSource = boxes;

            AddColumn(table, "ID", "Id");
            AddColumn(table, "Усл. имя коробки", "Name");
            AddColumn(table, "Длина коробки", "Lenght");
            AddColumn(table, "Ширина коробки", "Width");
            AddColumn(table, "Высота коробки", "Height");
            AddColumn(table, "Вес коробки", "Weight");
            AddColumn(table, "Объем коробки", "Volume");
            AddColumn(table, "Дата изготовления", "DateStart");
            AddColumn(table, "Срок годности коробки", "DateEnd");
            AddColumn(table, "Место хранения", "Pallet_id");

            Grid.SetRow(table, 1);
            grid.Children.Add(table);
        }

        private void Pallets_3Btn_Click(object sender, RoutedEventArgs e)//добавляем на страницу таблицу из 3-х последних по срокам паллет
        {
            DataGrid table = new DataGrid();
            table.AutoGenerateColumns = false;
            table.RowBackground = new SolidColorBrush(Colors.LightBlue);
            table.IsReadOnly = true;
            table.Margin = new Thickness(20);
            table.ItemsSource = pallets3;

            AddColumn(table, "ID", "Id");
            AddColumn(table, "Усл. имя паллета", "Name");
            AddColumn(table, "Длина паллеты", "Lenght");
            AddColumn(table, "Ширина паллеты", "Width");
            AddColumn(table, "Высота паллеты", "Height");
            AddColumn(table, "Вес паллеты", "Weight");
            AddColumn(table, "Объем паллеты", "Volume");
            AddColumn(table, "Срок годности паллеты", "DateEnd");

            Grid.SetRow(table, 1);
            grid.Children.Add(table);
        }

        private void AllPalletsBtn_Click(object sender, RoutedEventArgs e)//добавляем на страницу таблицу паллет
        {
            DataGrid table = new DataGrid();
            table.AutoGenerateColumns = false;
            table.RowBackground = new SolidColorBrush(Colors.LightBlue);
            table.IsReadOnly = true;
            table.Margin = new Thickness(20);
            table.ItemsSource = pallets;

            AddColumn(table, "ID", "Id");
            AddColumn(table, "Усл. имя паллета", "Name");
            AddColumn(table, "Длина паллеты", "Lenght");
            AddColumn(table, "Ширина паллеты", "Width");
            AddColumn(table, "Высота паллеты", "Height");
            AddColumn(table, "Вес паллеты", "Weight");
            AddColumn(table, "Объем паллеты", "Volume");
            AddColumn(table, "Срок годности паллеты", "DateEnd");

            Grid.SetRow(table, 1);
            grid.Children.Add(table);
        }

        private void AddColumn(DataGrid table, string headerName, string bindingName)//метод добавления в даблицу новой колонки
        {
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = headerName;
            column.Binding = new Binding(bindingName);
            table.Columns.Add(column);
        }
    }
}
