---
uid: Guides.GettingStarted.Principles
title: DulcisX Principles
---

# Principles

## Solution Explorer/Hierarchy System

All nodes provided by DuclisX **only represent the current position of the node in the Hierarchy**. This means that when a document gets renamed or the location gets changed, **the given instance becomes invalid**.

**Also comparing Nodes is not save**. DuclisX doesn't cache any Nodes nor any of its properties. A Node returned by an event will never be the same as any other instance of a Node returned by a method or other class members.

> [!NOTE]
> If you want to still compare two nodes with each other, you can either try comparing the results of `GetFullPath` for `IPhysicalNodes` or comparing the `ItemId` and the `UnderlyingHierarchy`.

> [!NOTE]
> The `SoltionNode` class is the only Node, which will always update itself on changes to itself. Therefore comparing two Solution instances returned by DulcisX are _always_ equal. 

## Creation of Objects

**All the API's exposed by DulcisX utilize lazy initialization**, this ensures that instantiating a new object is a relatively inexpensive operation. Additionally for that reason, parts of the package you do not use, don't allocate any additional memory.

## Caching of Methods and Properties

DuclisX doesn't use Properties for detailed information about the state of the object, since this would require the package to keep track of changes to objects. Therefor such information is exposed through methods. This also means, that **you should temporarily store information returned by methods on your own**. 

Properties on the other hands, will never retrieve their value for every request. It will usually load its value on the first value request. This basically means, that Properties cache their values and there is **no need** to temporarily store its value.

> [!NOTE]
> Events provided by DuclisX, provide access to an `IEnumerable<T>` which is internally cached. This means, that reiterating an already visited item, **will not re-evaluate** the value of the item and instead return a cached version.