## ResizableArray<T>

This class serves as a wrapper around `T[]`, implementing `ICollection` and `IReadOnlyList<T>`. It
is intended to be used when direct access to the underlying array is required; unlike `List<T>`, the
actual, underlying `T[]` may be safely and directly accessed in user code (without reflection or
hacks) via `ResizableArray<T>.Array` that forms this part of the contract.

`ResizableArray` is a lower-level data structure, it does not abstract away the `Count` of the
underlying array (i.e. there is no distinction between `Count` and `Capacity`). Its benefit over a
plain `T[]` is the incorporation of various static functions found in the `System.Array` namespace
as direct methods of this object-oriented container.

If and when Microsoft's APIs all move over to `Span<T>`, this will cease to have any purpose. Until
then, `ResizableArray` comes in handy to avoid deduplication when dealing with various
Microsoft-provided APIs that require a plain `T[]` parameter as a buffer into which the result of an
operation is stored.

### `ResizableArray<T>()`

Creates a new `ResizableArray<T>` with zero size.

### `ResizableArray<T>(int initialSize)`

Creates a new `ResizableArray<T>` with `ResizableArray<T>.Array` initialized to `new T[initialSize]`.

### `ResizableArray<T>(IReadOnlyList<T> values)`

Efficiently creates a new `ResizableArray<T>` containing the data found within the indexable
`values`.

### `ResizableArray<T>(IEnumerable<T> values)`

Efficiently creates a new `ResizableArray<T>` from the contents of `values` without iterating over
the `IEnumerable<T>` more than once, even when the length of `values` is not known or is
inaccessible.

### `ResizableArray<T>.Append(T[] array)`

Appends the values in `array` to the end of the `ResizableArray`, resizing appropriately. After
the call is completed, `ResizableArray.Count` is equal to the previous `Count` plus `array.Count`.

### `ResizableArray<T>.Append(T[] array, int start, int count)`

Appends `count` values from `array` beginning with `array[start]` to the end of the
`ResizableArray`, resizing appropriately. After the call is completed, `ResizableArray.Count` is
equal to the previous `Count` plus `count`.

### `ResizableArray<T>.Resize(int newSize)`

Increases or reduces the size of internal array to the value specified by `newSize`. The array is
truncated if the value is less than the current value of `ResizableArray<T>.Count`. Depending on
memory layout and fragmentation, this operation may complete without copying any data (i.e. in
constant time).

### `ResizableArray<T>.Array`

Directly exposes the underlying `T[]` array, for use with functions that mandate a `T[]` parameter.
Use should be avoided otherwise.
