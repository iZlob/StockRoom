using StockRoom.Objects;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace StockRoom.DataBase
{
    public class DBOperations
    {
        public DB db { get; set; }
        private SQLiteConnection _connection;
        private SQLiteCommand _command;
        public static string nameDB = "StockRoomDB.sqlite";

        public DBOperations()
        {
            //создаем БД если она не создана
            if (!File.Exists(nameDB))
            {
                SQLiteConnection.CreateFile(nameDB);

                using (_connection = new SQLiteConnection($"Data Source={nameDB};"))
                {
                    _connection.Open();

                    using (_command = new SQLiteCommand())
                    {
                        _command.Connection = _connection;

                        CreatePalletsDbo("Pallets");
                        CreateBoxesDbo("Boxes", "Pallets");
                    }
                }
            }

            //получаем контекст БД
            db = new DB();
        }

        //приватный метод для создания таблицы паллет
        private void CreatePalletsDbo(string nameDbo)
        {
            _command.CommandText = $"CREATE TABLE IF NOT EXISTS {nameDbo} " +
                                   "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                   "Name TEXT, " +
                                   "Lenght FLOAT, " +
                                   "Width FLOAT, " +
                                   "Height FLOAT, " +
                                   "Weight FLOAT, " +
                                   "Volume FLOAT, " +
                                   "DateEnd TEXT);";
            _command.ExecuteNonQuery();
        }

        //приватный метод для создания таблицы коробок
        private void CreateBoxesDbo(string nameDbo, string palletDbo)
        {
            _command.CommandText = $"CREATE TABLE IF NOT EXISTS {nameDbo} " +
                                   "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                   "Name TEXT, " +
                                   "Lenght FLOAT, " +
                                   "Width FLOAT, " +
                                   "Height FLOAT, " +
                                   "Weight FLOAT, " +
                                   "Volume FLOAT, " +
                                   "DateStart TEXT, " +
                                   "DateEnd TEXT, " +
                                   $"Pallet_id INTEGER REFERENCES {palletDbo} (Id));";
            _command.ExecuteNonQuery();
        }

        //Получение всех паллет сгруппированных по датам и отсортированных по весу внк=утри каждой группы
        public List<IOrderedEnumerable<Pallet>> GetAllPallets()
        {
            List<IOrderedEnumerable<Pallet>> AllPallets = new List<IOrderedEnumerable<Pallet>>();

            if (db.pallets != null)
                AllPallets = db.pallets.GroupBy(item => item.DateEnd).
                                        OrderBy(group => group.Key).
                                        Select(group => group.OrderBy(item => item.Weight)).ToList();

            return AllPallets;
        }

        //Получение трех первых паллет сгруппированных по дате в порядке убывания и отсортированных внутри группы по объему
        public List<IOrderedEnumerable<Pallet>> Get3Pallets()
        {
            List<IOrderedEnumerable<Pallet>> Pallets3 = new List<IOrderedEnumerable<Pallet>>();

            if (db.pallets != null)
                Pallets3 = db.pallets.GroupBy(item => item.DateEnd).
                                      OrderByDescending(group => group.Key).
                                      Select(group => group.OrderBy(item => item.Volume)).Take(3).ToList();

            return Pallets3;
        }

        //Получение всех коробок
        public List<Box> GetAllBoxes()
        {
            List<Box> boxes = new List<Box>();

            boxes = db.boxes.OrderByDescending(item => item.DateEnd).ToList();

            return boxes;
        }

        //Обновление данных паллета
        public void UpdatePalette(int id, float weight, float volume, DateTime date)
        {
            Pallet pallet = db.pallets.Where(item => item.Id == id).FirstOrDefault();

            pallet.Weight += weight;
            pallet.Volume += volume;

            if (date < pallet.DateEnd)
            {
                pallet.DateEnd = date;
            }

            db.SaveChanges();
        }

        //Добавление паллетта
        public void AddPallet(Pallet pallet)
        {
            db.pallets.Add(pallet);
            db.SaveChanges();
        }

        //Добавление коробки
        public void AddBox(Box box)
        {
            db.boxes.Add(box);
            db.SaveChanges();
        }
    }
}
