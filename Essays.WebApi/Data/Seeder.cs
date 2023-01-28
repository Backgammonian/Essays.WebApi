using Essays.WebApi.Models;

namespace Essays.WebApi.Data
{
    public sealed class Seeder
    {
        private readonly DataContext _dataContext;

        public Seeder(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Seed()
        {
            var countries = new List<Country>()
            {
                new Country()
                {
                    CountryName = "Country 1",
                    CountryAbbreviation = "cto"
                },

                new Country()
                {
                    CountryName = "Country 2",
                    CountryAbbreviation = "ctt"
                },

                new Country()
                {
                    CountryName = "Country 3",
                    CountryAbbreviation = "ctth"
                },
            };

            var authors = new List<Author>()
            {
                new Author()
                {
                    AuthorId = "1",
                    FirstName = "First name 1",
                    LastName = "Last name 1"
                },

                new Author()
                {
                    AuthorId = "2",
                    FirstName = "First name 2",
                    LastName = "Last name 2"
                },

                new Author()
                {
                    AuthorId = "3",
                    FirstName = "First name 3",
                    LastName = "Last name 3"
                },
            };

            var countriesOfAuthors = new List<CountriesOfAuthors>()
            {
                new CountriesOfAuthors()
                {
                    AuthorId = authors[0].AuthorId,
                    CountryAbbreviation = countries[0].CountryAbbreviation
                },

                new CountriesOfAuthors()
                {
                    AuthorId = authors[1].AuthorId,
                    CountryAbbreviation = countries[0].CountryAbbreviation
                },

                new CountriesOfAuthors()
                {
                    AuthorId = authors[2].AuthorId,
                    CountryAbbreviation = countries[2].CountryAbbreviation
                },
            };

            var subjectCategories = new List<SubjectCategory>()
            {
                new SubjectCategory()
                {
                    SubjectCategoryId = "1",
                    Name = "Classic literature"
                },

                new SubjectCategory()
                {
                    SubjectCategoryId = "2",
                    Name = "Non-fiction"
                },

                new SubjectCategory()
                {
                    SubjectCategoryId = "3",
                    Name = "Sci-Fi"
                },

                new SubjectCategory()
                {
                    SubjectCategoryId = "4",
                    Name = "Fantasy"
                },
            };

            var subjects = new List<Subject>()
            {
                new Subject()
                {
                    SubjectId = "1",
                    Name = "Subject 1",
                    Description = "Subject 1 description"
                },

                new Subject()
                {
                    SubjectId = "2",
                    Name = "Subject 2",
                    Description = "Subject 2 description"
                },

                new Subject()
                {
                    SubjectId = "3",
                    Name = "Subject 3",
                    Description = "Subject 3 description"
                },

                new Subject()
                {
                    SubjectId = "4",
                    Name = "Subject 4",
                    Description = "Subject 4 description"
                },

                new Subject()
                {
                    SubjectId = "5",
                    Name = "Subject 5",
                    Description = "Subject 5 description"
                },
            };

            var essays = new List<Essay>()
            {
                new Essay()
                {
                    EssayId = "1",
                    Title = "Essay 1",
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
                        "Sed ultricies leo vel blandit condimentum.",
                },

                new Essay()
                {
                    EssayId = "2",
                    Title = "Essay 2",
                    Content = "Cras at finibus lacus, sed faucibus lorem. Etiam tempor et risus vitae lobortis.",
                },

                new Essay()
                {
                    EssayId = "3",
                    Title = "Essay 3",
                    Content = "Fusce sit amet nibh magna. Sed a pretium dui, sit amet finibus tellus.",
                },

                new Essay()
                {
                    EssayId = "4",
                    Title = "Essay 4",
                    Content = "Suspendisse ac blandit metus, eu facilisis enim. Sed porttitor lorem massa.",
                },
            };

            var essaysAboutSubjects = new List<EssaysAboutSubjects>()
            {
                new EssaysAboutSubjects()
                {
                    EssayId = essays[0].EssayId,
                    SubjectId = subjects[0].SubjectId
                },

                new EssaysAboutSubjects()
                {
                    EssayId = essays[0].EssayId,
                    SubjectId = subjects[1].SubjectId
                },

                new EssaysAboutSubjects()
                {
                    EssayId = essays[1].EssayId,
                    SubjectId = subjects[1].SubjectId
                },

                new EssaysAboutSubjects()
                {
                    EssayId = essays[2].EssayId,
                    SubjectId = subjects[1].SubjectId
                },

                new EssaysAboutSubjects()
                {
                    EssayId = essays[2].EssayId,
                    SubjectId = subjects[2].SubjectId
                },

                new EssaysAboutSubjects()
                {
                    EssayId = essays[3].EssayId,
                    SubjectId = subjects[0].SubjectId
                },

                new EssaysAboutSubjects()
                {
                    EssayId = essays[3].EssayId,
                    SubjectId = subjects[4].SubjectId
                },

                new EssaysAboutSubjects()
                {
                    EssayId = essays[3].EssayId,
                    SubjectId = subjects[3].SubjectId
                },
            };

            _dataContext.Database.EnsureCreated();

            await _dataContext.Countries.AddRangeAsync(countries);
            await _dataContext.Authors.AddRangeAsync(authors);
            await _dataContext.CountriesOfAuthors.AddRangeAsync(countriesOfAuthors);
            await _dataContext.SubjectCategories.AddRangeAsync(subjectCategories);
            await _dataContext.Subjects.AddRangeAsync(subjects);
            await _dataContext.Essays.AddRangeAsync(essays);
            await _dataContext.EssaysAboutSubjects.AddRangeAsync(essaysAboutSubjects);

            await _dataContext.SaveChangesAsync();
        }
    }
}
