using Essays.WebApi.Controllers;
using FakeItEasy;

namespace Essays.WebApi.Tests.Controllers
{
    public class SubjectControllerTests
    {
        private readonly IMapper _mapper;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IRandomGenerator _randomGenerator;
        private readonly SubjectController _subjectController;

        public SubjectControllerTests()
        {
            _mapper = A.Fake<IMapper>();
            _randomGenerator = A.Fake<IRandomGenerator>();
            _subjectRepository = A.Fake<ISubjectRepository>();
            _subjectController = new SubjectController(_mapper,
                _subjectRepository,
                _randomGenerator);
        }

        [Fact]
        public async Task SubjectController_GetSubjects_ReturnsOK()
        {
            var subjects = A.Fake<ICollection<Subject>>();
            var subjectsDto = A.Fake<ICollection<SubjectDto>>();
            A.CallTo(() => _subjectRepository.GetSubjects()).Returns(subjects);
            A.CallTo(() => _mapper.Map<ICollection<SubjectDto>>(subjects)).Returns(subjectsDto);

            var result = await _subjectController.GetSubjects();

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task SubjectController_GetSubjectsFromPage_ReturnsOK()
        {
            var subjects = A.Fake<ICollection<Subject>>();
            var subjectsDto = A.Fake<ICollection<SubjectDto>>();
            A.CallTo(() => _subjectRepository.GetSubjects()).Returns(subjects);
            A.CallTo(() => _mapper.Map<ICollection<SubjectDto>>(subjects)).Returns(subjectsDto);

            var result = await _subjectController.GetSubjects(1, 10);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task SubjectController_GetSubject_ReturnsOK()
        {
            var subjectId = "1";
            var subject = A.Fake<Subject>();
            var subjectDto = A.Fake<SubjectDto>();
            A.CallTo(() => _subjectRepository.DoesSubjectExist(subjectId)).Returns(true);
            A.CallTo(() => _subjectRepository.GetSubject(subjectId)).Returns(subject);
            A.CallTo(() => _mapper.Map<SubjectDto>(subject)).Returns(subjectDto);

            var result = await _subjectController.GetSubject(subjectId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task SubjectController_GetCategoryOfSubject_ReturnsOK()
        {
            var subjectId = "1";
            var category = A.Fake<SubjectCategory>();
            var categoryDto = A.Fake<SubjectCategoryDto>();
            A.CallTo(() => _subjectRepository.GetCategoryOfSubject(subjectId)).Returns(category);
            A.CallTo(() => _mapper.Map<SubjectCategoryDto>(category)).Returns(categoryDto);

            var result = await _subjectController.GetCategoryOfSubject(subjectId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task SubjectController_GetEssaysAboutSubject_ReturnsOK()
        {
            var subjectId = "1";
            var essays = A.Fake<ICollection<Essay>>();
            var essaysDto = A.Fake<ICollection<EssayDto>>();
            A.CallTo(() => _subjectRepository.GetEssaysAboutSubject(subjectId)).Returns(essays);
            A.CallTo(() => _mapper.Map<ICollection<EssayDto>>(essays)).Returns(essaysDto);

            var result = await _subjectController.GetEssaysAboutSubject(subjectId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task SubjectController_Create_ReturnsOK()
        {
            var subjectDto = A.Fake<SubjectDto>();
            var subject = A.Fake<Subject>();
            A.CallTo(() => _mapper.Map<Subject>(subjectDto)).Returns(subject);
            A.CallTo(() => _subjectRepository.CreateSubject(subject)).Returns(true);

            var result = await _subjectController.CreateSubject(subjectDto);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task SubjectController_Update_ReturnsOK()
        {
            var subjectDto = A.Fake<SubjectDto>();
            var subject = A.Fake<Subject>();
            A.CallTo(() => _subjectRepository.DoesSubjectExist(subjectDto.SubjectId)).Returns(true);
            A.CallTo(() => _mapper.Map<Subject>(subjectDto)).Returns(subject);
            A.CallTo(() => _subjectRepository.UpdateSubject(subject)).Returns(true);

            var result = await _subjectController.UpdateSubject(subjectDto);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task SubjectController_Delete_ReturnsOK()
        {
            var subjectId = "1";
            var subject = A.Fake<Subject>();
            A.CallTo(() => _subjectRepository.GetSubject(subjectId)).Returns(subject);
            A.CallTo(() => _subjectRepository.DeleteSubject(subject)).Returns(true);

            var result = await _subjectController.DeleteSubject(subjectId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
