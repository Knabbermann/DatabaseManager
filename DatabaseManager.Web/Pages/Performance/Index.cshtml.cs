using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Globalization;

namespace DatabaseManager.Web.Pages.Performance
{
    public class IndexModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, IEnumerable<Models.Performance> performances, IConfiguration configuration) : CustomPageModel<IndexModel>(configuration)
    {
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int RowCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public IEnumerable<Models.Performance> Performances { get; set; } = performances;
        [BindProperty]
        public int Runs { get; set; }
        [BindProperty]
        public string Table { get; set; }
        [BindProperty]
        public string Type { get; set; }

        public void OnGet(int pageNumber = 1)
        {
            CurrentPage = pageNumber;
            Performances = unitOfWork.Performance.GetPagedEntities(x => x.Id, pageNumber, 10);
            RowCount = unitOfWork.Performance.GetRowCount();
        }

        public void OnPostAnalyze()
        {
            var results = new List<TimeSpan>();
            var sqlCommand = GetSqlCommand();
            using var dbConnection = unitOfWork.GetDbConnection();
            for (var i = 0; i < Runs; i++)
            {
                var startTime = DateTime.Now;
                dbConnection.Open();
                using var command = dbConnection.CreateCommand();
                command.CommandText = sqlCommand;
                command.ExecuteNonQuery();
                dbConnection.Close();
                var endTime = DateTime.Now;
                results.Add(endTime-startTime);
            }
            var totalSeconds = results.Sum(ts => ts.TotalSeconds);
            var averageSeconds = totalSeconds / results.Count;
            var cPerformance = new Models.Performance
            {
                Runs = Runs,
                Table = Table,
                Type = Type,
                AverageSeconds = averageSeconds
            };
            unitOfWork.Performance.Add(cPerformance);
            unitOfWork.SaveChanges();
            toastNotification.AddSuccessToastMessage($"Average duration: {averageSeconds} seconds");
            OnGet();
        }

        private string GetSqlCommand()
        {
            if (Table.Equals("Customer"))
            {
                var cFirstname = GetRandomFromList("firstNames");
                switch (Type)
                {
                    case "Select":
                        return $"SELECT * FROM Customers WHERE FirstName LIKE '%{cFirstname}%' AND GcRecord is NULL";
                    case "Add":
                    {
                        var cLastName = GetRandomFromList("lastNames");
                        var cEmail = $"{cFirstname}.{cLastName}" + GetRandomMailSuffix();
                        return $"INSERT INTO Customers (Id, FirstName, LastName, Email, IsActive) VALUES ('{Guid.NewGuid()}','{cFirstname}', '{cLastName}', '{cEmail}', '{1}')";
                    }
                    case "Update":
                    {
                        var uFirstName = GetRandomFromList("firstNames");
                        return $"UPDATE Customers SET FirstName = '{uFirstName}' WHERE FirstName LIKE '%{cFirstname}%' AND GcRecord is NULL";
                    }
                    case "Delete":
                        return $"DELETE FROM Customers WHERE FirstName LIKE '%{cFirstname}%' AND GcRecord is NULL";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (Table.Equals("Order"))
            {
                var cCustomerId = GetRandomFromIds(unitOfWork.Customer.GetAllIds(1));
                switch (Type)
                {
                    case "Select":
                        return $"SELECT * FROM Orders WHERE CustomerId = '{cCustomerId}' AND GcRecord is NULL";
                    case "Add":
                    {
                        var cOrderDate = GetRandomDateTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        var cTotalAmount = GetRandomDecimal().ToString(CultureInfo.InvariantCulture);
                        return $"INSERT INTO Orders (Id, CustomerId, OrderDate, TotalAmount, Status) VALUES ('{Guid.NewGuid()}','{cCustomerId}', '{cOrderDate}', '{cTotalAmount}', 'Pending')";
                    }
                    case "Update":
                    {
                        var uCustomerId = GetRandomFromIds(unitOfWork.Customer.GetAllIds());
                        return $"UPDATE Orders SET CustomerId = '{uCustomerId}' WHERE CustomerId = '{cCustomerId}' AND GcRecord is NULL";
                    }
                    case "Delete":
                        return $"DELETE FROM Orders WHERE CustomerId = '{cCustomerId}' AND GcRecord is NULL";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (Table.Equals("Payment"))
            {
                var cCustomerId = GetRandomFromIds(unitOfWork.Customer.GetAllIds(1));
                switch (Type)
                {
                    case "Select":
                        return $"SELECT * FROM Payments WHERE CustomerId = '{cCustomerId}' AND GcRecord is NULL";
                    case "Add":
                    {
                        var cPaymentDate = GetRandomDateTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        var cAmount = GetRandomDecimal().ToString(CultureInfo.InvariantCulture);
                        return $"INSERT INTO Payments (Id, CustomerId, PaymentDate, Amount, PaymentMethod) VALUES ('{Guid.NewGuid()}','{cCustomerId}', '{cPaymentDate}', '{cAmount}', 'BloomPay')";
                    }
                    case "Update":
                    {
                        var uCustomerId = GetRandomFromIds(unitOfWork.Customer.GetAllIds());
                        return $"UPDATE Payments SET CustomerId = '{uCustomerId}' WHERE CustomerId = '{cCustomerId}' AND GcRecord is NULL";
                    }
                    case "Delete":
                        return $"DELETE FROM Payments WHERE CustomerId = '{cCustomerId}' AND GcRecord is NULL";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (Table.Equals("Product"))
            {
                var cName = GetRandomFromList("productNames");
                switch (Type)
                {
                    case "Select":
                        return $"SELECT * FROM Products WHERE Name LIKE '%{cName}%' AND GcRecord is NULL";
                    case "Add":
                    {
                        var cPrice = GetRandomDecimal().ToString(CultureInfo.InvariantCulture);
                        var cStock = GetRandomInt(1, 9999);
                        return $"INSERT INTO Products (Id, Name, Price, Stock) VALUES ('{Guid.NewGuid()}','{cName}', '{cPrice}', '{cStock}')";
                    }
                    case "Update":
                    {
                        var uName = GetRandomFromList("productNames");
                        return $"UPDATE Products SET Name = '{uName}' WHERE Name LIKE '%{cName}%' AND GcRecord is NULL";
                    }
                    case "Delete":
                        return $"DELETE FROM Products WHERE Name LIKE '%{cName}%' AND GcRecord is NULL";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (Table.Equals("Review"))
            {
                var cCustomerId = GetRandomFromIds(unitOfWork.Customer.GetAllIds(1));
                switch (Type)
                {
                    case "Select":
                        return $"SELECT * FROM Reviews WHERE CustomerId = '{cCustomerId}' AND GcRecord is NULL";
                    case "Add":
                    {
                        var cReviewDate = GetRandomDateTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        var cRating = GetRandomInt(1, 5);
                        return $"INSERT INTO Reviews (Id, CustomerId, ReviewDate, Rating, Content) VALUES ('{Guid.NewGuid()}','{cCustomerId}', '{cReviewDate}', '{cRating}', 'Hervorragende App! Sehr benutzerfreundlich und effizient.')";
                    }
                    case "Update":
                    {
                        var uCustomerId = GetRandomFromIds(unitOfWork.Customer.GetAllIds());
                        return $"UPDATE Reviews SET CustomerId = '{uCustomerId}' WHERE CustomerId = '{cCustomerId}' AND GcRecord is NULL";
                    }
                    case "Delete":
                        return $"DELETE FROM Reviews WHERE CustomerId = '{cCustomerId}' AND GcRecord is NULL";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            throw new NotImplementedException();
        }
    }
}
