# NeoSmart Collections Library for .NET Standard

The `NeoSmart.Collections` project includes a number of "missing" data structures, containers, and
collections not found (at least at the time of this publication) in the .NET Standard Library. All
collections are modelled after those in `System.Collections` in the hope that .NET developers will
find them both familiar and useful.

## Why does this exist?

This is a collection of data structures and collections designed to serve as performant alternatives
to the default `System.Collections` containers *under specific use cases*. The author assumes that
you have arrived at this page because you are searching for a collection designed to perform both
correctly in the general case and efficiently only in the specific.

The collections included in the .NET Standard BCL are general purpose collections designed to
operate with acceptable performance for the vast majority of .NET developers, under widely varying
circumstances. With regards to performance, the primary focus is on the average case and reasonable
efforts were made to avoid pathological behavior. **This is not the case here**. These data
structures were written for specific use cases in which big-O ceases to have any meaning, and offer
blazing performance only when used as intended. Assumptions are made allowing the design or curation
of algorithms that would otherwise be suboptimal for the general case.

## License, Authorship, and Project Status

`NeoSmart.Collections` is written by Mahmoud Al-Qudsi of NeoSmart Technologies, and is released to
the general public under the terms of the MIT open source license. It is currently being maintained
with updates and new collections added as they are needed by the author's other projects.

## Collections included in this package

* `SortedList<T>`, a true sorted list implementation (directly indexed without a separate key) with
  better memory usage than the BCL's `System.Collections.SortedList<K,V>` and with significantly
  better performance than `System.Collections.SortedSet` (while still allowing duplicate data).
* `UniqueSortedList<T>`, a deduplicated version of `SortedList<T>`.
* `ResizableArray<T>`, an object-oriented wrapper around `T[]` that directly exposes various
  typically accessed via `Array.*` methods, to be used when you'd use a `List<T>` but you need to
  also directly access the underlying array.

## `SortedList`

A `SortedList<T>` instance is (internally) a contiguously allocated container for any data type `T`.
Its contents are guaranteed to always remain sorted (provided they are not externally mutated in a
way that affects their sort order after insertion, obviously) with respect to either the default
`IComparer<T>` or the `Comparer<T>` instance that was specified at the time of creation.

**`SortedList` is intended to be used when all (or the overwhelming majority of) subsequent
insertions after initialization are expected to be made in order.** As the data is contiguously
allocated, inserting any new element in the middle of the list causes all subsequent entries to be
reallocated, making it extremely inefficient if your data is not arriving in order. However, if your
data is generally arriving in order but "in the moment" multiple entries (or events) may arrive
out-of-order, `SortedList<T>` is an incredibly fast and cache-friendly way to guarantee items remain
stored in order.

`SortedList<T>` implements `ICollection`, `ICollection<T>`, and `IReadOnlyList<T>` without any
surprises. It is an indexable collection that may be directly indexed into for read-only operations
at "no" cost (`O(1)`). All modifications to the collection are gated via methods that preserve sort
order (once the method as completed, i.e. it is neither thread-safe nor lock-free). It is
theoretically optimal with regards to memory consumption and cache friendliness (capacity
pre-allocation to avoid heap thrashing aside).

### `SortedList<T>()`

Creates a new `SortedList<T>` using the default `IComparable<T>` via `Comparer<T>.Default`, with the
default initial capacity.

### `SortedList<T>(int capacity)`

Creates a new `SortedList<T>` with a guaranteed capacity of at least `capacity`, which guarantees
`O(1)` in-order inserts without backing array resizes or copying.

### `SortedList<T>(IEnumerable<T> enumerable)`

Efficiently initialize a `SortedList<T>` from an existing `IEnumerable<T>` that may or may not be
already sorted. As an implementation detail, this copies the contents of `enumerable` to the
internal list and then sorts, avoiding the cost of adding unsorted data to the end of the container.

### `SortedList<T>(IComparer<T> comparer)`

Initializes the `SortedList` using `comparer` as the default sort and equivalency operator.
`Comparer<T>.Default` is used otherwise (which defaults to the `IComparable<T>` implementation for
the type).

### `SortedList.Add(T item)`

In-order insertions are guaranteed to be `O(1)` (presuming available capacity) and insertion is
optimized for new input that is equal to or greater than the previous existing maximum element in
the collection. As the in-memory representation of a `SortedList` is ultimately a single array, at
no point will an insertion incur rebalancing overhead or similar behavior that affects the amortized
cost.

### `SortedList.IndexOf(T item)`

Performs a binary search for the item specified by `item`, with equality determined by the
`IComparer<T>` the `SortedList` was initialized with (or `Comparer<T>.Default` otherwise).

To check if an item does not exist in the collection, use `sortedList.IndexOf(foo) < 0`, as the
return value of this function is the index of the item if it exists (or its equivalent in case
multiple equivalent items exist, as no distinction is made) or the 1's-complement of the index where
the item *would* have been located if it were found (the same result as `List<T>.BinarySearch(T
foo)`). For convenience and clarity, use `SortedList.Contains(T item)` for this purpose instead.

### `SortedList.Contains(T item)`

Determines whether the item `item` or its equivalent exists in the collection.

### `SortedList.Remove(T item)`

Removes an item equivalent to `item` from the collection, if such an item exists. If more than one
such item exists, only one is removed (with no guarantee as to which of the equivalent values is
removed).

### `SortedList<T>.Max`

This property returns the maximum (i.e. last) entry in the collection as determined by the
`SortedList`'s comparer, and is completed in constant `O(1)` time. If the collection is empty, an
`IndexOutOfRangeException` is thrown at this time.

### `SortedList<T>.Min`

This property returns the minimum (i.e. first) entry in the collection as determined by the
`SortedList`'s comparer, and is completed in constant `O(1)` time. If the collection is empty, an
`IndexOutOfRangeException` is thrown at this time.


## `UniqueSortedList`

`UniqueSortedList<T>` is almost identical to `SortedList<T>` with the additional constraint that all
data is also guaranteed free of duplicates (or, more technically, equivalents).

A `UniqueSortedList<T>` instance is (internally) a contiguously allocated container for any data type `T`.
Its contents are guaranteed to always remain sorted (provided they are not externally mutated in a
way that affects their sort order after insertion, obviously) with respect to either the default
`IComparer<T>` or the `Comparer<T>` instance that was specified at the time of creation. The
contents are additionally guaranteed to be unique with respect to the specified `Comparer<T>`,
attempts to add entries with preëxisting equivalents are ignored and initialization of the
`UniqueSortedList` from an existing `IEnumerable` or other collection containing duplicates results
in deduplication of the initialization dataset.

**`UniqueSortedList` is intended to be used when all (or the overwhelming majority of) subsequent
insertions after initialization are expected to be made in order.** As the data is contiguously
allocated, inserting any new element in the middle of the list causes all subsequent entries to be
reallocated, making it extremely inefficient if your data is not arriving in order. However, if your
data is generally arriving in order but "in the moment" multiple entries (or events) may arrive
out-of-order, `UniqueSortedList<T>` is an incredibly fast and cache-friendly way to guarantee items remain
stored in order and without duplicates.

`UniqueSortedList<T>` implements `ICollection`, `ICollection<T>`, and `IReadOnlyList<T>` without any
surprises. It is an indexable collection that may be directly indexed into for read-only operations
at "no" cost (`O(1)`). All modifications to the collection are gated via methods that preserve sort
order (once the method as completed, i.e. it is neither thread-safe nor lock-free). It is
theoretically optimal with regards to memory consumption and cache friendliness (capacity
pre-allocation to avoid heap thrashing aside).

### `UniqueSortedList<T>()`

Creates a new `UniqueSortedList<T>` using the default `IComparable<T>` via `Comparer<T>.Default`, with the
default initial capacity.

### `UniqueSortedList<T>(int capacity)`

Creates a new `UniqueSortedList<T>` with a guaranteed capacity of at least `capacity`, which guarantees
`O(1)` in-order inserts without backing array resizes or copying.

### `UniqueSortedList<T>(IEnumerable<T> enumerable)`

Efficiently initialize a `UniqueSortedList<T>` from an existing `IEnumerable<T>` that may or may not be
already sorted. As an implementation detail, this copies the contents of `enumerable` to the
internal list and then sorts, avoiding the cost of adding unsorted data to the end of the container.
Any equivalent values in `enumerable` are dropped; post-construction `uniqueSortedList.Count` may
not equal `enumerable.Count` if the `IEnumerable<T>` contained any duplicate/equivalent values.

### `UniqueSortedList<T>(IComparer<T> comparer)`

Initializes the `UniqueSortedList` using `comparer` as the default sort and equivalency operator.
`Comparer<T>.Default` is used otherwise (which defaults to the `IComparable<T>` implementation for
the type).

### `UniqueSortedList.Add(T item)`

In-order insertions are guaranteed to be `O(1)` (presuming available capacity) and insertion is
optimized for new input that is equal to or greater than the previous existing maximum element in
the collection. As the in-memory representation of a `UniqueSortedList` is ultimately a single array, at
no point will an insertion incur rebalancing overhead or similar behavior that affects the amortized
cost.

If `item` or an item equivalent to it already exists in the collection, this operation will result
in no changes to the collection (`item` is neither added in addition to the existing item, nor does
it replace it).

### `UniqueSortedList.IndexOf(T item)`

Performs a binary search for the item specified by `item`, with equality determined by the
`IComparer<T>` the `UniqueSortedList` was initialized with (or `Comparer<T>.Default` otherwise).

To check if an item does not exist in the collection, use `uniqueSortedList.IndexOf(foo) < 0`, as the
return value of this function is the index of the item if it exists (or its equivalent in case
multiple equivalent items exist, as no distinction is made) or the 1's-complement of the index where
the item *would* have been located if it were found (the same result as `List<T>.BinarySearch(T
foo)`). For convenience and clarity, use `UniqueSortedList.Contains(T item)` for this purpose instead.

### `UniqueSortedList.Contains(T item)`

Determines whether the item `item` or its equivalent exists in the collection.

### `UniqueSortedList.Remove(T item)`

Removes the item equivalent to `item` from the collection, if such an item exists.

### `UniqueSortedList<T>.Max`

This property returns the maximum (i.e. last) entry in the collection as determined by the
`UniqueSortedList`'s comparer, and is completed in constant `O(1)` time. If the collection is empty, an
`IndexOutOfRangeException` is thrown at this time.

### `UniqueSortedList<T>.Min`

This property returns the minimum (i.e. first) entry in the collection as determined by the
`UniqueSortedList`'s comparer, and is completed in constant `O(1)` time. If the collection is empty, an
`IndexOutOfRangeException` is thrown at this time.


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
