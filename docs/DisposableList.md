## DisposableList<T>

This class is a specialized version of `List<T>` (inheriting from `List<T>` directly rather than wrapping around it), implementing `IDispose`.
When this disposable type is disposed it disposes of each of its elements, exposing a cleaner and less error-prone alternative to manually iterating over the contents of a `List<T>` and calling `Dispose()` on them one-by-one after you have finished with the type.

### `DisposableList<T>()`

Creates a new `DisposableList<T>` with the default capacity and no elements.

### `DisposableList<T>(int capacity)`

Creates a new `DisposableList<T>` with its capacity set to `capacity`.

### `DisposableList<T>(IEnumerable<T> enumerable)`

Creates a new `DisposableList<T>` initialized with the items in `enumerable`.

### `DisposableList<T>.Dispose()`

Disposes of each of the items currently in the disposable list.
If any exceptions are encountered calling `T.Dispose()` against any of the elements, an `AggregateException` is returned with the encountered exceptions.
An exception does not halt the dispose process, and the `AggregateException` is only returned after all items have been disposed.
The contents of the list after a call to `DisposableList<T>.Dispose()` are undefined and accessing any of the lists properties or values after such a call will result in undefined behavior.
