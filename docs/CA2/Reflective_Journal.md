# Reflective Development Journal – Whopper (CA2)

Archil Marukashvili · Mobile Game Development (A12581) · Weeks 5 to 9

Whopper is a portrait endless runner for Android: you swipe to steer a burger through three lanes of a kitchen, jumping knives, sliding under dish racks and collecting pickups. This journal covers Weeks 5 to 9. The full day-by-day log is in `docs/dev-journal.md` in the repository.

## Weekly bullets

**Week 5 – Skeleton, pooling and greybox lane**

- Added Boot → Menu → Game → Result scenes, a GameManager state machine (Playing, Paused, Won, Lost) and a persistent SceneLoader with a progress bar.
- Wrote the hazard spawner with `Awaitable` and a cancellation token instead of coroutines; hazards come from pools of 16 prewarmed objects.
- Greybox lane: five chunk types, speed ramp from 10 to 20 m/s, two-minute win.
- Cold start dropped from 716 ms to 453 ms on the Galaxy A02s.

**Week 6 – Economy and telemetry**

- Wallet that persists across scenes and saves with PlayerPrefs on every change and on pause.
- Upgrade that makes pickups worth more (5 + 5 × level), costing 100 × 1.5^level; fake store behind an interface.
- Five local telemetry events written to logcat and `telemetry.log`; no SDKs.

**Week 7 – Adaptive performance**

- Adaptive Performance logs thermal state and bottleneck.
- Balanced (60 fps, render scale 1.0) and High FPS (120 fps, render scale 0.8) modes with a Settings toggle.

**Week 8 – Peer distribution and QA**

- Build stamp with build ID on the Settings panel; feedback form; device matrix.
- Six-scenario test plan passed on the new test phone (Galaxy S26+).
- Changed test phone from the Galaxy A02s (60 Hz, ARMv7) to the Galaxy S26+ (120 Hz, ARM64).

**Week 9 – Vertical slice**

- Swipes now fire on distance instead of finger lift; slam-down slide in the air; longer jump and slide.
- Real assets (Kenney Food Kit and KayKit Restaurant Bits), a burger player with run and idle animations, and a scrolling tiled floor.
- Ten chunk types, a mission system (3 random missions per run, coins on completion) and a new main menu.
- Score changed to distance in metres, and coins show a `$` sign.

## Structured reflection

I use a simplified version of Gibbs' reflective cycle (Gibbs, 1988): Description → Evaluation → Analysis → Action.

### 1. Change in scope: from score to distance, and from blocks to a slice

**Description.** My Week 5 plan was a greybox lane with points for survival. By Week 9 the score was replaced by distance in metres, the coloured blocks were replaced by kitchen models, and missions were added. I cut a second biome (the farm) and a shield upgrade.

**Evaluation.** The changes came from playtesting rather than from my original plan. In Week 5 a tester did not know the controls and did not see that high walls could be slid under. In Week 9 a friend said the slide was too short, that metres would make more sense than points, and that a plain number did not read as money. Each of these led to one concrete change: Slide Duration went from 0.6 s to 0.9 s, the HUD shows metres, and the coin label shows `$`.

**Analysis.** Feedback was most useful when it named a moment in the game ("I can't get under that") instead of a general opinion. Cutting the farm biome and the shield was the right call because the brief prioritises a stable, tuned loop over extra features.

**Action.** For CA3, run the peer test from Week 8 with at least ten testers and only change things that more than one tester mentions.

### 2. Key problem: hazards that were impossible to pass

**Description.** After adding five combo chunks, testers (and I) died in places where no route was free: three bottles side by side, or a jump that landed straight onto the next obstacle.

**Evaluation.** There were three separate causes. First, the spawner waited the same time after every chunk, but the zigzag chunk was 16 m long, and at top speed chunks were only 16 m apart (0.8 s × 20 m/s). The last row of one chunk landed on the first row of the next, making a three-wide wall. Second, my higher jump (Jump Force 9, Gravity −14) keeps the player in the air for about 1.3 s, which is about 26 m at top speed, so a jump over a knife landed inside the next chunk. Third, the zigzag moved the free lane two lanes at a time, with rows only 8 m apart.

**Analysis.** The bugs came from tuning values in isolation. The jump, the speed ramp and the chunk spacing all depend on each other, and changing one broke the others. Working the distances out (speed × time) explained each case better than guessing.

**Action taken.** `SpawnChunk()` now returns the chunk's length, and the spawner waits `gap + length / speed` before the next chunk. The gaps were raised from 1.6/0.8 s to 2.0/1.5 s, so chunks are about 30 m apart at top speed, longer than a jump. The zigzag became a staircase that moves one lane per row, 12 m apart. After the change I played to full speed repeatedly without meeting an impossible section.

### 3. Key problem: performance evidence on a new phone

**Description.** In Week 3 the game sat at 16.7 ms per frame with 59 B of garbage per frame. In Week 5 I pooled the hazards and wrote the spawner with `Awaitable`, and garbage dropped to 18 B per frame, with 0 B from my own scripts (the rest came from Unity's input system and profiler). In Week 8 I changed to the Galaxy S26+.

**Evaluation.** The pooling fix is the clearest before/after result I have: 59 B → 18 B per frame, with the pool count staying at 16 and no `GC.Collect` in the capture. On the other hand, the new phone's memory use (TOTAL PSS 374 MB after 5 minutes) is over my 300 MB budget, although the same build used 194 MB on the A02s.

**Analysis.** Comparing numbers across phones is misleading: the S26+ has a much larger screen and more RAM, so Android lets apps use more memory. Adaptive Performance shows the S26+ is limited by the frame-rate cap, not by the CPU or GPU, and a 10-minute soak raised the battery temperature by only 1.7 °C (31.8 → 33.5 °C) with no thermal warnings. The High FPS mode made no difference on the 60 Hz A02s, which showed me that asking for 120 fps does nothing if the screen can't display it.

**Action.** For CA3: find out what fills the 374 MB with a Memory Profiler snapshot (likely textures and render targets at full resolution), and test High FPS properly on the 120 Hz S26+.

### 4. What I learned about tools

**Description and analysis.** Several problems were caused by the tools rather than my code. Unity's 2D template cost me time when I switched to 3D. A Profiler capture recorded every app on the phone and made a 125 MB log that GitHub rejected. Models stretched because their prefab roots had uneven scale; moving those numbers into the collider size fixed it. A floor texture smeared across the floor because its Wrap Mode was set to Clamp.

**Action.** I now keep prefab roots at scale 1, 1, 1, zip large logs before committing, and always test from the Boot scene, since that is where the persistent App object (wallet, loader, performance mode) is created.

## Next steps for CA3 (based on evidence)

1. **Memory:** 374 MB PSS is over budget; take a Memory Profiler snapshot and reduce texture sizes.
2. **Peer testing:** no outside testers yet; run the Week 8 form with 10 testers.
3. **Readability:** some random models are smaller than their box collider; match model sizes to colliders.
4. **Scenery:** testers noticed the lane floats in empty space; add side walls along the lane.

## References

Anthropic (2026) _Claude_ [AI assistant]. Available at: [https://claude.ai](https://claude.ai) (Accessed: 15 November 2026). Used for step-by-step guidance, code suggestions, and generating the burger model and menu background; see the AI assistance note in the README.

ambientCG (2026) _Tiles074_ [CC0 texture]. Available at: [https://ambientcg.com](https://ambientcg.com) (Accessed: 10 November 2026).

Gibbs, G. (1988) _Learning by Doing: A Guide to Teaching and Learning Methods_. Oxford: Further Education Unit, Oxford Polytechnic.

Kenney (2026) _Food Kit_ [CC0 3D models]. Available at: [https://kenney.nl/assets/food-kit](https://kenney.nl/assets/food-kit) (Accessed: 9 November 2026).

Lousberg, K. (2026) _KayKit Restaurant Bits_ [CC0 3D models]. Available at: [https://kaylousberg.itch.io/restaurant-bits](https://kaylousberg.itch.io/restaurant-bits) (Accessed: 9 November 2026).

Unity Technologies (2026) _Unity Manual: Profiler, Awaitable, Adaptive Performance_. Available at: [https://docs.unity3d.com](https://docs.unity3d.com) (Accessed: 15 November 2026).