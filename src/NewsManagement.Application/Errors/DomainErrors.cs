using Shared.Library;

namespace NewsManagement.Application.Errors
{
    public static class DomainErrors
    {
        public static class Todo
        {
            public static readonly Func<Guid, Error> NotFound = id => new(
                "Todo.NotFound",
                $"The todo with the identifier {id} was not found.",
                ErrorTypeEnum.NotFound
            );

            public static readonly Error ListEmpty = new(
                "Todo.ListEmpty",
                "Todo list is empty",
                ErrorTypeEnum.NoContent
            );
        }
    }
}