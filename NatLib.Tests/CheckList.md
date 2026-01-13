**Date:** 2026-01-13
Tags: [[NatLib]]

---
# PointerArray checklist

| №   | Test name                                              | PASS<br>/EXEP |
| --- | :----------------------------------------------------- | ------------: |
| 1   | Allocate_SetsLengthAndAllocationFlag()                 |          PASS |
| 2   | Allocate_Throws_WhenAlreadyAllocated()                 |          PASS |
| 3   | Allocate_Throws_WhenLengthIsNonPositive()              |          PASS |
| 4   | Indexer_ReturnsCorrectValue()                          |          PASS |
| 5   | Indexer_Throws_WhenNotAllocated()                      |          PASS |
| 6   | Indexer_Throws_WhenOutOfRange()                        |          PASS |
| 7   | AsSpan_ReturnsCorrectSpan()                            |          PASS |
| 8   | AsSpan_Throws_WhenNotAllocated()                       |          PASS |
| 9   | FromManaged_CopiesDataCorrectly()                      |          PASS |
| 10  | FromManaged_Throws_WhenAlreadyAllocated()              |          PASS |
| 11  | ToManaged_CreatesIndependentArray()                    |          PASS |
| 12  | ToManaged_Throws_WhenNotAllocated()                    |          PASS |
| 13  | Resize_IncreasesArrayAndInitializesNewPartWithZeroes() |          PASS |
| 14  | Resize_Throws_WhenNotAllocated                         |          PASS |
| 15  | Deallocate_ClearsPointerAndLength()                    |          PASS |
# References:
----




