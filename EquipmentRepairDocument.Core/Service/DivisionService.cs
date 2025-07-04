using EquipmentRepairDocument.Core.Data.StandardModel;
using EquipmentRepairDocument.Core.DBContext;
using EquipmentRepairDocument.Core.Exceptions;
using EquipmentRepairDocument.Core.Exceptions.AppException;
using EquipmentRepairDocument.Core.FactoryData;
using Microsoft.EntityFrameworkCore;

namespace EquipmentRepairDocument.Core.Service
{
    public class DivisionService(AppDbContext dbContext, DivisionFactory divisionFactory)
    {
        public async Task AddRangeAsync(List<Division> divisions, CancellationToken cancellationToken = default)
        {
            BusinessLogicException.ThrowIfNull(divisions);
            foreach (var division in divisions)
            {
                await AddAsync(division, cancellationToken);
            }
        }

        public async Task AddAsync(Division division, CancellationToken cancellationToken = default)
        {
            BusinessLogicException.ThrowIfNull(division);

            var existingDivision = await dbContext.Divisions.FirstOrDefaultAsync(e => e.Abbreviation == division.Abbreviation
                                                                           || e.Name == division.Name
                                                                           || e.Number == division.Number,
                                                                           cancellationToken);

            if (existingDivision == null)
            {
                var divisionNormalize = divisionFactory.Create(division.Name, division.Abbreviation, division.Number);
                await dbContext.Divisions.AddAsync(divisionNormalize, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
                return;
            }

            if (existingDivision.Abbreviation == division.Abbreviation)
            {
                BusinessLogicException.EnsureUniqueProperty<Division>(existingDivision.Abbreviation);
            }

            if (existingDivision.Name == division.Name)
            {
                BusinessLogicException.EnsureUniqueProperty<Division>(existingDivision.Name);
            }

            if (existingDivision.Number == division.Number)
            {
                BusinessLogicException.EnsureUniqueProperty<Division>(existingDivision.Number);
            }
        }

        public async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var count = await dbContext.Divisions.Where(e => e.Id == id).ExecuteDeleteAsync(cancellationToken);
            if (count == 0)
            {
                throw new NotFoundException($"The ID for the division with \"{id}\" is already taken.");
            }
        }

        public async Task RemoveAsync(byte number, CancellationToken cancellationToken = default)
        {
            var count = await dbContext.Divisions.Where(e => e.Number == number).ExecuteDeleteAsync(cancellationToken);
            if (count == 0)
            {
                throw new NotFoundException($"The division number \"{number}\" not found.");
            }
        }
    }
}
