using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StorefrontSectionsController : ControllerBase
    {
        private readonly ECommerceDbContext _context;

        public StorefrontSectionsController(ECommerceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StorefrontSection>>> GetSections()
        {
            var sections = await _context.StorefrontSections
                .AsNoTracking()
                .OrderBy(section => section.Category)
                .ToListAsync();

            return Ok(sections);
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<StorefrontSection>> GetSection(string slug)
        {
            var normalizedSlug = NormalizeSlug(slug);
            var section = await _context.StorefrontSections
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Slug == normalizedSlug);

            if (section != null)
            {
                return Ok(section);
            }

            return Ok(BuildDefaultSection(normalizedSlug, slug));
        }

        [HttpPut("{slug}")]
        public async Task<ActionResult<StorefrontSection>> UpsertSection(string slug, [FromBody] StorefrontSectionUpsertRequest request)
        {
            var normalizedSlug = NormalizeSlug(slug);
            var section = await _context.StorefrontSections.FirstOrDefaultAsync(item => item.Slug == normalizedSlug);

            if (section == null)
            {
                section = new StorefrontSection
                {
                    Slug = normalizedSlug,
                };

                _context.StorefrontSections.Add(section);
            }

            section.Category = request.Category.Trim();
            section.Title = request.Title.Trim();
            section.Description = request.Description.Trim();
            section.FeaturedEyebrow = request.FeaturedEyebrow.Trim();
            section.AllEyebrow = request.AllEyebrow.Trim();
            section.AllTitle = request.AllTitle.Trim();
            section.AllDescription = request.AllDescription.Trim();
            section.EmptyTitle = request.EmptyTitle.Trim();
            section.EmptyDescription = request.EmptyDescription.Trim();
            section.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(section);
        }

        private static string NormalizeSlug(string value)
        {
            return value.Trim().ToLowerInvariant();
        }

        private static StorefrontSection BuildDefaultSection(string normalizedSlug, string rawSlug)
        {
            var category = BuildCategoryFromSlug(rawSlug);
            var categoryLower = category.ToLowerInvariant();

            return new StorefrontSection
            {
                Slug = normalizedSlug,
                Category = category,
                Title = $"{category} disponibles",
                Description = $"Explora la seleccion activa de {categoryLower} publicada actualmente en la tienda.",
                FeaturedEyebrow = $"Featured {categoryLower}",
                AllEyebrow = $"All {categoryLower}",
                AllTitle = $"Todos los {categoryLower}",
                AllDescription = $"Catalogo completo de {categoryLower} activos publicado en la API.",
                EmptyTitle = $"No hay {categoryLower} publicados",
                EmptyDescription = $"Cuando existan productos activos en la categoria {category}, apareceran automaticamente aqui.",
                UpdatedDate = DateTime.UtcNow,
            };
        }

        private static string BuildCategoryFromSlug(string slug)
        {
            var normalized = slug.Trim().Replace('-', ' ');
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(normalized.ToLowerInvariant());
        }
    }
}