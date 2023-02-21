using AutoMapper;
using Essays.WebApi.Data.Interfaces;
using Essays.WebApi.DTOs;
using Essays.WebApi.Models;
using Essays.WebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Essays.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectCategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISubjectCategoryRepository _subjectCategoryRepository;
        private readonly IRandomGenerator _randomGenerator;

        public SubjectCategoryController(IMapper mapper,
            ISubjectCategoryRepository subjectCategoryRepository,
            IRandomGenerator randomGenerator)
        {
            _mapper = mapper;
            _subjectCategoryRepository = subjectCategoryRepository;
            _randomGenerator = randomGenerator;
        }

        [HttpGet("GetSubjectCategories")]
        [ProducesResponseType(200, Type = typeof(ICollection<SubjectCategoryDto>))]
        public async Task<IActionResult> GetSubjectCategories()
        {
            var subjectCategories = await _subjectCategoryRepository.GetSubjectCategories();
            var subjectCategoriesDto = _mapper.Map<ICollection<SubjectCategoryDto>>(subjectCategories);

            return Ok(subjectCategoriesDto);
        }

        [HttpGet("GetSubjectCategoriesFromPage")]
        [ProducesResponseType(200, Type = typeof(ICollection<SubjectCategoryDto>))]
        [ProducesResponseType(422)]
        public async Task<IActionResult> GetSubjectCategories([FromQuery] int pageNumber, int pageSize)
        {
            if (pageNumber < 1 ||
                pageSize < 1)
            {
                return StatusCode(422, $"Wrong page '{pageNumber}' of size '{pageSize}'");
            }

            var subjectCategories = await _subjectCategoryRepository.GetSubjectCategories(pageNumber, pageSize);
            var subjectCategoriesDto = _mapper.Map<ICollection<SubjectCategoryDto>>(subjectCategories);

            return Ok(subjectCategoriesDto);
        }

        [HttpGet("GetSubjectCategory")]
        [ProducesResponseType(200, Type = typeof(SubjectCategoryDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSubjectCategory([FromQuery] string subjectCategoryId)
        {
            var any = await _subjectCategoryRepository.DoesSubjectCategoryExist(subjectCategoryId);
            if (!any)
            {
                return NotFound("Subject category with such ID doesn't exist");
            }

            var subjectCategory = await _subjectCategoryRepository.GetSubjectCategory(subjectCategoryId);
            var subjectCategoryDto = _mapper.Map<SubjectCategoryDto>(subjectCategory);

            return Ok(subjectCategoryDto);
        }

        [HttpGet("GetSubjectsFromCategory")]
        [ProducesResponseType(200, Type = typeof(ICollection<SubjectDto>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetSubjectsFromCategory([FromQuery] string subjectCategoryId)
        {
            var any = await _subjectCategoryRepository.DoesSubjectCategoryExist(subjectCategoryId);
            if (!any)
            {
                return NotFound("Subject category with such ID doesn't exist");
            }

            var subjects = await _subjectCategoryRepository.GetSubjectsFromCategory(subjectCategoryId);
            if (subjects == null)
            {
                return NoContent();
            }

            var subjectsDto = _mapper.Map<ICollection<SubjectDto>>(subjects);

            return Ok(subjectsDto);
        }

        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateSubjectCategory([FromBody] SubjectCategoryDto subjectCategoryCreate)
        {
            if (subjectCategoryCreate == null)
            {
                return BadRequest("Subject category model is null!");
            }

            var subjectCategories = await _subjectCategoryRepository.GetSubjectCategories();
            var existingSubjectCategory = subjectCategories
                .Where(sc => sc.Name.ToLower() == subjectCategoryCreate.Name.ToLower())
                .FirstOrDefault();

            if (existingSubjectCategory != null)
            {
                return StatusCode(422, $"Subject category with name '{subjectCategoryCreate.Name}' already exists");
            }

            var subjectCategory = _mapper.Map<SubjectCategory>(subjectCategoryCreate);
            subjectCategory.SubjectCategoryId = _randomGenerator.GetRandomId();

            var created = await _subjectCategoryRepository.CreateSubjectCategory(subjectCategory);
            if (!created)
            {
                return StatusCode(500, "Failed to create a new subject category");
            }

            return Ok(subjectCategory.SubjectCategoryId);
        }

        [HttpPut("Update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateSubjectCategory([FromBody] SubjectCategoryDto subjectCategoryUpdate)
        {
            if (subjectCategoryUpdate == null)
            {
                return BadRequest("Subject category model is null!");
            }

            var subjectCategoryId = subjectCategoryUpdate.SubjectCategoryId;
            var subjectCategory = await _subjectCategoryRepository.GetSubjectCategoryTracking(subjectCategoryId);
            if (subjectCategory == null)
            {
                return NotFound("Subject category with such ID doesn't exist");
            }

            subjectCategory.Name = subjectCategoryUpdate.Name;

            var updated = await _subjectCategoryRepository.UpdateSubjectCategory(subjectCategory);
            if (!updated)
            {
                return StatusCode(500, $"Failed to update the subject category with ID '{subjectCategoryId}'");
            }

            return Ok(subjectCategoryId);
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteSubjectCategory([FromQuery] string subjectCategoryId)
        {
            var subjectCategoryToDelete = await _subjectCategoryRepository.GetSubjectCategoryTracking(subjectCategoryId);
            if (subjectCategoryToDelete == null)
            {
                return NotFound("Subject category with such ID doesn't exist");
            }

            var deleted = await _subjectCategoryRepository.DeleteSubjectCategory(subjectCategoryToDelete);
            if (!deleted)
            {
                return StatusCode(500, $"Failed to delete the subject category with ID '{subjectCategoryId}'");
            }

            return Ok(subjectCategoryToDelete.SubjectCategoryId);
        }
    }
}
