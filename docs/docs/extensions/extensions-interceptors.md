---
title: Interceptors
sidebar_position: 2
---

# Interceptors
Interceptors behavior similar to middlewares. They would be executed
in reverse order they added and same order after calling `next.InvokeAsync(args)`.
If interceptor returns failed CodedResult, than request pipeline interrupted and CodedResult returned.  
Default ClippoController would return failed status code with serialized `x-coded-failure` header.  

## Built-in Interceptors

* `.AddMaxSize(int sizeInBytes)`: validates store file content size. Returns `INV.CLIPPO.MAX`
coded failure if content exceed max size
* `.AddNoEmpty()`: validates store file not empty. Returns `INV.CLIPPO.NO_EMPTY` coded
failure if content exceed max size
* EF `.AddSaveChanges`: calls `dbContext.SaveChangesAsync` on `ApplyAsync` and `StoreAsync` successful results
* EF `.AddSaveChangesWhenUrlPathStartsWith(string path)`: calls `dbContext.SaveChangesAsync` on `ApplyAsync`
and `StoreAsync` successful results when request path starts with `path` segment

## Example

For example, interceptor to validate content not exceed max size  

```csharp
internal class MaxSizeInterceptor<Attachment> : ClippoInterceptor<Attachment>
{
    private readonly MaxSizeSettings _maxSizeSettings;

    public MaxSizeInterceptor(MaxSizeSettings maxSizeSettings)
    {
        _maxSizeSettings = maxSizeSettings;
    }

    public override async Task<CodedResult<Attachment[]>> OnStoreAsync(
        IEnumerable<StoreClipArgs> args,
        INext<IEnumerable<StoreClipArgs>, CodedResult<Attachment[]>> next)
    {
        args = args.ToArray();

        foreach (var arg in args)
        {
            if (arg.Content.Value.Length > _maxSizeSettings.Value)
                return ClippoCodes.Results.MaxSize<Attachment[]>(arg.Content);
        }

        return await base.OnStoreAsync(args, next);
    }
}
```

Register your interceptor

```csharp
    return services.AddClippo<Attachment>(x =>
    {
        x
            ...
            .Interceptors
            .Add<MaxSizeInterceptor>();
    });
```