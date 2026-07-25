namespace MenuFast.Api.Middlewares {
    [Serializable]
    public class BusinessLogicException : Exception {
        public BusinessLogicException() {
        }

        public BusinessLogicException(string? message) : base(message) {
        }

        public BusinessLogicException(string? message, Exception? innerException) : base(message, innerException) {
        }
        public static void ThrowIfFalse(bool condicao, string mensagem) {
            if(!condicao)
            {
                throw new BusinessLogicException(mensagem);
            }
        }

        public static void ThrowIfNull(object? value, string message) {
            if(value is null)
                throw new BusinessLogicException(message);
        }

        public static void ThrowIfNullOrEmpty(string? value, string message) {
            if(string.IsNullOrWhiteSpace(value))
                throw new BusinessLogicException(message);
        }


    }
}