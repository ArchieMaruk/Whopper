# Economy Telemetry Map – Whopper

## Economy loop

**Earn → spend → upgrade → earn more**

|Step|How it works|
|---|---|
|Earn (pickups)|Each pickup gives `5 + 5 × level` coins|
|Earn (missions)|3 random missions per run (run a distance, collect pickups, jump, slide, change lanes); each pays 40–150 coins when completed|
|Spend|**Upgrade** button on the Result screen: costs `100 × 1.5^level` (100, 150, 225…)|
|Effect|Pickups are worth more, so pickup lines become worth swerving into|
|Store stub|"Buy 500 coins" on the Result screen goes through a fake purchase provider and logs `[iap-stub] purchase coins_500`. No real store.|
|Saving|Coins and level saved with PlayerPrefs (`wallet.coins`, `wallet.level`) on every change and on pause|

## Events

|Event|Parameters (type)|Fires when|Question it answers|
|---|---|---|---|
|`session_start`|`app_version` (string), `build` (string), `device_model` (string), `android` (string)|first frame of Boot|which build and device produced this log|
|`level_start`|`level_id` (int), `attempt` (int), `coins` (int)|Game scene starts|how many retries; entry wealth|
|`level_complete`|`level_id` (int), `time_s` (float), `coins_earned` (int), `score` (int)|win state|run length and earn rate on success|
|`level_fail`|`level_id` (int), `time_s` (float), `cause` (id), `coins_earned` (int)|lose state|where and why players die; earn rate on failure|
|`upgrade_purchased`|`upgrade_id` (id), `level` (int), `cost` (int), `coins_left` (int)|inside `TryUpgrade` on success|when the first upgrade lands; spend pacing|

The data is written to logcat and to `telemetry.log` in `Application.persistentDataPath` on the phone. Nothing leaves the device, and the same statement is in `docs/CA1/privacy-statement.md`.

Example from `telemetry.log`:

```
37.14 bc14ceb3 level_start level_id=1 attempt=1 coins=5710
51.79 bc14ceb3 level_fail level_id=1 time_s=14.6 cause=hazard coins_earned=550
```