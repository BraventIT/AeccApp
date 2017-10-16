namespace AeccApp.Core.Models
{
    public class ApiResponse
	{
		public string Error { get; set; }

		public bool Success { get { return string.IsNullOrWhiteSpace(Error); } }

        public int StatusCode { get; set; }
	}

	public class ApiResponse<T> : ApiResponse where T : class
	{
		public T Results { get; set; }
	}
}
