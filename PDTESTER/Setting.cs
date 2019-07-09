using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDTESTER
{
    public class SettingContext : DbContext
    {
        public DbSet<SWSetting> SWSettings { get; set; }
        public DbSet<ValueSetting> ValueSettings { get; set; }
        public DbSet<JigModel> JigModels { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=PDTESTER11.db");
        }
    }

    public class SWSetting
    {
        public uint SWSettingID { get; set; }
        public bool UD3Test { get; set; } = true;
        public bool UO3Test { get; set; } = true;
        public bool UO2Test { get; set; } = true;
        public bool PDCTest { get; set; } = true;
        public bool LOADTest { get; set; } = true;
        public bool VCONNTest { get; set; } = true;
        public bool SBUTest { get; set; } = true;
    }
    public class ValueSetting
    {
        public uint ValueSettingID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class Database
    {
        public static string GetDbValue(string Key)
        {
            using (var Db = new SettingContext())
            {
                var DbValue = Db.ValueSettings.Where(x => x.Key == Key).FirstOrDefault();
                if (DbValue != null)
                {
                    return (DbValue.Value);
                }
                else
                {
                    Db.Add(new ValueSetting() { Key = Key });
                    Db.SaveChanges();
                }
                return null;
            }
        }

        public static void SetDbValue(string Key, string Value)
        {
            using (var Db = new SettingContext())
            {
                var DbValue = Db.ValueSettings.Where(x => x.Key == Key).FirstOrDefault();
                if (DbValue != null)
                {
                    DbValue.Value = Value;
                    Db.SaveChanges();
                }
                else
                {
                    Db.Add(new ValueSetting() { Key = Key, Value = Value });
                    Db.SaveChanges();
                }
            }
        }
    }
}
