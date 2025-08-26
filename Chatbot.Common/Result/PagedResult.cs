namespace Chatbot.Common.Result
{
    public class RequestBase
    {
        //[Required(ErrorMessage = "Không xác định tài khoản truy cập")]
        public string UserReqId { get; set; }
        //[Required(ErrorMessage = "Không xác định đơn vị truy cập")]
        public string UniqueCode { get; set; }

        public virtual bool IsValid { get => !String.IsNullOrWhiteSpace(UserReqId) /*&& !String.IsNullOrWhiteSpace(UniqueCode)*/; }
    }

    public class PagingRequestBase : RequestBase
    {
        public int PageIndex { get; set; } = SystemConstants.pageIndex;

        public int PageSize { get; set; } = SystemConstants.pageSize;
    }

    public class PagedResultBase
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int PageCount
        {
            get
            {
                var pageCount = (double)TotalRecords / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
        }
    }

    public class PagedResult<T> : PagedResultBase
    {
        public string Keyword { get; set; } = String.Empty;
        public List<T> Items { set; get; }
    }

    public class GetPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; } = string.Empty;
        public int OrganId { get; set; }
        public bool IsRoot { get; set; }
    }
    public class PagingBase
    {
        public int PageIndex { get; set; } = SystemConstants.pageIndex;

        public int PageSize { get; set; } = SystemConstants.pageSize;
    }
}
