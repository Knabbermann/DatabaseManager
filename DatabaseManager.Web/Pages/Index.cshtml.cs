using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseManager.Web.Pages
{
    public class IndexModel(IUnitOfWork unitOfWork) : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string OsVersion { get; set; }

        [BindProperty(SupportsGet = true)]
        public string OsArchitecture { get; set; }

        [BindProperty(SupportsGet = true)]
        public string OsCores { get; set; }

        [BindProperty(SupportsGet = true)]
        public string OsRam { get; set; }

        [BindProperty(SupportsGet = true)]
        public string DbVersion { get; set; }

        [BindProperty(SupportsGet = true)]
        public string DbCores { get; set; }

        [BindProperty(SupportsGet = true)]
        public string DbRam { get; set; }

        [BindProperty(SupportsGet = true)]
        public string DbSize { get; set; }

        public void OnGet()
        {
            var osData = GetDataForOs();
            OsVersion = osData[0] + " " + osData[1];
            OsArchitecture = osData[2];
            OsCores = osData[3];
            OsRam = osData[4];

            var dbData = GetDataForDb();
            DbVersion = dbData[0] + " " + dbData[1];
            DbSize = dbData[2];
            DbRam = dbData[3];
            DbCores = dbData[4];
        }

        private string[] GetDataForOs()
        {
            var result = new List<string>();

            using var dbConnection = unitOfWork.GetDbConnection();
            dbConnection.Open();
            using var command = dbConnection.CreateCommand();
            command.CommandText = @"
                SELECT host_distribution, host_release, host_architecture
                FROM sys.dm_os_host_info;";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(reader.GetString(0)); // host_distribution
                    result.Add(reader.GetString(1)); // host_release
                    result.Add(reader.GetString(2)); // host_architecture
                }
            }
            command.CommandText = @"
                SELECT CAST(cpu_count AS NVARCHAR(50)), FORMAT((physical_memory_kb / 1024.0 / 1024.0), 'N2') as physical_memory_gb
                FROM sys.dm_os_sys_info;";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(reader.GetString(0)); // cpu_count
                    result.Add(reader.GetString(1)); // physical_memory_kb
                }
            }
            return result.ToArray();
        }

        private string[] GetDataForDb()
        {
            var result = new List<string>();

            using var dbConnection = unitOfWork.GetDbConnection();
            dbConnection.Open();
            using var command = dbConnection.CreateCommand();
            command.CommandText = @"
                SELECT SERVERPROPERTY('ProductVersion'), SERVERPROPERTY('Edition');";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(reader.GetString(0)); // ProductVersion
                    result.Add(reader.GetString(1)); // Edition
                }
            }
            command.CommandText = @"
                SELECT FORMAT(SUM(size * 8.0 / 1024 / 1024), 'N2') AS TotalSizeInGB
                FROM sys.master_files;";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(reader.GetString(0)); // size
                }
            }

            command.CommandText = @"
                SELECT FORMAT((physical_memory_in_use_kb / 1024 / 1024), 'N2') AS MemoryUsageInGB 
                FROM sys.dm_os_process_memory;";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(reader.GetString(0)); // UsedMemory
                }
            }

            command.CommandText = @"
                SELECT CAST(COUNT(*) AS NVARCHAR(20)) AS NumberOfCores
                FROM sys.dm_os_schedulers 
                WHERE status = 'VISIBLE ONLINE';";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(reader.GetString(0)); // UsedCores
                }
            }

            return result.ToArray();
        }
    }
}
