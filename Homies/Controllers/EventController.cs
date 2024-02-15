using Homies.Data;
using Homies.Data.Models;
using Homies.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;
using static Homies.Data.DataConstants;

namespace Homies.Controllers
{
    public class EventController : Controller
    {
        private HomiesDbContext context;

        public EventController(HomiesDbContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var models = await context.Events
                .AsNoTracking()
                .Select(e => new AllEventViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Organiser = e.Organiser.UserName,
                    Start = e.Start.ToString(DateTimeFormat),
                    Type = e.Type.Name
                })
                .ToListAsync();

            return View(models);
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            var _event = await context.Events
                .Where(e => e.Id == id)
                .Include(e => e.EventsParticipants)
                .FirstOrDefaultAsync();

            if (_event == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (!_event.EventsParticipants.Any(ep => ep.HelperId == userId))
            {
                _event.EventsParticipants
                    .Add(new EventParticipant()
                    {
                        EventId = id,
                        HelperId = userId
                    });

                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Joined));
        }

        [HttpGet]
        public async Task<IActionResult> Joined(int id)
        {
            string userId = GetUserId();

            var models = await context.EventsParticipants
                .AsNoTracking()
                .Where(ep => ep.HelperId == userId)
                .Select(ep => new AllEventViewModel()
                {
                    Id = ep.EventId,
                    Name = ep.Event.Name,
                    Organiser = ep.Event.Organiser.UserName,
                    Start = ep.Event.Start.ToString(DateTimeFormat),
                    Type = ep.Event.Type.Name
                })
                .ToListAsync();

            return View(models);
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            var _event = await context.Events
                .Where(e => e.Id == id)
                .Include(e => e.EventsParticipants)
                .FirstOrDefaultAsync();

            if (_event == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            var ep = _event.EventsParticipants
                .FirstOrDefault(ep => ep.HelperId == userId);

            if (ep == null)
            {
                return BadRequest();
            }

            _event.EventsParticipants.Remove(ep);

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            EventFormViewModel model = new();
            model.Types = await GetTypesAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(EventFormViewModel model)
        {
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;

            if (!DateTime.TryParseExact(
                model.Start,
                DateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out start))
            {
                ModelState.AddModelError(nameof(model.Start), $"Invalid date format! The format should be {DateTimeFormat}");
            }

            if (!DateTime.TryParseExact(
                model.End,
                DateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out end))
            {
                ModelState.AddModelError(nameof(model.End), $"Invalid date format! The format should be {DateTimeFormat}");
            }

            if (!ModelState.IsValid)
            {
                model.Types = await GetTypesAsync();

                return View(model);
            }

            Event entity = new()
            {
                CreatedOn = DateTime.Now,
                OrganiserId = GetUserId(),
                Name = model.Name,
                Description = model.Description,
                Start = start,
                End = end,
                TypeId = model.TypeId
            };

            await context.Events.AddAsync(entity);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                return BadRequest();
            }

            if (entity.OrganiserId != GetUserId())
            {
                return Unauthorized();
            }

            EventFormViewModel model = new()
            {
                Name = entity.Name,
                Description = entity.Description,
                Start = entity.Start.ToString(DateTimeFormat),
                End = entity.End.ToString(DateTimeFormat),
                TypeId = entity.TypeId,
                Types = await GetTypesAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EventFormViewModel model, int id)
        {
            Event entity = await context.Events.FindAsync(id);

            if (entity == null)
            {
                return BadRequest();
            }

            if (entity.OrganiserId != GetUserId() )
            {
                return Unauthorized();
            }

            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;

            if (!DateTime.TryParseExact(
                model.Start,
                DateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out start))
            {
                ModelState.AddModelError(nameof(model.Start), $"Invalid date format! The format should be {DateTimeFormat}");
            }

            if (!DateTime.TryParseExact(
                model.End,
                DateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out end))
            {
                ModelState.AddModelError(nameof(model.End), $"Invalid date format! The format should be {DateTimeFormat}");
            }

            if (!ModelState.IsValid)
            {
                model.Types = await GetTypesAsync();

                return View(model);
            }

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Start = start;
            entity.End = end;
            entity.TypeId = model.TypeId;

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await context.Events
                .Where(e => e.Id == id)
                .Select(e => new EventDetailsViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Start = e.Start.ToString(DateTimeFormat),
                    End = e.End.ToString(DateTimeFormat),
                    CreatedOn = e.CreatedOn.ToString(DateTimeFormat),
                    Organiser = e.Organiser.UserName,
                    Type = e.Type.Name
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        private async Task<IEnumerable<TypeViewModel>> GetTypesAsync()
        {
            return await context.Types
                .Select(t => new TypeViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                })
                .ToListAsync();
        }
    }
}
