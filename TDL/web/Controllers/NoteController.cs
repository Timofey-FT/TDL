using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace web.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        private readonly ApplicationDbContext _context;
        public NoteController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var notes = await _context.Notes.Include(x=>x.NoteTags).ThenInclude(x=>x.Tag).ToListAsync();
            return View(notes);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Tags = new SelectList(await _context.Tags.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateNoteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Tags = new SelectList(await _context.Tags.ToListAsync(), "Id", "Name");
                return View(model);
            }

            var note = new Note
            {
                Title = model.Title,
                Content = model.Content,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                NoteTags = model.SelectedTagIds.Select(tagId => new NoteTag
                {
                    TagId = tagId
                }).ToList()
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            ViewBag.Tags = new SelectList(await _context.Tags.ToListAsync(), "Id", "Name");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var note = await _context.Notes
                .Include(n => n.NoteTags)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (note == null)
            {
                return NotFound();
            }

            var viewModel = new EditNoteViewModel
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                SelectedTagIds = note.NoteTags.Select(nt => nt.TagId).ToList(),
            };
            ViewBag.Tags = new SelectList(await _context.Tags.ToListAsync(), "Id", "Name");

            return View(viewModel);
        }

        // POST: Note/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditNoteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Tags = new SelectList(await _context.Tags.ToListAsync(), "Id", "Name");
                return View(model);
            }

            var note = await _context.Notes
                .Include(n => n.NoteTags)
                .FirstOrDefaultAsync(n => n.Id == model.Id);

            if (note == null)
            {
                return NotFound();
            }

            // Обновляем свойства заметки
            note.Title = model.Title;
            note.Content = model.Content;
            note.UpdatedAt = DateTime.Now;

            // Обновляем теги
            note.NoteTags.Clear();
            foreach (var tagId in model.SelectedTagIds)
            {
                note.NoteTags.Add(new NoteTag { NoteId = note.Id, TagId = tagId });
            }

            await _context.SaveChangesAsync();

            ViewBag.Tags = new SelectList(await _context.Tags.ToListAsync(), "Id", "Name");
            return RedirectToAction("Index");
        }   

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);
            if (note == null) return NotFound();

            return View(note);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleComplete([FromBody] ToggleCompleteRequest request)
        {
            var note = await _context.Notes.FindAsync(request.Id);
            if (note == null) return NotFound();

            note.IsCompleted = true;
            await _context.SaveChangesAsync(); // обязательно!

            return Ok();
        }

        public class ToggleCompleteRequest
        {
            public int Id { get; set; }
        }


    }
}
