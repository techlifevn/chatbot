using Chatbot.Common;
using Chatbot.Common.Result;
using Chatbot.Data.EF;
using Chatbot.Data.Entity;
using Chatbot.Model.Intent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Chatbot.Service
{
    public interface IIntentService
    {
        Task<PagedResult<IntentVm>> GetPaging(GetPagingRequest request);
        Task<List<IntentVm>> GetAll();
        Task<Result<bool>> Create(IntentCreateRequest request);
        Task<Result<bool>> Update(IntentUpdateRequest request);
        Task<Result<bool>> Delete(int id, string userId);
        Task<Result<bool>> UpdateStatus(int id, string userId);

    }

    public class IntentService : IIntentService
    {
        private readonly DataContext _context;
        private readonly ILogger<IIntentService> _logger;

        public IntentService(DataContext context, ILogger<IIntentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<bool>> Create(IntentCreateRequest request)
        {
            try
            {
                var entity = new Intent
                {
                    Name = request.Name.Trim(),
                    Tag = request.Tag.Trim().ToLower(),
                    DefaultResponse = request.DefaultResponse,
                    Priority = request.Priority,
                    CreateByUserId = request.UserId
                };

                await _context.Intents.AddAsync(entity);

                await _context.SaveChangesAsync();

                return new SuccessResult<bool>("Thêm nhãn thành công");
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
                var entity = await _context.Intents.FindAsync(id);

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

        public async Task<List<IntentVm>> GetAll()
        {
            try
            {
                var data = await _context.Intents
                    .Where(x => !x.IsDelete && x.IsStatus)
                    .Select(x => new IntentVm
                    {
                        Id = x.Id,
                        Tag = x.Tag,
                        DefaultResponse = x.DefaultResponse,
                        Priority = x.Priority,
                    }).ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<PagedResult<IntentVm>> GetPaging(GetPagingRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<bool>> Update(IntentUpdateRequest request)
        {
            try
            {
                int id = Functions.DecodeId(request.Id);
                var entity = await _context.Intents.FindAsync(id);

                if (entity == null || entity.IsDelete) return new ErrorResult<bool>("Dữ liệu không tồn tại");

                entity.Name = request.Name.Trim();
                entity.Tag = request.Tag.Trim().ToLower();
                entity.DefaultResponse = request.DefaultResponse.Trim();
                entity.Priority = request.Priority;

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
                var entity = await _context.Intents.FindAsync(id);

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
