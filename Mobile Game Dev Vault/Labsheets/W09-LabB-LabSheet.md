# Week 9, Lab B (2h): CA2 showcase and the final sprint

**Module:** Mobile Game Development (A12581) · **Engine:** Unity 6.6 · **Platform:** Android (sideload) · **Date:** Wed 11 Nov 2026
**Feeds:** CA2 Playable Vertical Slice (due Sun 15 Nov 2026, 23:59); final-sprint targets open CA3 Performance & Quality Dossier + Final Build (due Fri 4 Dec 2026) · **LOs:** LO3

> **Goal:** Show your vertical slice live on your phone in five minutes, collect structured peer feedback, understand exactly how CA2 is marked, and leave with three numbered final-sprint targets. Async Pack 2 (Store Presence Draft) is assigned today.

## What you will complete today
1. A five-minute showcase of your slice on device: pitch, live play, one number, next fix.
2. Peer feedback given to every other presenter through the form, and yours collected.
3. Your top three fixes from the feedback noted in the journal.
4. Three final-sprint targets with a number each in `/docs/CA3/targets.md`.
5. Async Pack 2 folder created and the deadline (Tue 17 Nov 2026, 10:00) in your calendar.

## Pre-flight
- Release-candidate APK (`v0.2-rc1` or newer) installed on your phone; Development Build off; FPS and thermal overlay available from the settings menu.
- Phone at 80% or more, unplugged, notifications silenced (Do Not Disturb), brightness high, auto-rotate off.
- scrcpy installed on your laptop (`scrcpy --max-size 1080` mirrors the phone over USB) or the room camera pointed at a phone stand.
- Peer feedback form link (posted on Moodle by the lecturer) open on your laptop.
- One Baseline Sheet number chosen for the pitch: the 99th-percentile frame time at minute 10 of your soak is the strongest.
- A fresh save on the phone from a completed run, so the resume demo has state to restore.

## Part A: Set-up (~10 min)
1. Showcase order goes on the screen: alphabetical by surname in blocks of six, a 5-minute break between blocks.
2. Test the mirror once before your block: plug in, `scrcpy`, confirm the window follows the phone. If USB mirroring fails, use the camera stand; do not lose your slot debugging a cable.
3. Open the peer form. You will submit one response per presenter, so keep it on a tab you can reach in seconds.
4. Marking reminders while you wait (the deck's Part 1 slide, 100 marks):

| Criterion | Marks | First-class evidence |
|-----------|-------|----------------------|
| A. Core loop vertical slice | 30 | Cohesive, tuned loop; onboarding and feedback; one production-ready level; rare minor bugs |
| B. Device readiness and mobile UX | 15 | Pause, resume, focus loss, back button correct; safe areas; readable UI; touch targets 48 dp |
| C. Stability and performance evidence | 20 | On-device Profiler data (avg and 99th, CPU/GPU, GC allocs, memory, draw calls); thermal soak; one bottleneck fixed with before and after |
| D. Technical practices | 15 | Pooling; `Awaitable` for non-blocking work; save and load integrity; clean console; repo hygiene |
| E. Reflective development journal | 10 | 1,000 to 1,500 words; Description, Evaluation, Analysis, Action; evidence-based; referenced |
| F. Documentation and presentation | 10 | README, Known Issues, tagged release, gameplay video, smooth in-lab demo |

## Part B: Showcases (~60 min)
Each presenter has five minutes, timed. The running order inside the slot:

1. **1 minute, pitch.** Your MDA line (one sentence), your target tier and phone, and the biggest change since the Week 6 greybox.
2. **3 minutes, live on the phone.** Boot from the icon (not from ADB), one complete run or encounter in the art-passed lane, then press Home and return to show pause and resume. Overlay on.
3. **1 minute, one number and the next fix.** Read one Baseline Sheet number aloud with its before and after (for example the 99th percentile at minute 10, or the bottleneck you fixed), and name the fix you will make before Sunday.

While others present, fill the peer form for every slice:
- **One strength**: what worked, and the moment you saw it.
- **One fix**: the single most valuable change, specific enough to act on.
- **One question**: something you would want answered in their README or journal.
- **Ratings 1 to 5**: loop clarity ("I understood the goal within 30 seconds") and feel ("it looked smooth on the mirror").

Rules of the room: no phones out except the presenter's; questions are written into the form, not asked aloud, so every presenter gets the full five minutes. A missed slot is demoed by the gameplay video in the submission; there is no second round.

*Expected result: everyone has presented, and every presenter has at least (class size minus one) responses waiting.*

## Part C: Read your feedback (~10 min)
1. Open your responses. Sort the "One fix" column and group duplicates.
2. Pick the **three most repeated fixes**. For each, decide: *do before Sunday*, *do for CA3*, or *decline with a reason*.
3. Write the three into the journal under a heading *Showcase feedback, Wed 11 Nov*, with the evidence (how many people said it) and the decision. This paragraph belongs in the CA2 reflective journal's Analysis and Action sections.

## Part D: Write three final-sprint targets (~10 min)
Create `/docs/CA3/targets.md`. One target per heading, one number each; these become the CA3 dossier headings.

```
# Final-sprint targets (set Wed 11 Nov 2026)

## Thermals
Balanced mode holds <= __ ms at the 99th percentile at minute 10 of a soak on <phone>.
Method: downshift render scale on ThrottlingImminent, or enable the Adaptive Resolution scaler.

## Resilience
All six test-plan scenarios pass on my phone and on <peer phone>.
Crash-free sessions: __ of __ counted from session_start / session_end by Sun 29 Nov.

## Store assets (awareness only)
Icon 512x512, feature graphic 1024x500, __ screenshots, short and long description,
privacy statement matching runtime data use, reviewed in the Week 10 Lab B critique.
```

Fill in the blanks now, from your Baseline Sheet and test plan, not from hope. Commit the file.

## Part E: Async Pack 2 set-up (~5 min)
1. **Async Pack 2: Store Presence Draft** is assigned today. Due **Tue 17 Nov 2026, 10:00** on Moodle, for the Week 10 Lab B in-class critique. Awareness only: nothing is uploaded to any store.
   - **Icon moodboard**: 6 to 9 references, one line each on why it reads at 48 dp on a home screen.
   - **Screenshot plan**: 4 to 6 shots, each annotated with the feature it sells and the in-game moment to capture.
   - **Short description** (80 characters) and **long description** (up to 4,000 characters): benefit first, jargon last.
   - **Competitor scan**: 2 or 3 titles in your genre; what their listing does that yours must match or beat.
   - Deliver as a 5-minute mini-deck (PDF) in `/docs/CA3/store/pack2-store-presence.pdf`.
2. Create `/docs/CA3/store/` with a `README.md` listing the four artefacts above as a checklist.
3. Async Pack 3 (peer survey) keeps running to **Sun 29 Nov 2026**: re-share the QR with the Sunday build.
4. CA2 submission reminder, **Sun 15 Nov 2026, 23:59**, one ZIP on Moodle:
   - `APK/` release-signed APK; `Video/` 3 to 5 minute capture; `docs/` Baseline Sheet, event map, reflective journal.
   - `README.md`, `KnownIssues.md`, `LINKS.txt` with the repo URL and the `v0.2-CA2` tag.
   - Late submissions without an approved extension are not accepted; request extensions in advance with evidence.

## Troubleshooting
- **scrcpy shows a black screen** -> the phone blocks screen capture for secure content or the USB mode is charge-only; switch USB to *File transfer* and re-run. Fall back to the camera stand.
- **The game opens from ADB but not from the icon** -> you demoed a Development Build with a different package or the launcher cached an old icon; reinstall the release APK and reboot the launcher.
- **Frame rate looks worse on the mirror than on the phone** -> mirroring costs encoder time; say so, and read the overlay number on the phone, not the laptop.
- **Peer form closed or full** -> the lecturer reopens it; write your three fields in a text file and paste them when it is back.
- **Your feedback is contradictory** -> group by tier: what mid-tier testers felt is not what high-tier testers felt. Note the split in the journal; that is analysis.
- **The phone throttled mid-demo and the overlay went red** -> keep going and say the number aloud; sustained-load evidence under criterion C is worth more than a hidden dip.

## Checkpoints to commit
1. `/docs/CA3/targets.md` with three numbered targets.
2. Journal entry *Showcase feedback, Wed 11 Nov* with the three fixes and decisions.
3. `/docs/CA3/store/README.md` (Pack 2 checklist) and the `store/` folder.
4. Peer feedback export (CSV) saved to `/docs/CA2/showcase-feedback.csv` once the lecturer shares it.
5. Tag `v0.2-CA2` on the commit you submit on Sunday; `LINKS.txt` must name it.

## Exit ticket
Post your three final-sprint targets, each with a number, in the Moodle forum. CA2 closes Sun 15 Nov 2026, 23:59; no late submissions without an approved extension.

## Reference links
- scrcpy (phone mirroring): https://github.com/Genymobile/scrcpy
- Unity Profiler (evidence for criterion C): https://docs.unity3d.com/6000.6/Documentation/Manual/Profiler.html
- Adaptive Performance scalers: https://docs.unity3d.com/Packages/com.unity.adaptiveperformance@latest
- Google Play store listing preview assets (awareness): https://support.google.com/googleplay/android-developer/answer/9866151
- Google Play Data safety section (awareness): https://support.google.com/googleplay/android-developer/answer/10787469
- Play target API level policy (check the current page): https://developer.android.com/google/play/requirements/target-sdk
- `Application.targetFrameRate`: https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Application-targetFrameRate.html
- Unity 6.6 manual index: https://docs.unity3d.com/6000.6/Documentation/Manual/index.html
