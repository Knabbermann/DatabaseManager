using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseManager.Web.Pages.Generators.Customer
{
    public class IndexModel(IUnitOfWork unitOfWork) : CustomPageModel<IndexModel>
    {
        [BindProperty]
        public int Quantity { get; set; }
        [BindProperty]
        public bool FillOptionalProperties { get; set; }
        [BindProperty]
        public bool SetReferences { get; set; }
        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string MiddleName { get; set; }
        [BindProperty]
        public int MiddleNameChance { get; set; }
        [BindProperty]
        public string SecondaryEmail { get; set; }
        [BindProperty]
        public int SecondaryEmailChance { get; set; }
        [BindProperty]
        public string PhoneNumber { get; set; }
        [BindProperty]
        public int PhoneNumberChance { get; set; }
        [BindProperty]
        public string SecondaryPhoneNumber { get; set; }
        [BindProperty]
        public int SecondaryPhoneNumberChance { get; set; }
        [BindProperty]
        public string DateOfBirth { get; set; }
        [BindProperty]
        public int DateOfBirthChance { get; set; }
        [BindProperty]
        public string Gender { get; set; }
        [BindProperty]
        public int GenderChance { get; set; }
        [BindProperty]
        public string Nationality { get; set; }
        [BindProperty]
        public int NationalityChance { get; set; }
        [BindProperty]
        public string AddressLine1 { get; set; }
        [BindProperty]
        public int AddressLine1Chance { get; set; }
        [BindProperty]
        public string AddressLine2 { get; set; }
        [BindProperty]
        public int AddressLine2Chance { get; set; }
        [BindProperty]
        public string City { get; set; }
        [BindProperty]
        public int CityChance { get; set; }
        [BindProperty]
        public string State { get; set; }
        [BindProperty]
        public int StateChance { get; set; }
        [BindProperty]
        public string Country { get; set; }
        [BindProperty]
        public int CountryChance { get; set; }
        [BindProperty]
        public string PostalCode { get; set; }
        [BindProperty]
        public int PostalCodeChance { get; set; }
        [BindProperty]
        public bool IsActive { get; set; }
        [BindProperty]
        public int IsActiveChance { get; set; }
        [BindProperty]
        public string LastOnline { get; set; }
        [BindProperty]
        public int LastOnlineChance { get; set; }
        [BindProperty]
        public string CreatedAt { get; set; }
        [BindProperty]
        public int CreatedAtChance { get; set; }
        [BindProperty]
        public string UpdatedAt { get; set; }
        [BindProperty]
        public int UpdatedAtChance { get; set; }
        [BindProperty]
        public string PreferredLanguage { get; set; }
        [BindProperty]
        public int PreferredLanguageChance { get; set; }
        [BindProperty]
        public string PreferredPaymentMethod { get; set; }
        [BindProperty]
        public int PreferredPaymentMethodChance { get; set; }
        [BindProperty]
        public string GcRecord { get; set; }
        [BindProperty]
        public int GcRecordChance { get; set; }

        public void OnPostGenerate()
        {
            var random = new Random();

            for (var i = 0; i < Quantity; i++)
            {
                var cCustomer = new Models.Customer
                {
                    FirstName = FirstName.Equals("Random") ? GetRandomString() : GetRandomFromList("firstNames"),
                    LastName = FirstName.Equals("Random") ? GetRandomString() : GetRandomFromList("lastNames"),
                };
                cCustomer.Email = Email.Equals("Random")
                    ? GetRandomString()
                    : $"{cCustomer.FirstName}.{cCustomer.LastName}" + GetRandomMailSuffix();
                if (FillOptionalProperties)
                {
                    if (random.Next(0, 100) <= MiddleNameChance)
                        cCustomer.MiddleName = MiddleName.Equals("Random") ? GetRandomString() : GetRandomFromList("firstNames");
                    if (random.Next(0, 100) <= SecondaryEmailChance)
                        cCustomer.SecondaryEmail = SecondaryEmail.Equals("Random") ? GetRandomString() : $"{cCustomer.FirstName}.{cCustomer.LastName}" + GetRandomMailSuffix();
                    if (random.Next(0, 100) <= PhoneNumberChance)
                        cCustomer.PhoneNumber = "+" + GetRandomInt();
                    if (random.Next(0, 100) <= SecondaryPhoneNumberChance)
                        cCustomer.SecondaryPhoneNumber = "+" + GetRandomInt();
                    if (random.Next(0, 100) <= DateOfBirthChance)
                        cCustomer.DateOfBirth = DateOfBirth.Equals("Random") ? GetRandomDateTime() : DateTime.Now;
                    if (random.Next(0, 100) <= GenderChance)
                        cCustomer.Gender = Gender.Equals("Random") ? GetRandomString() : GetRandomFromList("genders");
                    if (random.Next(0, 100) <= NationalityChance)
                        cCustomer.Nationality = Nationality.Equals("Random") ? GetRandomString() : GetRandomFromList("countries");
                    if (random.Next(0, 100) <= AddressLine1Chance)
                        cCustomer.AddressLine1 = AddressLine1.Equals("Random") ? GetRandomString() : GetRandomFromList("addressLines");
                    if (random.Next(0, 100) <= AddressLine2Chance)
                        cCustomer.AddressLine2 = AddressLine2.Equals("Random") ? GetRandomString() : GetRandomFromList("addressLines");
                    if (random.Next(0, 100) <= CityChance)
                        cCustomer.City = City.Equals("Random") ? GetRandomString() : GetRandomFromList("cities");
                    if (random.Next(0, 100) <= StateChance)
                        cCustomer.State = State.Equals("Random") ? GetRandomString() : GetRandomFromList("states");
                    if (random.Next(0, 100) <= CountryChance)
                        cCustomer.Country = Country.Equals("Random") ? GetRandomString() : GetRandomFromList("countries");
                    if (random.Next(0, 100) <= PostalCodeChance)
                        cCustomer.PostalCode = PostalCode.Equals("Random") ? GetRandomString() : GetRandomInt(10000,100000).ToString();
                    if (random.Next(0, 100) <= IsActiveChance)
                        cCustomer.IsActive = GetRandomBool();
                    if (random.Next(0, 100) <= LastOnlineChance)
                        cCustomer.LastOnline = LastOnline.Equals("Random") ? GetRandomDateTime() : DateTime.Now;
                    if (random.Next(0, 100) <= CreatedAtChance)
                        cCustomer.CreatedAt = CreatedAt.Equals("Random") ? GetRandomDateTime() : DateTime.Now;
                    if (random.Next(0, 100) <= UpdatedAtChance)
                        cCustomer.UpdatedAt = UpdatedAt.Equals("Random") ? GetRandomDateTime() : DateTime.Now;
                    if (random.Next(0, 100) <= PreferredLanguageChance)
                        cCustomer.PreferredLanguage = PreferredLanguage.Equals("Random") ? GetRandomString() : GetRandomFromList("preferredLanguages");
                    if (random.Next(0, 100) <= PreferredPaymentMethodChance)
                        cCustomer.PreferredPaymentMethod = PreferredPaymentMethod.Equals("Random") ? GetRandomString() : GetRandomFromList("preferredPaymentMethods");
                    if (random.Next(0, 100) <= GcRecordChance)
                        cCustomer.GcRecord = GcRecord.Equals("Random") ? GetRandomDateTime() : DateTime.Now;
                }
                unitOfWork.Customer.Add(cCustomer);
            }
            unitOfWork.SaveChanges();
        }
    }
}
