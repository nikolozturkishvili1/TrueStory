using System.Linq.Expressions;
using System.Reflection;

/// <summary>
/// Provides a mechanism for mapping properties from a source type (<typeparamref name="TSource"/>) 
/// to a response type (<typeparamref name="TResponse"/>).
/// </summary>
/// <typeparam name="TSource">The source type to map from.</typeparam>
/// <typeparam name="TResponse">The response type to map to.</typeparam>
public static class EntityMapper<TSource, TResponse>
    where TSource : class
{
    private static readonly Func<TSource, TResponse> _mapFunction;

    /// <summary>
    /// Static constructor that initializes the mapping function.
    /// </summary>
    static EntityMapper()
    {
        var sourceParam = Expression.Parameter(typeof(TSource), "source");

        var constructor = typeof(TResponse).GetConstructors()
            .OrderBy(c => c.GetParameters().Length)
            .FirstOrDefault();

        if (constructor is null)
            throw new InvalidOperationException($"Type {typeof(TResponse).Name} must have at least one public constructor.");

        var constructorParams = constructor.GetParameters();
        var arguments = constructorParams.Select(param =>
        {
            var sourceProp = typeof(TSource).GetProperty(param.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (sourceProp is null)
                throw new InvalidOperationException($"Cannot map property '{param.Name}' from {typeof(TSource).Name} to {typeof(TResponse).Name}.");

            return Expression.Convert(Expression.Property(sourceParam, sourceProp), param.ParameterType);
        }).ToArray();

        var body = Expression.New(constructor, arguments);
        var lambda = Expression.Lambda<Func<TSource, TResponse>>(body, sourceParam);

        _mapFunction = lambda.Compile();
    }

    /// <summary>
    /// Maps an instance of <typeparamref name="TSource"/> to <typeparamref name="TResponse"/>.
    /// </summary>
    /// <param name="source">The source object to be mapped.</param>
    /// <returns>A new instance of <typeparamref name="TResponse"/> mapped from the source object.</returns>
    public static TResponse Map(TSource source) => _mapFunction(source);
}
