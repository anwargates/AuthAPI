namespace AuthAPI.models.response
{
    public class GenericResponse
    {
        public int StatusCode { get; set; }
        public string Description { get; set; }
        public object? Data { get; set; }
        public DateTime Timestamp { get; set; }


        public GenericResponse(int statusCode, string description, object? data)
        {
            StatusCode = statusCode;
            Description = description;
            Data = data;
            Timestamp = DateTime.UtcNow;
        }

        public GenericResponse(int statusCode, string description)
        {
            StatusCode = statusCode;
            Description = description;
            Timestamp = DateTime.UtcNow;
        }
    }

}