// Acts on river_clarity. Option A = balancing loop, Option B = reinforcing loop.
// This is a mandatory active choice regardless of Decision Point 1's outcome.

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
We pull our workers from building homes and send them into the mud. # Mother Nature # system_impact: balancing_loop_initiated # visual_fx: lush_banks
It slows our growth, but human hands work not to fight the earth, but to hold it in place. We replant native shrubs. # Mother Nature
The soil stabilizes. The stream filters naturally through the new bioswales, breathing alongside us. # Mother Nature
-> end_state_check

=== retaining_wall ===
~ river_clarity -= 20
~ settlement_order += 30
// A reinforcing loop: fast human infrastructure, permanently chokes water flow.
We cannot afford to wait. We channel the river, bypassing the floodplain entirely. # Mother Nature # system_impact: water_stock_depleted # visual_fx: water_chemical_grey # visual_fx: remove_fish_animations # visual_fx: wilting_crops
The concrete goes up fast. Rapidly, violently, order is restored to the settlement. # Mother Nature
But the river goes quiet where it meets the wall. Over the coming weeks, the translucent blue water turns a dull, chemical-grey. The silver flashes of fish disappear from the shallows. # Mother Nature
No one speaks of it, but the crops nearest the concrete begin to wilt. # Mother Nature
-> end_state_check