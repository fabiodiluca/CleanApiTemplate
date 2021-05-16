using AutoMapper;
using CleanTemplate.UnitOfWork;

namespace CleanTemplate.Application.UseCases
{
    public class UseCaseBase : IUseCaseBase
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public UseCaseBase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void BeginTransaction()
        {
            _unitOfWork.BeginTransaction();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
