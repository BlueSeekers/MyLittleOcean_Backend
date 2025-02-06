public class ServiceResult<T> {
    public bool Success { get; }
    public string Message { get; }
    public T? Data { get; }

    /// <summary>
    /// Service Result
    /// </summary>
    /// <param name="success">���� ����</param>
    /// <param name="message">�޼���</param>
    /// <param name="data">Return Data</param>
    public ServiceResult(bool success, string message, T? data = default) {
        Success = success;
        Message = message;
        Data = data;
    }
}