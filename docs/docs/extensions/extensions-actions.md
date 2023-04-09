---
title: Actions
sidebar_position: 3
---

# Actions

## Description

Actions was originally designed for _defered_ behavior. When on client side we need to update clips only
after click on _Save_ button. But now, they can be used to extend clippo behavior. They can contain additional properties
and can be called from client side or from backend `IClippo<TClip>.ApplyAsync`.  

Actions might have corresponding `ClippoActionHandler<TClip, TAction>` or `ClippoNewActionHandler<TClip, TAction>` implementations.
`ClippoActionHandler<TClip, TAction>` would be called when `clippo.ApplyAsync(...)` called. `ClippoNewActionHandler<TClip, TAction>` would be called when `StoreClipArgs.Actions` contains action.  

Implementations of `IClippoActionHandler<TClip>` and `IClippoNewActionHandler<TClip>` called on every 
action (if you need to add common behavior for all actions).  

## Built-in Actions

* [ActivateClippoAction](https://github.com/webinex/starter-kit/blob/master/src/StarterKit/Webinex.Clippo.Abstractions/ActivateClippoAction.cs)
activates previously created clip
* [ActivateNewClippoAction](https://github.com/webinex/starter-kit/blob/master/src/StarterKit/Webinex.Clippo.Abstractions/ActivateNewClippoAction.cs) this action might be in StoreClipArgs.Actions to immediately activate new clip
* [DeleteClipAction](https://github.com/webinex/starter-kit/blob/master/src/StarterKit/Webinex.Clippo.Abstractions/DeleteClipAction.cs) deletes clip
* [SetValuesClippoAction](https://github.com/webinex/starter-kit/blob/master/src/StarterKit/Webinex.Clippo.Abstractions/SetValuesClippoAction.cs) calls SetValues against clip entity

## Example

### Server Side

For example, we need to add Comments behavior to clips:  

Create action:  

```csharp
public class ClippoCommentAction : IClippoAction
{
    public ClippoCommentAction(
        [NotNull] string id,
        [NotNull] string comment)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Comment = comment ?? throw new ArgumentNullException(nameof(comment));
    }

    [NotNull] public string Id { get; }
    [NotNull] public string Comment { get; }
}
```

If you using default controller, you might add action to serialization settings:  

```c#
mvcBuilder
    // ....
    .AddClippoJson(x => x
      // actions with action.kind == "myactions://Comment" would be deserialized to ClippoCommentAction
      .AddAction("myactions://Comment", typeof(ClippoCommentAction)))
    .AddClippoController<Attachment>()
```

Add action handler:  

```csharp
internal class ClippoCommentActionHandler : ClippoActionHandler<Attachment, ClippoCommentAction>
{
    private readonly IClippoStore<Attachment> _clippoStore;

    public ClippoCommentActionHandler(IClippoStore<Attachment> clippoStore)
    {
        _clippoStore = clippoStore;
    }

    protected override async Task<CodedResult> HandleAsync(ClippoCommentAction action)
    {
        var clip = await _clippoStore.ByIdAsync(action.Id);
        if (clip == null) return ClippoCodes.Results.NotFound<TClip>();
        
        clip.Comment = action.Comment;
        await _clippoStore.UpdateAsync(clip);

        return CodedResults.Success();
    }
}
```

Register it:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // For actions applied on newly created attachments (in Store request)
    // Our Action doesn't implement it
    // services.AddScoped<IClippoNewActionHandler<Attachment>, ClippoCommentActionHandler>();

    services.AddScoped<IClippoActionHandler<Attachment>, ClippoCommentActionHandler>();
}        
```

Use it: 

```csharp
public class YourService
{
    private readonly IClippo<Attachment> _clippo;
    
    public async Task AddCommentAsync(Guid attachmentId, string comment)
    {
        var action = new ClippoCommentAction(attachmentId.ToString(), comment);
        await _clippo.ApplyAsync(action);
    }
}
```

### Client Side

Define action:

```typescript jsx
class ClippoCommentAction implements ClippoAction {
    public kind: string = "myactions://Comment";
    public id: string;
    public comment: string;

    constructor(id: string, comment: string) {
        this.id = id;
        this.comment = comment;
    }
}
```

Call it:

```typescript jsx
const clippo = useClippo({...});
const {apply} = clippo;

// in defer mode, it would be saved in actions.
// In non-defer mode - it would be automatically sent to server.
await apply([new ClippoCommentAction("123", "my comment")]);
```

When your action might affect clip items, you might define `apply` method in action.

:::caution
`apply` might not mutate value
:::caution

```jsx typescript
class ClippoCommentAction implements ClippoAction {
  public kind: string = "myactions://Comment";
  public id: string;
  public comment: string;

  constructor(id: string, comment: string) {
      this.id = id;
      this.comment = comment;
  }

  public apply<Attachment>(
    value: ClippoStateValue<Attachment>,
    options: ClippoOptions<Attachment>,
  ) {
    const { getId } = options.model;
    const { items } = value;

    return items.map((attachment) => {
      if (this.id === getId(attachment)) {
        return { ...attachment, comment: this.comment };
      }

      return attachment;
    });
  }
}
```

When your action might replace another actions, you might define `shake` method in action.

```jsx typescript
class ClippoCommentAction implements ClippoAction {
  public kind: string = "myactions://Comment";
  public id: string;
  public comment: string;

  constructor(id: string, comment: string) {
      this.id = id;
      this.comment = comment;
  }
  
  // .....

  public shake(
    value: ClippoStateValue<Attachment>,
    options: ClippoOptions<Attachment>,
  ): ClippoStateValue<Attachment> {
    const index = value.actions.indexOf(this);
    const previous = value.actions.slice(0, index);
    const surplus = value.actions.slice(index);

    const shakedPrevious = previous.filter(action => action.kind !== this.kind || action.id !== this.id);

    return {
      ...value,
      actions: [...shakedPrevious, ...surplus],
    }
  }
}
```