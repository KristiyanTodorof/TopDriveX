using TopDriveX.Domain.BaseEntities;
using TopDriveX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TopDriveX.Domain.Models;

namespace TopDriveX.Infrastructure.ExternalServices.Nhtsa;

public class NhtsaImportService
{
    private readonly NhtsaService _nhtsaService;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<NhtsaImportService> _logger;

    public NhtsaImportService(
        NhtsaService nhtsaService,
        ApplicationDbContext context,
        ILogger<NhtsaImportService> logger)
    {
        _nhtsaService = nhtsaService;
        _context = context;
        _logger = logger;
    }

    public async Task ImportMakesAsync()
    {
        _logger.LogInformation("Starting import of makes from NHTSA");

        var nhtsaMakes = await _nhtsaService.GetAllMakesAsync();

        _logger.LogInformation("Received {Count} makes from NHTSA API", nhtsaMakes.Count);

        if (!nhtsaMakes.Any())
        {
            _logger.LogWarning("No makes returned from NHTSA API");
            return;
        }

        var imported = 0;
        var skipped = 0;

        foreach (var nhtsaMake in nhtsaMakes.Take(50)) // Limit to first 50 for now
        {
            _logger.LogInformation("Processing make: {MakeName} (ID: {MakeId})", nhtsaMake.Make_Name, nhtsaMake.Make_ID);

            var exists = await _context.Makes
                .AnyAsync(m => m.NhtsaMakeId == nhtsaMake.Make_ID);

            if (exists)
            {
                _logger.LogInformation("Make {MakeName} already exists, skipping", nhtsaMake.Make_Name);
                skipped++;
                continue;
            }

            var make = new Make
            {
                Name = nhtsaMake.Make_Name,
                NhtsaMakeId = nhtsaMake.Make_ID
            };

            _context.Makes.Add(make);
            imported++;
            _logger.LogInformation("Added make: {MakeName}", nhtsaMake.Make_Name);
        }

        _logger.LogInformation("Saving {Count} makes to database", imported);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Import completed. Imported: {Imported}, Skipped: {Skipped}", imported, skipped);
    }

    public async Task ImportModelsForMakeAsync(Guid makeId)
    {
        var make = await _context.Makes.FindAsync(makeId);
        if (make == null || make.NhtsaMakeId == null)
        {
            _logger.LogWarning("Make not found or doesn't have NHTSA ID");
            return;
        }

        _logger.LogInformation("Importing models for make: {MakeName}", make.Name);

        var nhtsaModels = await _nhtsaService.GetModelsForMakeAsync(make.NhtsaMakeId.Value);

        var imported = 0;

        foreach (var nhtsaModel in nhtsaModels)
        {
            var exists = await _context.Models
                .AnyAsync(m => m.NhtsaModelId == nhtsaModel.Model_ID && m.MakeId == makeId);

            if (exists) continue;

            var model = new Model
            {
                MakeId = makeId,
                Name = nhtsaModel.Model_Name,
                NhtsaModelId = nhtsaModel.Model_ID
            };

            _context.Models.Add(model);
            imported++;
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Imported {Count} models for {MakeName}", imported, make.Name);
    }
}