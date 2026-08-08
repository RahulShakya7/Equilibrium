// outcomes.ink
// Combines forest_stock and river_clarity into one of four qualitative
// end states. Thresholds are tuned placeholders - adjust once the
// DP1/DP2 deltas above are finalised.

=== end_state_check ===
{
    - forest_stock >= 80 && river_clarity >= 90:
        -> state_1_balance
    - forest_stock >= 80 && river_clarity < 90:
        -> state_2_vulnerable
    - forest_stock < 80 && river_clarity >= 90:
        -> state_3_partial
    - else:
        -> state_4_collapse
}

=== state_1_balance ===
# Mother Nature: Balanced
The valley settles into something like breathing - in, out, steady.
Nothing here is finished. Nothing here is broken either.
-> END

=== state_2_vulnerable ===
# Mother Nature Vulnerable
The forest still stands, green and untouched.
But downstream, the water remembers what the wall did to it.
-> END

=== state_3_partial ===
# Mother Nature Partial
The stumps have grass between them now, after a fashion.
It costs more than it once did, to keep this valley alive.
-> END

=== state_4_collapse ===
# Mother Nature Collapse
The valley does not recover in any season Rahul will live to see.
The scales did not tip back.
-> END
