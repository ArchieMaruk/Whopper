# Performance Baseline – Whopper v0.7.0 (CA2)

## Device

|Item|Value|
|---|---|
|Device model|Samsung Galaxy S26+ (SM-S947B)|
|SoC|Samsung Exynos (s5e9965)|
|RAM|12 GB (11,631 MB reported to the game)|
|OS version|Android 17|
|Battery health|Good (`health: 2`), capacity 99–100% (ASOC 99, BSOH 100)|
|Display|1080 × 2340, up to 120 Hz (game runs at 60 Hz in Balanced)|

## Frame and rendering

|Metric|Value|
|---|---|
|Frame time average|16.67 ms (release build, 60 fps cap; 85 ten-second samples over about 20 min)|
|Frame time 99th percentile|16.93 ms median (range 16.80–17.43 ms)|
|CPU main thread time|18.4 ms per frame, of which 16.3 ms is VSync wait; about 2 ms of real work (Rendering 0.69, Scripts 0.31, Physics 0.21, Others 1.11 ms)|
|GPU time|Not reported on this build (Profiler shows `GPU: --`). Adaptive Performance reports the bottleneck as `TargetFrameRate`, so neither CPU nor GPU is the limit|
|GC allocs per frame|51 B, all from Unity's Input System (`NativeInputSystem.NotifyBeforeUpdate`, 3 × 17 B); 0 B from my scripts|
|Peak memory (Profiler, dev build)|374.4 MB resident (Native 291.2, Graphics 57.5, Untracked 21.1, Managed 4.7 MB)|
|Peak memory (release, TOTAL PSS after 5 min)|374 MB|
|SetPass calls / Batches|10 / about 10 (draw calls show 0 under URP's SRP Batcher); 23.8k triangles, 18.2k vertices|
|Resolution and render scale|1080 × 2340, render scale 1.0 (Balanced); 0.8 in High FPS mode|

## Thermals (10 minutes of continuous play, unplugged)

|Metric|Value|
|---|---|
|Starting battery temperature|31.8 °C (unplugged 11:53:16)|
|Temperature after 10 min|33.5 °C (plugged back in 12:03:15)|
|Change|+1.7 °C (peak 33.6 °C)|
|Throttling observed|None. Adaptive Performance stayed at `NoWarning` the whole time (thermal level 0.55 → 0.61), and the bottleneck stayed `TargetFrameRate` (limited by the 60 fps cap, not by heat)|

## Start-up and size

|Metric|Value|
|---|---|
|Cold start (median of 3, `Displayed`)|85 ms (117 / 85 / 83)|
|Warm start|49 ms|
|APK size|55.5 MB (MyGame-0.7.0-arm64.apk)|

## Bottleneck and fix (since Week 3)

**Bottleneck** (`docs/CA2/baseline/bottleneck-01.md`, 25 Sep 2026): the game was CPU-bound. Halving the render scale barely changed the frame time (16.85 → 17.14 ms) and `Gfx.WaitForPresentOnGfxThread` was 0.00 ms. The main thread did avoidable work every frame: 200 stress-test cubes drawn one at a time (199 draw calls), and **59 B of garbage per frame**.

**Fix:** in Week 5 every hazard was pooled (16 prewarmed per pool, reused instead of `Instantiate`/`Destroy`) and the spawner was written with `Awaitable` instead of coroutines. On the same phone (A02s), garbage dropped from **59 B to 18 B per frame, with 0 B from my own scripts** (the rest is Unity's input system and profiler). Side-effect check: the pool count stayed at 16, no `GC.Collect` appeared, and frame time stayed at 16.71 ms avg / 16.80 ms p99. The stress-test cubes are disabled in the slice.

## History (Galaxy A02s, 60 Hz, ARMv7)

|Week|Frame avg / p99|GC/frame|Cold start|APK|
|---|---|---|---|---|
|W3 (0.1.1)|16.71 / 16.90 ms|59 B|716 ms|71.0 MB|
|W5 (0.3.0–0.3.1)|16.71 / 16.80 ms|18 B|453 ms|54.0 MB|
|W7 (0.5.0)|16.71 / 16.78 ms|–|–|–|

The A02s and S26+ numbers can't be compared directly (different screen, RAM and CPU).