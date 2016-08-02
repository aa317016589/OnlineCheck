namespace OnlineCheck.Web.Models
{
    public class ActionHandleResult
    {
        public int Success { get; set; }

        public dynamic Data { get; set; }

        public string Message { get; set; }

        public static ActionHandleResult FromSuccess(int success=0, dynamic data = default(dynamic), string message = "")
        {
            return new ActionHandleResult()
            {
                Success = success,
                Data = data,
                Message = message
            };
        }

        public static ActionHandleResult FromFail(int success=1, dynamic data = default(dynamic), string message = "")
        {
            return new ActionHandleResult()
            {
                Success = success,
                Data = data,
                Message = message
            };
        }

       

    }
}