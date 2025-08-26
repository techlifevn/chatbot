using Chatbot.Common;
using Chatbot.Common.Result;
using Chatbot.Data.EF;
using Chatbot.Data.Entity;
using Chatbot.Model.Pattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Chatbot.Service
{

    public interface IPatternService
    {
        Task<PagedResult<PatternVm>> GetPaging(GetPagingRequest request);
        Task<List<PatternVm>> GetAll();
        Task<Result<bool>> Create(PatternCreateRequest request);
        Task<Result<bool>> Update(PatternUpdateRequest request);
        Task<Result<bool>> Delete(int id, string userId);
        Task<Result<bool>> UpdateStatus(int id, string userId);
    }

    public class PatternService : IPatternService
    {
        private readonly DataContext _context;
        private readonly ILogger<PatternService> _logger;

        public PatternService(DataContext context, ILogger<PatternService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<bool>> Create(PatternCreateRequest request)
        {
            try
            {
                int intentId = Functions.DecodeId(request.IntentId);

                if (!await _context.Intents.AnyAsync(x => x.Id == intentId && !x.IsDelete))
                {
                    return new ErrorResult<bool>("Nhãn không tồn tại");
                }

                var entity = new Pattern
                {
                    PatternText = request.PatternText.Trim(),
                    IntentId = intentId,
                    CreateByUserId = request.UserId,
                    CreateOnDate = DateTime.Now
                };

                await _context.Patterns.AddAsync(entity);

                await _context.SaveChangesAsync();

                return new SuccessResult<bool>("Thêm thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<Result<bool>> Delete(int id, string userId)
        {
            try
            {
                var entity = await _context.Patterns.FindAsync(id);

                if (entity == null || entity.IsDelete) return new ErrorResult<bool>("Dữ liệu không tồn tại");

                entity.IsDelete = true;
                entity.LastModifiedOnDate = DateTime.Now;
                entity.LastModifiedByUserId = userId;

                await _context.SaveChangesAsync();

                return new SuccessResult<bool>("Xóa thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<PatternVm>> GetAll()
        {
            try
            {
                var data = await _context.Patterns
                    .Where(x => !x.IsDelete && x.IsStatus)
                    .Select(x => new PatternVm
                    {
                        Id = x.Id,
                        PatternText = x.PatternText,
                        IntentId = x.IntentId,
                    }).ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<PagedResult<PatternVm>> GetPaging(GetPagingRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<bool>> Update(PatternUpdateRequest request)
        {
            try
            {
                int id = Functions.DecodeId(request.Id);

                var entity = await _context.Patterns.FindAsync(id);

                if (entity == null || entity.IsDelete) return new ErrorResult<bool>("Dữ liệu không tồn tại");

                int intentId = Functions.DecodeId(request.IntentId);

                if (!await _context.Intents.AnyAsync(x => x.Id == intentId && !x.IsDelete)) return new ErrorResult<bool>("Dữ liệu không tồn tại");

                entity.PatternText = request.PatternText.Trim();
                entity.IntentId = intentId;
                entity.LastModifiedByUserId = request.UserId;
                entity.LastModifiedOnDate = DateTime.Now;

                await _context.SaveChangesAsync();

                return new SuccessResult<bool>("Cập nhật thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<Result<bool>> UpdateStatus(int id, string userId)
        {
            try
            {
                var entity = await _context.Patterns.FindAsync(id);

                if (entity == null || entity.IsDelete) return new ErrorResult<bool>("Dữ liệu không tồn tại");

                entity.IsStatus = !entity.IsStatus;
                entity.LastModifiedByUserId = userId;
                entity.LastModifiedOnDate = DateTime.Now;

                await _context.SaveChangesAsync();

                return new SuccessResult<bool>("Cập nhật trạng thái thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}