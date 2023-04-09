---
title: Authorization
sidebar_position: 8
---

# Authorization

There two approaches (you can use both):
1. You can add your own interceptor
2. You can setup global authorization  

For example, user might have role Admin for attachments:  

## Inteceptor

```csharp
internal class AuthorizationInterceptor : ClippoInterceptor<Attachment>
{
    private readonly IRoleService _roleService;

    public AuthorizationInterceptor(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public override async Task<CodedResult<Attachment[]>> OnStoreAsync(
        IEnumerable<StoreClipArgs> args,
        INext<IEnumerable<StoreClipArgs>, CodedResult<Attachment[]>> next)
    {
        var isAdmin = await _roleService.HasAdminRole();
        if (!isAdmin) {
            return CodedResults.Of<Attachment[]>().Failed("NOT_ADMIN");
        }

        return await base.OnStoreAsync(args, next);
    }
}
```

And register it:  

```csharp
return services.AddClippo<Attachment>(x =>
{
    x
        ...
        .Interceptors
        .Add<AuthorizationInterceptor>();
});
```

## Controller level  

```csharp
const POLICY_NAME = "my_policy";

services
    .AddControllers()
    // ....
    .AddClippoJson()
    .AddClippoController<Attachment>(x => x
        .UsePolicy(JwtBearerDefaults.AuthenticationScheme, POLICY_NAME))
    .Services
    // ...
    .AddAuthorization(auth =>
    {
        auth.AddPolicy(POLICY_NAME, options => options
            .RequireAuthenticatedUser()
            .RequireRole("ADMIN"));
    });
```