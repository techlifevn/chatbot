using Chatbot.Common;
using Chatbot.Common.Result;
using Chatbot.Data.EF;
using Chatbot.Data.Entity;
using Chatbot.Model.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chatbot.Service
{
    public interface IResponseService
    {
        Task<PagedResult<ResponseVm>> GetPaging(GetPagingRequest request);
        Task<List<ResponseVm>> GetAll();
        Task<Result<bool>> Create(ResponseCreateRequest request);
        Task<Result<bool>> Update(ResponseUpdateRequest request);
        Task<Result<bool>> Delete(int id, string userId);
        Task<Result<bool>> UpdateStatus(int id, string userId);
        Task IncrementResponseUsage(int id);
    }
    public class ResponseService : IResponseService
    {
        private readonly DataContext _context;
        private readonly ILogger<ResponseService> _logger;

        public ResponseService(DataContext context, ILogger<ResponseService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<bool>> Create(ResponseCreateRequest request)
        {
            try
            {
                int intentId = Functions.DecodeId(request.IntentId);

                if (!await _context.Intents.AnyAsync(x => x.Id == intentId && !x.IsDelete))
                    return new ErrorResult<bool>("Nhãn không tồn tại");

                var entity = new Response
                {
                    ResponseText = request.ResponseText.Trim(),
                    IntentId = intentId,
                    UsageCount = 0,
                    CreateByUserId = request.UserId,
                    CreateOnDate = DateTime.Now
                };

                await _context.Responses.AddAsync(entity);

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
                var entity = await _context.Responses.FindAsync(id);

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

        public async Task<List<ResponseVm>> GetAll()
        {
            try
            {
                var data = await _context.Responses
                    .Where(x => !x.IsDelete && x.IsStatus)
                    .Select(x => new ResponseVm
                    {
                        Id = x.Id,
                        ResponseText = x.ResponseText,
                        IntentId = x.IntentId,
                        UsageCount = x.UsageCount
                    }).ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<PagedResult<ResponseVm>> GetPaging(GetPagingRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task IncrementResponseUsage(int id)
        {
            var r = await _context.Responses.FindAsync(id);

            if (r != null) r.UsageCount++;

            await _context.SaveChangesAsync();
        }

        public async Task<Result<bool>> Update(ResponseUpdateRequest request)
        {
            try
            {
                int id = Functions.DecodeId(request.Id);

                var entity = await _context.Responses.FindAsync(id);

                if (entity == null || entity.IsDelete)
                    return new ErrorResult<bool>("Dữ liệu không tồn tại");

                int intentId = Functions.DecodeId(request.IntentId);

                if (!await _context.Intents.AnyAsync(x => x.Id == intentId && !x.IsDelete))
                    return new ErrorResult<bool>("Dữ liệu không tồn tại");

                entity.ResponseText = request.ResponseText.Trim();
                entity.IntentId = intentId;

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
                var entity = await _context.Responses.FindAsync(id);

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
