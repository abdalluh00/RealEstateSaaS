using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyCodeGenerator : IPropertyCodeGenerator
    {
        private readonly AppDbContext _context;

        public PropertyCodeGenerator(AppDbContext context) => _context = context;

        public async Task<string> GenerateAsync(
            Guid companyId,string shortCode = "PROP",
            CancellationToken ct = default)
        {
            var year = DateTime.UtcNow.Year;

            var count = await _context.Properties
                .CountAsync(x => x.CompanyId == companyId, ct);

            // PROP-2024-0001
            return $"{shortCode}-{year}-{(count + 1):D4}";
        }
    }
}
