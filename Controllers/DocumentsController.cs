using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentsService.API.DTOs.Request;
using DocumentsService.API.Storage.Abstractions;
using System.Xml.Serialization;
using System.Xml;
using DocumentsService.API.Models;
using DocumentsService.DTOs.Request;

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

            return Ok(document);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var document = new Document
            {
                Id = "1",
                Tags = new List<string> { "tag1", "tag2" },
                Data = new Dictionary<string, object>
            {
                { "key1", "value1" },
                { "key2", 2 },
                { "key3", new List<string> { "item1", "item2", "item3" } },
                { "key4", new Dictionary<string, object>
                    {
                        { "subKey1", "subValue1" },
                        { "subKey2", new List<int> { 1, 2, 3 } }
                    }
                },
                { "key5", new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            { "innerKey1", "innerValue1" },
                            { "innerKey2", 100 }
                        },
                        new Dictionary<string, object>
                        {
                            { "innerKey3", "innerValue3" },
                            { "innerKey4", 200 }
                        }
                    }
                }
            }
            };

            return Ok(document);
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
