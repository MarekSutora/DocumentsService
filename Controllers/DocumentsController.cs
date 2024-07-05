using Microsoft.AspNetCore.Mvc;
using DocumentsService.API.DTOs.Request;
using DocumentsService.API.Storage.Abstractions;
using DocumentsService.API.Models;
using DocumentsService.API.DTOs.Response;

namespace DocumentsService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentsRepository _repository;

        public DocumentsController(IDocumentsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentById(string id)
        {
            var document = await _repository.GetDocumentByIdAsync(id);

            if (document == null)
            {
                return NotFound("Document not found.");
            }
            var readDocument = new ReadDocumentDto
            {
                Id = document.Id,
                Tags = document.Tags,
                Data = document.Data
            };

            return Ok(readDocument);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocument([FromBody] CreateDocumentDto createDocumentDto)
        {
            if (createDocumentDto == null || string.IsNullOrEmpty(createDocumentDto.Id))
            {
                return BadRequest("Invalid document data. Ensure all fields are properly formatted.");
            }

            var existingDocument = await _repository.GetDocumentByIdAsync(createDocumentDto.Id);
            if (existingDocument != null)
            {
                return Conflict("Document with the same ID already exists.");
            }

            var document = new Document
            {
                Id = createDocumentDto.Id,
                Tags = createDocumentDto.Tags,
                Data = createDocumentDto.Data
            };

            await _repository.SaveDocumentAsync(document);
            return CreatedAtAction(nameof(GetDocumentById), new { id = document.Id }, document);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(string id, [FromBody] UpdateDocumentDto updateDocumentDto)
        {
            if (updateDocumentDto == null)
            {
                return BadRequest("Invalid document data. Ensure all fields are properly formatted.");
            }

            var existingDocument = await _repository.GetDocumentByIdAsync(id);
            if (existingDocument == null)
            {
                return NotFound("Document not found.");
            }

            var document = new Document
            {
                Id = id,
                Tags = updateDocumentDto.Tags,
                Data = updateDocumentDto.Data
            };

            await _repository.UpdateDocumentAsync(document);
            return Ok(document);
        }
    }
}
