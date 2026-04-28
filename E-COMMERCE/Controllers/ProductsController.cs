using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ECommerce.Data;
using ECommerce.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private const string FallbackImageUrl = "https://www.shutterstock.com/image-vector/missing-picture-page-website-design-600nw-1552421075.jpg";
        private readonly ECommerceDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductsController(ECommerceDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet("storefront")]
        public async Task<ActionResult<StorefrontResponse>> GetStorefrontProducts()
        {
            var products = await _context.Products
                .AsNoTracking()
                .Where(p => p.IsActive)
                .Include(p => p.ProductImages)
                .OrderByDescending(p => p.IsNew)
                .ThenByDescending(p => p.CreatedDate)
                .ThenByDescending(p => p.ProductId)
                .ToListAsync();

            var categories = products
                .Where(p => !string.IsNullOrWhiteSpace(p.Category))
                .GroupBy(p => p.Category.Trim())
                .Select(group => new StorefrontCategoryCard
                {
                    Name = group.Key,
                    Tagline = $"{group.Count()} referencias disponibles",
                    Image = GetPrimaryImage(group.First())
                })
                .OrderBy(category => category.Name)
                .ToList();

            var featureProduct = products.FirstOrDefault(p => p.IsNew) ?? products.FirstOrDefault();
            var mosaicProducts = products.Take(4).ToList();

            return Ok(new StorefrontResponse
            {
                Products = products,
                Categories = categories,
                LeftFeature = BuildHighlightCard(featureProduct, "Special drop"),
                MosaicCards = mosaicProducts
                    .Select((product, index) => BuildHighlightCard(product, $"Top edit {index + 1}"))
                    .ToList(),
                LatestProducts = products
                    .OrderByDescending(p => p.CreatedDate)
                    .ThenByDescending(p => p.ProductId)
                    .Take(4)
                    .ToList(),
                BestSellerProducts = products
                    .OrderByDescending(p => p.Price)
                    .ThenByDescending(p => p.CreatedDate)
                    .Take(4)
                    .ToList(),
                NewCollectionProducts = products
                    .Skip(1)
                    .Take(5)
                    .ToList()
            });
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<CategoryProductsResponse>> GetProductsByCategory(string category, [FromQuery] int featuredTake = 4)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return BadRequest("category is required.");
            }

            featuredTake = Math.Min(Math.Max(featuredTake, 1), 8);
            var normalizedCategory = category.Trim().ToLower();

            var products = await _context.Products
                .AsNoTracking()
                .Where(p => p.IsActive && p.Category != null && p.Category.Trim().ToLower() == normalizedCategory)
                .Include(p => p.ProductImages)
                .OrderByDescending(p => p.IsNew)
                .ThenByDescending(p => p.CreatedDate)
                .ThenByDescending(p => p.ProductId)
                .ToListAsync();

            var resolvedCategory = products.FirstOrDefault()?.Category?.Trim() ?? category.Trim();
            var sectionConfig = await ResolveCategorySectionContentAsync(resolvedCategory, normalizedCategory);

            return Ok(new CategoryProductsResponse
            {
                Category = resolvedCategory,
                Title = sectionConfig.Title,
                Description = sectionConfig.Description,
                SectionConfig = sectionConfig,
                Products = products,
                FeaturedProducts = products.Take(featuredTake).ToList()
            });
        }

        //Get: api/products auth
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products
                .Where(p => p.IsActive)
                .Include(p => p.ProductImages)
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet("admin/{id}")]
        [Authorize]
        public async Task<ActionResult<Product>> GetAdminProduct(int id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        //Get: api/products/newproducts
        [HttpGet("newproducts")]
        public async Task<ActionResult<Product>> GetNewProducts()
        {
            var products = await _context.Products
                .Where(p => p.IsActive && p.IsNew)
                .Include(p => p.ProductImages)
                .Take(3)
                .ToListAsync();

            return Ok(products);  
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products
                .Where(p => p.ProductId == id)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync();

            if (product == null || !product.IsActive)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("{id}/related")]
        public async Task<ActionResult<IEnumerable<Product>>> GetRelatedProducts(int id, [FromQuery] int take = 3)
        {
            if (take < 1)
            {
                return BadRequest("take must be greater than 0.");
            }

            take = Math.Min(take, 12);

            var category = await _context.Products
                .AsNoTracking()
                .Where(p => p.ProductId == id && p.IsActive)
                .Select(p => p.Category)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(category))
            {
                return Ok(Array.Empty<Product>());
            }

            var relatedProducts = await _context.Products
                .AsNoTracking()
                .Where(p => p.IsActive && p.ProductId != id && p.Category == category)
                .Include(p => p.ProductImages)
                .OrderByDescending(p => p.IsNew)
                .ThenByDescending(p => p.CreatedDate)
                .ThenByDescending(p => p.ProductId)
                .Take(take)
                .ToListAsync();

            return Ok(relatedProducts);
        }

        // auth get all products
        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<PaginatedResult<Product>>> GetAllProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("page and pageSize must be greater than 0.");
            }

            pageSize = Math.Min(pageSize, 100);

            var query = _context.Products
                .AsNoTracking()
                .OrderByDescending(p => p.UpdatedDate)
                .ThenByDescending(p => p.ProductId);

            var totalItems = await query.CountAsync();

            var products = await query
                .Include(p => p.ProductImages)
                .AsSplitQuery()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new PaginatedResult<Product>
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                Items = products
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductUpsertRequest request)
        {
            var normalizedRequest = NormalizeProductRequest(request);
            var validationError = ValidateProductRequest(normalizedRequest);

            if (validationError != null)
            {
                return BadRequest(validationError);
            }

            var product = new Product
            {
                CreatedDate = DateTime.UtcNow,
            };

            ApplyProductRequest(product, normalizedRequest);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAdminProduct), new { id = product.ProductId }, product);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Product>> UpdateProduct(int id, [FromBody] ProductUpsertRequest request)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            var normalizedRequest = NormalizeProductRequest(request);
            var validationError = ValidateProductRequest(normalizedRequest);

            if (validationError != null)
            {
                return BadRequest(validationError);
            }

            ApplyProductRequest(product, normalizedRequest);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("images/upload")]
        [Authorize]
        [RequestSizeLimit(25_000_000)]
        public async Task<ActionResult<IEnumerable<UploadedProductImageResponse>>> UploadProductImages([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("At least one image file is required.");
            }

            if (files.Count > 6)
            {
                return BadRequest("You can upload up to 6 images at a time.");
            }

            var allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".webp",
                ".gif"
            };

            var webRootPath = _environment.WebRootPath;

            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
            }

            var uploadsDirectory = Path.Combine(webRootPath, "uploads", "products");
            Directory.CreateDirectory(uploadsDirectory);

            var uploadedImages = new List<UploadedProductImageResponse>();

            foreach (var file in files)
            {
                if (file.Length == 0)
                {
                    continue;
                }

                var extension = Path.GetExtension(file.FileName);

                if (!allowedExtensions.Contains(extension))
                {
                    return BadRequest($"Unsupported file type: {extension}");
                }

                var uniqueFileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
                var filePath = Path.Combine(uploadsDirectory, uniqueFileName);

                await using var stream = System.IO.File.Create(filePath);
                await file.CopyToAsync(stream);

                uploadedImages.Add(new UploadedProductImageResponse
                {
                    FileName = uniqueFileName,
                    ImageUrl = $"{Request.Scheme}://{Request.Host}/uploads/products/{uniqueFileName}"
                });
            }

            return Ok(uploadedImages);
        }

        private static StorefrontHighlightCard BuildHighlightCard(Product? product, string fallbackTag)
        {
            if (product == null)
            {
                return new StorefrontHighlightCard
                {
                    Title = "Catalogo disponible",
                    Description = "Explora las referencias activas publicadas actualmente en la tienda.",
                    Tag = fallbackTag,
                    Image = string.Empty
                };
            }

            return new StorefrontHighlightCard
            {
                Title = product.Name,
                Description = BuildCardDescription(product.Description),
                Tag = product.IsNew ? "Nueva referencia" : fallbackTag,
                Image = GetPrimaryImage(product)
            };
        }

        private static string BuildCardDescription(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return "Referencia activa disponible para compra asistida desde la tienda.";
            }

            var normalized = Regex.Replace(description.Trim(), "\\s+", " ");
            return normalized.Length <= 110 ? normalized : $"{normalized[..107]}...";
        }

        private static string GetPrimaryImage(Product product)
        {
            return product.ProductImages.FirstOrDefault(image => image.IsMain)?.ImageUrl
                ?? product.ProductImages.FirstOrDefault()?.ImageUrl
                ?? FallbackImageUrl;
        }

        private async Task<CategorySectionContent> ResolveCategorySectionContentAsync(string resolvedCategory, string normalizedCategory)
        {
            var section = await _context.StorefrontSections
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Slug == normalizedCategory || item.Category.ToLower() == normalizedCategory);

            return section == null
                ? BuildDefaultCategorySectionContent(resolvedCategory, normalizedCategory)
                : MapSectionContent(section);
        }

        private static CategorySectionContent BuildDefaultCategorySectionContent(string category, string normalizedCategory)
        {
            return new CategorySectionContent
            {
                Slug = normalizedCategory,
                Category = category,
                Title = $"{category} disponibles",
                Description = $"Explora la seleccion activa de {category.ToLower()} publicada actualmente en la tienda.",
                FeaturedEyebrow = $"Featured {category.ToLower()}",
                AllEyebrow = $"All {category.ToLower()}",
                AllTitle = $"Todos los {category.ToLower()}",
                AllDescription = $"Catalogo completo de {category.ToLower()} activos publicado en la API.",
                EmptyTitle = $"No hay {category.ToLower()} publicados",
                EmptyDescription = $"Cuando existan productos activos en la categoria {category}, apareceran automaticamente aqui."
            };
        }

        private static CategorySectionContent MapSectionContent(StorefrontSection section)
        {
            return new CategorySectionContent
            {
                Slug = section.Slug,
                Category = section.Category,
                Title = section.Title,
                Description = section.Description,
                FeaturedEyebrow = section.FeaturedEyebrow,
                AllEyebrow = section.AllEyebrow,
                AllTitle = section.AllTitle,
                AllDescription = section.AllDescription,
                EmptyTitle = section.EmptyTitle,
                EmptyDescription = section.EmptyDescription,
            };
        }

        private static ProductUpsertRequest NormalizeProductRequest(ProductUpsertRequest request)
        {
            return new ProductUpsertRequest
            {
                Name = request.Name?.Trim() ?? string.Empty,
                Sku = string.IsNullOrWhiteSpace(request.Sku) ? null : request.Sku.Trim(),
                Description = request.Description?.Trim() ?? string.Empty,
                Price = request.Price,
                Stock = request.Stock,
                Category = request.Category?.Trim() ?? string.Empty,
                IsActive = request.IsActive,
                IsNew = request.IsNew,
                ProductImages = request.ProductImages
                    .Where(image => !string.IsNullOrWhiteSpace(image.ImageUrl))
                    .Select(image => new ProductImageUpsertRequest
                    {
                        ImageUrl = image.ImageUrl.Trim(),
                        IsMain = image.IsMain,
                    })
                    .Take(6)
                    .ToList()
            };
        }

        private static string? ValidateProductRequest(ProductUpsertRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return "Name is required.";
            }

            if (string.IsNullOrWhiteSpace(request.Description))
            {
                return "Description is required.";
            }

            if (string.IsNullOrWhiteSpace(request.Category))
            {
                return "Category is required.";
            }

            if (request.Price < 0)
            {
                return "Price must be greater than or equal to 0.";
            }

            if (request.Stock < 0)
            {
                return "Stock must be greater than or equal to 0.";
            }

            if (request.ProductImages.Count > 0 && request.ProductImages.Count(image => image.IsMain) != 1)
            {
                return "Exactly one product image must be marked as main when images are provided.";
            }

            return null;
        }

        private static void ApplyProductRequest(Product product, ProductUpsertRequest request)
        {
            product.Name = request.Name;
            product.Sku = request.Sku;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Stock = request.Stock;
            product.Category = request.Category;
            product.IsActive = request.IsActive;
            product.IsNew = request.IsNew;
            product.UpdatedDate = DateTime.UtcNow;

            product.ProductImages.Clear();

            foreach (var image in request.ProductImages)
            {
                product.ProductImages.Add(new ProductImage
                {
                    ImageUrl = image.ImageUrl,
                    IsMain = image.IsMain,
                });
            }
        }
    }
}



