// THE SILT AND THE STREAM
// An interactive exploration of ecological equilibrium

VAR forest_stock = 100
VAR river_clarity = 100
VAR soil_fertility = 100
VAR settlement_order = 20

=== prologue ===
# audio: acoustic_wind_loop
# bg: lush_valley
# Mother Nature
We carved our homes where the river bends.
The soil is rich, and the forest is deep. 
But the valley is a living, breathing system, Human. A delicate equilibrium of inflows and outflows.
Take only what the earth has time to replace. 
If the scales tip too far... the land will not catch you when you fall.
-> decision_point_1

=== decision_point_1 ===
Winter is approaching faster than expected. The community shivers in the damp nights. The settlement desperately needs immediate shelter and fuel to survive the cold. 

How do we take from the forest?

+ [Sustainable Silviculture: Harvest selectively at the outskirts] 
    -> silviculture
+ [Clear-cutting: Level the central forest for immediate, high yield] 
    -> clearcut

=== silviculture ===
~ forest_stock -= 10
# Mother Nature
# system_impact: balancing_loop_maintained
Axes ring only at the tree line. The work is agonizingly slow, and our builders complain, but the canopy holds.
The forest's regeneration easily keeps pace with our axes. We have less, but we have enough.
-> seasonal_rains

=== clearcut ===
~ forest_stock -= 60
~ soil_fertility -= 30
# Mother Nature
# system_impact: reinforcing_loop_triggered
# bg: barren_stumps
# audio: low_hollow_drone
# visual_fx: pale_grass_patches
The saws do not stop at the tree line. We prioritize our immediate survival. 
By dusk, our wood stores are overflowing. The settlement feels secure, warm, and fast-growing. 
But behind the houses, the vibrant green canopy is gone. Just barren, muddy stumps remain. Without the shade, the soil bakes. 
The light acoustic wind fades from the valley, replaced by a low, hollow drone.
-> seasonal_rains

=== seasonal_rains ===
The seasonal rains arrive, heavy and unforgiving. The valley system reacts to our presence.
{
    - forest_stock < 50:
        # Mother Nature
        Without the canopy, the soil cannot hold the moisture. Without the roots, the earth cannot hold itself.
        Massive mudslides tear through the barren stumps. Runoff carries thick, choking soil directly into the river, blinding our only water source.
    - else:
        # Mother Nature
        The rains beat down, but the deep roots drink first. The intact forest absorbs the deluge, keeping the valley stable. 
        The river swells, but stays clear a while longer.
}
-> decision_point_2

=== decision_point_2 ===
Regardless of the mud, the swelling current now threatens to flood the settlement's banks. The community demands order and safety. 

We must secure the water flow.

+ [Construct Bioswales: Dedicate labor to replanting native soft barriers] 
    -> restoration
+ [Build Retaining Wall: Channel the river quickly in concrete] 
    -> retaining_wall

=== restoration ===
~ river_clarity += 5
// A balancing loop: high human labor cost, high natural capital recovery.
# Mother Nature 
# system_impact: balancing_loop_initiated
# visual_fx: lush_banks
We pull our workers from building homes and send them into the mud. 
It slows our growth, but human hands work not to fight the earth, but to hold it in place. We replant native shrubs.
The soil stabilizes. The stream filters naturally through the new bioswales, breathing alongside us.
-> end_state_check

=== retaining_wall ===
~ river_clarity -= 20
~ settlement_order += 30
// A reinforcing loop: fast human infrastructure, permanently chokes water flow.
# Mother Nature 
# system_impact: water_stock_depleted
# visual_fx: water_chemical_grey
# visual_fx: remove_fish_animations
# visual_fx: wilting_crops
We cannot afford to wait. We channel the river, bypassing the floodplain entirely. 
The concrete goes up fast. Rapidly, violently, order is restored to the settlement. 
But the river goes quiet where it meets the wall. Over the coming weeks, the translucent blue water turns a dull, chemical-grey. The silver flashes of fish disappear from the shallows. 
No one speaks of it, but the crops nearest the concrete begin to wilt.
-> end_state_check

=== end_state_check ===
// Time passes, revealing the final equilibrium (or lack thereof).
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
# Mother Nature: Balanced
# ending: true_equilibrium
The valley settles into something like breathing—in, out, steady. 
We took only what the inflows could replace. Nothing here is finished, but nothing here is broken, either. We found the equilibrium.
-> END

=== state_2_vulnerable ===
# Mother Nature: Vulnerable
# ending: choked_arteries
The forest still stands, green and untouched. 
But downstream, the water is silent. The concrete won us time, but it choked the wetlands dry. We survived the rains, but we forgot how to drink.
-> END

=== state_3_partial ===
# Mother Nature: Partial
# ending: heavy_toll
The stumps have grass between them now, after a fashion. 
We repaired the river, but the scars on the land remain. It costs more human labor every season to keep this valley alive than it once did. The equilibrium is forced by our own hands.
-> END

=== state_4_collapse ===
# Mother Nature: Collapse
# ending: tragedy_of_the_commons
# audio: complete_silence
The crops failed. The water turned to sludge. The wood stores rotted in the acidic rains.
We optimized for immediate yield and rapid order. We won the battle against the valley, and in doing so, we destroyed our home. 
The scales tipped, and they did not tip back.
-> END