using FluentValidation;
using MediatR;

namespace OnlineStore.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

            var errors = validationResults
                .SelectMany(r => r.Errors)
                .Where(e => e != null)
                .ToList();

            if (errors.Any())
                throw new ValidationException(errors);

            return await next();
        }

        //public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        //{
        //    if (!_validators.Any())
        //        return await next();

        //    ValidationContext<TRequest> context = new ValidationContext<TRequest>(request);
        //    var errors = _validators
        //        .Select(v => v.Validate(context))
        //        .SelectMany(r => r.Errors)
        //        .Where(e => e != null)
        //        .ToList();
        //    if (errors.Any())
        //        throw new ValidationException(errors);

        //    return await next();

        //}
    }
}
