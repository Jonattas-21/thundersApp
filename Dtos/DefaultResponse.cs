namespace thundersApp.Dtos
{
    public class DefaultResponse
    {
        public DefaultResponse() { }

        public DefaultResponse(string message, object responseObject)
        {
            Message = message;
            Data = responseObject;
        }

        public string Message { get; set; }
        public object Data { get; set; }
    }
}
