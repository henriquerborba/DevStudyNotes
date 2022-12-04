using DevStudyNotes.API.Entities;
using DevStudyNotes.API.models;
using DevStudyNotes.API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevStudyNotes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudyNotesController : ControllerBase
    {
        private readonly StudyNoteDbContext _context;

        public StudyNotesController(StudyNoteDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Busca todas as notas de estudo
        /// </summary>
        /// <returns>Lista de todas as notas de estudo</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var studyNotes = _context.StudyNotes.ToList();
            return Ok(studyNotes);
        }

        /// <summary>
        /// Busca uma nota de estudo por id
        /// </summary>
        /// <param name="id">Id da nota de estudo</param>
        /// <returns>
        /// Retorna a nota de estudo caso encontrada, caso não retorna notFund
        /// </returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var studyNote = _context.StudyNotes
                .Include(s=> s.Reactions)
                .FirstOrDefault(s=> s.Id == id);
            
            if (studyNote == null)
            {
                return NotFound();
            }
            
            return Ok(studyNote);
        }

        /// <summary>
        /// Cadastrar uma nota de estudo
        /// </summary>
        /// <remarks>
        /// { "title": "Estudos AZ-400", "description": "Sobre o Azure Blob Storage", "isPublic": true }
        /// </remarks>
        /// <param name="model">Dados de uma nota de estudo</param>
        /// <returns>Objeto recém-criado</returns>
        /// <response code="201">Retorna o objeto recém-criado</response>
        [HttpPost]
        public IActionResult Post(AddStudyNoteInputModel model)
        {
            var studyNote = new StudyNote(model.Title, model.Description, model.IsPublic);
            
            _context.StudyNotes.Add(studyNote);
            _context.SaveChanges();

            return CreatedAtAction("GetById", new { id = studyNote.Id }, model);
        }

        /// <summary>
        /// Adicionar uma reação a uma nota de estudo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        [HttpPost("{id}/reactions")]
        public IActionResult PostReaction(int id, AddReactionStudyNoteModel model)
        {
            var studyNotes = _context.StudyNotes.Find(id);
            
            if (studyNotes == null)
            {
                return BadRequest();
            }
            
            studyNotes.AddReaction(model.IsPositive);
            _context.SaveChanges();

            return NoContent();
        }

    }
}