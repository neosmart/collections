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


