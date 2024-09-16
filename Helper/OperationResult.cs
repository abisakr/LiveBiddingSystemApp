namespace Live_Bidding_System_App.Helper
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; set; }  // Renamed from Success to IsSuccess
        public string Message { get; set; }
        public T Data { get; set; }

        public OperationResult(bool isSuccess, string message, T data = default)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public static OperationResult<T> SuccessResult(string message, T data = default) => new OperationResult<T>(true, message, data);
        public static OperationResult<T> FailureResult(string message) => new OperationResult<T>(false, message);
        public static OperationResult<T> NotFoundResult() => new OperationResult<T>(false, "Item not found");
    }


}
