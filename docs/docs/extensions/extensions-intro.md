---
title: Intro
sidebar_position: 1
---

# Extensions

Clippo provides two main approaches for extensions:

1. [Interceptors](/docs/extensions/extensions-interceptors)
1. [Actions](/docs/extensions/extensions-actions)

Interceptors are primary intended to perform pre- or post- actions.
Like validation or syncing with 3rd party stores. It's similar to aspnetcore middlewares.

Actions was originally designed for _defered_ behavior. When on client side we need to update clips only
after click on _Save_ button. But now, they can be used to extend clippo behavior. They can contain additional properties
and can be called from client side or from backend `IClippo<TClip>.ApplyAsync`.
