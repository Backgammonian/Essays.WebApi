using Essays.WebApi.Controllers;
using Essays.WebApi.DTOs;

namespace Essays.WebApi.Tests.Controllers
{
    public class SubjectCategoryControllerTests
    {
        private readonly IMapper _mapper;
        private readonly ISubjectCategoryRepository _subjectCategoryRepository;
        private readonly IRandomGenerator _randomGenerator;
        private readonly SubjectCategoryController _subjectCategoryController;

        public SubjectCategoryControllerTests()
        {
            _mapper = A.Fake<IMapper>();
            _subjectCategoryRepository = A.Fake<ISubjectCategoryRepository>();
            _randomGenerator = A.Fake<IRandomGenerator>();
            _subjectCategoryController = new SubjectCategoryController(_mapper,
                _subjectCategoryRepository,
                _randomGenerator);
        }

        [Fact]
        public async Task SubjectCategoryController_GetSubjectCategories_ReturnsOK()
        {
            var subjectCategories = A.Fake<ICollection<SubjectCategory>>();
            var subjectCategoriesDto = A.Fake<ICollection<SubjectCategoryDto>>();
            A.CallTo(() => _subjectCategoryRepository.GetSubjectCategories()).Returns(subjectCategories);
            A.CallTo(() => _mapper.Map<ICollection<SubjectCategoryDto>>(subjectCategories)).Returns(subjectCategoriesDto);

            var result = await _subjectCategoryController.GetSubjectCategories();

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task SubjectCategoryController_GetSubjectCategoriesFromPage_ReturnsOK()
        {
            var subjectCategories = A.Fake<ICollection<SubjectCategory>>();
            var subjectCategoriesDto = A.Fake<ICollection<SubjectCategoryDto>>();
            A.CallTo(() => _subjectCategoryRepository.GetSubjectCategories()).Returns(subjectCategories);
            A.CallTo(() => _mapper.Map<ICollection<SubjectCategoryDto>>(subjectCategories)).Returns(subjectCategoriesDto);

            var result = await _subjectCategoryController.GetSubjectCategories(1, 10);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task SubjectCategoryController_GetSubjectCategory_ReturnsOK()
        {
            var subjectCategoryId = "1";
            var subjectCategory = A.Fake<SubjectCategory>();
            var subjectCategoryDto = A.Fake<SubjectCategoryDto>();
            A.CallTo(() => _subjectCategoryRepository.DoesSubjectCategoryExist(subjectCategoryId)).Returns(true);
            A.CallTo(() => _subjectCategoryRepository.GetSubjectCategory(subjectCategoryId)).Returns(subjectCategory);
            A.CallTo(() => _mapper.Map<SubjectCategoryDto>(subjectCategory)).Returns(subjectCategoryDto);

            var result = await _subjectCategoryController.GetSubjectCategory(subjectCategoryId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task SubjectCategoryController_GetSubjectsFromCategory_ReturnsOK()
        {
            var subjectCategoryId = "1";
            var subjects = A.Fake<ICollection<Subject>>();
            var subjectsDto = A.Fake<ICollection<SubjectDto>>();
            A.CallTo(() => _subjectCategoryRepository.DoesSubjectCategoryExist(subjectCategoryId)).Returns(true);
            A.CallTo(() => _subjectCategoryRepository.GetSubjectsFromCategory(subjectCategoryId)).Returns(subjects);
            A.CallTo(() => _mapper.Map<ICollection<SubjectDto>>(subjects)).Returns(subjectsDto);

            var result = await _subjectCategoryController.GetSubjectsFromCategory(subjectCategoryId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task SubjectCategoryController_Create_ReturnsOK()
        {
            var subjectCategoryDto = A.Fake<SubjectCategoryDto>();
            var subjectCategory = A.Fake<SubjectCategory>();
            A.CallTo(() => _mapper.Map<SubjectCategory>(subjectCategoryDto)).Returns(subjectCategory);
            A.CallTo(() => _subjectCategoryRepository.CreateSubjectCategory(subjectCategory)).Returns(true);

            var result = await _subjectCategoryController.CreateSubjectCategory(subjectCategoryDto);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task SubjectCategoryController_Update_ReturnsOK()
        {
            var subjectCategoryDto = A.Fake<SubjectCategoryDto>();
            var subjectCategory = A.Fake<SubjectCategory>();
            A.CallTo(() => _subjectCategoryRepository.DoesSubjectCategoryExist(subjectCategoryDto.SubjectCategoryId)).Returns(true);
            A.CallTo(() => _mapper.Map<SubjectCategory>(subjectCategoryDto)).Returns(subjectCategory);
            A.CallTo(() => _subjectCategoryRepository.UpdateSubjectCategory(subjectCategory)).Returns(true);

            var result = await _subjectCategoryController.UpdateSubjectCategory(subjectCategoryDto);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task SubjectCategoryController_Delete_ReturnsOK()
        {
            var subjectCategoryId = "1";
            var subjectCategory = A.Fake<SubjectCategory>();
            A.CallTo(() => _subjectCategoryRepository.GetSubjectCategory(subjectCategoryId)).Returns(subjectCategory);
            A.CallTo(() => _subjectCategoryRepository.DeleteSubjectCategory(subjectCategory)).Returns(true);

            var result = await _subjectCategoryController.DeleteSubjectCategory(subjectCategoryId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
