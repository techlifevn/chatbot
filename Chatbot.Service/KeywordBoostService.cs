using Chatbot.Common;
using Chatbot.Common.Result;
using Chatbot.Data.EF;
using Chatbot.Data.Entity;
using Chatbot.Model.KeywordBoost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chatbot.Service
{
    public interface IKeywordBoostService
    {
        Task<PagedResult<KeywordBoostVm>> GetPaging(GetPagingRequest request);
        Task<List<KeywordBoostVm>> GetAll();
        Task<KeywordBoostVm?> GetById(int id);
        Task<Result<bool>> Create(KeywordBoostCreateRequest request);
        Task<Result<bool>> Update(KeywordBoostUpdateRequest request);
        Task<Result<bool>> Delete(int id, string userId);
        Task<Result<bool>> UpdateStatus(int id, string userId);
    }

    public class KeywordBoostService : IKeywordBoostService
    {
        private readonly DataContext _context;
        private readonly ILogger<IKeywordBoostService> _logger;

        public KeywordBoostService(DataContext context, ILogger<IKeywordBoostService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<bool>> Create(KeywordBoostCreateRequest request)
        {
            try
            {
                var entity = new KeywordBoost
                {
                    Keyword = request.Keyword.Trim(),
                    Boost = request.Boost,
                    CreateByUserId = request.UserId,
                    CreateOnDate = DateTime.Now
                };

                await _context.KeywordBoosts.AddAsync(entity);

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
                var entity = await _context.KeywordBoosts.FindAsync(id);

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

        public async Task<List<KeywordBoostVm>> GetAll()
        {
            try
            {
                var data = await _context.KeywordBoosts
                    .Where(x => !x.IsDelete && x.IsStatus)
                    .Select(x => new KeywordBoostVm
                    {
                        Id = x.Id,
                        Keyword = x.Keyword,
                        Boost = x.Boost,
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

        public async Task<KeywordBoostVm?> GetById(int id)
        {
            try
            {
                var query = _context.KeywordBoosts
                    .Where(x => x.Id == id && !x.IsDelete)
                    .Select(x => new KeywordBoostVm
                    {
                        Id = x.Id,
                        Keyword = x.Keyword,
                        Boost = x.Boost,
                        IntentId = x.IntentId
                    });

                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagedResult<KeywordBoostVm>> GetPaging(GetPagingRequest request)
        {
            try
            {
                string keyword = !string.IsNullOrWhiteSpace(request.Keyword) ? request.Keyword.Trim().ToLower() : "";

                var query = _context.KeywordBoosts.Where(x => !x.IsDelete);

                if (string.IsNullOrWhiteSpace(keyword)) query = query.Where(x => x.Keyword.ToLower().Contains(keyword));

                int totalRow = await query.CountAsync();

                var data = await query
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new KeywordBoostVm
                    {
                        Id = x.Id,
                        Keyword = x.Keyword,
                        Boost = x.Boost,
                        IsStatus = x.IsStatus,
                        IntentId = x.IntentId
                    }).ToListAsync();

                return new PagedResult<KeywordBoostVm>
                {
                    TotalRecords = totalRow,
                    Keyword = keyword,
                    Items = data,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<Result<bool>> Update(KeywordBoostUpdateRequest request)
        {
            try
            {
                int id = Functions.DecodeId(request.Id);

                var entity = await _context.KeywordBoosts.FindAsync(id);

                if (entity == null || entity.IsDelete) return new ErrorResult<bool>("Dữ liệu không tồn tại");

                entity.Keyword = request.Keyword.Trim();
                entity.Boost = request.Boost;
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
                var entity = await _context.KeywordBoosts.FindAsync(id);

                if (entity == null || entity.IsDelete) return new ErrorResult<bool>("Dữ liệu không tồn tại");

                entity.IsStatus = !entity.IsStatus;
                entity.LastModifiedByUserId = userId;
                entity.LastModifiedOnDate = DateTime.Now;

                await _context.SaveChangesAsync();

                return new SuccessResult<bool>("Câph nhật trạng thái thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
