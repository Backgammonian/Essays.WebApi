﻿namespace Essays.WebApi.Tests.Repositories
{
    public class SubjectRepositoryTests
    {
        private readonly TestDatabaseGenerator _dbGenerator;

        public SubjectRepositoryTests()
        {
            _dbGenerator = new TestDatabaseGenerator();
        }

        [Fact]
        public async Task SubjectRepository_GetSubjects_ReturnSuccess()
        {
            var repo = new SubjectRepository(await _dbGenerator.GetDatabase());

            var result = await repo.GetSubjects();

            result.Should().NotBeNull();
            result.Should().HaveCountGreaterThan(1);
            result.Should().BeAssignableTo<ICollection<Subject>>();
        }

        [Fact]
        public async Task SubjectRepository_GetSubjectsFromPage_ReturnSuccess()
        {
            var repo = new SubjectRepository(await _dbGenerator.GetDatabase());
            var page = 1;
            var size = 2;

            var result = await repo.GetSubjects(page, size);

            result.Should().NotBeNull();
            result.Should().HaveCountLessThanOrEqualTo(size);
            result.Should().BeAssignableTo<ICollection<Subject>>();
        }

        [Fact]
        public async Task SubjectRepository_GetSubject_ReturnSuccess()
        {
            var repo = new SubjectRepository(await _dbGenerator.GetDatabase());
            var subjectId = "1";

            var result = await repo.GetSubject(subjectId);

            result.Should().NotBeNull();
            result.Should().BeOfType<Subject>();
        }

        [Fact]
        public async Task SubjectRepository_GetCategoryOfSubject_ReturnSuccess()
        {
            var repo = new SubjectRepository(await _dbGenerator.GetDatabase());
            var subjectId = "1";

            var result = await repo.GetCategoryOfSubject(subjectId);

            result.Should().NotBeNull();
            result.Should().BeOfType<SubjectCategory>();
        }

        [Fact]
        public async Task SubjectRepository_GetEssaysAboutSubject_ReturnSuccess()
        {
            var repo = new SubjectRepository(await _dbGenerator.GetDatabase());
            var subjectId = "1";

            var result = await repo.GetEssaysAboutSubject(subjectId);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ICollection<Essay>>();
        }

        [Fact]
        public async Task SubjectRepository_DoesSubjectExist_ReturnSuccess()
        {
            var repo = new SubjectRepository(await _dbGenerator.GetDatabase());
            var subjectId = "1";

            var result = await repo.DoesSubjectExist(subjectId);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task SubjectRepository_CreateSubject_ReturnSuccess()
        {
            var repo = new SubjectRepository(await _dbGenerator.GetDatabase());
            var oldCount = (await repo.GetSubjects()).Count;
            var subject = new Subject()
            {
                SubjectId = "50",
                Name = "Subject name",
                Description = "Subject description",
                CategoryId = "1"
            };

            var result = await repo.CreateSubject(subject);
            var newCount = (await repo.GetSubjects()).Count;

            result.Should().BeTrue();
            newCount.Should().Be(oldCount + 1);
        }

        [Fact]
        public async Task SubjectRepository_UpdateSubject_ReturnSuccess()
        {
            var repo = new SubjectRepository(await _dbGenerator.GetDatabase());
            var subjectId = "1";
            var subject = await repo.GetSubjectTracking(subjectId);
            var newName = "Absolutely new subject name";
            var newDescription = "Absolutely new subject description";
            subject.Name = newName;
            subject.Description = newDescription;

            var result = await repo.UpdateSubject(subject);
            var changedEntity = await repo.GetSubject(subjectId);

            result.Should().BeTrue();
            changedEntity.Name.Should().Be(newName);
            changedEntity.Description.Should().Be(newDescription);
        }

        [Fact]
        public async Task SubjectRepository_DeleteSubject_ReturnSuccess()
        {
            var repo = new SubjectRepository(await _dbGenerator.GetDatabase());
            var oldCount = (await repo.GetSubjects()).Count;
            var subjectId = "1";
            var subject = await repo.GetSubjectTracking(subjectId);

            var result = await repo.DeleteSubject(subject);
            var newCount = (await repo.GetSubjects()).Count;

            result.Should().BeTrue();
            newCount.Should().Be(oldCount - 1);
        }
    }
}
