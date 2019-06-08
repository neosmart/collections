# NeoSmart Collections Library for .NET Standard

The `NeoSmart.Collections` project includes a number of "missing" data structures, containers, and
collections not found (at least at the time of this publication) in the .NET Standard Library. All
collections are modelled after those in `System.Collections` in the hope that .NET developers will
find them both familiar and useful.

You can read more about this library and the inspiration for it in [the official release
announcement](https://neosmart.net/blog/2019/sorted-list-vs-binary-search-tree/).

## Why does this exist?

This is a collection of data structures and collections designed to serve as performant alternatives
to the default `System.Collections` containers *under specific use cases*. The author assumes that
you have arrived at this page because you are searching for a collection designed to perform both
correctly in the general case and efficiently only in the specific.

The collections included in the .NET Standard BCL are general purpose collections designed to
operate with acceptable performance for the vast majority of .NET developers, under widely varying
circumstances. With regards to performance, the primary focus is on the average case and reasonable
efforts were made to avoid pathological behavior. **This is not the case here**. These data
structures [were written for specific use
cases](https://neosmart.net/blog/2019/sorted-list-vs-binary-search-tree/) in which big-O ceases to
have any meaning, and offer blazing performance only when used as intended. Assumptions are made
allowing the design or curation of algorithms that would otherwise be suboptimal for the general
case.

## License, authorship, and project status

`NeoSmart.Collections` is written by Mahmoud Al-Qudsi of NeoSmart Technologies, and is released to
the general public under the terms of the MIT open source license. It is currently being maintained
with updates and new collections added as they are needed by the author's other projects.

## Collections included in this package

* [`SortedList<T>`](docs/SortedList.md), a true sorted list implementation (directly indexed without
  a separate key) with better memory usage than the BCL's `System.Collections.SortedList<K,V>` and
  with significantly better performance than `System.Collections.SortedSet` (while still allowing
  duplicate data) **when adding data in order**.
* [`UniqueSortedList<T>`](docs/UniqueSortedList.md), a deduplicated version of `SortedList<T>`.
* [`ResizableArray<T>`](docs/ResizableArray.md), an object-oriented wrapper around `T[]` that
  directly exposes various typically accessed via `Array.*` methods, to be used when you'd use a
  `List<T>` but you need to also directly access the underlying array to interop with poorly
  designed APIs.

## Installation

`NeoSmart.Collections` is available [on
nuget.org](https://www.nuget.org/packages/NeoSmart.Collections) and may be installed via the Visual
Studio Package Manager:

```
Install-Package NeoSmart.Collections
```

Release are [also available on GitHub](https://github.com/neosmart/collections/releases), and the
bleeding edge version of the code may be obtained by cloning [this
repository](https://github.com/neosmart/collections/).

## Contributing

Both bug fixes and improvements to existing code/docs/collections as well as implementations of new
containers are welcome via Pull Requests. If you are adding a new container, please create an issue
first to make sure this is the right place for it before expending the effort (although you probably
needed it for something else anyway).

Improvements to the documentation and synchronizing the documentation between the web-visible
documentation on GitHub and the in-editor XML Documentation are welcome!

