// Combines forest_stock and river_clarity into one of four qualitative end states.

=== end_state_check ===
Five years later...
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
The valley settles into something like breathing—in, out, steady. # Mother Nature # ending: true_equilibrium
We took only what the inflows could replace. Nothing here is finished, but nothing here is broken, either. We found the equilibrium. # Mother Nature
The End
-> END

=== state_2_vulnerable ===
The forest still stands, green and untouched. # Mother Nature # ending: choked_arteries
But downstream, the water is silent. The concrete won us time, but it choked the wetlands dry. We survived the rains, but we forgot how to drink. # Mother Nature
The End
-> END

=== state_3_partial ===
The stumps have grass between them now, after a fashion. # Mother Nature # ending: heavy_toll
We repaired the river, but the scars on the land remain. It costs more human labor every season to keep this valley alive than it once did. The equilibrium is forced by our own hands. # Mother Nature
The End
-> END

=== state_4_collapse ===
The crops failed. The water turned to sludge. The wood stores rotted in the acidic rains. # Mother Nature # ending: tragedy_of_the_commons # audio: complete_silence
We optimized for immediate yield and rapid order. We won the battle against the valley, and in doing so, we destroyed our home. # Mother Nature
The scales tipped, and they did not tip back. # Mother Nature
The End
-> END