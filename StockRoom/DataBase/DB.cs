using StockRoom.Objects;
using System.Data.Entity;

namespace StockRoom.DataBase
{
    public class DB : DbContext
    {
        public DbSet<Box> boxes { get; set; }
        public DbSet<Pallet> pallets { get; set; }

        public DB() : base("storage")
        {
            
        }
    }
}
