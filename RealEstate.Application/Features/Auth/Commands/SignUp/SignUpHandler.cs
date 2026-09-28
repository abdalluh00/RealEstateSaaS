//using MediatR;
//using RealEstate.Domain.Common.Enums;
//using RealEstate.Domain.Entities;
//using RealEstate.Domain.Interfaces;
//using RealEstate.Shared.Common;
//using RealEstate.Shared.Common.Exceptions;
//using RealEstate.Shared.Helpers;
//namespace RealEstate.Application.Features.Auth.Commands.SignUp
//{
//    public class SignUpHandler : IRequestHandler<SignUpCommand, ApiResponse<SignUpResult>>
//    {
//        private readonly IUserRepository _userRepo;
//        private readonly ICompanyRepository _companyRepo;
//        private readonly IJwtService _jwtService;
//        private readonly IUnitOfWork _unitOfWork;

//        public SignUpHandler(
//            IUserRepository userRepo,
//            ICompanyRepository companyRepo,
//            IJwtService jwtService,
//            IUnitOfWork unitOfWork)
//        {
//            _userRepo = userRepo;
//            _companyRepo = companyRepo;
//            _jwtService = jwtService;
//            _unitOfWork = unitOfWork;
//        }

//        public async Task<ApiResponse<SignUpResult>> Handle(
//            SignUpCommand request,
//            CancellationToken ct)
//        {
//            // 1 — تحقق من البريد الإلكتروني
//            var emailExists = await _userRepo.EmailExistsAsync(request.Email);
//            if (emailExists)
//                throw new ConflictException("البريد الإلكتروني مسجل مسبقاً");

//            // 2 — تحقق من رقم جوال الشركة
//            var phoneExists = await _companyRepo.PhoneExistsAsync(request.CompanyPhone);
//            if (phoneExists)
//                throw new ConflictException("رقم جوال الشركة مسجل مسبقاً");

//            await _unitOfWork.BeginTransactionAsync();

//            try
//            {
//                // 3 — إنشاء الشركة
//                var company = new Company
//                {
//                    Name = request.CompanyName,
//                    Phone = request.CompanyPhone,
//                    Address = request.CompanyAddress,
                    
//                    SubscriptionExpiry = DateTime.UtcNow.AddDays(30), // تجربة مجانية 30 يوم
//                    IsActive = true
//                };

//                await _companyRepo.AddAsync(company);

//                // 4 — إنشاء المستخدم Owner
//                var user = new User
//                {
//                    FullName = request.FullName,
//                    Email = request.Email,
//                    Phone = request.Phone,
//                    PasswordHash = PasswordHelper.Hash(request.Password),
//                    Role = UserRole.Owner,
//                    CompanyId = company.Id,
//                    IsActive = true
//                };

//                await _userRepo.AddAsync(user);

//                // 5 — Save كل شيء في Transaction واحدة
//                await _unitOfWork.CommitAsync();

//                // 6 — ولّد الـ Token
//                var token = _jwtService.GenerateToken(user);

//                return ApiResponse<SignUpResult>.Ok(new SignUpResult(
//                    token,
//                    user.FullName,
//                    user.Email,
//                    user.Role,
//                    company.Id,
//                    DateTime.UtcNow.AddDays(7)
//                ), "تم إنشاء الحساب بنجاح 🎉");
//            }
//            catch
//            {
//                await _unitOfWork.RollbackAsync();
//                throw;
//            }
//        }
//    }
//}
