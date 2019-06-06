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


