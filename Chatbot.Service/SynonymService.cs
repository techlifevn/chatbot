using Chatbot.Common;
using Chatbot.Common.Result;
using Chatbot.Data.EF;
using Chatbot.Data.Entity;
using Chatbot.Model.Synonym;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chatbot.Service
{
    public interface ISynonymService
    {

        Task<PagedResult<SynonymVm>> GetPaging(GetPagingRequest request);
        Task<List<SynonymVm>> GetAll();
        Task<Result<bool>> Create(SynonymCreateRequest request);
        Task<Result<bool>> Update(SynonymUpdateRequest request);
        Task<Result<bool>> Delete(int id, string userId);
        Task<Result<bool>> UpdateStatus(int id, string userId);
    }

    public class SynonymService : ISynonymService
    {
        private readonly DataContext _context;
        private readonly ILogger<SynonymService> _logger;

        public SynonymService(DataContext context, ILogger<SynonymService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<bool>> Create(SynonymCreateRequest request)
        {
            try
            {
                var entity = new Synonym
                {
                    SynonymText = request.SynonymText.Trim(),
                    MainTerm = request.MainTerm.Trim(),
                    CreateByUserId = request.UserId,
                    CreateOnDate = DateTime.Now
                };

                await _context.AddAsync(entity);

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
                var entity = await _context.Synonyms.FindAsync(id);

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

        public async Task<List<SynonymVm>> GetAll()
        {
            try
            {
                var data = await _context.Synonyms
                    .Where(x => !x.IsDelete && x.IsStatus)
                    .Select(x => new SynonymVm
                    {
                        Id = x.Id,
                        MainTerm = x.MainTerm,
                        SynonymText = x.SynonymText
                    }).ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagedResult<SynonymVm>> GetPaging(GetPagingRequest request)
        {
            try
            {
                string keyword = !string.IsNullOrWhiteSpace(request.Keyword) ? request.Keyword.Trim().ToLower() : "";

                var query = _context.Synonyms.Where(x => !x.IsDelete);

                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    query = query.Where(x => x.SynonymText.ToLower().Contains(keyword) || x.MainTerm.ToLower().Contains(keyword));

                int totalRow = await query.CountAsync();

                var data = await query
                    .OrderByDescending(x => x.Id)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new SynonymVm
                    {
                        Id = x.Id,
                        SynonymText = x.SynonymText,
                        MainTerm = x.MainTerm,
                        IsStatus = x.IsStatus
                    }).ToListAsync();

                return new PagedResult<SynonymVm>
                {
                    Keyword = keyword,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalRecords = totalRow,
                    Items = data
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<Result<bool>> Update(SynonymUpdateRequest request)
        {
            try
            {
                int id = Functions.DecodeId(request.Id);

                var entity = await _context.Synonyms.FindAsync(id);

                if (entity == null || entity.IsDelete) return new ErrorResult<bool>("Dữ liệu không tồn tại");

                entity.SynonymText = request.SynonymText.Trim();
                entity.MainTerm = request.MainTerm.Trim();
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
                var entity = await _context.Synonyms.FindAsync(id);

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
