using Convey.CQRS.Queries;
using System.Threading.Tasks;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Services;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetValueHandler : IQueryHandler<GetValue, ValueDto>
    {
        private readonly IValueRepository _service;

        public GetValueHandler(IValueRepository service)
        {
            _service = service;
        }

        public async Task<ValueDto> HandleAsync(GetValue query)
        {
            return await _service.GetById(query.Id);
        }
    }
}
