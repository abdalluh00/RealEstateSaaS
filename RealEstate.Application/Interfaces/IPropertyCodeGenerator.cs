using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyCodeGenerator
    {
        Task<string> GenerateAsync(Guid companyId, string? shortCode , CancellationToken ct = default);
    }
}
