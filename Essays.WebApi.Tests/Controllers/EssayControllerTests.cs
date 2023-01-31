namespace Essays.WebApi.Tests.Controllers
{
    public class EssayControllerTests
    {
        private readonly IMapper _mapper;
        private readonly IRandomGenerator _randomGenerator;
        private readonly IEssayRepository _essayRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly EssayController _essayController;

        public EssayControllerTests()
        {
            _mapper = A.Fake<IMapper>();
            _randomGenerator = A.Fake<IRandomGenerator>();
            _essayRepository = A.Fake<IEssayRepository>();
            _subjectRepository = A.Fake<ISubjectRepository>();
            _essayController = new EssayController(_mapper,
                _randomGenerator,
                _essayRepository,
                _subjectRepository);
        }

        [Fact]
        public async Task EssayController_GetEssays_ReturnsOK()
        {
            var essays = A.Fake<ICollection<Essay>>();
            var essaysDto = A.Fake<ICollection<EssayDto>>();
            A.CallTo(() => _essayRepository.GetEssays()).Returns(essays);
            A.CallTo(() => _mapper.Map<ICollection<EssayDto>>(essays)).Returns(essaysDto);

            var result = await _essayController.GetEssays();

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task EssayController_GetEssaysFromPage_ReturnsOK()
        {
            var essays = A.Fake<ICollection<Essay>>();
            var essaysDto = A.Fake<ICollection<EssayDto>>();
            A.CallTo(() => _essayRepository.GetEssays()).Returns(essays);
            A.CallTo(() => _mapper.Map<ICollection<EssayDto>>(essays)).Returns(essaysDto);

            var result = await _essayController.GetEssays(1, 10);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task EssayController_GetEssay_ReturnsOK()
        {
            var essayId = "1";
            var essay = A.Fake<Essay>();
            var essayDto = A.Fake<EssayDto>();
            A.CallTo(() => _essayRepository.DoesEssayExist(essayId)).Returns(true);
            A.CallTo(() => _essayRepository.GetEssay(essayId)).Returns(essay);
            A.CallTo(() => _mapper.Map<EssayDto>(essay)).Returns(essayDto);

            var result = await _essayController.GetEssay(essayId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task EssayController_GetSubjectsOfEssay_ReturnsOK()
        {
            var essayId = "1";
            var subjects = A.Fake<ICollection<Subject>>();
            var subjectsDto = A.Fake<ICollection<SubjectDto>>();
            A.CallTo(() => _essayRepository.GetSubjectsOfEssay(essayId)).Returns(subjects);
            A.CallTo(() => _mapper.Map<ICollection<SubjectDto>>(subjects)).Returns(subjectsDto);

            var result = await _essayController.GetSubjectsOfEssay(essayId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task EssayController_GetAuthorOfEssay_ReturnsOK()
        {
            var essayId = "1";
            var author = A.Fake<Author>();
            var authorDto = A.Fake<AuthorDto>();
            A.CallTo(() => _essayRepository.GetAuthorOfEssay(essayId)).Returns(author);
            A.CallTo(() => _mapper.Map<AuthorDto>(author)).Returns(authorDto);

            var result = await _essayController.GetAuthorOfEssay(essayId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }


        [Fact]
        public async Task EssayController_CreateEssay_ReturnsOK()
        {
            var essayDto = A.Fake<EssayDto>();
            var essay = A.Fake<Essay>();
            A.CallTo(() => _mapper.Map<Essay>(essayDto)).Returns(essay);
            A.CallTo(() => _essayRepository.CreateEssay(essay)).Returns(true);

            var result = await _essayController.CreateEssay(essayDto);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task EssayController_AddSubjectOfEssay_ReturnsOK()
        {
            var essayId = "1";
            var subjectId = "2";
            A.CallTo(() => _essayRepository.DoesEssayExist(essayId)).Returns(true);
            A.CallTo(() => _subjectRepository.DoesSubjectExist(subjectId)).Returns(true);
            A.CallTo(() => _essayRepository.AddSubjectOfEssay(essayId, subjectId)).Returns(true);

            var result = await _essayController.AddSubjectOfEssay(essayId, subjectId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task EssayController_RemoveSubjectOfEssay_ReturnsOK()
        {
            var essayId = "1";
            var subjectId = "2";
            A.CallTo(() => _essayRepository.DoesEssayExist(essayId)).Returns(true);
            A.CallTo(() => _subjectRepository.DoesSubjectExist(subjectId)).Returns(true);
            A.CallTo(() => _essayRepository.RemoveSubjectOfEssay(essayId, subjectId)).Returns(true);

            var result = await _essayController.RemoveSubjectOfEssay(essayId, subjectId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task EssayController_UpdateEssay_ReturnsOK()
        {
            var essayDto = A.Fake<EssayDto>();
            var essay = A.Fake<Essay>();
            A.CallTo(() => _essayRepository.DoesEssayExist(essayDto.EssayId)).Returns(true);
            A.CallTo(() => _mapper.Map<Essay>(essayDto)).Returns(essay);
            A.CallTo(() => _essayRepository.UpdateEssay(essay)).Returns(true);

            var result = await _essayController.UpdateEssay(essayDto);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task EssayController_DeleteEssay_ReturnsOK()
        {
            var essayId = "1";
            var essay = A.Fake<Essay>();
            A.CallTo(() => _essayRepository.GetEssay(essayId)).Returns(essay);
            A.CallTo(() => _essayRepository.DeleteEssay(essay)).Returns(true);

            var result = await _essayController.DeleteEssay(essayId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
